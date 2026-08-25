# XP-072: TOOLS::deriv

**Inventory ID:** `XP-072`
**Trigger:** library API `TOOLS::deriv`
**Status:** Ready for Requirements
**Version:** v1
**Date:** 2026-08-25
**Owner:** Archaeologist (`/trace-path`)

## Summary

When a caller invokes `TOOLS::deriv(f, dh)`, the function returns a `real(8)` array the same length as `f`: a forward difference at the first sample, centered differences in the interior, and a backward difference at the last sample, all divided by spacing `dh`. The body does not check `size(f)<2` or `dh==0`. Weakest planning-binding grade is E3. XP-388 observed one file-backed case (`deriv(y, x(2)-x(1))` on `numutils/test/xy2.data`) as E1; that driver I/O is XP-388, not this API. External DEP is DEP-001 only. Unguarded short arrays and zero `dh` are DEF-005 and DEF-006 (`fix-now`).

---

## 1. Sequence of operations

| Step | What happens | Legacy location | Evidence |
|------|--------------|-----------------|----------|
| 1 | Caller supplies rank-1 `real(8)` `f` (`intent(in)`) and scalar `dh`. Result `df` has `size(f)`. | `src/TOOLS.f90:31,175-178` | E3 `src/TOOLS.f90:175-178` |
| 2 | `L=size(f)`. No check that `L>=2` or `dh/=0`. | `src/TOOLS.f90:179-180` | E3 `src/TOOLS.f90:179-180` |
| 3 | Left endpoint: `df(1)=(f(2)-f(1))/dh`. | `src/TOOLS.f90:181` | E3 `src/TOOLS.f90:181` |
| 4 | Interior `i=2..L-1`: `df(i)=(f(i+1)-f(i-1))/(2.d0*dh)`. | `src/TOOLS.f90:182-184` | E3 `src/TOOLS.f90:182-184` |
| 5 | Right endpoint: `df(L)=(f(L)-f(L-1))/dh`. Return `df`. | `src/TOOLS.f90:185-186` | E3 `src/TOOLS.f90:185-186` |
| 6 | XP-388 reads `numutils/test/xy2.data` as `x,y`, sets `dh=x(2)-x(1)`, calls `deriv(y, dh)`, prints `x(i), dy(i)` with list-directed `write`. | `fidelity/driver.f90:41-66` | E3 `fidelity/driver.f90:62-65` |
| 7 | Oracle probe: that driver stdout was bit-identical across two runs (1049 lines). | `docs/modernization/oracle.md` §1, §4 | E1 `docs/modernization/oracle.md` §1 (two identical XP-388 runs) |

## 2. Internal dependencies

| Module / function | Role in this path | Evidence |
|-------------------|-------------------|----------|
| `TOOLS::deriv` | Entire algorithm is in-module; no call to `linspace` or other TOOLS publics. | E3 `src/TOOLS.f90:175-186` |
| `TOOLS` USE of `COMMON_VARS`, `TIMER`, `IOTOOLS` | Compilation-unit USEs; this function body does not call them. | E3 `src/TOOLS.f90:7-9,175-186` |

## 3. External dependencies

| DEP ID | How this path uses it | Evidence |
|--------|-----------------------|----------|
| DEP-001 | Fortran array arithmetic and `size`; gfortran runtime. This body does not call BLAS. | E3 `src/TOOLS.f90:175-186`; E3 `docs/modernization/dependency-graph.md` XP-072 |

## 4. Data flow

| Direction | What | Shape / units | Side effects | Evidence |
|-----------|------|---------------|--------------|----------|
| in | `f` | `real(8)` rank-1, length `L` | none | E3 `src/TOOLS.f90:176` |
| in | `dh` | `real(8)` assumed uniform abscissa spacing, same units as the independent variable of `f` | none | E3 `src/TOOLS.f90:177`; E3 `fidelity/driver.f90:62` (driver chooses `x(2)-x(1)`) |
| out | `df` | `real(8)` rank-1 length `L`; units of `f/dh` | none in this function | E3 `src/TOOLS.f90:178-186` |
| out | probed sample | list-directed pairs `x, dy` for `xy2.data` (XP-388 formatting, not the function) | stdout from the driver | E1 `docs/modernization/oracle.md` §1, §4 |

## 6. Edge cases and error handling

| Case | What the legacy path actually does | Evidence | Defect |
|------|--------------------------------------|----------|--------|
| `size(f)=0` | `L=0`; `df(1)` and `f(2)` are out of range. No `error` call. | E3 `src/TOOLS.f90:180-181` | DEF-005 (`fix-now`) |
| `size(f)=1` | `df(1)=(f(2)-f(1))/dh` reads past the end of `f`; loop `2..0` is skipped; `df(1)=(f(1)-f(0))/dh` also invalid. | E3 `src/TOOLS.f90:180-185` | DEF-005 (`fix-now`) |
| `size(f)=2` | `df(1)=(f(2)-f(1))/dh`; interior loop empty; `df(2)=(f(2)-f(1))/dh`. Both ends equal the one-sided slope. | E3 `src/TOOLS.f90:181-185` | none |
| `dh=0` | Division by zero; no check. | E3 `src/TOOLS.f90:181-185` | DEF-006 (`fix-now`) |
| Nonuniform `x` | API has no `x` array; caller must pass a single `dh`. XP-388 uses first-interval spacing for the whole series. | E3 `src/TOOLS.f90:175-177`; E3 `fidelity/driver.f90:62-63` | none (API contract); driver sampling is XP-388 |
| End vs interior stencil | First/last use one-sided first differences; interior uses two-point central differences. | E3 `src/TOOLS.f90:181-185` | none |

## 7. Open questions

- [x] Should `size(f)<2` and `dh==0` be recorded as defects or as caller-contract undefined behavior? See DEF-005 and DEF-006 (`fix-now`).
- [ ] XP-388 list-directed print format is a harness concern (`oracle.md` §4), not this function's return value.

## Errata

None.

## Links

- Path inventory: [`docs/modernization/execution-path-inventory.md`](../execution-path-inventory.md)
- Dependency graph: [`docs/modernization/dependency-graph.md`](../dependency-graph.md)
- Path test plan: [`docs/modernization/test-plans/XP-NNN-tests.md`](../test-plans/XP-NNN-tests.md)
- Defect ledger: [`docs/modernization/defect-ledger.md`](../defect-ledger.md)
- Oracle: [`docs/modernization/oracle.md`](../oracle.md)

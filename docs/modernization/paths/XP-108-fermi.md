# XP-108: FUNCTIONS::fermi

**Inventory ID:** `XP-108`
**Trigger:** library API `FUNCTIONS::fermi`
**Status:** Ready for Requirements
**Version:** v1
**Date:** 2026-08-25
**Owner:** Archaeologist (`/trace-path`)

## Summary

When a caller invokes elemental `FUNCTIONS::fermi(x, beta)`, the function returns `real(8)` Fermi-Dirac occupation `1/(1+exp(beta*x))`, except that if `x*beta > 100.d0` it returns `0.d0` without evaluating `exp` (overflow guard; the comparison is strict `>`). It is elemental, so it may be applied to arrays. Weakest planning-binding grade is E3. XP-388 observed five scalars at `beta=100` as E1 (`x=-2,-1,0,1,2` → `1, 1, 0.5, ~3.72e-44, 0`). External DEP is DEP-001 only. Zhang/Jin sources compile in the same `FUNCTIONS.f90` unit (DEP-009) but this procedure does not call them.

---

## 1. Sequence of operations

| Step | What happens | Legacy location | Evidence |
|------|--------------|-----------------|----------|
| 1 | Caller supplies `x` and `beta` (`real(8)`, `intent(in)`). Function is `elemental`. | `src/FUNCTIONS.f90:181,240-242` | E3 `src/FUNCTIONS.f90:240-242` |
| 2 | If `x*beta > 100.d0`, set result `0.d0` and return. | `src/FUNCTIONS.f90:243-246` | E3 `src/FUNCTIONS.f90:243-246` |
| 3 | Otherwise `fermi = 1.d0/(1.d0+exp(beta*x))`. No clamp when `x*beta` is large and negative. | `src/FUNCTIONS.f90:247` | E3 `src/FUNCTIONS.f90:247` |
| 4 | XP-388 called `fermi(x(i), 100.d0)` for `x = -2,-1,0,1,2` and printed two-column `es24.17`. | `fidelity/driver.f90:32-38` | E3 `fidelity/driver.f90:32-38` |
| 5 | That probe printed occupations `1`, `1`, `0.5`, `3.72007597602083562e-44`, `0` for `x = -2,-1,0,1,2` at `beta=100`, bit-identical across two runs. | `docs/modernization/oracle.md` §1 | E1 `docs/modernization/oracle.md` §1 |

## 2. Internal dependencies

| Module / function | Role in this path | Evidence |
|-------------------|-------------------|----------|
| `FUNCTIONS::fermi` | Entire formula and cutoff; no call to `wfun`/`zerf`/Zhang-Jin file-scope names. | E3 `src/FUNCTIONS.f90:181,240-248` |
| `FUNCTIONS` `USE COMMON_VARS` | Module depends on `COMMON_VARS`; this function body does not reference it. | E3 `src/FUNCTIONS.f90:174,240-248` |
| `include "functions_special_funcs.f90"` | Same compilation unit as `fermi`; not executed on this path. | E3 `src/FUNCTIONS.f90:171,240-248` |

## 3. External dependencies

| DEP ID | How this path uses it | Evidence |
|--------|-----------------------|----------|
| DEP-001 | Fortran `exp` and `real(8)`; gfortran runtime. | E3 `src/FUNCTIONS.f90:247`; E3 `docs/modernization/dependency-graph.md` XP-108 |

## 4. Data flow

| Direction | What | Shape / units | Side effects | Evidence |
|-----------|------|---------------|--------------|----------|
| in | `x` | `real(8)`; energy-like, same units as `1/beta` | none | E3 `src/FUNCTIONS.f90:241-242` |
| in | `beta` | `real(8)`; inverse-temperature-like | none | E3 `src/FUNCTIONS.f90:241-242` |
| out | `fermi` | `real(8)` in `[0,1]` for finite `exp`; exactly `0` when `x*beta>100` | none | E3 `src/FUNCTIONS.f90:243-247` |
| out | probed sample | five pairs `(x, fermi)` at `beta=100` | XP-388 stdout | E1 `docs/modernization/oracle.md` §1 |

## 6. Edge cases and error handling

| Case | What the legacy path actually does | Evidence | Defect |
|------|--------------------------------------|----------|--------|
| `x*beta > 100` | Returns `0.d0` without `exp`. | E3 `src/FUNCTIONS.f90:243-246` | none (overflow guard; inventory already states saturation) |
| `x*beta > 100` observed | XP-388 `x=2`, `beta=100` (product `200`) printed `0`. | E1 `docs/modernization/oracle.md` §1 | none |
| `x*beta == 100` | Condition is `>`; evaluates `1/(1+exp(100))`. | E3 `src/FUNCTIONS.f90:243-247` | none |
| `x*beta == 100` observed | XP-388 `x=1`, `beta=100` printed `3.72007597602083562e-44`. | E1 `docs/modernization/oracle.md` §1 | none |
| `x*beta` large negative | No early return; `exp(beta*x)` underflows toward 0 and the formula tends to `1`. | E3 `src/FUNCTIONS.f90:247` | none |
| `x*beta` large negative observed | XP-388 `x=-2` and `x=-1` at `beta=100` printed `1`. | E1 `docs/modernization/oracle.md` §1 | none |
| `x=0` | `1/(1+exp(0))=0.5`. | E3 `src/FUNCTIONS.f90:247` | none |
| `x=0` observed | XP-388 printed `0.5`. | E1 `docs/modernization/oracle.md` §1 | none |
| `beta=0` | `x*beta` is 0, not `>100`; result is `0.5` for any finite `x`. | E3 `src/FUNCTIONS.f90:243-247` | none |
| Elemental array call | Language elemental rules apply; not exercised by XP-388 (scalars only). | E3 `src/FUNCTIONS.f90:240` | none |

## 7. Open questions

- [ ] Elemental array application was not in the XP-388 probe; E3 covers the body.
- [ ] Compile-unit presence of DEP-009 Zhang/Jin does not bind this path's runtime; graph v2 lists DEP-001 only. No extra DEP found.

## Errata

None.

## Links

- Path inventory: [`docs/modernization/execution-path-inventory.md`](../execution-path-inventory.md)
- Dependency graph: [`docs/modernization/dependency-graph.md`](../dependency-graph.md)
- Path test plan: [`docs/modernization/test-plans/XP-NNN-tests.md`](../test-plans/XP-NNN-tests.md)
- Defect ledger: [`docs/modernization/defect-ledger.md`](../defect-ledger.md)
- Oracle: [`docs/modernization/oracle.md`](../oracle.md)

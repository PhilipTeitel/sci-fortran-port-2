# XP-061: TOOLS::logspace

**Inventory ID:** `XP-061`
**Trigger:** library API `TOOLS::logspace`
**Status:** Ready for Requirements
**Version:** v1
**Date:** 2026-08-25
**Owner:** Archaeologist (`/trace-path`)

## Summary

When a caller invokes `TOOLS::logspace(start, stop, num [, base])`, the function returns `num` `real(8)` values that are uniformly spaced in log-`base` coordinates between `start` and `stop` (default `base=10`). Zero endpoints are silently replaced with `1.d-12` before taking `log`; the path then calls `linspace` with both endpoints included (`iend=.true.`, `istart` default true) and raises the result as `base**array`. Weakest planning-binding grade is E3. XP-388 observed `logspace(1.d0, 1000.d0, 5)` as E1. External DEP is DEP-001 only. Silent zero-rewrite is DEF-002 (`reproduce-faithfully`). Unguarded non-positive endpoints and invalid `base` are DEF-003 and DEF-004 (`fix-now`).

---

## 1. Sequence of operations

| Step | What happens | Legacy location | Evidence |
|------|--------------|-----------------|----------|
| 1 | Caller supplies `start`, `stop`, `num`, optional `base`. Result `array(num)` is `real(8)`. | `src/tools_grids.f90:32-38`; public `src/TOOLS.f90:19` | E3 `src/tools_grids.f90:32-38` |
| 2 | `base_` defaults to `10.d0`; overridden if `base` present. | `src/tools_grids.f90:40` | E3 `src/tools_grids.f90:40` |
| 3 | If `num<0`, `error("logspace: N<0, abort.")` then `stop`. No `num<2` check in `logspace` itself. | `src/tools_grids.f90:41` | E3 `src/tools_grids.f90:41` |
| 4 | If `start==0.d0`, replace local `A` with `1.d-12`. If `stop==0.d0`, replace local `B` with `1.d-12`. No warning. | `src/tools_grids.f90:42-43` | E3 `src/tools_grids.f90:42-43` |
| 5 | `A=log(A)/log(base_)`; `B=log(B)/log(base_)`. Negative `start`/`stop` are not redirected; Fortran `log` of a non-positive argument is undefined. | `src/tools_grids.f90:44` | E3 `src/tools_grids.f90:44` |
| 6 | `array = linspace(A, B, num=num, iend=.true.)` (both endpoints: `istart` omitted so default true). | `src/tools_grids.f90:45`; `src/tools_grids.f90:8-14` | E3 `src/tools_grids.f90:45,8-14` |
| 7 | `array = base_**array` (element-wise exponentiation). Return. | `src/tools_grids.f90:46` | E3 `src/tools_grids.f90:46` |
| 8 | XP-388 calls `logspace(1.d0, 1000.d0, n)` with `n=5` and prints each element as `es24.17`. | `fidelity/driver.f90:19-24` | E3 `fidelity/driver.f90:19-24` |
| 9 | That probe printed `1, 5.62341325190349117, 31.6227766016837926, 177.827941003892278, 1000` (`es24.17`), bit-identical across two runs. | `docs/modernization/oracle.md` §1 | E1 `docs/modernization/oracle.md` §1 |

## 2. Internal dependencies

| Module / function | Role in this path | Evidence |
|-------------------|-------------------|----------|
| `TOOLS::logspace` | Entry; log-map, call `linspace`, exponentiate. | E3 `src/TOOLS.f90:19`; E3 `src/tools_grids.f90:32-47` |
| `TOOLS::linspace` | Builds the uniform grid in log-space with both endpoints. | E3 `src/tools_grids.f90:45` |
| `COMMON_VARS::error` | Abort on `num<0` here, and on `num<2` with both endpoints inside `linspace`. | E3 `src/tools_grids.f90:41,12` |

## 3. External dependencies

| DEP ID | How this path uses it | Evidence |
|--------|-----------------------|----------|
| DEP-001 | Fortran `log`, `**`, `real(8)`; gfortran runtime. This body does not call another inventoried library. | E3 `src/tools_grids.f90:32-47`; E3 `docs/modernization/dependency-graph.md` XP-061 |

## 4. Data flow

| Direction | What | Shape / units | Side effects | Evidence |
|-----------|------|---------------|--------------|----------|
| in | `start`, `stop` | `real(8)`; interpreted as positive magnitudes for `log` | zeros rewritten to `1.d-12` | E3 `src/tools_grids.f90:32-43` |
| in | `num` | `integer` length | abort if `num<0`; `linspace` may abort if `num<2` | E3 `src/tools_grids.f90:41,45,12` |
| in | `base` | optional `real(8)`, default 10 | none | E3 `src/tools_grids.f90:37-40` |
| out | `array` | `real(8)` rank-1 length `num`; first ≈ `start` (or `1e-12` if start was 0), last ≈ `stop` (or `1e-12` if stop was 0) when `num>=2` | none | E3 `src/tools_grids.f90:45-46` |
| out | probed sample | five values from 1 to 1000 in log10 | XP-388 stdout only | E1 `docs/modernization/oracle.md` §1 |

## 6. Edge cases and error handling

| Case | What the legacy path actually does | Evidence | Defect |
|------|--------------------------------------|----------|--------|
| `num<0` | `error("logspace: N<0, abort.")` then `stop`. | E3 `src/tools_grids.f90:41` | none |
| `num=0` or `num=1` | Falls through to `linspace` with both endpoints; `num<2` aborts `N<2 with both start and end points`. | E3 `src/tools_grids.f90:45,11-12` | none |
| `start==0` or `stop==0` | Silent substitute `1.d-12` for that endpoint; no message. | E3 `src/tools_grids.f90:42-43` | DEF-002 (`reproduce-faithfully`) |
| `start<0` or `stop<0` (and not exactly 0) | `log` of a negative; no guard. | E3 `src/tools_grids.f90:42-44` | DEF-003 (`fix-now`) |
| `base<=0` or `base==1` | `log(base_)` is invalid or zero; no guard. | E3 `src/tools_grids.f90:44` | DEF-004 (`fix-now`) |
| Commented `iend` in source | `iend` is commented out; callers cannot drop the last point except by changing `linspace` defaults inside this wrapper. | E3 `src/tools_grids.f90:35-36,45` | none |

## 7. Open questions

- [x] Is the `1.d-12` substitution for zero intended product behavior or a defect? See DEF-002 (`reproduce-faithfully`).
- [ ] Non-default `base` was not executed by XP-388.

## Errata

None.

## Links

- Path inventory: [`docs/modernization/execution-path-inventory.md`](../execution-path-inventory.md)
- Dependency graph: [`docs/modernization/dependency-graph.md`](../dependency-graph.md)
- Path test plan: [`docs/modernization/test-plans/XP-NNN-tests.md`](../test-plans/XP-NNN-tests.md)
- Defect ledger: [`docs/modernization/defect-ledger.md`](../defect-ledger.md)
- Oracle: [`docs/modernization/oracle.md`](../oracle.md)

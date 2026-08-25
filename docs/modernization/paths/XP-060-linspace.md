# XP-060: TOOLS::linspace

**Inventory ID:** `XP-060`
**Trigger:** library API `TOOLS::linspace`
**Status:** Ready for Requirements
**Version:** v1
**Date:** 2026-08-25
**Owner:** Archaeologist (`/trace-path`)

## Summary

When a caller invokes `TOOLS::linspace(start, stop, num [, istart, iend, mesh])`, the function returns a length-`num` `real(8)` vector of evenly spaced values between `start` and `stop`, with optional flags that include or exclude each endpoint and an optional output `mesh` equal to the spacing used. Default flags include both endpoints (`step = (stop-start)/(num-1)`). Negative `num` (and `num<2` when both endpoints are requested) abort via `COMMON_VARS::error`. Weakest planning-binding grade is E3 (source). One default-argument case (`linspace(0,1,5)`) was observed as E1 via XP-388 / oracle v1; optional-flag combinations were not in that probe. Comparison rules belong in the path test plan. No external DEP beyond DEP-001.

---

## 1. Sequence of operations

| Step | What happens | Legacy location | Evidence |
|------|--------------|-----------------|----------|
| 1 | Caller supplies `start`, `stop`, `num`, and optionally `istart`, `iend`, `mesh`. Result array is `real(8) :: array(num)`. | `src/tools_grids.f90:1-6`; public `src/TOOLS.f90:18` | E3 `src/tools_grids.f90:1-6`; E3 `src/TOOLS.f90:18` |
| 2 | If `num<0`, call `error("linspace: N<0, abort.")` then `stop`. | `src/tools_grids.f90:7`; `src/COMVARS.f90:192-208` | E3 `src/tools_grids.f90:7`; E3 `src/COMVARS.f90:192-208` |
| 3 | Default `startpoint_=.true.`, `endpoint_=.true.`; override from present `istart` / `iend`. | `src/tools_grids.f90:8-9` | E3 `src/tools_grids.f90:8-9` |
| 4 | Both endpoints: if `num<2`, abort with `"linspace: N<2 with both start and end points"`; else `step=(stop-start)/real(num-1,8)` and `array(i)=start+(i-1)*step`. | `src/tools_grids.f90:11-14` | E3 `src/tools_grids.f90:11-14` |
| 5 | Start only: `step=(stop-start)/real(num,8)`; `array(i)=start+(i-1)*step` (last point is not `stop`). | `src/tools_grids.f90:16-18` | E3 `src/tools_grids.f90:16-18` |
| 6 | End only: `step=(stop-start)/real(num,8)`; `array(i)=start+i*step` (first point is not `start`). | `src/tools_grids.f90:20-22` | E3 `src/tools_grids.f90:20-22` |
| 7 | Neither endpoint: `step=(stop-start)/real(num+1,8)`; `array(i)=start+i*step`. | `src/tools_grids.f90:24-26` | E3 `src/tools_grids.f90:24-26` |
| 8 | If `mesh` is present, set `mesh=step`. Return `array`. | `src/tools_grids.f90:28-29` | E3 `src/tools_grids.f90:28-29` |
| 9 | XP-388 calls `linspace(0.d0, 1.d0, n)` with `n=5` and prints each element as `es24.17`. | `fidelity/driver.f90:10-16` | E3 `fidelity/driver.f90:10-16` |
| 10 | That probe's five printed values were `0, 0.25, 0.5, 0.75, 1`, bit-identical across two runs. | `docs/modernization/oracle.md` §1 | E1 `docs/modernization/oracle.md` §1 |

## 2. Internal dependencies

| Module / function | Role in this path | Evidence |
|-------------------|-------------------|----------|
| `TOOLS` (include `tools_grids.f90`) | Owns and exports `linspace`. | E3 `src/TOOLS.f90:18,169` |
| `COMMON_VARS::error` | Prints ANSI-styled `error:` line and `stop` on invalid `num`. | E3 `src/TOOLS.f90:7`; E3 `src/COMVARS.f90:192-208` |

## 3. External dependencies

| DEP ID | How this path uses it | Evidence |
|--------|-----------------------|----------|
| DEP-001 | Fortran `real(8)` arithmetic, `forall`, and runtime `stop` via gfortran. No BLAS/LAPACK call in this body. | E3 `src/tools_grids.f90:1-29`; E3 `docs/modernization/dependency-graph.md` XP-060 |

## 4. Data flow

| Direction | What | Shape / units | Side effects | Evidence |
|-----------|------|---------------|--------------|----------|
| in | `start`, `stop` | `real(8)` scalars; same units as the grid | none | E3 `src/tools_grids.f90:1-2` |
| in | `num` | `integer` length of the result | abort if `num<0`, or if `num<2` with both endpoints | E3 `src/tools_grids.f90:2-3,7,12` |
| in | `istart`, `iend` | optional `logical`; default both true | none | E3 `src/tools_grids.f90:4-9` |
| in | `mesh` | optional `real(8)` scalar, intent not declared; assigned the step | overwrites caller `mesh` if present | E3 `src/tools_grids.f90:6,28` |
| out | `array` | `real(8)` rank-1 length `num`, increasing if `stop>start` | none (pure return; no file I/O) | E3 `src/tools_grids.f90:1-2,14` |
| out | probed sample | five values `0, 0.25, 0.5, 0.75, 1` | XP-388 writes stdout; this function does not | E1 `docs/modernization/oracle.md` §1 |

## 6. Edge cases and error handling

| Case | What the legacy path actually does | Evidence | Defect |
|------|--------------------------------------|----------|--------|
| `num<0` | Prints `error:linspace: N<0, abort.` (MPI-rank gated) and `stop`. | E3 `src/tools_grids.f90:7`; E3 `src/COMVARS.f90:192-208` | none |
| Default flags and `num=0` or `num=1` | Treated as both-endpoints; `num<2` aborts with `N<2 with both start and end points`. | E3 `src/tools_grids.f90:8-12` | none |
| `num=0` with an endpoint excluded | Skips the `num<2` guard. Start-only and end-only divide by `real(num,8)` (zero). Neither-endpoint divides by `real(num+1,8)` (one). Result array length is 0. | E3 `src/tools_grids.f90:16-26` | DEF-001 (`fix-now`) |
| `stop<start` | Negative `step`; grid decreases. No extra check. | E3 `src/tools_grids.f90:13-14` | none |
| `start==stop` and `num>=2` both endpoints | All entries equal `start`. | E3 `src/tools_grids.f90:13-14` | none |

## 7. Open questions

- [ ] Optional `istart`/`iend`/`mesh` combinations were not executed by XP-388; planning may treat E3 as sufficient, or the path test plan can add FIX cases.
- [x] Should unguarded `num=0` with a dropped endpoint be a defect? See DEF-001 (`fix-now`).

## Errata

None.

## Links

- Path inventory: [`docs/modernization/execution-path-inventory.md`](../execution-path-inventory.md)
- Dependency graph: [`docs/modernization/dependency-graph.md`](../dependency-graph.md)
- Path test plan: [`docs/modernization/test-plans/XP-NNN-tests.md`](../test-plans/XP-NNN-tests.md)
- Defect ledger: [`docs/modernization/defect-ledger.md`](../defect-ledger.md)
- Oracle: [`docs/modernization/oracle.md`](../oracle.md)

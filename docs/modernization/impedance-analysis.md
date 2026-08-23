<!-- Technology and impedance analysis contract:
- Decision support: what about the two stacks changes how a path should be ported.
- Seeded language-family sections are CONDITIONAL. Include only the family that matches the discovered source stack. Omit the others entirely. Never fill N/A rows.
- Include hosting/deployment notes only when an execution path is a service, job host, or UI — not for a class library with no host.
- IMP-NNN rows are snapshot findings. Later pattern choices are DEC-NNN or ADR-NNN, cited from those artifacts.
- One evidence grade per row. Name affected XP IDs; do not restate path behavior.
- After Status: Snapshot, append Errata or produce vN+1.
-->

# Technology and Impedance Analysis

**Source stack:** Fortran 90/95 free-form modules with Fortran 2003 command-line intrinsics / no application framework (static library `libscifor.a`)
**Target stack:** C# / .NET 8 / ASP.NET Core (HTTP/JSON services). Product is a rewritten service API, not a Fortran `.mod` / `libscifor.a` drop-in.
**Version:** v2
**Status:** Snapshot
**Date:** 2026-08-22
**Owner:** Migration Strategist (`/analyze-impedance`)

## Summary

The mismatches that change a preserve / wrap / rewrite / adapter choice for the first slice are Fortran kind-8 vs `System.Double` (**IMP-001**), 1-based vs 0-based arrays (**IMP-002**), Fortran procedure calls vs ASP.NET Core HTTP/JSON (**IMP-010**), optional dummies vs JSON fields (**IMP-012**), `error`/`STOP` vs HTTP error responses (**IMP-013**), `INCLUDE` packaging vs C# types (**IMP-014**), dummy-shaped results vs JSON arrays (**IMP-035**), and oracle harness formatted text vs parsed-numeric comparison (**IMP-006**). Candidate pattern for those rows is **rewrite** behind HTTP/JSON. **IMP-011** (compiler pin) is closed as **preserve** of the Mac gfortran 16.1.0 / OpenBLAS 0.3.34 T1 pin. Later-slice legal and native-library mismatches (NR FFT/solvers **IMP-024** / **IMP-029**, MKL **IMP-025**, BLAS/LAPACK **IMP-026**, FFTPACK **IMP-027**, FFTW **IMP-028**, `SYSTEM` **IMP-018**, libmatheval **IMP-020**) do not bind XP-062 / XP-063 / XP-110 / XP-074. `COMMON` and `EQUIVALENCE` were not found. Oracle.md v1 Snapshot shows the four library calls are runnable on the pinned host; it is not a comparison rule — path test plans own parsed-numeric tolerances. No global tolerance is implied. Exact route/JSON schema is Architect ADR work, not an unnamed analogue.

---

## 1. Source characteristics that matter for porting

| Characteristic | Observation | Evidence |
|----------------|-------------|----------|
| Language / packaging | Fortran 90/95 free-form modules; F2003 `command_argument_count` / `get_command_argument`; umbrella `SCIFOR` re-exports a subset of compiled modules; bodies often `INCLUDE` fragments; consumers link `libscifor.a` and `USE` `.mod` files. | E3 `src/SCIFOR.f90:1-16`; `src/Makefile:4-12,169`; `src/TOOLS.f90:169`; `src/PARSECMD.f90:43-44` |
| Runtime / compilers | Native `gfortran` + `libgfortran` is the merged default; `include/options.inc` also names Intel Fortran Classic (`ifort`) flags including `-openmp`. No application framework or hosted process. | E3 `scripts/build.sh`; `include/options.inc:1-16`; DEP-001, DEP-002 |
| Memory / arrays | Default 1-based indexing; column-major rank-2; assumed-shape everywhere in first-slice APIs; explicit `0:` and `-L:L` bounds on shifts/FFT/Green's functions; Fortran `pointer` components on GF and list types; dummy-shaped automatic results (`array(num)`). No `COMMON` or `EQUIVALENCE`. | E3 `src/tools_grids.f90:2,14`; `src/tools_shifts.f90:8-14`; `src/FFTGF_NR.f90:46,144`; `src/GREENFUNX.f90:13-34`; `src/LIST_D_ORDERED.f90:32-38` |
| Numeric types | Public math is `real(8)` / `complex(8)` with `1.d0` literals (`COMVARS` `dbl=8`, `dp=8`). `ddp=16` is declared and unused. `real(4)` appears in `tools_sort1d` local RNG and NR `rfour1` (`sngl`). `integer(4)` date fields. | E3 `src/COMVARS.f90:32-34`; `src/tools_grids.f90:1-14`; `src/tools_sort1d.f90:147-159`; `src/FFTGF_NR.f90` `rfour1`; `src/COMVARS.f90:139` |
| Control / failure | Unstructured `GOTO`/`GO TO` in vendored QUADPACK, MINPACK, NR FFT `rfour1`, cubspl, Faddeeva/zerf, and `TOOLS::fastsearchreal` / `sort`. `COMMON_VARS::error` prints and `stop`s. First-slice `linspace`/`logspace` call `error` on `num<0`. | E3 `src/COMVARS.f90:192-208`; `src/tools_grids.f90:7,41`; `src/TOOLS.f90:382-388`; `src/tools_sort1d.f90:78` |
| Shared state | Module publics: `MPIID`/`MPISIZE` (no MPI calls), uninitialized `omp_*`, `IOFILE::store_size`, `PARSE_CMD::cmd_var`, `SPLINE_FINTER_MOD` tables, TIMER `SAVE` stacks. `nrand` uses `SAVE`+`DATA`. | E3 `src/COMVARS.f90:61-67`; `src/IOFILE.f90:13`; `src/PARSECMD.f90:16`; `src/TIMER.f90:12-28`; `src/RANDOM.f90:145-176`; `src/spline_finter_mod.f90:5-7` |
| I/O | Formatted `es24.17` in the fidelity driver; list-directed `write(*,*)` in that driver's `deriv` section and numutils CLIs; SLPLOT `f12.4` plus generated gnuplot. Locale-sensitive. `call system` for gzip/mkdir/mv/chmod. | E3 `fidelity/driver.f90:15,23,37,65`; `numutils/src/linspace.f90:48`; `src/slplot_splot_3d.f90:22,32-40,66`; `src/IOFILE.f90:230,266,290` |
| Native math / FFT | LAPACK-facing `MATRIX` always `include "mkl_lapack.fi"`. Merged probe links OpenBLAS. FFT compile unit is a symlink; `scripts/build.sh` default `FFT_BACKEND=NR`. FFTW uses fixed-form `fftw3.f`. | E3 `src/MATRIX.f90:10`; `include/sfmake.inc:7-22`; `scripts/build.sh`; `src/FFTGF.f90`; `src/fftw3.f`; DEP-009–DEP-015 |
| Typing | Host modules `TOOLS` / `FUNCTIONS` / `TIMER` / `SLPLOT` have `implicit none`; many `INCLUDE` fragments do not. `IOTOOLS.f90` (USE-only) and MKL/`fftw3.f` units lack `implicit none`. File-scope Zhang/Jin pack sits *before* `MODULE FUNCTIONS`. | E3 `src/TOOLS.f90:10,169`; `src/tools_grids.f90`; `src/FUNCTIONS.f90:171-176`; `src/IOTOOLS.f90`; `src/mkl_dfti.f90`; `src/fftw3.f` |

## 2. Target characteristics that differ

| Characteristic | Difference from source | Evidence |
|----------------|------------------------|----------|
| Runtime / packaging | Source is a native static archive plus compiler-specific `.mod` files. Target is ASP.NET Core on .NET 8 (Kestrel HTTP/JSON). Fortran ABI is not the product boundary (DEP-003). | E2 owner (C# / .NET 8 / HTTP/JSON services, rewrite). E3 DEP-003; `.cursor/workflow.config.yml` `targetStack` |
| Numeric types | Source `real(8)` / `complex(8)` map to IEEE-754 `System.Double` and `System.Numerics.Complex`. `real(4)` islands and unused `ddp=16` stay out of the first slice. Rounding mode is BCL default; comparison is parsed numeric, per path. | E3 `src/COMVARS.f90:32-34`. E2 owner: parsed numbers, per-path tolerance. |
| Arrays / indexing | Source 1-based, column-major, dummy-shaped rank-1 results vs 0-based `double[]` / JSON number arrays. First-slice APIs are rank-1; no rank-2 layout conversion on those four. | E3 `src/tools_grids.f90:2,14`; `src/TOOLS.f90:176-178` |
| Failure model | Source `error` prints ANSI text and `STOP` vs HTTP status + JSON problem body (shape is an ADR). First-slice `linspace`/`logspace` `num<0` is an invalid-input path. | E3 `src/COMVARS.f90:192-208`; `src/tools_grids.f90:7,41` |
| Formatted I/O / culture | Source Fortran `es24.17` / list-directed writes vs JSON numbers. Product parity is parsed numeric, not byte-identical Fortran text. Oracle T1 ran with `LC_ALL=C`; JSON should use invariant culture. | E1 `docs/modernization/oracle.md` §2 locale. E2 owner: parsed numbers. E3 `fidelity/driver.f90:15` |
| Native libraries | BLAS/LAPACK/FFT/MKL/FFTW vs later-slice C# or native adapters. Not used by first-slice execution. | E3 DEP-009–DEP-015; graph XP-062/063/074/110 → DEP-001 only |
| Time / RNG | `date_and_time`, `SYSTEM_CLOCK`, intrinsic `random_number`, SAVE `nrand` vs `DateTime` / `RandomNumberGenerator`. Not first slice. | E3 `src/COMVARS.f90:142`; `src/RANDOM.f90:42,145-176,203` |
| Host commands | POSIX `SYSTEM` strings vs `System.IO` / `Process`. Not first slice. | E3 DEP-027 |
| Verification | Numeric precision, JSON round-trip, ordering, and invalid-input mapping are **per-path**. Path test plans own the bound. Oracle T1 documents runnability of XP-062/063/110/074 on macOS arm64 / gfortran 16.1.0 / OpenBLAS 0.3.34, not a tolerance. | E1 `docs/modernization/oracle.md` §1; workflow `parity.comparisonRules: per-path`. E2 owner: per-path parsed numeric |

## 3. Impedance mismatches

Candidate pattern is a recommendation, not a decision. Record the decision as `DEC-NNN` or `ADR-NNN`. First-slice rows use **rewrite** to match the owner-stated C# HTTP/JSON rewrite (still cited from `/record-decision` after M1).

| ID | Mismatch | Severity | Affected XP IDs | Candidate pattern | Evidence |
|----|----------|----------|-----------------|-------------------|----------|
| IMP-001 | Fortran `real(8)` / `complex(8)` vs `System.Double` / `System.Numerics.Complex`; `real(4)` islands in sort RNG and NR `rfour1` `sngl`; unused `ddp=16`. **Binds first slice** (`double` only). | critical | XP-062, XP-063, XP-074, XP-110, XP-225 (first slice); later almost all numeric library/CLI paths | rewrite | E3 `src/tools_grids.f90:1-47`; `src/TOOLS.f90:175-186`; `src/FUNCTIONS.f90:240-248`; `src/COMVARS.f90:32-34`. E1 oracle.md: those four library calls ran under T1 (runnability only). |
| IMP-002 | Default 1-based arrays (`forall(i=1:num)`) vs 0-based `double[]` / JSON arrays. **Binds first slice.** | high | XP-062, XP-063, XP-064, XP-074, XP-110, XP-225 (first slice); later array APIs generally | rewrite | E3 `src/tools_grids.f90:14,22,45`; `src/TOOLS.f90:180-185`; `fidelity/driver.f90:14-16,57-65` |
| IMP-003 | Column-major rank-2 layout (SLPLOT `Y(i,j)`, LAPACK Fortran API) vs row-major / C# 2-d arrays. 1-d first-slice results do not require a layout conversion. | medium | XP-055–XP-059, XP-171–XP-179; not XP-062/063/110/074 | adapter | E3 `src/slplot_splot_3d.f90:6,19-22`; `src/MATRIX.f90` `dsyev`/`zheev`/`dgetrf` family; DEP-009–DEP-010 |
| IMP-004 | Unstructured `GOTO` / `GO TO` in vendored kernels and some TOOLS internals vs structured C#. Absent from linspace/logspace/fermi/deriv. | medium | XP-068, XP-089–XP-093, XP-105–XP-107, XP-112–XP-113, XP-121, XP-181; private `fastsearchreal` in `TOOLS.f90`; not first-slice public APIs | rewrite | E3 `src/tools_sort1d.f90:78`; `src/TOOLS.f90:382-388`; `src/FFTGF_NR.f90:506,533`; `src/integrate_d_quadpack.f90`; `src/minpack.f90`; `src/functions_wofz.f90:20,110`; `src/functions_zerf.f90`; `src/spline_cubspl_routines.f90` |
| IMP-005 | Include fragments and some compile units omit their own `implicit none` (host modules often supply it). MKL units and fixed-form `fftw3.f` do not. First-slice hosts `TOOLS` and `FUNCTIONS` have `implicit none` covering included grids/fermi. | medium | XP-231 (MKL FFT unit), DEP-013 FFTW include; SLPLOT/TIMER/TOOLS fragments if compiled standalone; not first-slice compilation of `TOOLS.f90` / `FUNCTIONS.f90` | rewrite | E3 `src/TOOLS.f90:10,169`; `src/tools_grids.f90`; `src/FUNCTIONS.f90:175-176`; `src/IOTOOLS.f90`; `src/mkl_dfti.f90`; `src/fftw3.f` |
| IMP-006 | Formatted `es24.17` and list-directed `write(*,*)` vs JSON numbers. Driver uses `es24.17` for linspace/logspace/fermi and list-directed for deriv. **Binds harness XP-225** (and XP-224's extract). Product comparison is parsed numeric, not byte-identical Fortran text. | high | XP-225, XP-224 (first-slice harness); XP-217–XP-219, XP-208 CLIs; later XP-055–XP-057 (`f12.4`) | rewrite | E3 `fidelity/driver.f90:15,23,29,37,65`; `numutils/src/linspace.f90:48`. E1 oracle.md `LC_ALL=C`. E2 owner: parsed numbers, per-path tolerance. |
| IMP-007 | Module-level mutable data (no `COMMON`): `MPIID`/`MPISIZE`, `omp_*`, `store_size`, TIMER `SAVE` arrays, `cmd_var`, `finterX`/`finterF`. First slice touches this only if `error()` runs (`mpiID` default 0). | high | XP-003–XP-008, XP-036–XP-039, XP-045, XP-186, CLI parsers; first-slice invalid-input via XP-005/XP-006 | rewrite | E3 `src/COMVARS.f90:61-67`; `src/TIMER.f90:12-28`; `src/IOFILE.f90:13`; `src/PARSECMD.f90:16`; `src/spline_finter_mod.f90:5-7` |
| IMP-008 | `SAVE` + `DATA` Park–Miller state in `nrand` vs injectable `RandomNumberGenerator`. Not `EQUIVALENCE` (none found). | high | XP-187; not first slice | rewrite | E3 `src/RANDOM.f90:145-176` |
| IMP-009 | Fixed-form Fortran-77 `PARAMETER` include `src/fftw3.f` vs C# FFT wrappers. | medium | FFT paths when `FFT_BACKEND=FFTW3` (DEP-013); not first slice | adapter | E3 `src/fftw3.f:1-10`; `src/FFTGF_FFTW3.f90` `include "fftw3.f"` |
| IMP-010 | Compiler-specific `.mod` + `libscifor.a` ABI vs ASP.NET Core HTTP/JSON resources. **Binds every first-slice product path.** Wrap of `libscifor.a` is not the product pattern. | critical | All active library XPs (XP-003–XP-207) if later sliced as services; first slice XP-062, XP-063, XP-074, XP-110 | rewrite | E3 `src/Makefile:8-12`; `etc/Makefile.template`. E2 owner: C# as HTTP/JSON services, rewrite. |
| IMP-011 | Merged T1 pin is gfortran 16.1.0 / OpenBLAS / macOS arm64; tree also names ifort Classic and MKL. Numeric results can change with compiler. **Closed for this POC:** accept the T1 pin. | high | XP-062, XP-063, XP-074, XP-110, XP-223–XP-225 | preserve (pin this T1 host as oracle) | E3 `include/options.inc:1-16`; DEP-002. E1 oracle.md §2, §6. E2 owner: matching numbers on this Mac is sufficient. |
| IMP-012 | Optional dummies + `present()` (`linspace` `istart`/`iend`/`mesh`; `logspace` `base`) vs omitted JSON properties or C# optional parameters. Driver uses defaults. **Binds first-slice API shape.** | medium | XP-062, XP-063, XP-064–XP-067 (first slice); later many optionals | rewrite | E3 `src/tools_grids.f90:4-6,37`; `fidelity/driver.f90:13,21` |
| IMP-013 | `error`/`abort` print ANSI-colored text and `stop` vs HTTP error status + JSON body. `linspace`/`logspace` call `error` on `num<0`. **Binds first-slice invalid input.** | medium | XP-005, XP-006; XP-062, XP-063 error paths | rewrite | E3 `src/COMVARS.f90:192-208`; `src/tools_grids.f90:7,41` |
| IMP-014 | Module bodies assembled by `INCLUDE` of free-form fragments vs C# types/files. **Binds first-slice packaging** (grids live in `tools_grids.f90` inside `TOOLS`). | medium | XP-062–XP-088 (TOOLS includes); XP-036–XP-039; XP-055–XP-059 | rewrite | E3 `src/TOOLS.f90:169-222`; `src/TIMER.f90:41-43`; `src/SLPLOT.f90` includes |
| IMP-015 | Generic interfaces, defined assignment, and operators vs C# overloads. First-slice `linspace`/`logspace`/`deriv`/`fermi` are single-signature (`fermi` is `elemental`). | high | XP-055–XP-059, XP-072–XP-073, XP-108–XP-111, XP-114–XP-120, XP-139–XP-144, XP-171–XP-182; elemental `fermi` is the only first-slice generic concern | rewrite | E3 `src/TOOLS.f90:49-112`; `src/FUNCTIONS.f90:182-185,240`; `src/GREENFUNX.f90:37-66`; `src/MATRIX.f90:14-59`; `src/VECTORS.f90:24-38` |
| IMP-016 | Fortran `pointer` components on Green's-function and list types vs GC references / spans. | high | XP-114–XP-120, XP-145–XP-170; not first slice | rewrite | E3 `src/GREENFUNX.f90:13-34`; `src/LIST_D_ORDERED.f90:32-38`; `src/LIST_D_UNORDERED.f90:8-14` |
| IMP-017 | Non-default bounds `dimension(0:)` and `dimension(-L:L)` vs 0-based collections. | high | XP-072, XP-073, XP-094–XP-104, XP-118; not first slice | rewrite | E3 `src/tools_shifts.f90:8-14,31-38`; `src/FFTGF_NR.f90:46,144,206`; `src/GREENFUNX.f90:80` |
| IMP-018 | `call system` POSIX strings vs `System.IO` / `Process`. | high | XP-041, XP-043, XP-044, XP-050, XP-077, XP-085, XP-132, XP-055–XP-056; not first slice | adapter | E3 `src/IOFILE.f90:230,266,290`; `src/TOOLS.f90:273-274`; `src/tools_bethe.f90:19`; `src/SQUARE_LATTICE.f90:159-160`; `src/slplot_splot_3d.f90:66`; DEP-027 |
| IMP-019 | Generated gnuplot `.gp` scripts / `wxt` terminal vs a later plotting choice. | medium | XP-055, XP-056, XP-211; not first slice | adapter | E3 `src/slplot_splot_3d.f90:38-40,66`; `numutils/src/splot.f90`; DEP-025 |
| IMP-020 | Unprototyped C names `evaluator_create_` / `evaluator_evaluate_x_` vs a C# expression evaluator. | high | XP-212; not first slice | adapter | E3 `numutils/src/func.f90:11-12,58-60`; `numutils/src/Makefile` `-lmatheval`; DEP-026 |
| IMP-021 | F2003 command-line intrinsics and `NAME=value` parsing vs ASP.NET configuration / query strings. | medium | XP-032–XP-035, XP-208–XP-222; not the library first slice | rewrite | E3 `src/PARSECMD.f90:18-26,43-44,91-96`; numutils CLI `command_argument_count` loops |
| IMP-022 | `date_and_time` / `SYSTEM_CLOCK` vs `DateTime` / `Stopwatch`. Outside bounded T1. | high | XP-003, XP-004, XP-037–XP-039, XP-191; not first slice | rewrite | E3 `src/COMVARS.f90:136-145`; `src/timer_chrono.f90:11,34`; `src/RANDOM.f90:197-208`. E1 oracle.md §4 excludes these from T1. |
| IMP-023 | Two RNGs: intrinsic `random_number` vs SAVE `nrand`. | high | XP-187 vs XP-188–XP-193, XP-221; not first slice | rewrite | E3 `src/RANDOM.f90:34-50,145-176` |
| IMP-024 | Numerical Recipes radix-2 FFT with no license grant; default compiled backend. Copying source is not a production route; owner accepted POC license risk. Compiled in T1 build, not called by the driver. | critical | XP-094–XP-107, XP-214; not first-slice execution | rewrite | E3 `src/FFTGF.f90` symlink; `src/FFTGF_NR.f90`; `scripts/build.sh` `FFT_BACKEND=NR`; DEP-014. E1 oracle.md: NR compiled, unused by driver. E2 owner: licensing not a POC constraint. |
| IMP-025 | INTEL CONFIDENTIAL MKL headers vs a later math provider. `MATRIX` includes the LAPACK header even when the probe links OpenBLAS. Must not copy headers unless the owner accepts that risk. | critical | XP-171–XP-176 (compile-time include); XP-231 if MKL FFT selected; not first-slice execution | adapter | E3 `src/MATRIX.f90:10`; `src/mkl_dfti.f90:1-17`; `src/mkl_lapack.fi:1-17`; DEP-012. E2 owner: licensing not a POC constraint. |
| IMP-026 | BLAS/LAPACK Fortran column-major ABI vs C# / native adapter. | high | XP-171–XP-179; not first slice | adapter | E3 `src/MATRIX.f90`; `include/sfmake.inc:7-22`; `scripts/build.sh` OpenBLAS; DEP-009–DEP-011. E1 oracle.md: OpenBLAS linked, LAPACK not exercised by the driver. |
| IMP-027 | FFTPACK wrapper calls `zffti`/`zfftf`/`zfftb` with no in-tree callees (DEP-015 `no-route`). | high | XP-232 only | adapter (to a real FFT; this wrapper cannot be preserved as-is) | E3 `src/FFTGF_FFTPACK.f90`; DEP-015; path inventory appendix |
| IMP-028 | FFTW3 Fortran-77 include + GPL static-link impact vs a C# FFT. | high | FFT XPs when `FFT_BACKEND=FFTW3`; not first slice | adapter | E3 `src/FFTGF_FFTW3.f90`; `include/sfmake.inc:28-32`; DEP-013; IMP-009 |
| IMP-029 | NR-derived `polint`/`locate`, Brent `zbrent`, Broyden `broydn` with no complete license grant. | critical | XP-089–XP-091, XP-128, XP-180, XP-186; not first slice | rewrite | E3 `src/BRENT.f90`; `src/BROYDEN.f90`; `src/spline_nr_mod.f90`; `src/INTEGRATE.f90` local `polint`; DEP-020. E2 owner: licensing not a POC constraint. |
| IMP-030 | Zhang/Jin file-scope special-function pack plus assumed-size dummies; acknowledgment-copyright. Live `wfun`/`zerf` use separate wofz/zerf includes. `fermi` is not this pack. | high | XP-226 (unknown reachability); XP-112, XP-113; not first slice | rewrite | E3 `src/FUNCTIONS.f90:171-189,284-302`; `src/functions_special_funcs.f90`; DEP-018 |
| IMP-031 | Vendored QUADPACK / MINPACK snapshots (GOTO-heavy; MINPACK LGPL notices) vs C# quadrature/nonlinear solvers. | medium | XP-121–XP-124, XP-092, XP-093; not first slice | rewrite | E3 `src/integrate_d_quadpack.f90`; `src/WRAP_MINPACK.f90`; `src/minpack.f90`; DEP-016, DEP-017 |
| IMP-032 | MPI-named shared state without `USE MPI` vs omitting rank/size on HTTP services. | medium | XP-003–XP-008, XP-060–XP-061 diagnostics; not first-slice happy path | rewrite | E3 `src/COMVARS.f90:61-62`; DEP-029 |
| IMP-033 | ifort `-openmp` and `omp_*` globals with no `$OMP` / `omp_get_*` uses found. | low | none observed calling OpenMP APIs | preserve (leave unused) until a DEC adds parallelism | E3 `include/options.inc:2`; `src/COMVARS.f90:67`; DEP-028 |
| IMP-034 | Umbrella `SCIFOR` omits compiled `SQUARE_LATTICE`, list modules, `PADE`, `VECTORS`, `SPLINE_FINTER_MOD`. HTTP first slice is four named operations, not the umbrella. | medium | XP-129–XP-170, XP-186, XP-207 vs XP-001 consumers of `SCIFOR` | rewrite (API surface) | E3 `src/SCIFOR.f90:1-16`; path inventory notes. E2 owner: first slice is four operations as services. |
| IMP-035 | Dummy-shaped automatic results (`array(num)`, `df(size(f))`) vs JSON arrays / `double[]`. **Binds first-slice return shape.** | medium | XP-062–XP-067, XP-074 (first slice); `fidelity/driver.f90` copies into `allocatable` | rewrite | E3 `src/tools_grids.f90:2,33`; `src/TOOLS.f90:176-178`; `fidelity/driver.f90:7,12-16` |

## 4. Fortran family notes

This section only records constructs **present in this tree** and the `IMP-NNN` rows they became. It is not a generic Fortran checklist.

**`COMMON` / global state.** No `COMMON` blocks. Shared mutable state is module-level (`MPIID`, TIMER `SAVE` arrays, `store_size`, `cmd_var`, interpolant tables) → **IMP-007**. Procedure `SAVE` in `nrand` → **IMP-008**.

**`EQUIVALENCE` / storage aliasing.** No `EQUIVALENCE`. Not an IMP row. Complex FFT packing uses explicit `real`/`aimag` copies into a `real(8)` work array (`src/FFTGF_NR.f90` `four1`), which is ordinary assignment, not storage association.

**Implicit typing.** Host modules used by the first slice (`TOOLS`, `FUNCTIONS`) have `implicit none`. Many `INCLUDE` fragments do not, and inherit the host. `IOTOOLS.f90` (USE-only), MKL compile units, and `fftw3.f` lack `implicit none` → **IMP-005**. The Zhang/Jin pack is file-scope before `MODULE FUNCTIONS` and carries its own `implicit none` per routine.

**Column-major arrays.** Fortran default; rank-2 SLPLOT and LAPACK calls depend on it → **IMP-003**. First-slice APIs are rank-1.

**Unstructured `GOTO`.** Present in QUADPACK, MINPACK, NR `rfour1`, cubspl, wofz/zerf, `sort`, and private `fastsearchreal`. Absent from `linspace` / `logspace` / `fermi` / `deriv` → **IMP-004**.

**Fixed-form / formatted I/O.** Source is free-form `.f90` except FFTW's fixed-form `src/fftw3.f` → **IMP-009**. Formatted (not fixed-form) writes `es24.17`, list-directed `write(*,*)`, and `f12.4` → **IMP-006**.

**`real*4` / `real*8` numeric semantics.** The tree uses `real(8)` / `complex(8)` / `1.d0`, not `real*8` syntax. Same kind-8 IEEE-like double on the T1 gfortran pin; analogue is `System.Double`. `real(4)` islands exist. `ddp=16` is unused → **IMP-001**.

**Other Fortran constructs that change a porting choice (also in §3):** 1-based indexing **IMP-002**; optional `present()` **IMP-012**; `INCLUDE` composition **IMP-014**; generics/operators **IMP-015**; `pointer` types **IMP-016**; non-default bounds **IMP-017**; `SYSTEM` **IMP-018**; F2003 argv **IMP-021**; dummy-shaped results **IMP-035**; library ABI vs HTTP/JSON **IMP-010**.

## 9. Open impedance questions

These are Architect/test-plan items. They do not leave the target stack unnamed.

- [ ] **ADR (HTTP contract):** What are the route templates, JSON field names, and invalid-input status codes for XP-062 / XP-063 / XP-110 / XP-074? Does not change rewrite vs wrap (**IMP-010**, **IMP-012**, **IMP-013**, **IMP-035**).
- [ ] **Path test plans:** Numeric bound (abs/rel/ulp) per XP. Owner already required parsed numbers and per-path tolerance; the number itself is not this document (**IMP-001**, **IMP-006**).
- [ ] **Later-slice provider ADRs:** Which BLAS/LAPACK/FFT package, if those slices start (DEP-009–DEP-015 / **IMP-024**–**IMP-028**). Does not bind the first slice.

## Errata

None. This file supersedes v1 Draft (target stack was TBD). v1 was Draft, not Snapshot, so it is replaced in place.

## Links

- Path inventory: `docs/modernization/execution-path-inventory.md` (v1 Snapshot)
- Dependency inventory: `docs/modernization/dependency-inventory.md` (v2 Snapshot, DEP-001..DEP-029)
- Dependency graph: `docs/modernization/dependency-graph.md` (v1 Snapshot)
- Decision register: `docs/modernization/decision-register.md` (not written; `/record-decision` assigns DEC-NNN)
- Oracle: `docs/modernization/oracle.md` (v1 Snapshot, bounded T1)

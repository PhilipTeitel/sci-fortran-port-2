<!-- Dependency inventory contract:
- External dependencies only: runtime libraries, native libraries, build tools, vendored code, OS tools, data stores, network services.
- Disposition is exactly one of: available, reimplementable, undecided, no-route. Never "blocked".
- undecided = owner decision needed. no-route = no known replacement. They are not the same.
- Dispositions are snapshot classifications. Later substitution choices are DEC-NNN rows, not cell edits.
- One evidence grade per row.
- After Status: Snapshot, append Errata or produce vN+1. Do not rewrite cells to reflect decisions.
- Omit unused category sections. Do not write N/A rows.
-->

# Dependency Inventory

**Legacy repo:** `/Users/philipteitel/code/ADD-migrations/sci-fortran-legacy` (`e586903a26cc50ca8942f20ca3bccbd8814e6252`)
**Source stack:** Fortran 90/95 free-form modules with Fortran 2003 command-line intrinsics; static library `libscifor.a`; no application framework. Merged build defaults to GNU Fortran; checked-in flags also name Intel Fortran Classic (`ifort`).
**Target stack:** TBD (this project has no `.cursor/workflow.config.yml`; user-profile `modernization.targetStack` is TBD)
**Version:** v1
**Status:** Draft
**Date:** 2026-08-22
**Owner:** Migration Strategist (`/inventory-dependencies`)

## Summary

This inventory lists **29** external dependencies of SciFor (cloned from `https://github.com/liangjj/SciFortran.git`) as they appear in the legacy tree and merged build/fidelity scripts. Dispositions assume a **language port to a managed class library**, not a drop-in Fortran `.mod` / `libscifor.a` replacement; if that assumption is wrong, several `available` rows must be reclassified. **One** row is `no-route`: the FFTPACK adapter (`DEP-015`) calls `zffti` / `zfftf` / `zfftb` with no implementation in this checkout — that binds only an FFTPACK-selected FFT path, not a plausible first numeric slice (grids, Fermi, derivatives). **Eight** rows are `undecided` and need `/record-decision` before they can be used as slice prerequisites: target language/framework, Fortran ABI compatibility, Intel MKL versus other math providers, the default Numerical Recipes FFT and NR-derived solvers/splines, the Zhang/Jin special-function surface, unused CHRPACK, and OpenMP intent. Binding replacements still need an ADR; this table does not choose packages. Next: `/map-dependency-graph` after the owner confirms the target stack, or `/record-decision` on the questions below.

---

## 1. Counts

| Disposition | Count | Notes |
|-------------|-------|-------|
| available | 15 | Credible equivalents exist on a managed numeric stack. Not an approval of a named package. |
| reimplementable | 5 | Bounded algorithms or host-shell effects that can be rewritten behind adapters. |
| undecided | 8 | Target stack, ABI scope, MKL, NR-derived FFT/solvers, special-function subset, CHRPACK, OpenMP. |
| no-route | 1 | `DEP-015` FFTPACK callees absent; binds the FFTPACK backend only. |

## 2. Inventory

| ID | Dependency | Version | License | Support | Disposition | Decision needed | Evidence |
|----|------------|---------|---------|---------|-------------|-----------------|----------|
| DEP-001 | GNU Fortran (`gfortran`) and `libgfortran` | Host probe has Homebrew GCC 16.1.0; legacy-required version unknown | GCC GPLv3 with runtime-library exception (not vendored) | supported | available | none | E3 `scripts/build.sh` (default `FC=gfortran`, links `-lgfortran`); `bin/setup_sf.sh` offers gfortran; `include/options.inc` gfortran flags |
| DEP-002 | Intel Fortran Classic (`ifort`) and Intel runtime | Checked-in comments name ifort 10/11/12-era MKL linkage; exact version unknown | Proprietary Intel toolchain | unsupported (`ifort` discontinued after 2024.2) | undecided | Is ifort-built numeric behavior part of the oracle, or is gfortran the only accepted compiler? | E3 `include/options.inc` (`ifeq ($(FC),ifort)`); `etc/library.conf` `export FC=ifort`; `include/sfmake.inc` MKL ifort comments |
| DEP-003 | Fortran module / static-library ABI (`.mod`, `libscifor.a`) | Compiler-specific; unknown | Repository has no LICENSE | unknown | undecided | Must the port remain a drop-in Fortran consumer ABI, or is a managed API the product boundary? | E3 `src/Makefile` builds `libscifor.a` / `libscifor_deb.a` and rsyncs `.mod` to `$(SFINCLUDE)`; `etc/Makefile.template` consumer link line |
| DEP-004 | GNU Make | unknown | GPL (external tool) | supported | available | none | E3 `src/Makefile`; `numutils/src/Makefile`; `scripts/build.sh` `require_cmd make` |
| DEP-005 | Archive and install tools (`ar`, `ranlib`, `rsync`) | unknown | System / GPL tools | supported on Unix-like hosts | available | none | E3 `src/Makefile` `ar cvq`, `ranlib`, `rsync -avP mods/*.mod` |
| DEP-006 | Git revision stamping | unknown; `src/scifor_version.inc` embeds `722cde3…` while HEAD is `e586903` | GPLv2 (external tool) | supported | available | none | E3 `src/Makefile` `REV = $(shell git rev-parse HEAD)` writes `scifor_version.inc`; `src/COMVARS.f90` includes it |
| DEP-007 | Bash install, env, and probe scripts | unknown | GPLv3 (external shell); scripts unlicensed | supported; interactive `setup_sf.sh` is not the merged path | available | none | E3 `scripts/build.sh`, `scripts/fidelity.sh`, `bin/sciforvars.sh`, `bin/setup_sf.sh` |
| DEP-008 | Python 3 fidelity comparison tooling | unpinned | Python license; stdlib only in the script | supported | available | none | E3 `scripts/fidelity.sh` uses `python3` to parse sections, compare numerics, and bootstrap four formula goldens |
| DEP-009 | BLAS (Netlib interface) | unknown | Depends on provider | unknown (interface in use; provider unverified here) | available | none (provider choice is a later DEC/ADR citing DEP-009–DEP-012) | E3 `src/MATRIX.f90` is a LAPACK-facing module on a BLAS-capable link; `include/sfmake.inc` `-lblas` or `-lopenblas` |
| DEP-010 | LAPACK | unknown | Depends on provider | unknown | available | none (same provider DEC as DEP-009) | E3 `src/MATRIX.f90` calls `dsyev`, `zheev`, `dgetrf`/`dgetrs`/`dgetri`, `zgetrf`, `dtrtri`; includes `mkl_lapack.fi` |
| DEP-011 | OpenBLAS (Darwin / merged-probe provider) | Homebrew package version unknown | BSD-3-Clause upstream | supported upstream | available | none (not an approval that OpenBLAS is the product provider) | E3 `scripts/build.sh` sets `SFBLAS` to Homebrew OpenBLAS and links `-lopenblas`; `include/sfmake.inc` Darwin non-MKL `MATHLIB = -L$(SFBLAS) -lopenblas` |
| DEP-012 | Intel MKL (BLAS/LAPACK/DFTI) plus vendored interface files | Runtime unknown; headers copyright 1999–2010 | Proprietary; `src/mkl_dfti.f90`, `src/mkl_trig_transforms.f90`, `src/mkl_lapack.fi` are marked INTEL CONFIDENTIAL with no license grant | supported as a product; checked-in headers must not be copied into a target tree | undecided | Use licensed MKL (without copying these headers), substitute another provider, or drop MKL-only paths? | E3 `include/sfmake.inc` `ifdef MKLROOT`; `src/FFTGF_MKL.f90`; confidential banners in `src/mkl_dfti.f90`, `src/mkl_lapack.fi` |
| DEP-013 | FFTW3 (`libfftw3` + `fftw3.f`) | unknown; `etc/library.conf` has `SFFFTW3` commented | GPL (FFTW); static-link impact needs legal review | supported upstream; not the checked-in default | available | none (FFT backend choice is a later DEC citing DEP-013–DEP-015) | E3 `src/FFTGF_FFTW3.f90` `include "fftw3.f"` and `dfftw_plan_dft_1d`; `include/sfmake.inc` `ifdef SFFFTW3` `-lfftw3`; `scripts/build.sh` `FFT_BACKEND=FFTW3` |
| DEP-014 | Numerical Recipes-derived radix-2 FFT (`four1`, `realft`, `cosft2`) | Local snapshot; provenance unstated in file | No license grant in tree; NR is proprietary | unknown | undecided | Replace with a licensed FFT, reimplement, or drop FFT paths? Copying this source into the target is not a route. | E3 tracked `src/FFTGF.f90` → `FFTGF_NR.f90`; comment “original NumRec”; `scripts/build.sh` default `FFT_BACKEND=NR` |
| DEP-015 | FFTPACK adapter (`zffti`, `zfftf`, `zfftb`) | Wrapper only; callee sources absent | unknown | unsupported in this checkout | no-route | none — no in-tree or linked implementation; does not bind NR/FFTW/MKL FFT paths | E3 `src/FFTGF_FFTPACK.f90` calls `zffti`/`zfftf`/`zfftb`; tree search finds no definitions |
| DEP-016 | Vendored QUADPACK (QAG/QAGS family) | Local snapshot; 1983 algorithm references | Not stated in bundled file (classic QUADPACK is public-domain/SLATEC; this snapshot’s aggregate terms unknown) | unknown as a snapshot | available | none | E3 `src/Makefile` compiles `integrate_d_quadpack.f90` into `INTEGRATE`; `src/integrate_d_quadpack.f90` QAG/QAGS |
| DEP-017 | Vendored MINPACK (Burkardt F90) | F77 original (More/Garbow/Hillstrom); F90 conversion modified 2010 | GNU LGPL (per-routine notices) | unknown as a snapshot | available | none | E3 `src/WRAP_MINPACK.f90` `include "minpack.f90"`; `src/minpack.f90` LGPL + MINPACK-1 citation; `src/OPTIMIZE.f90` uses `WRAP_MINPACK` |
| DEP-018 | Zhang/Jin special-function collection (Burkardt F90) | Mixed routine dates; local revision unknown | Per-routine permission to use with copyright acknowledgment; not a single OSI license | unknown | undecided | Which functions are in product scope, and is acknowledgment-copyright acceptable for the port? | E3 `src/FUNCTIONS.f90` lists Airy/Bessel/hypergeometric/etc.; `src/functions_special_funcs.f90` Zhang/Jin copyright block |
| DEP-019 | Burkardt spline / Clenshaw–Curtis interpolation | Local snapshot; routine dates vary | GNU LGPL | unknown as a snapshot | reimplementable | none | E3 `src/SPLINE.f90` includes `spline_interp.f90`; LGPL notices in `src/spline_interp.f90` |
| DEP-020 | Numerical Recipes-derived helpers (`polint`/`locate`, Broyden, Brent `zbrent`) | Local snapshots; versions unknown | No complete license grant recovered | unknown | undecided | Same legal/replace-or-drop question as DEP-014, for OPTIMIZE and SPLINE/INTEGRATE interpolation | E3 `src/spline_nr_mod.f90` `polint`; `src/INTEGRATE.f90` local `polint`/`locate`; `src/BRENT.f90` `zbrent`; `src/BROYDEN.f90` + `broydn_routines.f90`; `src/MATRIX.f90` comment “from nr90” |
| DEP-021 | `cubspl` cubic-spline (de Boor / PGS style) | unknown | Not stated in file; classic SLATEC/de Boor sources are public domain | unknown | reimplementable | none | E3 `src/SPLINE.f90` includes `spline_cubspl_routines.f90`; `cubspl` header describes interpolatory cubic spline |
| DEP-022 | Faddeeva / complex error `WOFZ` | unknown; matches TOMS Algorithm 680 shape | Not stated in file (ACM TOMS 680 typically ACM Software License) | unknown | available | none | E3 `src/FUNCTIONS.f90` `wfun` includes `functions_wofz.f90` and `functions_zerf.f90` |
| DEP-023 | ACM Algorithm 712 `random_normal` (Kinderman–Monahan) | TOMS 18(4) 1992 | ACM Software License (stated in comments) | unknown as a snapshot | reimplementable | none | E3 `src/random_routines.f90` “ALGORITHM 712, COLLECTED ALGORITHMS FROM ACM”; `src/RANDOM.f90` includes those routines |
| DEP-024 | CHRPACK character/format collection | Local snapshot; Burkardt notices dated 2000 | GNU LGPL | unused by current Makefile | undecided | Retire from port scope, or keep? Not on `src/Makefile` `allmod` | E3 `src/CHRPACK.f90` LGPL notices; `src/Makefile` `allmod` does not compile CHRPACK |
| DEP-025 | Gnuplot (generated `.gp` scripts, `wxt` terminal) | unknown | Gnuplot license (external) | supported upstream | available | none (scope of plotting is a product DEC, not a missing route) | E3 `src/slplot_splot_3d.f90` writes `gnuplot -persist` scripts and `call system("chmod +x …")`; `numutils/src/splot.f90` documents gnuplot scripts |
| DEP-026 | GNU `libmatheval` (`evaluator_create_` / `evaluator_evaluate_x_`) | Linked version unknown; upstream last release 1.1.11 (2008) | GPLv3 | unsupported / unmaintained | available | none | E3 `numutils/src/func.f90` calls `evaluator_create_` / `evaluator_evaluate_x_`; `numutils/src/Makefile` `-lmatheval` |
| DEP-027 | POSIX host commands via `SYSTEM` (`gzip`/`gunzip`, `mkdir`, `mv`, `chmod`, bash `if`) | unknown | Varied system licenses | supported on Unix-like hosts | reimplementable | none | E3 `src/IOFILE.f90` gzip/gunzip/mkdir; `src/SQUARE_LATTICE.f90` / `src/TOOLS.f90` mkdir/mv; `src/slplot_splot_3d.f90` chmod |
| DEP-028 | OpenMP compiler flag and `omp_*` globals | unknown; no OpenMP API/directives found | Compiler/runtime dependent | unknown | undecided | Is any parallel runtime in product scope? `ifort -openmp` is set; no `$OMP` / `omp_get_*` use found | E3 `include/options.inc` ifort `OPT` includes `-openmp`; `src/COMVARS.f90` `omp_num_threads`, `omp_id`, `omp_size` |
| DEP-029 | MPI-named shared state (`MPIID`, `MPISIZE`) without MPI library calls | `etc/library.conf` still names MPICH2 1.4.1 paths; no `USE MPI` | MPICH license would apply only if that library were linked (it is not in this checkout) | MPICH2 1.4.1 is obsolete; actual MPI linkage not present | reimplementable | none | E3 `src/COMVARS.f90` defaults `MPISIZE=1`, `MPIID=0`; diagnostics gate on `mpiID`; `etc/library.conf` `SFMPIDIR=/opt/mpich2-1.4.1/intel`; no MPI calls in `src/` |

Commented-out CUDA, FGSL/GSL, and DISLIN/`DLPLOT` blocks in `include/sfmake.inc` / `bin/setup_sf.sh` are not current dependencies and are omitted. No database, network service, message bus, container, or web-framework dependency was found.

## 3. Support, license, and vulnerability notes

| DEP ID | Issue | Severity | Evidence |
|--------|-------|----------|----------|
| DEP-002 | `ifort` discontinued after 2024.2; remaining Intel Fortran is `ifx` | high | E3 `include/options.inc`; E2 Intel Fortran Classic deprecation |
| DEP-012 | Vendored MKL headers are INTEL CONFIDENTIAL; no license to copy, modify, or distribute | critical | E3 `src/mkl_dfti.f90` lines 1–17; `src/mkl_lapack.fi` lines 1–17 |
| DEP-014 | Numerical Recipes-derived FFT has no license grant; copying into a target tree is not a legal route | critical | E3 `src/FFTGF_NR.f90` four1/realft “NumRec” comments |
| DEP-020 | Same NR provenance for Broyden/Brent/`polint` | critical | E3 `src/BRENT.f90`, `src/BROYDEN.f90`, `src/spline_nr_mod.f90` |
| DEP-013 | FFTW3 is GPL; static linking into a proprietary target needs legal review | high | E3 `include/sfmake.inc` `-lfftw3`; E2 FFTW license |
| DEP-017 | MINPACK snapshot is LGPL; aggregate repo has no LICENSE/notice file | medium | E3 `src/minpack.f90` LGPL blocks |
| DEP-018 | Zhang/Jin copyright requires acknowledgment; not a standard redistribution license | high | E3 `src/functions_special_funcs.f90` licensing block |
| DEP-026 | `libmatheval` unmaintained (last upstream 2008) | medium | E3 `numutils/src/Makefile`; E2 FSF/GNU libmatheval record |
| DEP-015 | FFTPACK backend cannot be built from this tree | high | E3 `src/FFTGF_FFTPACK.f90` vs missing callees |

## 4. Open inventory questions

- [ ] What is the **target language, version, and framework** for this repository? (Profile is TBD. A sibling experiment used C# / .NET 8; that is not binding here. ASP.NET Core is not implied by this library.)
- [ ] Is a **drop-in Fortran ABI** (`.mod` / `libscifor.a`) required? If yes, DEP-003 and several managed-stack `available` rows need reclassification.
- [ ] Which **BLAS/LAPACK provider** is accepted for parity (OpenBLAS probe default, licensed MKL, another native lib, or a managed implementation)?
- [ ] Which **FFT backend** is in product scope (NR default vs FFTW3 vs MKL vs drop)? FFTPACK has no route in this checkout.
- [ ] May **NR-derived**, **Intel-confidential**, **Zhang/Jin**, and **mixed-LGPL** sources appear in the target tree, or must those surfaces be replaced or dropped?
- [ ] Is **gnuplot / CLI `func` / CHRPACK** in product scope? Missing them does not bind a library-core first slice.

## Errata

None.

## Links

- Dependency graph: `docs/modernization/dependency-graph.md`
- Decision register: `docs/modernization/decision-register.md`
- Impedance analysis: `docs/modernization/impedance-analysis.md`

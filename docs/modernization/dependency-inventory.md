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
**Target stack:** C# / .NET 8 / ASP.NET Core (HTTP/JSON services). Product boundary is a rewritten service API, not a Fortran `.mod` / `libscifor.a` drop-in. First-slice surface: `linspace`, `logspace`, `fermi`, `deriv`.
**Version:** v2
**Status:** Snapshot
**Date:** 2026-08-22
**Owner:** Migration Strategist (`/inventory-dependencies`)

## Summary

This inventory lists **29** external dependencies of SciFor (cloned from `https://github.com/liangjj/SciFortran.git`) against **C# / .NET 8 / ASP.NET Core HTTP/JSON**. **Zero** rows are `undecided`. **One** row is `no-route`: FFTPACK callees (`DEP-015`) — that binds only an FFTPACK-selected FFT path, not the first slice (XP-062, XP-063, XP-110, XP-074 and oracle harness XP-223–XP-225). First-slice library calls have no BLAS/LAPACK/FFT/MKL/gnuplot/`SYSTEM`/CLI-parser route requirement; they need a C# numeric rewrite plus an HTTP/JSON host (DEP-003). Owner policy for this POC: license risk is not a constraint; the Mac gfortran 16.1.0 / OpenBLAS T1 pin is the oracle. Named NuGet packages are not approved here — ADRs own substitutions. Next: `/map-dependency-graph`.

---

## 1. Counts

| Disposition | Count | Notes |
|-------------|-------|-------|
| available | 18 | Credible C# / .NET 8 or host-tool equivalent, or the dependency is legacy-oracle-only and not required on the product runtime. |
| reimplementable | 10 | Bounded algorithms or host effects to rewrite behind C# / HTTP adapters. Includes the Fortran ABI (DEP-003) as HTTP/JSON. |
| undecided | 0 | Owner answers for target stack, ABI, oracle pin, and POC license policy are recorded as inventory inputs, not as open dispositions. |
| no-route | 1 | `DEP-015` FFTPACK callees absent; does not bind the first slice. |

## 2. Inventory

| ID | Dependency | Version | License | Support | Disposition | Decision needed | Evidence |
|----|------------|---------|---------|---------|-------------|-----------------|----------|
| DEP-001 | GNU Fortran (`gfortran`) and `libgfortran` | Homebrew GCC 16.1.0 on the T1 host; legacy-required version unknown | GCC GPLv3 with runtime-library exception (not vendored) | supported | available | none — oracle compiler only; not a C# product runtime | E3 `scripts/build.sh`; `include/options.inc` gfortran flags. E1 `docs/modernization/oracle.md` T1 pin. E2 owner: Mac gfortran pin is the product oracle. |
| DEP-002 | Intel Fortran Classic (`ifort`) and Intel runtime | Comments name ifort 10/11/12-era MKL linkage; exact version unknown | Proprietary Intel toolchain | unsupported (`ifort` discontinued after 2024.2) | available | none — not the oracle; not on the C# product runtime | E3 `include/options.inc` `ifeq ($(FC),ifort)`; `etc/library.conf`. E2 owner: gfortran T1 pin is sufficient. |
| DEP-003 | Fortran module / static-library ABI (`.mod`, `libscifor.a`) | Compiler-specific | Repository has no LICENSE | unknown as a Fortran ABI | reimplementable | none — product is C# HTTP/JSON services, not a Fortran drop-in. Binding host/API shape still needs an Architect ADR, not a disposition change. | E3 `src/Makefile:8-12`; `etc/Makefile.template`. E2 owner: C# API as services, HTTP/JSON, rewrite first slice. |
| DEP-004 | GNU Make | unknown | GPL (external tool) | supported | available | none — legacy/oracle build only | E3 `src/Makefile`; `scripts/build.sh` `require_cmd make` |
| DEP-005 | Archive and install tools (`ar`, `ranlib`, `rsync`) | unknown | System / GPL tools | supported on Unix-like hosts | available | none — legacy/oracle packaging only | E3 `src/Makefile` `ar cvq`, `ranlib`, `rsync` |
| DEP-006 | Git revision stamping | unknown; `src/scifor_version.inc` embeds `722cde3…` while HEAD is `e586903` | GPLv2 (external tool) | supported | reimplementable | none — not first-slice numeric services | E3 `src/Makefile` `REV = $(shell git rev-parse HEAD)`; `src/COMVARS.f90` |
| DEP-007 | Bash install, env, and probe scripts | unknown | GPLv3 (external shell); scripts unlicensed | supported; interactive `setup_sf.sh` is not the merged path | available | none — oracle/legacy scripts; C# host is ASP.NET Core | E3 `scripts/build.sh`, `scripts/fidelity.sh`, `bin/sciforvars.sh`, `bin/setup_sf.sh` |
| DEP-008 | Python 3 fidelity comparison tooling | unpinned | Python license; stdlib only in the script | supported | available | none — oracle self-check, not the C# comparison rule | E3 `scripts/fidelity.sh`; E1 oracle.md: PASS vs rewritten goldens is not the path comparison rule |
| DEP-009 | BLAS (Netlib interface) | unknown | Depends on provider | unknown (interface in use; provider unverified here) | available | none for first slice (XP-062/063/110/074 do not call BLAS). Later provider is an ADR citing DEP-009–DEP-012. | E3 `src/MATRIX.f90`; `include/sfmake.inc` |
| DEP-010 | LAPACK | unknown | Depends on provider | unknown | available | none for first slice | E3 `src/MATRIX.f90` `dsyev`/`zheev`/`dgetrf` family; includes `mkl_lapack.fi` |
| DEP-011 | OpenBLAS (Darwin / T1 provider) | Homebrew 0.3.34 on the T1 host | BSD-3-Clause upstream | supported upstream | available | none — T1 link pin; not exercised by first-slice library math | E1 oracle.md OpenBLAS 0.3.34; E3 `scripts/build.sh` `-lopenblas` |
| DEP-012 | Intel MKL (BLAS/LAPACK/DFTI) plus vendored interface files | Runtime unknown; headers copyright 1999–2010 | Proprietary; `src/mkl_dfti.f90`, `src/mkl_trig_transforms.f90`, `src/mkl_lapack.fi` INTEL CONFIDENTIAL | supported as a product; headers must not be copied unless the owner accepts that risk | available | none for this POC (owner: license not a constraint) and none for first slice (MKL not executed). Later-slice provider still needs an ADR. | E3 confidential banners; `src/MATRIX.f90:10`. E2 owner: licensing not an issue for the POC. |
| DEP-013 | FFTW3 (`libfftw3` + `fftw3.f`) | unknown | GPL (FFTW); static-link impact needs legal review on a production target | supported upstream; not the checked-in default | available | none for first slice | E3 `src/FFTGF_FFTW3.f90`; `include/sfmake.inc` `-lfftw3` |
| DEP-014 | Numerical Recipes-derived radix-2 FFT (`four1`, `realft`, `cosft2`) | Local snapshot | No license grant in tree; NR is proprietary | unknown | reimplementable | none for first slice (compiled, not called by the driver). POC owner accepts license risk if a later slice copies or wraps it; a licensed rewrite remains the clean route. | E3 `src/FFTGF_NR.f90`; `scripts/build.sh` `FFT_BACKEND=NR`. E1 oracle.md: NR compiled, unused by driver. E2 owner: licensing not an issue for the POC. |
| DEP-015 | FFTPACK adapter (`zffti`, `zfftf`, `zfftb`) | Wrapper only; callee sources absent | unknown | unsupported in this checkout | no-route | none — no in-tree or linked implementation; does not bind NR/FFTW/MKL FFT paths or the first slice | E3 `src/FFTGF_FFTPACK.f90`; tree search finds no definitions |
| DEP-016 | Vendored QUADPACK (QAG/QAGS family) | Local snapshot; 1983 algorithm references | Not stated in bundled file (classic QUADPACK is public-domain/SLATEC; this snapshot’s aggregate terms unknown) | unknown as a snapshot | reimplementable | none for first slice | E3 `src/integrate_d_quadpack.f90` |
| DEP-017 | Vendored MINPACK (Burkardt F90) | F77 original; F90 conversion modified 2010 | GNU LGPL (per-routine notices) | unknown as a snapshot | reimplementable | none for first slice | E3 `src/WRAP_MINPACK.f90`; `src/minpack.f90` LGPL notices |
| DEP-018 | Zhang/Jin special-function collection (Burkardt F90) | Mixed routine dates | Per-routine permission with copyright acknowledgment | unknown | reimplementable | none for first slice (`fermi` is not this pack). Later slice must name which functions ship. | E3 `src/FUNCTIONS.f90`; `src/functions_special_funcs.f90`. E3 `src/FUNCTIONS.f90:240-248` `fermi`. |
| DEP-019 | Burkardt spline / Clenshaw–Curtis interpolation | Local snapshot | GNU LGPL | unknown as a snapshot | reimplementable | none for first slice | E3 `src/SPLINE.f90`; `src/spline_interp.f90` |
| DEP-020 | Numerical Recipes-derived helpers (`polint`/`locate`, Broyden, Brent `zbrent`) | Local snapshots | No complete license grant recovered | unknown | reimplementable | none for first slice. Same POC license policy as DEP-014. | E3 `src/BRENT.f90`; `src/BROYDEN.f90`; `src/spline_nr_mod.f90`; `src/INTEGRATE.f90`. E2 owner: licensing not an issue for the POC. |
| DEP-021 | `cubspl` cubic-spline (de Boor / PGS style) | unknown | Not stated in file | unknown | reimplementable | none for first slice | E3 `src/spline_cubspl_routines.f90` |
| DEP-022 | Faddeeva / complex error `WOFZ` | unknown; TOMS Algorithm 680 shape | Not stated in file | unknown | reimplementable | none for first slice | E3 `src/functions_wofz.f90`; `src/functions_zerf.f90` |
| DEP-023 | ACM Algorithm 712 `random_normal` (Kinderman–Monahan) | TOMS 18(4) 1992 | ACM Software License (stated in comments) | unknown as a snapshot | reimplementable | none for first slice | E3 `src/random_routines.f90`; `src/RANDOM.f90` |
| DEP-024 | CHRPACK character/format collection | Local snapshot; Burkardt notices dated 2000 | GNU LGPL | unused by current Makefile | available | none — not on `src/Makefile` `allmod`; omit from first slice and default port surface | E3 `src/CHRPACK.f90`; `src/Makefile` `allmod` |
| DEP-025 | Gnuplot (generated `.gp` scripts, `wxt` terminal) | unknown | Gnuplot license (external) | supported upstream | available | none for first slice (owner first slice is four numeric HTTP services) | E3 `src/slplot_splot_3d.f90`; `numutils/src/splot.f90`. E2 owner: first slice is linspace/logspace/fermi/deriv as services. |
| DEP-026 | GNU `libmatheval` (`evaluator_create_` / `evaluator_evaluate_x_`) | Linked version unknown; upstream last release 1.1.11 (2008) | GPLv3 | unsupported / unmaintained | reimplementable | none for first slice | E3 `numutils/src/func.f90`; `numutils/src/Makefile` `-lmatheval` |
| DEP-027 | POSIX host commands via `SYSTEM` (`gzip`/`gunzip`, `mkdir`, `mv`, `chmod`, bash `if`) | unknown | Varied system licenses | supported on Unix-like hosts | reimplementable | none for first slice | E3 `src/IOFILE.f90`; `src/TOOLS.f90:273-274`; `src/SQUARE_LATTICE.f90`; `src/slplot_splot_3d.f90` |
| DEP-028 | OpenMP compiler flag and `omp_*` globals | unknown; no OpenMP API/directives found | Compiler/runtime dependent | unknown | available | none — no `$OMP` / `omp_get_*` use found; not first-slice math | E3 `include/options.inc` ifort `-openmp`; `src/COMVARS.f90` `omp_*` |
| DEP-029 | MPI-named shared state (`MPIID`, `MPISIZE`) without MPI library calls | `etc/library.conf` names MPICH2 1.4.1 paths; no `USE MPI` | MPICH would apply only if linked (it is not) | MPICH2 1.4.1 obsolete; no MPI linkage in this checkout | reimplementable | none — first-slice `error()` may print default `mpiID=0`; no MPI library | E3 `src/COMVARS.f90` defaults `MPISIZE=1`, `MPIID=0`; no MPI calls in `src/` |

Commented-out CUDA, FGSL/GSL, and DISLIN/`DLPLOT` blocks in `include/sfmake.inc` / `bin/setup_sf.sh` are not current dependencies and are omitted. No database, message bus, or legacy HTTP host was found. The **target** HTTP/JSON host is ASP.NET Core on .NET 8 (E2 owner); that is the product runtime, not a legacy DEP row.

## 3. Support, license, and vulnerability notes

| DEP ID | Issue | Severity | Evidence |
|--------|-------|----------|----------|
| DEP-002 | `ifort` discontinued after 2024.2; remaining Intel Fortran is `ifx` | high as a legacy toolchain; not a first-slice product risk | E3 `include/options.inc`; E2 Intel Fortran Classic deprecation |
| DEP-012 | Vendored MKL headers are INTEL CONFIDENTIAL | critical if redistributed; owner accepted POC license risk | E3 `src/mkl_dfti.f90:1-17`; `src/mkl_lapack.fi:1-17`. E2 owner. |
| DEP-014 | NR-derived FFT has no license grant | critical if redistributed as a product; owner accepted POC license risk | E3 `src/FFTGF_NR.f90` NumRec comments. E2 owner. |
| DEP-020 | Same NR provenance for Broyden/Brent/`polint` | critical if redistributed as a product; owner accepted POC license risk | E3 `src/BRENT.f90`, `src/BROYDEN.f90`, `src/spline_nr_mod.f90`. E2 owner. |
| DEP-013 | FFTW3 is GPL; static linking into a proprietary product needs legal review | high for a production target; not first slice | E3 `include/sfmake.inc` `-lfftw3` |
| DEP-017 | MINPACK snapshot is LGPL; aggregate repo has no LICENSE/notice file | medium | E3 `src/minpack.f90` LGPL blocks |
| DEP-018 | Zhang/Jin copyright requires acknowledgment | high for those later functions; not `fermi` | E3 `src/functions_special_funcs.f90` |
| DEP-026 | `libmatheval` unmaintained (last upstream 2008) | medium; not first slice | E3 `numutils/src/Makefile` |
| DEP-015 | FFTPACK backend cannot be built from this tree | high for FFTPACK-selected paths only | E3 `src/FFTGF_FFTPACK.f90` vs missing callees |

## 4. Open inventory questions

None. Every row has a disposition. Later-slice package names (which BLAS, which FFT, which JSON host library) are ADR work after M1, not missing dispositions.

## Errata

None. This file supersedes v1 Draft (target stack was TBD; eight `undecided` rows). v1 is replaced in place because it was Draft, not Snapshot.

## Links

- Dependency graph: `docs/modernization/dependency-graph.md`
- Decision register: `docs/modernization/decision-register.md`
- Impedance analysis: `docs/modernization/impedance-analysis.md`

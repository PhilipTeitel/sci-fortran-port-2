# Dependency Inventory

**Legacy repo:** `/Users/philipteitel/code/ADD-migrations/sci-fortran-legacy`
**Source stack:** Fortran 90/95 (free-form; F2003 command-line intrinsics); checkout e586903; merged default gfortran 16.1.0 on T1 host; no application framework; static library `libscifor.a`
**Target stack:** C# / .NET 8 / ASP.NET Core (HTTP/JSON services)
**Version:** v2
**Status:** Snapshot
**Date:** 2026-08-25
**Owner:** Migration Strategist (`/inventory-dependencies`)

## Summary

This snapshot inventories 29 external dependencies of sci-fortran-legacy (checkout e586903). Twenty-seven have a credible C# / .NET 8 route (`available`); two (`DEP-009` Zhang/Jin special functions, `DEP-017` gnuplot) are `reimplementable` for the used subset. None are `undecided` or `no-route`, so no dead-end binds a numeric library-API first slice (linspace/logspace/deriv/fermi are XP-060, XP-061, XP-072, XP-108 and do not use `DEP-029`). v2 adds `DEP-029` Alan Miller `dcerf` (XP-111 `FUNCTIONS::zerf`); it is not `DEP-013` WOFZ and not `DEP-028` ACM 712 random. Dispositions classify routes only: they do not choose Math.NET, MKL.NET, or any other package, and they do not approve copying Numerical Recipes or Intel Confidential headers into the port. Fortran ABI wrapping of `libscifor.a` is not the product; Makefile/installer/CI scripts are listed because they exist, not because they become HTTP services. Look next at graph v2 (or graph Errata for XP-111); substitution choices are `DEC-NNN` via `/record-decision`, and binding replacements still need Architect ADRs.

---

## 1. Counts

| Disposition | Count | Notes |
|-------------|-------|-------|
| available | 27 | Target BCL, ASP.NET Core, or a well-known .NET numeric stack can cover the used behavior. Package choice is not approved here. |
| reimplementable | 2 | `DEP-009` (Zhang/Jin corpus has no complete .NET equivalent); `DEP-017` (gnuplot host is not an ASP.NET service; emit plottable data instead). |
| undecided | 0 | No owner question is required to assign a disposition. |
| no-route | 0 | Nothing in this tree lacks a known replacement. Missing FFTPACK/diagPAM sources bind only those appendix paths. |

## 2. Inventory

| ID | Dependency | Version | License | Support | Disposition | Decision needed | Evidence |
|----|------------|---------|---------|---------|-------------|-----------------|----------|
| DEP-001 | GNU Fortran compiler/runtime (gfortran; T1 `scripts/build.sh` default; fidelity links `-lgfortran`) | 16.1.0 (Homebrew GCC on T1 host) | FSF free software (`gfortran --version` notice) | supported | available | none | E1 `gfortran --version` → GNU Fortran (Homebrew GCC 16.1.0) 16.1.0 |
| DEP-002 | Intel Fortran (ifort) | unknown (classic ifort; `etc/library.conf` default `FC`) | proprietary (Intel) | unknown | available | none | E3 `etc/library.conf:1`; `include/options.inc:1-7` |
| DEP-003 | BLAS/LAPACK (Darwin default OpenBLAS; Linux `-llapack -lblas`; MKL when `MKLROOT` set) | unknown | OpenBLAS BSD; netlib BLAS/LAPACK BSD-style | supported | available | none | E3 `include/sfmake.inc:7-21`; `scripts/build.sh:33-35,74-76` |
| DEP-004 | Intel MKL (optional `MATHLIB` and `FFT_BACKEND=MKL`; vendored Intel Confidential Fortran interfaces) | unknown (headers copyright 1999–2010) | Intel Confidential (vendored `mkl_lapack.fi`, `mkl_dfti.f90`, `mkl_trig_transforms.f90`); MKL runtime proprietary | supported (oneAPI MKL); vendored headers are not a redistributable SDK | available | none | E3 `include/sfmake.inc:7-15`; `src/MATRIX.f90:10`; `src/FFTGF_MKL.f90:1-6`; `scripts/build.sh:50-62`; `src/mkl_lapack.fi:1-18` |
| DEP-005 | FFTW3 (optional `SFFFTW3` / `FFT_BACKEND=FFTW3`; vendored `src/fftw3.f` constants) | unknown | unknown in this tree | supported | available | none | E3 `include/sfmake.inc:28-33`; `src/FFTGF_FFTW3.f90:3,20-22`; `src/fftw3.f:1-25`; `scripts/build.sh:50-62` |
| DEP-006 | Numerical Recipes algorithms (vendored: `four1`/`cosft2`/`realft`, Broyden helpers, `zbrent`, `SPLINE_NR_MOD` `locate`/`polint`) | unknown | proprietary (Numerical Recipes; not licensed in this tree) | unsupported | available | none | E3 `src/FFTGF_NR.f90:11,424-427`; `src/BROYDEN.f90:1-6`; `src/BRENT.f90:29-46`; `src/spline_nr_mod.f90:1-14`; `src/spline_finter_mod.f90:1-28` |
| DEP-007 | MINPACK (`hybrd1` via `WRAP_MINPACK`; vendored Burkardt F90) | unknown (Burkardt F90 dated 2010 in file header) | GNU LGPL (file header) | unsupported (frozen Netlib/MINPACK-1) | available | none | E3 `src/WRAP_MINPACK.f90:1,20`; `src/minpack.f90:37-55,1383` |
| DEP-008 | QUADPACK (vendored `integrate_d_quadpack.f90`; compiled into `libscifor.a`; `INTEGRATE` public API does not call `qag*`) | unknown | unknown in this tree | unsupported | available | none | E3 `src/Makefile:149-153`; `src/integrate_d_quadpack.f90:1-16`; `src/INTEGRATE.f90:62-70` |
| DEP-009 | Zhang/Jin *Computation of Special Functions* (file-scope procedures compiled with `FUNCTIONS.f90`) | unknown (Burkardt F90 of Zhang/Jin 1996 book code; headers say modified 2012) | copyright Zhang/Jin; permission to incorporate if acknowledged (file headers) | unsupported | reimplementable | none | E3 `src/functions_special_funcs.f90:8-29`; `src/FUNCTIONS.f90:171` |
| DEP-010 | ORDERPACK `uniinv`/`unista` (inlined in `TOOLS`; standalone `src/uniinv.f90` / `src/unista.f90` not in `allmod`) | unknown | unknown in this tree | unknown | available | none | E3 `src/TOOLS.f90:57-69`; `src/uniinv.f90:1-22`; `src/unista.f90:1-13`; `src/Makefile:5` |
| DEP-011 | Burkardt interpolation helpers (`spline_interp.f90`, included by `SPLINE`) | unknown (headers dated 2007) | GNU LGPL (file headers) | unsupported | available | none | E3 `src/SPLINE.f90:1-4`; `src/spline_interp.f90:27-37` |
| DEP-012 | CUBSPL interpolatory cubic spline (`spline_cubspl_routines.f90`, included by `SPLINE`) | unknown | unknown in this tree | unknown | available | none | E3 `src/SPLINE.f90:4`; `src/spline_cubspl_routines.f90:1-8` |
| DEP-013 | WOFZ Faddeeva (`functions_wofz.f90` via `FUNCTIONS::wfun`) | unknown | unknown in this tree | unknown | available | none | E3 `src/FUNCTIONS.f90:188,281-293`; `src/functions_wofz.f90:1-18` |
| DEP-014 | GNU libmatheval (numutils `func` CLI) | unknown | unknown in this tree (linked as `-lmatheval`; not vendored) | unknown | available | none | E3 `numutils/src/Makefile:44-46`; `numutils/src/func.f90:11-15,58-60` |
| DEP-015 | gzip/gunzip (IOFILE `data_store` / `data_open`) | unknown (host POSIX gzip) | unknown in this tree | supported | available | none | E3 `src/IOFILE.f90:219-230,249-266` |
| DEP-016 | POSIX userland via Fortran `SYSTEM` (`mkdir`, `mv`, `chmod`, `rm`; bash dialect) | unknown | unknown (OS utilities) | supported | available | none | E3 `src/IOFILE.f90:289-290`; `src/SQUARE_LATTICE.f90:159-160`; `src/TOOLS.f90:273-274`; `src/slplot_splot_3d.f90:66`; `numutils/src/ffcmplx.f90:52` |
| DEP-017 | gnuplot (SLPLOT / `splot` CLI write `.gp` scripts with `gnuplot -persist`) | unknown | unknown (gnuplot license) | unknown | reimplementable | none | E3 `src/slplot_splot_3d.f90:39-66`; `numutils/src/splot.f90:33-40,155` |
| DEP-018 | Git (`git rev-parse HEAD` baked into `scifor_version.inc` / `revision.inc`) | unknown (build-time CLI) | GPL-2.0 (git) | supported | available | none | E3 `src/Makefile:2,24-25`; `include/sfmake.inc:36-37`; `src/COMVARS.f90:9,112-116` |
| DEP-019 | Python 3 (`scripts/fidelity.sh` numeric compare; not a product service) | unknown (script requires `python3`) | PSF | supported | available | none | E3 `scripts/fidelity.sh:32-68` |
| DEP-020 | GNU Make, rsync, `ar`, `ranlib`, `ln` (library build) | unknown | GPL (Make/rsync); binutils | supported | available | none | E3 `src/Makefile:8-12,67`; `scripts/build.sh:80-81,61` |
| DEP-021 | POSIX bash (build, fidelity, `sciforvars.sh`, `setup_sf.sh`) | unknown | GPL-3.0 (bash) | supported | available | none | E3 `scripts/build.sh:1`; `scripts/fidelity.sh:1`; `bin/sciforvars.sh:1`; `bin/setup_sf.sh:1` |
| DEP-022 | Homebrew (Darwin prefix for gcc/openblas/lapack in `scripts/build.sh`) | unknown | BSD-2-Clause (Homebrew) | supported | available | none | E3 `scripts/build.sh:16-24,31-35,83-84` |
| DEP-023 | MPICH2 (declared in `etc/library.conf`; `COMMON_VARS` MPI stubs `MPIID=0`/`MPISIZE=1`; not linked by `src/Makefile`) | 1.4.1 (path in conf) | unknown in this tree | unsupported | available | none | E3 `etc/library.conf:7-8`; `src/COMVARS.f90:59-62,114`; `src/Makefile` has no `-lmpi` |
| DEP-024 | FFTPACK / dfftpack (`FFTGF_FFTPACK` calls `zffti`/`zfftf`; `diagPAM` Makefile compiles `dfftpack.o`; sources not in this checkout) | unknown | unknown in this tree | unknown | available | none | E3 `src/FFTGF_FFTPACK.f90:15-21`; `numutils/src/Makefile:20-22`; `scripts/build.sh:50-62` |
| DEP-025 | DISLIN / DLPLOT (commented in `sfmake.inc`; `vfplot` USEs DLPLOT) | unknown | proprietary (DISLIN) | unknown | available | none | E3 `include/sfmake.inc:57-65`; `numutils/src/vfplot.f90:1-5`; `numutils/src/Makefile:84-86` |
| DEP-026 | NVIDIA CUDA (commented `CUDADIR` / cufft in `sfmake.inc` and `sciforvars.sh`) | unknown (comments name cuda_2.3) | proprietary (NVIDIA) | unsupported (commented 2.3-era paths) | available | none | E3 `include/sfmake.inc:42-47`; `bin/sciforvars.sh:58-63` |
| DEP-027 | FGSL / GSL (commented `FGSLDIR` in `sfmake.inc` and `setup_sf.sh`) | unknown | unknown in this tree | unknown | available | none | E3 `include/sfmake.inc:50-55`; `bin/setup_sf.sh:223-228`; `bin/sciforvars.sh:65-78` |
| DEP-028 | Alan Miller / ACM 712 random generators (`src/random_routines.f90`; `RANDOM.f90` comments out the include) | unknown | ACM / unknown (ACM 712 header in file) | unknown | available | none | E3 `src/RANDOM.f90:291`; `src/random_routines.f90:6-11` |
| DEP-029 | Alan Miller `dcerf` complex erf/erfc (`src/functions_zerf.f90`; `FUNCTIONS::zerf`) | unknown | unknown in this tree (Alan Miller attribution in file header) | unknown | available | none | E3 `src/functions_zerf.f90:1-20`; `src/FUNCTIONS.f90:189,301-302` |

There is no application framework, database, or GUI toolkit compiled into `libscifor.a`. OpenMP appears only as unused `COMMON_VARS` integers and `ifort` flags (`include/options.inc:2`; `src/COMVARS.f90:65-67`) and is not inventoried as a linked runtime. `src/CHRPACK.f90` (path inventory XP-393) is omitted: it is not compiled (`src/Makefile` `allmod` does not list it), no compiled unit includes it, and it is not a used external dependency in this checkout.

## 3. Support, license, and vulnerability notes

| DEP ID | Issue | Severity | Evidence |
|--------|-------|----------|----------|
| DEP-006 | Numerical Recipes source is proprietary. A C# port must not copy `four1`/`cosft2`/`realft`, Broyden NR helpers, `zbrent`, or `polint`/`locate`. A clean-room FFT/root/spline via the BCL or a chosen numeric library is the `available` route. | high | E3 `src/FFTGF_NR.f90:424-427` |
| DEP-004 | Vendored MKL Fortran interfaces are marked Intel Confidential and forbid copy/distribution without Intel permission. `MATRIX.f90` includes `mkl_lapack.fi` even when linking OpenBLAS. Do not copy those headers into the C# tree; call a public LAPACK-shaped API or a .NET numeric provider. | high | E3 `src/mkl_lapack.fi:1-18`; `src/MATRIX.f90:10` |
| DEP-009 | Zhang/Jin headers require copyright acknowledgment if the algorithms are incorporated. That is not a `no-route`; it is a provenance constraint on a rewrite. | medium | E3 `src/functions_special_funcs.f90:8-12` |
| DEP-007 | MINPACK F90 is GNU LGPL. Wrapping the Fortran objects would carry LGPL obligations; rewriting `hybrd1` in C# does not. | medium | E3 `src/minpack.f90:37-39` |
| DEP-011 | Burkardt `spline_interp.f90` is GNU LGPL. Same wrap-vs-rewrite distinction as DEP-007. | medium | E3 `src/spline_interp.f90:27-29` |
| DEP-005 | No FFTW license file is in this checkout. Linking an optional FFTW backend into a service would need a license check; the T1 default FFT backend is NR (`FFT_BACKEND` default), not FFTW. Prefer a BCL/.NET FFT over wrapping `libfftw3`. | medium | E3 `include/sfmake.inc:28-29`; `scripts/build.sh:7` |
| DEP-014 | No libmatheval license file is in this checkout. The `func` CLI is experimental; an HTTP expression-eval substitute must not assume clearance to wrap `-lmatheval`. | medium | E3 `numutils/src/Makefile:46`; `numutils/src/func.f90:27` |
| DEP-023 | MPICH2 1.4.1 is long past support. It is not linked into `libscifor.a`. | medium | E3 `etc/library.conf:7-8` |
| DEP-026 | Commented CUDA 2.3-era library paths are unsupported and unused. | low | E3 `include/sfmake.inc:44` |

## Errata

None.

## Links

- Dependency graph: [`docs/modernization/dependency-graph.md`](dependency-graph.md)
- Decision register: [`docs/modernization/decision-register.md`](decision-register.md)
- Impedance analysis: [`docs/modernization/impedance-analysis.md`](impedance-analysis.md)

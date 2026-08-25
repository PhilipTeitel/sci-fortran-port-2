<!-- Oracle contract:
- Classify the legacy oracle tier. That is this document's job.
- Per-path fixtures and comparison rules live in path test plans, not here.
- T3 documented-only cannot claim independent parity.
- After the probe, treat tier and environment as a snapshot unless a new probe is run (vN+1).
- Omit unused environment rows. Do not write N/A.
-->

# Legacy Oracle

**Legacy repo:** `/Users/philipteitel/code/ADD-migrations/sci-fortran-legacy`
**Oracle tier:** `T1 executable`
**Version:** `v1`
**Status:** `Snapshot`
**Date:** `2026-08-25`
**Owner:** Implementer (`/build-oracle`)

## Summary

This probe classifies the SciFor/SciFortran legacy oracle as **T1 executable** on the documented Darwin host: a gitignored snapshot clone of checkout `e586903a26cc50ca8942f20ca3bccbd8814e6252` built with Homebrew gfortran 16.1.0 and OpenBLAS, then `scifor-fidelity` (XP-388) ran twice with bit-identical stdout covering linspace, logspace, fermi, and deriv (XP-060, XP-061, XP-108, XP-072). Containment is required because `scripts/build.sh` (XP-386, default `FFT_BACKEND=NR`) compiles vendored Numerical Recipes units and Intel Confidential MKL Fortran headers into `libscifor.a`; those objects must stay in the snapshot, must not be copied into the C# tree, and were not invoked by this probe's runtime. First-slice parity may use repeated live legacy-vs-new comparison on this environment; per-path fixtures, FIX IDs, and numeric/text rules belong in path test plans (not written yet) and must not take the workflow-profile `0.000001` hint or `fidelity.sh`'s default `1e-10` as binding. Look next at `/assess-modernization` for the lane verdict; do not treat this file as a fixture catalog.

---

## 1. Oracle tier decision

| Tier | Selected? | Evidence | Consequence |
|------|-----------|----------|-------------|
| T1 executable | yes | E1 2026-08-25 probe: `gfortran --version` → GNU Fortran (Homebrew GCC 16.1.0) 16.1.0; sandbox `scripts/build.sh` exited 0 (`FFT backend: NR`, `Build complete.`, `.bin/scifor-fidelity` Mach-O arm64); two driver runs identical (`diff` empty, 1049 lines each); `scripts/fidelity.sh` 5 passed, 0 failed; legacy checkout `git status` clean | Repeated legacy-vs-new comparison is possible. |
| T2 recorded | no | E1 live execution succeeded on this host; a frozen corpus is not required for the probed driver | Use frozen corpus; do not require live legacy execution in every run. |
| T3 documented-only | no | E1 the library and fidelity driver compiled and ran; parity is not limited to documentation | Parity is unprovable; require user acceptance data. |

Exactly one tier is selected.

---

## 2. Legacy execution environment

| Requirement | Value | Evidence |
|-------------|-------|----------|
| OS / image | Darwin 25.5.0 arm64 (host `uname -srm`) | E1 `uname -srm` → `Darwin 25.5.0 arm64` |
| Runtime / compiler | GNU Fortran (Homebrew GCC 16.1.0) 16.1.0; `FC=gfortran`; `include/options.inc` compile flags include `-O2 -static` (`gfortran -c … -static` succeeded); linking a binary with `-static` fails on this host (`ld: library 'crt0.o' not found`); XP-386 driver link is `-O2` without `-static` and succeeded; driver also linked `-lopenblas` from `/opt/homebrew/opt/openblas` (`libopenblasp-r0.3.34`) even though XP-060/061/072/108 do not call BLAS | E1 `gfortran --version`; E1 `/tmp` `-static` link smoke; E1 sandbox `scripts/build.sh` log; E1 `ls -l /opt/homebrew/opt/openblas/lib/libopenblas.dylib` |
| Network | none | E1 clone, compile, link, and driver runs used already-installed Homebrew gcc/OpenBLAS; no package install and no network during the probe |
| Secrets | none | E1 XP-388 reads `numutils/test/xy2.data` (numeric x/y samples only); no credentials or PII in the driver or golden numeric files |

---

## 3. Containment and security

| Risk | Isolation control | Evidence | Owner |
|------|-------------------|----------|-------|
| XP-386 default build compiles Numerical Recipes units (DEP-006: `FFTGF_NR.f90` `four1`/`cosft2`/`realft`, plus Broyden/Brent/`spline_nr_mod` in `allmod`) into `libscifor.a` | Execute only from a copied snapshot, never the shared legacy checkout. Keep the snapshot gitignored (`.oracle-sandbox/`). Do not copy NR sources or objects into the C# port. Do not call FFT/Broyden/Brent APIs from the first-slice probe. No network during build/run. | E3 `scripts/build.sh:7,50-62`; E3 `src/FFTGF_NR.f90:11,424-427`; E1 sandbox log `FFT backend: NR (FFTGF_NR.f90)` and `fftgf.mod` / `FFTGF.o` archived | Implementer (sandbox); Human before any redistribution |
| `MATRIX.f90` includes Intel Confidential `mkl_lapack.fi` (DEP-004) even when `MKLROOT` is unset and the link is OpenBLAS | Same snapshot. Do not set `MKLROOT`. Do not select `FFT_BACKEND=MKL`. Do not copy `mkl_lapack.fi`, `mkl_dfti.f90`, or `mkl_trig_transforms.f90` into the C# tree. | E3 `src/MATRIX.f90:10`; E3 `src/mkl_lapack.fi:1-18`; E1 probe ran with `MKLROOT` empty; E1 `matrix.mod` / `MATRIX.o` archived | Implementer (sandbox); Human before any redistribution |
| `scripts/build.sh` mutates the tree (`ln -sf` `src/FFTGF.f90`, `ln` `options.inc`, `make clean`) | Archive-only / copied snapshot under `.oracle-sandbox/sci-fortran-legacy`. Never run XP-386 in `/Users/philipteitel/code/ADD-migrations/sci-fortran-legacy`. | E3 `scripts/build.sh:50-70`; E1 legacy `git status --porcelain` empty after the probe | Implementer |
| Zhang/Jin special-function corpus (DEP-009) compiles with `FUNCTIONS.f90` (needed for XP-108 `fermi` in the same translation unit) | Snapshot-only objects; C# rewrite of `fermi` must not paste Zhang/Jin sources. Acknowledgment rules apply only if those algorithms are incorporated later. | E3 `src/FUNCTIONS.f90:171,240-248`; E1 `FUNCTIONS.o` in `libscifor.a` | Implementer |

---

## 4. Determinism hazards that affect the tier

| Hazard | Why it affects repeated runs | Control |
|--------|------------------------------|---------|
| Compiler and flags | A different gfortran version or replacing `-O2` can change numeric stdout; this probe pinned Homebrew GCC 16.1.0 and the Makefile `STD` flags | Rebuild the snapshot with the same `FC` and `include/options.inc`; do not treat a new compiler as the same T1 host without a vN+1 probe |
| Compile-time FFT backend | `FFT_BACKEND` symlinks `FFTGF_*.f90` before `make`; a non-NR rebuild is a different library even if linspace/fermi bodies are unchanged | Keep checkout default `NR` for this T1 snapshot; other backends need a new probe |
| List-directed `WRITE` on the deriv section | XP-388 prints deriv with `write (*, *)` (not `es24.17`); format can change across compilers while values stay comparable | Same binary: two runs were identical (E1). Path test plans own numeric vs text comparison for XP-072/XP-388 |
| `fidelity.sh` rewrites Python goldens on every run | XP-387 is a convenience compare (`TOL` default `1e-10`), not frozen Fortran output; it is not the T1 oracle | Treat live `scifor-fidelity` stdout as the executable oracle; path test plans name any recorded FIX IDs later |
| OpenBLAS threading | Driver links OpenBLAS; MATRIX-class paths could vary with thread count. Probed helpers do not call BLAS. | Leave `MKLROOT` unset; pin OpenBLAS only when a path actually calls DEP-003 |

Path-specific hazards belong in that path's test plan.

---

## 5. Harness location

| Kind | Path | Notes |
|------|------|-------|
| probe notes | `.oracle-sandbox/sci-fortran-legacy` (gitignored clone) | Checkout `e586903`; `lib/libscifor.a`, `.bin/scifor-fidelity`; probe captures under `.oracle-sandbox/probe-out/` (`run1.out` == `run2.out`). Not a fixture catalog. Recreate with `git clone --local` of the legacy repo plus `FC=gfortran FFT_BACKEND=NR scripts/build.sh`. |
| legacy harness sources (read-only) | `/Users/philipteitel/code/ADD-migrations/sci-fortran-legacy/scripts/build.sh`, `scripts/fidelity.sh`, `fidelity/driver.f90` | XP-386, XP-387, XP-388. Probe did not copy them into `docs/`. |
| harness / fixture root | none | `/build-oracle build` writes target-repo harness or fixture files; path test plans name FIX IDs. |

---

## 6. Open oracle questions

- [ ] If CI cannot reproduce this Darwin + gfortran 16.1.0 + OpenBLAS host, does the lane keep T1 only on this machine or record driver stdout as T2 fixtures (vN+1 probe)?
- [ ] For later slices that must *execute* DEP-006 FFT/Broyden/Brent (not merely link the objects), is live T1 still allowed under the same snapshot containment, or are those paths T2/T3?

## Links

- Assessment: `docs/modernization/ASSESSMENT.md`
- Path test plans: `docs/modernization/test-plans/XP-NNN-tests.md`
- Parity reports: `docs/modernization/parity/{STORY-ID}-parity.md`

<!-- Oracle contract:
- Classify the legacy oracle tier. That is this document's job.
- Per-path fixtures and comparison rules live in path test plans, not here.
- T3 documented-only cannot claim independent parity.
- After the probe, treat tier and environment as a snapshot unless a new probe is run (vN+1).
- Omit unused environment rows. Do not write N/A.
-->

# Legacy Oracle

**Legacy repo:** `/Users/philipteitel/code/ADD-migrations/sci-fortran-legacy` (`e586903a26cc50ca8942f20ca3bccbd8814e6252`, tree `691333e7709f4d3396fbe714726e0a010a71355b`)
**Oracle tier:** `T1 executable` (bounded to the core library plus the in-tree fidelity driver)
**Version:** `v1`
**Status:** `Snapshot`
**Date:** `2026-08-22`
**Owner:** Implementer (`/build-oracle`)

## Summary

This probe classifies the SciFor/SciFortran checkout as a **bounded T1 executable** oracle: two independent `git archive` snapshots of commit `e586903` each compiled `libscifor.a` / `libscifor_deb.a` and ran `scripts/fidelity.sh` with exit `0`, and the five numeric sections were byte-identical across those runs. Containment is required: do not execute the legacy tree in place (it already has ignored build residue), do not run installer/CLI/`SYSTEM`-backed surfaces, and do not retain binaries. First-slice parity can use repeated live comparison only for the executed fidelity driver calls (`linspace`, `logspace`, `fermi`, `deriv`); fixtures and comparison rules belong in path test plans (`docs/modernization/test-plans/XP-NNN-tests.md`), which do not exist yet. Surfaces that were not run (FFT, LAPACK, RNG, timestamps, numutils CLIs, plotting, gzip/`mkdir`) remain outside this T1 boundary.

---

## 1. Oracle tier decision

| Tier | Selected? | Evidence | Consequence |
|------|-----------|----------|-------------|
| T1 executable | yes, bounded | E1 two clean archives of `e586903` (tar SHA-256 `e00dd2daeee84037674e99ef440a353a2fb3ac743d36d7179afa6758abf391f1`) each ran `scripts/build.sh` then `scripts/fidelity.sh` with exit `0`; full stdout SHA-256 `14a40532e4e308265dfd09cca956ab7f4249c80de884a1264bc96377d3688dd5` was identical. | Repeated legacy-vs-new comparison is possible for this revision, host, driver, and five-section corpus. |
| T2 recorded | no | E3 `scripts/fidelity.sh:105-159` rewrites four `fidelity/golden/` files from Python formulas and copies `numutils/test/xy2.deriv` before comparing; those files are not a frozen observed corpus. No fixture was retained in this probe. | Do not treat script PASS or checked-in goldens as recorded oracle output. |
| T3 documented-only | no for the bounded corpus | E1 compile, link, and two executions supersede documentation-only status for the driver corpus. | Unexecuted library/CLI surfaces are still undocumented-as-executable; they are not this document's selected tier. |

Exactly one tier is selected: **T1 executable**, scoped to the core library build plus `fidelity/driver.f90` sections that actually call legacy procedures.

**Scope of the T1 boundary (not a second tier):**

- Executed library calls: `linspace(0,1,5)`, `logspace(1,1000,5)`, `fermi(x,100)` for five abscissas, `deriv(y,dh)` on 1,024 rows from tracked `numutils/test/xy2.data`. E1 `fidelity/driver.f90:10-67`.
- Not a library call: the section labeled `arange-5` prints `real(i,8)` for `i=1..5` and does not call `TOOLS::arange`. E3 `fidelity/driver.f90:27-30`.
- Not executed: numutils `all` CLIs; FFT (`FFTGF`); BLAS/LAPACK (`MATRIX`); RNG; `timestamp`/`version`; `IOFILE` gzip/`mkdir`; gnuplot/`SYSTEM` plotting; `bin/setup_sf.sh`; FFTW3/MKL backends.
- Script self-check errors vs rewritten goldens (not parity rules): linspace `0`, logspace `1.023e-12`, arange-label `0`, fermi `0`, deriv `4.885e-15` at `TOL=1e-10`. E1 fidelity transcripts. The user-profile numeric hint is not a comparison rule.

**Probe record:**

| Probe | Result | Evidence |
|-------|--------|----------|
| Legacy identity | `master` `e586903a26cc50ca8942f20ca3bccbd8814e6252`; ignored residue only (`.bin/`, `.fidelity-out/`, `src/objs/COMVARS.o`, `src/debug_*`, `src/scifor_version.inc`, `numutils/src/{options.inc,sfmake.inc}`). Tracked porcelain empty. | E1 `git rev-parse` / `status --ignored=matching` before and after; no build ran in the checkout |
| Materialization | `git archive` tar SHA-256 `e00dd2daeee84037674e99ef440a353a2fb3ac743d36d7179afa6758abf391f1`; two extracts, 137 tracked files each; no secret-named tracked path | E1 archive hash and path scan |
| Builds | Both snapshots `scripts/build.sh` exit `0`; `FFT_BACKEND=NR`; produced `lib/libscifor.a`, `lib/libscifor_deb.a`, `.bin/scifor-fidelity` | E1 build status files |
| Runs | Both snapshots `scripts/fidelity.sh` exit `0`; `5 passed, 0 failed` | E1 fidelity transcripts |
| Repeatability | Full and per-section captures byte-identical. Section SHA-256: linspace `dabd07f92b83714a0e0740223261fc457c517025cf3f4b08cd51380a0082c35c`; logspace `c5b198afbc3ccea1a27ded3ac9f3919952a6662c2465000c84e35e8f964db4fe`; arange-label `2e104659f3bed55ec95de1c6b444990a60856fc1de30a3ab8aad51e4abda3c17`; fermi `6f35eadc7f917064b110353cebf6ab6468999625ceae0394744d68be67a1810a`; deriv `8a8879bc89a240338320275532f418c332b4120a536c23a8cb9428ca99b30b6f` | E1 `shasum -a 256` of ephemeral captures (not retained) |
| Source integrity | Zero archived files changed or disappeared; 223 generated files + `src/options.inc` symlink confined to the snapshot | E1 before/after tree hashes |
| Binaries | Optimized archive, debug archive, and driver SHA-256 differed across the two builds | E1 artifact hashes in §4 |
| Diagnostics | One expected `fatal: not a git repository`; 310 `Warning:` lines; one `ld: warning: ignoring duplicate libraries: '-lgfortran'` | E1 `snap-*-build.err` |

No harness, binary, or capture was kept in this repository. Only this document remains.

---

## 2. Legacy execution environment

| Requirement | Value | Evidence |
|-------------|-------|----------|
| OS / image | macOS 26.5.2 (`25F84`), Darwin 25.5.0, arm64 | E1 `sw_vers`; `uname -m` |
| Runtime / compiler | GNU Fortran 16.1.0 (Homebrew GCC 16.1.0); `gfortran` SHA-256 `1f2580f9691ce4a9bfcf7ca42f0243aa3a740d450faf5499396a7902a3d88ca6`; optimized objects `-O2 -static`; debug objects `-O0 -p -g -Wall -static`; driver `-O2 -lscifor -lopenblas -lgfortran` | E1 compiler version/hash and build transcript; E3 `include/options.inc:9-15`; `scripts/build.sh:74-76` |
| Native math | OpenBLAS 0.3.34 (`libopenblas.dylib` SHA-256 `aeb5f40d3b5cc0fca84e05e90b8e7da6921cea2c33ca701062b0ccd7b4caf117`). Driver `otool -L` resolved OpenBLAS, `libgfortran.5`, `libquadmath.0`, `libSystem.B`. LAPACK 3.12.1 is installed but was not the Darwin driver link. | E1 `otool -L` and dylib hashes |
| FFT backend | `NR` → `src/FFTGF.f90` → `FFTGF_NR.f90` (compiled; not called by the driver) | E1 build log `FFT backend: NR`; E3 `scripts/build.sh:7,50-62` |
| Locale / threads | `LC_ALL=C` `LANG=C` `TZ=UTC` `OPENBLAS_NUM_THREADS=1` `OMP_NUM_THREADS=1` `VECLIB_MAXIMUM_THREADS=1` `PYTHONHASHSEED=0` | E1 exact `env -i` prefix below |
| Network | none used; agent sandbox network was allowlist-only. macOS `sandbox-exec` `(deny network*)` was **not** applied (`sandbox_apply: Operation not permitted` in this agent) | E1 sandbox-exec exit `71`; no network-facing API invoked |
| Secrets | none observed; no secret-named tracked path; `HOME`/`TMPDIR` redirected into each snapshot | E1 archive path scan and invocation |
| Source pin | Git archive of `e586903`; `GIT_CEILING_DIRECTORIES=<snapshot>` so `src/Makefile` `git rev-parse` could not see this port repo. Generated `sf_version=""`. | E1 `src/scifor_version.inc`; E3 `src/Makefile:2,24-25` |
| Comparison tools | GNU Make 3.81; Bash 3.2.57; Apple Git 2.50.1; `/usr/bin/python3` 3.9.6; openrsync 2.6.9-compatible | E1 version probes on the pinned `PATH` |

Exact prefix for every canonical command (snapshot paths substituted):

`env -i HOME=<snapshot>/.probe-home TMPDIR=<snapshot>/.probe-tmp PATH=/opt/homebrew/opt/gcc/bin:/usr/bin:/bin:/usr/sbin:/sbin LC_ALL=C LANG=C TZ=UTC FC=gfortran FFT_BACKEND=NR TOL=1e-10 OPENBLAS_NUM_THREADS=1 OMP_NUM_THREADS=1 VECLIB_MAXIMUM_THREADS=1 PYTHONHASHSEED=0 GIT_CEILING_DIRECTORIES=<snapshot>`

`TOL` is the fidelity script's self-check against rewritten formula files. It is not a path comparison rule.

---

## 3. Containment and security

| Risk | Isolation control | Evidence | Owner |
|------|-------------------|----------|-------|
| Legacy checkout mutation | Read-only Git identity/status/archive only. Builds ran in disposable `git archive` extracts under this repo's `.oracle-probe/` (deleted after evidence extraction). Post-probe ignored residue in the legacy tree was unchanged. | E1 before/after `git status --ignored=matching` | Implementer / oracle operator |
| Network | Cursor agent sandbox (network allowlist). Reviewed scripts do not open sockets. OS `sandbox-exec` network deny was unavailable in this agent. | E1 `sandbox_apply: Operation not permitted`; E3 `scripts/build.sh`, `scripts/fidelity.sh` | Oracle operator |
| Host command execution | Did not run `bin/setup_sf.sh`, numutils CLIs, or APIs that `call system` (`gzip`/`gunzip`/`mkdir`/`mv`/`chmod`, gnuplot). | E3 `src/IOFILE.f90:230,266,290`; `src/slplot_splot_3d.f90:66`; `src/TOOLS.f90:273-274`; `src/SQUARE_LATTICE.f90:159-160` | Oracle operator |
| Proprietary / unclear licensing | Did not copy snapshot sources or binaries into the target tree. NR FFT (`four1` “NumRec”) was the compiled backend but unused by the driver. `MATRIX` still `include "mkl_lapack.fi"` (INTEL CONFIDENTIAL) at compile. Do not redistribute probe products. | E3 `src/FFTGF_NR.f90:424`; `src/MATRIX.f90:10`; `src/mkl_dfti.f90:1-17` | Legal / product owner |
| Secrets / PII | Tracked inputs are synthetic numerics (`fidelity/driver.f90` literals and `numutils/test/xy2.data`). Isolated `HOME`/`TMPDIR`. | E1 driver execution; E3 `fidelity/driver.f90:43` | Oracle operator |
| Resource isolation | No CPU/memory/PID/filesystem quota. Each build ~40s, each fidelity run <5s. Stronger sandboxing is required before malformed paths or `SYSTEM` surfaces. | E1 observed elapsed time | Security / oracle operator |

---

## 4. Determinism hazards that affect the tier

| Hazard | Why it affects repeated runs | Control |
|--------|------------------------------|---------|
| Archives and the fidelity binary are not byte-reproducible | Optimized `libscifor.a` SHA-256 `830e1141…4c0c` vs `8cca2d51…d1e3`; debug `456c80b5…63b3` vs `c80ecd96…8235`; driver `727bd517…34df` vs `1dc0ecad…50da`. Runtime stdout still matched. | Treat source/environment/stdout hashes as oracle provenance; do not pin binary hashes |
| Revision stamp is empty in archive snapshots | `git rev-parse` fails without `.git`; `sf_version=""`. Walking up into this port repo would stamp the wrong commit. | Keep `GIT_CEILING_DIRECTORIES`; record `e586903` externally; do not use `version` output as a fixture |
| Compiler/OS/library pin | Numerical results can change under ifort, other GCC, x86_64, or MKL vs OpenBLAS | T1 is this macOS arm64 / gfortran 16.1.0 / OpenBLAS 0.3.34 host only |
| Uninitialized / conversion warnings | 61 `-Wconversion`, 55 `-Wmaybe-uninitialized`, 12 `-Wuninitialized` (plus unused-code categories) on debug objects | Keep the warning inventory; do not extend T1 to unexecuted routines without characterization |
| Locale and formatted I/O | Fortran `es24.17` text depends on locale | Keep `LC_ALL=C`; path test plans own text vs parsed-numeric rules |
| `date_and_time` / `SYSTEM_CLOCK` | `timestamp` and `init_random_number` are time-seeded; not in this corpus | Exclude XP-004 / XP-191-class paths from this T1 boundary until seeded fixtures exist |
| FFT / LAPACK provider and threading | NR FFT compiled, OpenBLAS linked, neither exercised | Record `FFT_BACKEND=NR` and thread `=1` as build pin only |
| Fidelity script rewrites goldens | PASS vs Python formulas is not observed-legacy provenance | Path test plans must name retained E1 captures, not `fidelity/golden/` as-is |

Path-specific hazards belong in that path's test plan.

---

## 5. Harness location

| Kind | Path | Notes |
|------|------|-------|
| probe notes | none retained | Disposable snapshots, logs, binaries, and captures were deleted after this document was written |
| in-legacy build/run scripts | `/Users/philipteitel/code/ADD-migrations/sci-fortran-legacy/scripts/build.sh`, `scripts/fidelity.sh`, `fidelity/driver.f90` | Read-only. `/build-oracle build` writes target-repo files; do not copy these into the port until a path test plan names FIX IDs |
| fixture root | none | No `docs/modernization/test-plans/` files yet |

---

## 6. Open oracle questions

- [ ] Is this macOS 26.5.2 / gfortran 16.1.0 / OpenBLAS 0.3.34 / `FFT_BACKEND=NR` host the accepted product oracle, or must ifort/Linux/MKL define historical behavior (which would drop this T1 pin)?
- [ ] Should a project-local `.cursor/workflow.config.yml` record `oracleTier: T1 executable` so later commands do not inherit the user-profile default `T3 documented-only`?
- [ ] May NR-derived and Intel-confidential compile units be executed again for later slices, or must those surfaces move to T2/T3 until replaced?
- [ ] Is OS-level `sandbox-exec` (or a container) required before any further live run, given it could not be applied in this agent?
- [ ] Which unexecuted XP-NNN IDs must be inside the T1 boundary for the first slice? Expanding scope needs a new probe (v2), not an edit of this snapshot.

## Links

- Assessment: `docs/modernization/ASSESSMENT.md`
- Path test plans: `docs/modernization/test-plans/XP-NNN-tests.md`
- Parity reports: `docs/modernization/parity/{STORY-ID}-parity.md`
- Execution path inventory: `docs/modernization/execution-path-inventory.md`
- Dependency inventory: `docs/modernization/dependency-inventory.md`

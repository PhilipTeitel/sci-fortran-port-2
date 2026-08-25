# Technology and Impedance Analysis

**Source stack:** Fortran 90/95 (free-form; F2003 command-line intrinsics) / unknown (checkout e586903; gfortran 16.1.0 on T1 host) / none (static library `libscifor.a`)
**Target stack:** C# / .NET 8 / ASP.NET Core (HTTP/JSON services)
**Version:** v1
**Status:** Snapshot
**Date:** 2026-08-25
**Owner:** Migration Strategist (`/analyze-impedance`)

## Summary

The decision-changing mismatch is product shape: a Fortran static library plus CLIs/jobs versus C# HTTP/JSON services, so candidate in-slice numeric helpers (XP-060, XP-061, XP-072, XP-108) should be **rewritten** behind JSON, not wrapped as `libscifor.a`. That is **IMP-001** (critical) and **IMP-004** (high: `error`/`STOP` on bad `linspace`/`logspace` inputs). IEEE `real(8)` (**IMP-002**, preserve) and 1-based 1D arrays (**IMP-003**, rewrite indexing) also bind those four IDs but are medium. Column-major layout, NR/MKL wrap bans, FFT backends, `SYSTEM`/gnuplot, and SAVE/globals bind later paths, not those helpers. Build jobs (**IMP-006**) stay T1 tooling, not services. Look next at `/build-oracle probe`; path test plans own per-path numeric comparison (no global tolerance).

---

## 1. Source characteristics that matter for porting

| Characteristic | Observation | Evidence |
|----------------|-------------|----------|
| Delivery shape | Free-form F90/95 modules archived as `libscifor.a`; numutils CLIs and bash jobs; no application host or HTTP endpoints | E3 `src/Makefile:5-12`; path inventory summary; `docs/modernization/execution-path-inventory.md` §1 |
| Numeric kind | Public numeric APIs use `real(8)` / `complex(8)` (IEEE binary64 under gfortran) | E3 `src/tools_grids.f90:2`; `src/FUNCTIONS.f90:240-247`; `src/TOOLS.f90:175-178` |
| Arrays | Default 1-based; column-major storage; some assumed-shape `dimension(0:)` | E3 `src/tools_grids.f90:14`; `src/tools_shifts.f90:8,20` |
| Failure | `COMMON_VARS::error` prints and `stop`; FFT radix-2 check `stop`s | E3 `src/COMVARS.f90:192-209`; `src/tools_grids.f90:8`; `src/FFTGF_NR.f90:20-25` |
| Process I/O | CLIs/driver use list-directed and `es24.17` stdout; some paths call POSIX `SYSTEM` | E3 `fidelity/driver.f90:10-37,65`; `src/IOFILE.f90:230,266,290` |
| COMMON / EQUIVALENCE | No `COMMON` or `EQUIVALENCE` in this checkout | E3 repo-wide search of `src/` and `numutils/` |

## 2. Target characteristics that differ

| Characteristic | Difference from source | Evidence |
|----------------|------------------------|----------|
| Delivery shape | ASP.NET Core HTTP/JSON services, not a Fortran ABI or CLI process | E2 `.cursor/workflow.config.yml` `modernization.targetStack` |
| Numeric types | C# `double` / `System.Numerics.Complex` (IEEE binary64 analogue); no `real(8)` kind parameter | E2 `.cursor/workflow.config.yml` `stack.language: csharp` |
| Arrays | CLR 0-based, row-major; JSON arrays have no Fortran layout | E2 C# / .NET 8 language rules (target stack pin) |
| Failure | HTTP status + problem body (or exceptions), not process `STOP` | E2 ASP.NET Core as configured framework |
| Numbers on the wire | JSON number tokens (typically invariant culture), not Fortran `WRITE` | E2 ASP.NET Core HTTP/JSON services |
| Packaging | `dotnet` assemblies, not `libscifor.a` / Make / `ar` | E2 `.cursor/workflow.config.yml` `stack.buildCommand` |

## 3. Impedance mismatches

| ID | Mismatch | Severity | Affected XP IDs | Candidate pattern | Evidence |
|----|----------|----------|-----------------|-------------------|----------|
| IMP-001 | Fortran library/CLI process versus ASP.NET Core HTTP/JSON. Wrapping `libscifor.a` is not the product. | critical | XP-060, XP-061, XP-072, XP-108; other public library APIs if later sliced | rewrite | E2 `.cursor/workflow.config.yml` `modernization.targetStack` (path inventory: no service endpoints) |
| IMP-002 | 1D `real(8)` helpers versus C# `double` sequences. Analogue is IEEE binary64; comparison stays per path. | medium | XP-060, XP-061, XP-072, XP-108 | preserve | E3 `src/tools_grids.f90:1-47`; `src/TOOLS.f90:175-186`; `src/FUNCTIONS.f90:240-248` |
| IMP-003 | Default 1-based Fortran arrays versus 0-based CLR / JSON arrays | medium | XP-060, XP-061, XP-072 (1D results); XP-108 scalar (no array index) | rewrite | E3 `src/tools_grids.f90:14,18`; `src/TOOLS.f90:180-185` |
| IMP-004 | `COMMON_VARS::error` → `stop` versus HTTP error responses (`linspace`/`logspace` `num<0`) | high | XP-060, XP-061, XP-004; CLIs XP-370, XP-371 | rewrite | E3 `src/tools_grids.f90:8,41`; `src/COMVARS.f90:192-209` |
| IMP-005 | Fortran formatted / list-directed stdout versus JSON number tokens (culture and display rounding are per-path) | medium | XP-060, XP-061, XP-072, XP-108 as services; harness XP-370, XP-371, XP-373, XP-374, XP-387, XP-388 | rewrite | E3 `fidelity/driver.f90:15,23,37,65`; `numutils/src/linspace.f90:12-26` |
| IMP-006 | T1 build/fidelity shell jobs versus product HTTP services | high | XP-386, XP-387, XP-389 | preserve | E3 `scripts/build.sh:1,79-97`; `scripts/fidelity.sh:1,161-184`; path inventory XP-386–XP-389 |
| IMP-007 | Column-major Fortran rank-2+ arrays versus row-major CLR / JSON | high | XP-070, XP-071, XP-053, XP-054, XP-169, XP-171–XP-175 | rewrite | E3 `src/tools_shifts.f90:8-14`; `src/MATRIX.f90:14-16`; `src/SLPLOT.f90:12-31` |
| IMP-008 | Mixed `dimension(0:)` and default 1-based bounds versus a single C# index convention | medium | XP-070, XP-071, XP-096–XP-101, XP-179 | rewrite | E3 `src/tools_shifts.f90:8,32`; `src/FFTGF_NR.f90:144,196` |
| IMP-009 | Module public mutables (`MPIID`) and `SAVE` counters versus request-scoped HTTP | medium | XP-006, XP-048, XP-080–XP-082 | rewrite | E3 `src/COMVARS.f90:59-62`; `src/tools_check_scalar.f90:15`; `src/IOFILE.f90:289` |
| IMP-010 | Unstructured `GO TO` in vendored special-function/NR-style code versus structured C# | medium | XP-111; FFT/Broyden/Zhang-Jin paths that contain computed jumps | rewrite | E3 `src/functions_zerf.f90:47-67`; `src/FFTGF_NR.f90:426-457` |
| IMP-011 | Compile-time FFT backend symlink (NR default; FFTW3/MKL optional) versus one .NET FFT | high | XP-092–XP-105, XP-382, XP-397 | rewrite | E3 `scripts/build.sh:7,50-62`; `src/FFTGF_NR.f90:1-11`; graph XP-386 |
| IMP-012 | Fortran `SYSTEM` (gzip, mkdir, gnuplot scripts) versus managed I/O / JSON payloads | medium | XP-041, XP-042, XP-048, XP-054, XP-075, XP-379 | rewrite | E3 `src/IOFILE.f90:230,266,290`; `src/slplot_splot_3d.f90:39-66` |
| IMP-013 | Vendored Numerical Recipes and Intel Confidential MKL headers cannot be copied or ABI-wrapped into the C# tree | high | XP-087–XP-089, XP-092–XP-093, XP-169, XP-171–XP-175, XP-177, XP-386 | rewrite | E3 dependency inventory DEP-006, DEP-004; `src/FFTGF_NR.f90:424-427`; `src/MATRIX.f90:10` |
| IMP-014 | File-scope Zhang/Jin procedures (linker-visible, not `MODULE FUNCTIONS` publics) versus an explicit HTTP surface | medium | XP-205–XP-369 | rewrite | E3 `src/FUNCTIONS.f90:171-176`; path inventory XP-205–XP-369 |

Candidate pattern is a recommendation, not a decision. Record the decision as `DEC-NNN` or `ADR-NNN`.

## 4. Fortran family notes

Constructs checked against this checkout; each present item is an `IMP-NNN` in §3. Absent items are not rows.

- **COMMON / EQUIVALENCE:** not present. Closest analogue is module-level mutables and `SAVE` (**IMP-009**).
- **Implicit typing:** compiled modules used by XP-060/061/072/108 declare `implicit none`. File-scope Zhang/Jin units are separate compilation-unit style (**IMP-014**), not a first-slice typing issue.
- **Column-major storage:** present for rank-2+ (**IMP-007**). 1D JSON arrays for linspace/logspace/deriv are layout-neutral.
- **Unstructured `GOTO`:** present in `dcerf` and NR-style FFT (**IMP-010**); not in `linspace`/`logspace`/`deriv`/`fermi`.
- **Fixed-form / list-directed I/O:** present on CLIs and the fidelity driver (**IMP-005**); library helpers return arrays, they do not format JSON.
- **`real(8)` semantics:** present (**IMP-002**). Path test plans own comparison; do not apply a system-wide tolerance.

## 9. Open impedance questions

- [ ] If XP-370, XP-371, XP-373, or XP-374 become HTTP services, is stdin/argv mapped to JSON bodies, or do those CLIs stay oracle-only?
- [ ] For XP-060 `istart`/`iend` and XP-061 `base`, are optional Fortran arguments required JSON fields or omitted with Fortran defaults?
- [ ] For rank-2+ later slices, is JSON row-major with an explicit layout field, or are matrices serialized column-major to match Fortran memory?

## Errata

None.

## Links

- Path inventory: [`docs/modernization/execution-path-inventory.md`](execution-path-inventory.md)
- Dependency inventory: [`docs/modernization/dependency-inventory.md`](dependency-inventory.md)
- Dependency graph: [`docs/modernization/dependency-graph.md`](dependency-graph.md)
- Decision register: [`docs/modernization/decision-register.md`](decision-register.md)

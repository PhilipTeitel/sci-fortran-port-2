# Migration Plan

**Legacy repo:** `/Users/philipteitel/code/ADD-migrations/sci-fortran-legacy`
**Source stack:** Fortran 90/95 (free-form; F2003 command-line intrinsics) / unknown (checkout e586903; gfortran 16.1.0 on T1 host) / none (static library `libscifor.a`)
**Target stack:** C# / .NET 8 / ASP.NET Core (HTTP/JSON services)
**Strategy:** phased-rewrite
**Date:** 2026-08-25
**Owner:** Migration Strategist (`/plan-migration`)

## Summary

Staging is **phased-rewrite**: this POC rewrites SciFor numeric helpers as ASP.NET Core HTTP/JSON services; there is no live Fortran host to strangle and no production parallel-run window. First product slice is **XP-060, XP-061, XP-072, XP-108**. Slice prerequisites are resolved: **DEC-001–DEC-007** and **DEP-001** (`available`). No `DEF-NNN` exists. HTTP ADR (RISK-001 / IMP-004) and path test plans (RISK-002) are named remaining work, not missing DECs. Parity stays bounded **T1 executable** (oracle.md v1; DEC-004/DEC-005). XP-386, XP-387, XP-388 remain oracle tooling, not a product slice. Next command: `/document-legacy` scoped to XP-060, XP-061, XP-072, XP-108.

---

## 1. Strategy decision

| Strategy | Selected? | Why / why not | ADR |
|----------|-----------|---------------|-----|
| strangler | no | No live production system to keep running while traffic is shifted. The legacy artifact is a static library plus CLIs/jobs. | none |
| phased-rewrite | yes | Library → HTTP/JSON POC with no cutover. Deliver the four DEP-001-only helpers first, then further inventory groups in graph order. | none (strategy is this plan; process-boundary ADRs in §5) |
| big-bang-parallel-run | no | Assessment does not require a parallel production oracle period. T1 is a bounded probe host (DEC-004). | none |

Do not convert the Fortran monolith to event-driven services.

## 2. Binding assumptions

| Assumption | Source | Risk if wrong | Validation |
|------------|--------|---------------|------------|
| Target is C# / .NET 8 / ASP.NET Core HTTP/JSON | DEC-001; ASSESSMENT M1 accepted | Slice would target the wrong process boundary | Profile `modernization.targetStack`; Human M1 |
| Product is not a Fortran ABI / `libscifor.a` drop-in | DEC-002 | Implementer wraps the archive instead of rewriting | First-slice C# tree has no P/Invoke to `libscifor.a` |
| First-slice helpers are rewritten in C#, not wrapped | DEC-003; IMP-001 | POC ships a Fortran ABI | Port stories implement C# `linspace`/`logspace`/`deriv`/`fermi` |
| Bounded T1 pin is the numeric oracle; XP-386/387/388 are tooling | DEC-004; oracle.md v1 | Parity claimed on another host or against XP-387 goldens | Parity reports cite this pin and XP-388 stdout |
| Comparison is per-path parsed numbers | DEC-005; IMP-002, IMP-005 | Format-text or global-tolerance “fails” | Path test plans name per-path bounds; profile `0.000001` is not a gate |
| First-slice bar is those four HTTP/JSON operations | DEC-006 | CLIs, FFT, LAPACK, gnuplot, or build jobs enter the slice | Slice XP list; out-of-slice XP-370–XP-389 and FFT/MATRIX/`SYSTEM` |
| Do not copy DEP-006 or DEP-004 into the C# tree | DEC-007; RISK-004 | License/containment breach | First-slice tree has no NR/MKL headers or objects |
| Product execution of the four helpers uses DEP-001 only | Graph v2; DEP-001 available | Hidden BLAS/NR call appears in path details | `/trace-path` for those four IDs; graph rows XP-060/061/072/108 |

## 3. Slice plan

Each slice follows recover → refine → design → implement → UAT. A slice may start when **its** prerequisites are resolved, even if other slices still have open later-slice ADRs.

Named remaining work for S1 (not prerequisite IDs): path test plans for the four XP IDs (RISK-002); Architect ADR for HTTP routes, JSON field names, and invalid-input status (RISK-001 / IMP-004). That ADR is required before `/plan-port-story`, not before `/document-legacy`.

| Slice | Objective | XP IDs | Prerequisites | Structure fidelity | Oracle evidence | Exit criteria | Forecast |
|-------|-----------|--------|---------------|--------------------|-----------------|---------------|----------|
| S1 First product slice | Four numeric helpers as HTTP/JSON with per-path parsed-numeric parity | XP-060, XP-061, XP-072, XP-108 | [x] DEC-001; [x] DEC-002; [x] DEC-003; [x] DEC-004; [x] DEC-005; [x] DEC-006; [x] DEC-007; [x] DEP-001 (`available`). No DEF-NNN. | HTTP host: rewrite (DEC-003 / IMP-001). Numeric semantics: preserve (DEC-005 / IMP-002). Indexing on the wire is JSON 0-based (IMP-003 rewrite of Fortran 1-based storage, not wrap-Fortran). | T1 executable (bounded): Darwin 25.5.0 arm64 / gfortran 16.1.0 / OpenBLAS 0.3.34 / `FFT_BACKEND=NR`; live XP-388 stdout for those four operations. FIX IDs TBD until path test plans. | Four operations served over HTTP/JSON; per-path numeric PASS vs that T1 pin; no DEP-006/DEP-004 in the C# tree; Human M5 | Small (four operations). Confidence low until measured. Recalibrate after this slice completes. |
| S2 Further DEP-001-only numeric APIs | Additional library APIs callers would consume as services, still DEP-001-only | Inventory IDs after `/document-legacy` on S1 (example neighbor: XP-062 `arange`, already printed by XP-388 but out of S1 per DEC-006) | DEC-001, DEC-002, DEC-003, DEC-004, DEC-005, DEC-007; DEP-001. Recalibrate from S1. No DEF-NNN yet. | Same as S1: HTTP rewrite, numeric preserve | Same T1 pin unless a vN+1 probe reclassifies | Selected XP IDs as HTTP/JSON with per-path parity | Recalibrate from S1 effort and parity defect rate |
| S3 Rank-2 / MATRIX / LAPACK | Later inventory: rank-2 layout and LAPACK-shaped APIs | Inventory (IMP-007; graph DEP-003 on MATRIX-class paths). FFT/gnuplot/`SYSTEM` still out until their slices. | DEC-001, DEC-002, DEC-007 (do not copy DEP-004); DEP-003 (`available`). Package choice is an ADR, not a missing first-slice DEC. | HTTP rewrite; numeric preserve; layout per later ADR | T1 only if those paths are probed; do not claim S1 driver coverage | Inventory XP IDs + accepted layout/provider ADR | Recalibrate; do not date |
| S4 FFT | Later inventory: FFTGF backends | Inventory XP-092–XP-105, XP-382, XP-397 (IMP-011). Do not copy DEP-006. | DEC-007; DEP-005/DEP-024 routes are `available` without copying NR. FFT provider ADR required before that slice implements. | HTTP rewrite; do not wrap NR FFT | New probe if backend/host changes | Inventory XP IDs + FFT ADR | Recalibrate; do not date |
| S5 I/O, `SYSTEM`, gnuplot | Later inventory: POSIX/`SYSTEM` and plot scripts | Inventory (IMP-012; DEP-016, DEP-017 `reimplementable`) | DEC-001, DEC-002. gnuplot substitution ADR when this slice starts. | HTTP rewrite (emit data, not host gnuplot as a service) | T1 does not cover these as S1 fixtures | Inventory XP IDs + substitution ADR | Recalibrate; do not date |
| S6 Zhang/Jin specials | Later inventory: file-scope special functions | XP-205–XP-369 (DEP-009 `reimplementable`; IMP-014). XP-108 `fermi` is already in S1 and must not paste Zhang/Jin sources. | DEC-007 containment still; DEP-009 used-subset rewrite when sliced. Acknowledgment ADR if algorithms are incorporated. | HTTP rewrite of the used subset | T1 S1 driver does not cover this corpus | Inventory XP IDs + provenance ADR | Recalibrate; do not date |

Not a product slice: XP-386, XP-387, XP-388, XP-389 (DEC-004 / IMP-006). Numutils CLIs XP-370–XP-385 stay out of S1 (DEC-006).

Prerequisites list only IDs that bind this slice. Do not paste global blocker counts.

## 4. Per-slice ADD loop

| Slice | Recover | Refine | Design | Implement | UAT / parity |
|-------|---------|--------|--------|-----------|--------------|
| S1 | `/document-legacy` scoped to XP-060, XP-061, XP-072, XP-108 (`/trace-path` those IDs) | `/refine-feature`; `/recover-domain` | HTTP/JSON ADR (routes, fields, `num<0` status; optional `istart`/`iend`/`base`); `/plan-path-tests`; `/design-application` / walking skeleton | `/plan-port-story` then `/complete-port-story` | `/verify-parity` + human M5 (T1 pin; per-path parsed numbers) |
| S2–S6 | `/document-legacy` scoped to that slice’s inventory XP IDs | `/refine-feature` | Slice ADRs from §5 | `/plan-port-story` then `/complete-port-story` | `/verify-parity` + human M5 under the then-current oracle |

## 5. Recommended ADRs

Architect writes or accepts ADR files. This table is a trigger list.

| Decision | Suggested ADR | Status | Affected slices |
|----------|---------------|--------|-----------------|
| Document C# / .NET 8 / ASP.NET Core HTTP/JSON as the process boundary (DEC-001 already decided) | Target stack: ASP.NET Core HTTP/JSON services | recommended | S1–S6 |
| HTTP routes, JSON field names, and invalid-input status for `linspace`/`logspace` (`num<0`) plus `deriv`/`fermi` | First-slice HTTP/JSON contract (RISK-001 / IMP-004) | recommended | S1 |
| Optional Fortran args `istart`/`iend` (XP-060) and `base` (XP-061): required JSON fields vs omitted with Fortran defaults | Optional grid-argument JSON presence (impedance §9 Q2) | recommended | S1 |
| Hexagonal ports: numeric domain vs HTTP adapters (no Fortran ABI port) | Application boundary for rewritten helpers | recommended | S1 |
| Rank-2 / matrix JSON layout (row-major vs column-major) | Matrix serialization (IMP-007; impedance §9 Q3) | recommended | S3 |
| LAPACK-shaped .NET provider (do not copy DEP-004) | BLAS/LAPACK substitution (DEP-003) | recommended | S3 |
| FFT provider (do not copy DEP-006 NR) | FFT substitution (IMP-011) | recommended | S4 |
| gnuplot host vs plottable JSON/data | Plot/output substitution (DEP-017) | recommended | S5 |
| Zhang/Jin used-subset rewrite and acknowledgment | Special-function provenance (DEP-009) | recommended | S6 |

No persistence, auth, or cutover ADR is required for this library POC.

## 7. Forecast and recalibration

| Milestone | Initial estimate | Confidence | Recalibration trigger |
|-----------|------------------|------------|-----------------------|
| S1 (XP-060, XP-061, XP-072, XP-108) | Small: four HTTP/JSON operations plus T1 parity harness use | low | First completed port slice: measured implement/review/parity effort and parity defect rate |
| Remaining inventory (S2–S6) | Do not date. 389 active paths are not a first-slice gate. | low | Recalibrate after S1 using that effort and defect rate; sequence remaining inventory groups from the graph |

Do not promise a calendar date.

## 8. Deferred modernization debt

| Debt item | Why preserved initially | Refactor trigger | Backlog item |
|-----------|-------------------------|------------------|--------------|
| Rank-2 Fortran layout (IMP-007) | S1 helpers are 1D/scalar; layout-neutral JSON arrays | S3 | TBD |
| FFT backend compile-time symlink (IMP-011) | Out of S1 (DEC-006) | S4 | TBD |
| `SYSTEM` / gnuplot process coupling (IMP-012) | Out of S1 (DEC-006) | S5 | TBD |
| Zhang/Jin file-scope corpus (IMP-014 / DEP-009) | `fermi` rewrite must not paste that corpus; remainder out of S1 | S6 | TBD |

S1 does not preserve a Fortran wrap or `libscifor.a` ABI on purpose (DEC-002, DEC-003).

## Links

- Assessment: [`docs/modernization/ASSESSMENT.md`](ASSESSMENT.md)
- Decision register: [`docs/modernization/decision-register.md`](decision-register.md)
- Dependency graph: [`docs/modernization/dependency-graph.md`](dependency-graph.md)
- Defect ledger: [`docs/modernization/defect-ledger.md`](defect-ledger.md) (not written; no DEF-NNN)
- Oracle: [`docs/modernization/oracle.md`](oracle.md)

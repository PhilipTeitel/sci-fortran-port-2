# Modernization Assessment

**Legacy repo:** `/Users/philipteitel/code/ADD-migrations/sci-fortran-legacy`
**Source stack:** Fortran 90/95 (free-form; F2003 command-line intrinsics) / unknown (checkout e586903; gfortran 16.1.0 on T1 host) / none (static library `libscifor.a`)
**Target stack:** C# / .NET 8 / ASP.NET Core (HTTP/JSON services)
**Date:** 2026-08-25
**Verdict:** go-with-conditions
**Oracle tier:** T1 executable (bounded)
**Owner:** Migration Strategist (`/assess-modernization`)

## Summary

SciFor/SciFortran is a Fortran 90/95 numeric library (`libscifor.a`) plus CLIs and shell jobs, with no HTTP host. A port of **XP-060** (`TOOLS::linspace`), **XP-061** (`TOOLS::logspace`), **XP-072** (`TOOLS::deriv`), and **XP-108** (`FUNCTIONS::fermi`) to C# / .NET 8 ASP.NET Core HTTP/JSON is feasible as `go-with-conditions` on a bounded **T1 executable** oracle (Darwin 25.5.0 arm64, Homebrew gfortran 16.1.0, OpenBLAS 0.3.34, `FFT_BACKEND=NR`). Those four paths use DEP-001 only (graph v2); they do not require wrapping `libscifor.a` or copying DEP-006 / DEP-004. Conditions that bind this slice are owner-stated (record via `/record-decision`), plus path test plans with per-path parsed-numeric bounds and an Architect ADR for HTTP routes, JSON names, and invalid-input status (IMP-004). Harness XP-386, XP-387, XP-388 stay oracle tooling (IMP-006). Look next at `/record-decision`.

---

## 1. Scope snapshot

| Item | Count / value | Where the detail lives |
|------|---------------|------------------------|
| Active execution paths | 389 | Path inventory v1 |
| Suspected-dead / unknown / retired | 11 | Path inventory v1 appendix |
| Dependencies | 29 (`no-route` 0, `undecided` 0) | Dependency inventory v2 |
| Highest-severity impedance | IMP-001 (critical) | Impedance analysis v1 |
| Oracle tier | T1 executable (bounded) | Oracle v1 |
| Candidate first slice | XP-060, XP-061, XP-072, XP-108 (product HTTP/JSON); XP-386, XP-387, XP-388 (T1 oracle tooling, not product) | Will be the first migration-plan row |

## 2. Feasibility verdict

| Dimension | Result | Condition if not pass |
|-----------|--------|------------------------|
| Setup and runnability | pass | none (oracle v1 E1: snapshot build and two identical XP-388 runs) |
| Path inventory completeness | pass | none for this slice (XP-060, XP-061, XP-072, XP-108 are active library APIs) |
| First-slice dependency routes | pass | none for product execution (graph: those four → DEP-001 only). XP-388 links DEP-003; helpers do not call BLAS. |
| Impedance for first slice | conditional | IMP-001 rewrite-not-wrap (owner-stated); IMP-004 HTTP mapping of `error`/`STOP`; IMP-002 / IMP-003 / IMP-005 per-path in test plans and ADR |
| Oracle and parity | conditional | T1 on the pinned host only. Parsed numbers per path; not Fortran format text; not `fidelity/golden/` or XP-387 Python goldens. Path test plans own bounds (profile `0.000001` is not a gate). |
| Security containment | pass | Do not copy DEP-006 or DEP-004 into the C# tree. First-slice *execution* of the four helpers does not require those units (owner-stated; LICENSE.md is methodology-eval, not a go-blocker). |

## 3. Risk register

| ID | Risk | Likelihood | Impact | Evidence | Mitigation | Owner | Retire when |
|----|------|------------|--------|----------|------------|-------|-------------|
| RISK-001 | HTTP/JSON contract (routes, field names, `num<0` status) is unspecified, so IMP-001/IMP-004 could be implemented inconsistently | medium | high | E2 owner: Architect ADR required before this slice is implemented | ADR before `/plan-port-story` for this slice | Architect | ADR accepted for XP-060, XP-061, XP-072, XP-108 |
| RISK-002 | Parity compared with profile hint, XP-387 `1e-10`, or golden files instead of per-path parsed numbers vs live T1 stdout | medium | high | E2 owner parity rule; oracle v1 forbids treating XP-387 goldens as the T1 corpus | Path test plans name FIX IDs and numeric bounds; use XP-388 stdout, not `fidelity/golden/` | Architect | Path test plans exist for the four product XP IDs |
| RISK-003 | T1 is this Darwin/gfortran/OpenBLAS/NR snapshot; other hosts are a different oracle | medium | medium | E1 oracle v1 §2 | Keep first-slice parity on this pin; vN+1 probe if CI cannot reproduce | Human | `/record-decision` records the pin, or a new probe reclassifies |
| RISK-004 | Snapshot `libscifor.a` contains DEP-006 / DEP-004 objects even though the four helpers do not call them | low | high | E1 oracle v1 §3 | Snapshot-only oracle; do not copy those sources into C# | Implementer | First-slice C# tree has no NR/MKL headers or objects |

## 4. Conditions to start the first slice

Owner-stated (E2 Human test-double / profile). Recorded as DEC-001–DEC-007. Unchecked items are named remaining work, not open owner questions.

- [x] Target is C# / .NET 8 / ASP.NET Core HTTP/JSON services (`modernization.targetStack`). (DEC-001)
- [x] Product boundary (IMP-001): not a Fortran `.mod` / `libscifor.a` drop-in; callers invoke HTTP/JSON. (DEC-002)
- [x] First-slice tactic (IMP-001): rewrite XP-060, XP-061, XP-072, XP-108 in C# behind those services; do not wrap `libscifor.a` for the POC product. (DEC-003)
- [x] Oracle pin: Darwin 25.5.0 arm64 / Homebrew gfortran 16.1.0 / OpenBLAS 0.3.34 / `FFT_BACKEND=NR` as the numeric oracle for those four operations (oracle.md v1). Preserve XP-386, XP-387, XP-388 as oracle tooling, not product (IMP-006). (DEC-004)
- [x] Parity: parsed numbers within a **per-path** tolerance. Not byte-identical Fortran format text. Not a global tolerance (IMP-002, IMP-005). Do not use `fidelity/golden/` or `fidelity.sh` Python goldens as the observed corpus. (DEC-005)
- [x] POC bar: success is those four operations as HTTP/JSON with that numeric parity. Numutils CLIs XP-370–XP-385 are not product services in this slice. XP-386, XP-387, XP-389 are not product services. FFT/LAPACK/MATRIX/gnuplot/`SYSTEM` paths are out of this slice. (DEC-006)
- [x] Containment: do not copy Numerical Recipes (DEP-006) or Intel Confidential MKL headers (DEP-004) into the C# tree. (DEC-007)
- [ ] Path test plans for XP-060, XP-061, XP-072, XP-108 with per-path parsed-numeric bounds (not the profile `0.000001` hint as a gate).
- [ ] Architect ADR for HTTP routes, JSON field names, and invalid-input status (IMP-004). Optional Fortran args (`istart`/`iend`/`base`) are ADR scope, not a first-slice owner TBD. Rank-2 layout is a later slice.

## 5. Walking-skeleton feasibility

**Result:** normal skeleton viable

- **Smallest useful executable path:** XP-060, XP-061, XP-072, XP-108 as four ASP.NET Core HTTP/JSON operations, compared to bounded T1 `scifor-fidelity` (XP-388) stdout
- **Why this result:** The four library APIs are DEP-001-only, have no host coupling, and already run twice bit-identically on the pinned T1 driver. A black-box wrap of `libscifor.a` is excluded by product intent (IMP-001).
- **Revisit when:** M1 is recorded and the HTTP ADR exists, or a new oracle probe changes the tier

## 6. Tensions

| Canonical ID | Kind | Required resolution |
|--------------|------|---------------------|
| IMP-001 | owner decision | `/record-decision` for rewrite HTTP/JSON, not wrap `libscifor.a` |
| IMP-004 | ADR | Architect HTTP error/status contract for `linspace`/`logspace` `num<0` |
| IMP-005 / oracle v1 §4 | owner decision | `/record-decision` for parsed-numeric parity; exclude XP-387 goldens |
| Impedance §9 Q2 | ADR | JSON presence of optional `istart`/`iend`/`base` (not an owner TBD) |

## Links

- Path inventory: [`docs/modernization/execution-path-inventory.md`](execution-path-inventory.md)
- Dependency inventory: [`docs/modernization/dependency-inventory.md`](dependency-inventory.md)
- Dependency graph: [`docs/modernization/dependency-graph.md`](dependency-graph.md)
- Impedance analysis: [`docs/modernization/impedance-analysis.md`](impedance-analysis.md)
- Oracle: [`docs/modernization/oracle.md`](oracle.md)
- Decision register: [`docs/modernization/decision-register.md`](decision-register.md)

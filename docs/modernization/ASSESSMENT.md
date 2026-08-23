<!-- Modernization assessment contract:
- This is the five-minute document. A reader should understand scope and risk without opening the inventories.
- Preserve headings and order. Omit a section only when the INCLUDE WHEN comment says to.
- Verdict uses configured statuses only: go, go-with-conditions, no-go.
- Conditions and blockers name the first slice, not global unresolved counts.
- Cite PATH / DEP / IMP / RISK / DEC IDs. Do not restate their tables.
- One evidence grade per claim.
- Current state lives in cells. No prose changelog.
-->

# Modernization Assessment

**Legacy repo:** `/Users/philipteitel/code/ADD-migrations/sci-fortran-legacy` (`e586903a26cc50ca8942f20ca3bccbd8814e6252`)
**Source stack:** Fortran 90/95 free-form modules / static library `libscifor.a` / no application framework
**Target stack:** C# / .NET 8 / ASP.NET Core (HTTP/JSON services)
**Date:** `2026-08-22`
**Verdict:** `go-with-conditions`
**Oracle tier:** `T1 executable` (bounded to the core library plus the in-tree fidelity driver)
**Owner:** Migration Strategist (`/assess-modernization`)

## Summary

SciFor/SciFortran is a Fortran 90/95 numeric class library (`libscifor.a`) with no hosted application. A POC port to **C# / .NET 8 ASP.NET Core HTTP/JSON services** is feasible for a first numeric slice: rewrite `linspace`, `logspace`, `fermi`, and `deriv` as HTTP/JSON operations and compare **parsed numbers** to the bounded T1 oracle (macOS arm64 / gfortran 16.1.0 / OpenBLAS 0.3.34). That T1 is not continuous parity for unexecuted FFT/LAPACK/RNG/CLI/`SYSTEM` surfaces, and it is not a comparison rule. Candidate first slice: XP-062, XP-063, XP-074, XP-110, with T1 harness XP-223, XP-224, XP-225. Owner-stated inputs that bind *that* slice are listed in §4 (not TBD): rewrite not wrap, not a Fortran ABI drop-in, this T1 pin, per-path parsed-numeric tolerance, four services as the POC bar, license not a POC constraint. Remaining first-slice work is `/record-decision` (assign DEC-NNN to those statements), path test plans, and an HTTP-contract ADR — not an unnamed stack. Next: `/record-decision` so M1 can accept this verdict with DEC IDs.

---

## 1. Scope snapshot

| Item | Count / value | Where the detail lives |
|------|---------------|------------------------|
| Active execution paths | 225 (237 enumerated) | Path inventory v1 Snapshot |
| Suspected-dead / unknown / retired | 12 | Path inventory appendix |
| Dependencies | 29 (`no-route` 1, `undecided` 0) | Dependency inventory v2 Snapshot |
| Highest-severity impedance | IMP-001, IMP-010 (critical; first slice); IMP-024, IMP-025, IMP-029 (critical; later slices) | Impedance analysis v2 Snapshot |
| Oracle tier | `T1 executable` (bounded) | Oracle v1 Snapshot |
| Candidate first slice | XP-062, XP-063, XP-074, XP-110 (library → HTTP/JSON); XP-223, XP-224, XP-225 (T1 harness) | Will be the first migration-plan row |

## 2. Feasibility verdict

| Dimension | Result | Condition if not pass |
|-----------|--------|------------------------|
| Setup and runnability | pass (bounded T1 corpus); conditional (any T1 expansion) | Expanding T1 needs a new oracle probe (v2), containment, and a decision before executing NR/MKL units. E1 oracle.md §1, §3. |
| Path inventory completeness | pass for first-slice IDs | Completeness of 225 active rows is E3 inventory; first-slice library symbols and driver calls are named. Unexecuted library/CLI surfaces stay out of this T1. |
| First-slice dependency routes | pass | Library XP-062/063/074/110 use DEP-001 only (`available`). Product host is ASP.NET Core (DEP-003 `reimplementable` as HTTP/JSON). XP-224 uses DEP-007, DEP-008 (`available`). DEP-015 `no-route` binds XP-232/236 only. XP-223's extra DEPs are full-archive build/link; they do not bind first-slice *execution* of the four helpers. |
| Impedance for first slice | pass (pattern named) | IMP-001, IMP-002, IMP-010, IMP-012, IMP-013, IMP-014, IMP-035 bind the library APIs as **rewrite**. IMP-006 binds harness extract (parsed numeric, not Fortran text). IMP-011 is **preserve** of this T1 pin. NR/MKL/LAPACK IMPs do not bind first-slice execution. HTTP route/JSON field names are an ADR, not an unnamed analogue. |
| Oracle and parity | conditional | Bounded T1 supports repeated live comparison for the executed driver calls on this host pin. No path test plans yet; they must record per-path parsed-numeric bounds. `fidelity/golden/` is rewritten formulas, not observed corpus. Unexecuted surfaces are outside this T1. |
| Security containment | conditional | Legacy must not run in-place (E1). NR FFT is compiled into the T1 archive but unused by the driver; do not retain binaries. OS `sandbox-exec` was unavailable in the probe agent. POC license policy does not remove containment. |

Do not fail a dimension because *other* paths have unresolved dependencies.

## 3. Risk register

| ID | Risk | Likelihood | Impact | Evidence | Mitigation | Owner | Retire when |
|----|------|------------|--------|----------|------------|-------|-------------|
| RISK-003 | Kind-8 numerics, 1-based indexing, and JSON round-trip compared without per-path numeric bounds | medium | high | E3 IMP-001, IMP-002, IMP-006, IMP-035; E1 oracle.md: T1 is runnability, not a tolerance. E2 owner: parsed numbers, per-path tolerance (bound not chosen). | Path test plans with parsed-numeric rules per XP; no global tolerance; do not use `fidelity/golden/` as observed oracle | Architect | Test plans exist for XP-062/063/074/110 (and harness extract for XP-224/XP-225) |
| RISK-005 | Containment: in-place legacy runs, or later reuse of NR-compiled T1 archives, leaks unused proprietary compile units | medium | high | E1 oracle.md §3; E3 DEP-014 compiled, unused by driver | Keep archive-only execution; do not retain binaries; do not copy NR/MKL sources unless a later slice explicitly does so under POC policy | Implementer / oracle operator | First-slice work uses contained snapshots only; NR/MKL stay out of first-slice *execution* |
| RISK-006 | HTTP/JSON contract (routes, field names, invalid-input status) is not designed | medium | medium | E2 owner: HTTP/JSON services; E3 IMP-010, IMP-012, IMP-013. No ADR yet. | Architect ADR before `/plan-port-story` | Architect | ADR names routes/JSON/error mapping for the four operations |

Retired relative to the prior assessment (owner-stated; record as DEC-NNN at M1, do not keep as open target risk): unnamed stack; Fortran ABI vs managed API; rejection of this T1 pin.

Later-slice legal/provider risks (DEP-014/020 NR, DEP-012 MKL headers, DEP-015 FFTPACK, DEP-018 Zhang/Jin) do not bind this verdict. They belong in later slice prerequisite tables. Owner policy: licensing is not a constraint for this non-production POC (E2).

## 4. Conditions to start the first slice

**Owner-stated (not TBD). `/record-decision` assigns DEC-NNN so later artifacts can cite them. M1 is accepting the verdict and recording these answers.**

- [ ] **Target stack.** C# / .NET 8 / ASP.NET Core HTTP/JSON services. (E2 owner; `.cursor/workflow.config.yml`)
- [ ] **Product boundary / DEP-003 / IMP-010.** Not a Fortran `.mod` / `libscifor.a` drop-in. Callers invoke HTTP/JSON. (E2 owner)
- [ ] **First-slice tactic / IMP-010.** Rewrite XP-062, XP-063, XP-110, XP-074 in C# behind those services. Do not wrap `libscifor.a` for the POC product. (E2 owner)
- [ ] **Oracle pin / IMP-011.** Accept this T1 pin (macOS arm64 / gfortran 16.1.0 / OpenBLAS 0.3.34 / `FFT_BACKEND=NR` build) as the numeric oracle for those four operations. (E2 owner; E1 oracle.md)
- [ ] **Parity rule.** Parsed numbers within a **per-path** tolerance. Not byte-identical Fortran format text. Not a global tolerance. (E2 owner; IMP-001, IMP-006)
- [ ] **POC bar.** Success is those four operations as services with that numeric parity. numutils CLIs, gnuplot/`SYSTEM`, and FFT/LAPACK are out of this slice. License risk is not a POC constraint. (E2 owner)

**Still required before implementing that slice (named work, not unnamed stack):**

- [ ] Path test plans for XP-062, XP-063, XP-074, XP-110, and harness extract for XP-224/XP-225, with per-path parsed-numeric bounds. Do not treat `fidelity/golden/` as observed oracle. (RISK-003)
- [ ] Architect ADR for HTTP routes, JSON field names, and invalid-input status codes. (RISK-006; IMP-010, IMP-012, IMP-013)

Not first-slice conditions: DEP-015 FFTPACK `no-route`; NR FFT (DEP-014); MKL headers (DEP-012); Zhang/Jin (DEP-018); CHRPACK (DEP-024); OpenMP (DEP-028); gnuplot (DEP-025); libmatheval (DEP-026); BLAS provider for MATRIX (DEP-009–DEP-011).

## 5. Walking-skeleton feasibility

**Result:** `normal skeleton viable`

- **Smallest useful executable path:** Four ASP.NET Core HTTP/JSON operations corresponding to XP-062 (`TOOLS::linspace`), XP-063 (`TOOLS::logspace`), XP-110 (`FUNCTIONS::fermi`), and XP-074 (`TOOLS::deriv`), compared as parsed numbers to the bounded T1 driver corpus (XP-225 / XP-224). XP-223 is the legacy archive build used by that T1, not a product feature.
- **Why this result:** The four library paths are rank-1 helpers on DEP-001 only; T1 already executed them (E1). The owner named C# / HTTP/JSON and rewrite, so the skeleton is a hosted service with four routes, not a Fortran ABI wrap and not a class-library-only demo.
- **Revisit when:** An ADR changes the host (not HTTP/JSON) or a new oracle probe drops this T1 pin.

## 6. Tensions

| Canonical ID | Kind | Required resolution |
|--------------|------|---------------------|
| Oracle T1 vs user-profile `oracleTier: T3 documented-only` | owner decision | This assessment uses the probe's bounded T1. Project `.cursor/workflow.config.yml` now sets `oracleTier: T1 executable` so later commands do not inherit T3. |
| XP-225 | docs vs code | Driver section labeled `arange-5` prints `real(i,8)` for `i=1..5` and does not call `TOOLS::arange`. |
| `fidelity/golden/` vs observed T1 stdout | owner decision | Script goldens are rewritten Python formulas (E3 oracle.md). Path test plans must not use them as the observed corpus. |
| Graph v1 summary vs inventory v2 | snapshot vs later inventory | Dependency graph v1 Snapshot still *cites* inventory v1 Draft dispositions in its prose summary. Join tables (XP ↔ DEP) are unchanged; do not rewrite the graph snapshot to copy v2 dispositions. |

## Links

- Path inventory: `docs/modernization/execution-path-inventory.md`
- Dependency inventory: `docs/modernization/dependency-inventory.md`
- Dependency graph: `docs/modernization/dependency-graph.md`
- Impedance analysis: `docs/modernization/impedance-analysis.md`
- Oracle: `docs/modernization/oracle.md`
- Decision register: `docs/modernization/decision-register.md` (not written; `/record-decision` assigns DEC-NNN)
- Project workflow profile: `.cursor/workflow.config.yml`

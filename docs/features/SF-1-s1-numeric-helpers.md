<!-- Port story contract:
- Based on the standard user-story template, with modernization provenance and parity sections.
- Slice prerequisites are copied from the migration-plan row and checked off here. Do not edit analysis snapshots.
- Every covered XP-NNN must have a path detail, a path test plan, singular evidence grades, and Phase P coverage — unless no fixture exists, in which case do not invent a parity criterion.
- Comparison rules come from the path test plan, not a global default.
- Phase P is mandatory before Phase Y when a fixture or acceptance-data source exists. Z8 is mandatory in Phase Z.
- INCLUDE §5 only when the path exposes an HTTP/RPC/message API. INCLUDE §6 only when UI-facing. Omit entirely otherwise — do not write "None." placeholders for whole sections.
-->

# SF-1: S1 numeric helpers HTTP/JSON

**Story**: As a scientific/numeric HTTP client, I want linspace, logspace, deriv, and fermi available as HTTP/JSON so that I can exercise rewritten SciFor helpers and compare parsed numbers to the bounded T1 oracle.
**Epic**: 1 — S1 numeric helpers HTTP/JSON
**Size**: Medium
**Status**: Complete
**Modernization slice:** `phased-rewrite`
**Structure fidelity:** `HTTP rewrite (refactor-now); numeric preserve`
**Execution paths:** `XP-060, XP-061, XP-072, XP-108`

---

## 1. Summary

Port migration-plan slice S1: rewrite `TOOLS::linspace`, `TOOLS::logspace`, `TOOLS::deriv`, and `FUNCTIONS::fermi` as ASP.NET Core HTTP/JSON operations behind hexagonal domain ports (ADR-001–ADR-004). Prove correctness with REQ-001 scenarios and **per-path** Phase P parity against live T1 / XP-388 (oracle.md v1): tolerance-based parsed numbers with relative `1e-12` or absolute `1e-12` (whichever is larger), per path test plans / DEC-005. FIX files are **not yet written**; Human accepts live T1 (and provisional FIX-060/061/072/108 names) as Phase P acceptance data until `/build-oracle build`. DEF-002 reproduce-faithfully; DEF-001/003/004/005/006 fix-now → HTTP 400.

### 1a. Domain model touchpoints

| Purpose / domain section | Terms / entities / fields / invariants touched | Evidence |
|--------------------------|-----------------------------------------------|----------|
| `docs/PURPOSE.md` Thesis / north-star | Four helpers as HTTP/JSON with per-path T1 parity; anti-thesis: no ABI wrap, no production redistribution | E2 PURPOSE; DEC-002–DEC-006 |
| `docs/DOMAIN.md` §2–§4 Linear grid | `LinearGrid` start/stop/num/includeStart/includeStop/spacing/values; both-endpoints default; abort → 400 mapping | E3 XP-060; ADR-002/004 |
| `docs/DOMAIN.md` §2–§4 Logarithmic grid | `LogarithmicGrid` + zero rewrite `1e-12` (DEF-002); base default 10 | E3 XP-061; DEF-002 |
| `docs/DOMAIN.md` §2–§4 Sampled series / finite-difference | `SampledSeries`, `FiniteDifferenceDerivative` stencil; L≥2 and dh≠0 enforced (DEF-005/006) | E3 XP-072 |
| `docs/DOMAIN.md` §2–§4 Fermi–Dirac occupation | Formula + overflow cutoff `x*beta > 100` → 0 | E3 XP-108; E1 oracle |
| `docs/DOMAIN.md` §6 consistency boundaries | One request-scoped computation per helper | E2 DOMAIN §6 |

### 1b. Legacy source touchpoints

Taken from the path detail. One grade per row.

| Legacy artifact | Role in story | Evidence grade | Citation |
|-----------------|---------------|----------------|----------|
| `src/tools_grids.f90` linspace body | XP-060 algorithm + num guards | E3 | XP-060 path detail §1 |
| `src/tools_grids.f90` logspace body | XP-061 algorithm + zero rewrite | E3 | XP-061 path detail §1 |
| `src/TOOLS.f90` deriv body | XP-072 stencil | E3 | XP-072 path detail §1 |
| `src/FUNCTIONS.f90` fermi body | XP-108 formula + clamp | E3 | XP-108 path detail §1 |
| `fidelity/driver.f90` (XP-388) | T1 probe cases for four helpers | E3 | path details §1; oracle.md §1 |
| `docs/modernization/oracle.md` §1 | Bit-identical XP-388 stdout pin (E1 values) | E1 | oracle.md §1 |
| `docs/modernization/defect-ledger.md` | DEF-001–DEF-006 decisions | E2 | defect ledger §1 |

## 2. Linked architecture decisions (ADRs)

- `docs/decisions/ADR-001-target-stack-aspnet-http-json.md` — C# / .NET 8 / ASP.NET Core HTTP/JSON
- `docs/decisions/ADR-002-s1-http-json-contract.md` — routes, JSON shapes, HTTP 400 Problem Details
- `docs/decisions/ADR-003-hexagonal-numeric-vs-http.md` — domain vs HTTP; no `libscifor.a` P/Invoke
- `docs/decisions/ADR-004-optional-grid-arg-json-fields.md` — optional `istart`/`iend`/`mesh`/`base` defaults

Also cite: DEC-001–DEC-007 (target, rewrite, oracle pin, per-path parity, slice scope, containment); DEP-001 available.

## 3. Definition of Ready (DoR)

- [x] Purpose and domain touchpoints are current.
- [x] Covered `XP-NNN` path details exist and use singular evidence grades.
- [x] Path test plans exist; comparison rules are per path.
- [x] No covered claim is `E4 inferred` or `E5 unknown` unless a `DEC-NNN` accepts it.
- [x] Oracle tier is compatible with the planned Phase P evidence.
- [x] Defect-ledger decisions are recorded for known mismatches on these paths.
- [x] Slice prerequisites below are resolved.
- [x] Linked ADRs are `Accepted` or this story is explicitly a spike.

**DoR note (acceptance-data gap, non-blocking):** Path test plans record **fixture not yet written** (provisional FIX-060/061/072/108). Human accepts **live T1 / XP-388** on the oracle.md v1 pin as Phase P acceptance data for this POC until `/build-oracle build` records FIX files.

## 3b. Slice prerequisites

Copied from the migration-plan S1 row (plus defect decisions recorded after `/ledger-defects`). Check off here. Do not edit analysis snapshots.

| ID | Kind | What must be true | Status |
|----|------|-------------------|--------|
| DEC-001 | decision | Target is C# / .NET 8 / ASP.NET Core HTTP/JSON | resolved |
| DEC-002 | decision | Product is HTTP/JSON, not Fortran ABI drop-in | resolved |
| DEC-003 | decision | Rewrite helpers in C#; do not wrap `libscifor.a` | resolved |
| DEC-004 | decision | Bounded T1 pin is numeric oracle; XP-386/387/388 tooling only | resolved |
| DEC-005 | decision | Comparison is per-path parsed numbers (not global / not format text) | resolved |
| DEC-006 | decision | S1 bar is XP-060/061/072/108 only | resolved |
| DEC-007 | decision | Do not copy DEP-006 / DEP-004 into C# tree | resolved |
| DEP-001 | dependency | Runtime math available (`available`) | resolved |
| DEF-001 | defect | fix-now: `num=0` + excluded endpoint → HTTP 400 | resolved |
| DEF-002 | defect | reproduce-faithfully: zero endpoints → `1e-12` | resolved |
| DEF-003 | defect | fix-now: negative log endpoints → HTTP 400 | resolved |
| DEF-004 | defect | fix-now: invalid `base` → HTTP 400 | resolved |
| DEF-005 | defect | fix-now: `size(f)<2` → HTTP 400 | resolved |
| DEF-006 | defect | fix-now: `dh==0` → HTTP 400 | resolved |

## 4. Binding constraints (non-negotiable)

- Target stack and process boundary: ASP.NET Core HTTP/JSON rewrite (ADR-001, ADR-003; DEC-001–DEC-003).
- Wire contract: ADR-002 routes/JSON; ADR-004 optional field defaults; invalid → **400** Problem Details.
- No P/Invoke to `libscifor.a`; no DEP-006/DEP-004 materials in the C# tree (DEC-007).
- Per-path comparison only: relative `1e-12` or absolute `1e-12` (whichever larger) for numeric success paths — from each XP path test plan (DEC-005). No system-wide tolerance.
- Defect policies as ledgered (DEF-001–DEF-006).

## 4b. Ports & Adapters

| Port | Adapter | Boundary owned | Contract test | Integration test |
|------|---------|----------------|---------------|------------------|
| `ILinearGrid` | `LinearGridHttpAdapter` (`POST /v1/linspace`) | JSON ↔ `LinearGrid` compute + 400 mapping | `tests/SciFor.Domain.Tests/LinearGridPortContractTests.cs` | `tests/SciFor.Api.Tests/LinspaceEndpointTests.cs` |
| `ILogarithmicGrid` | `LogarithmicGridHttpAdapter` (`POST /v1/logspace`) | JSON ↔ `LogarithmicGrid` + DEF-002/003/004 | `tests/SciFor.Domain.Tests/LogarithmicGridPortContractTests.cs` | `tests/SciFor.Api.Tests/LogspaceEndpointTests.cs` |
| `IFiniteDifference` | `FiniteDifferenceHttpAdapter` (`POST /v1/deriv`) | JSON ↔ derivative + DEF-005/006 | `tests/SciFor.Domain.Tests/FiniteDifferencePortContractTests.cs` | `tests/SciFor.Api.Tests/DerivEndpointTests.cs` |
| `IFermiDirac` | `FermiDiracHttpAdapter` (`POST /v1/fermi`) | JSON ↔ occupation (scalar) | `tests/SciFor.Domain.Tests/FermiDiracPortContractTests.cs` | `tests/SciFor.Api.Tests/FermiEndpointTests.cs` |

## 5. API Endpoints + Schemas

| Endpoint / schema | Purpose | Request / input | Response / output | Covers |
|-------------------|---------|-----------------|-------------------|--------|
| `POST /v1/linspace` | Linear grid | `start`, `stop`, `num`, `istart?`, `iend?`, `mesh?` (bool) | `values[]`, optional `mesh` number | XP-060; S1–S11 |
| `POST /v1/logspace` | Logarithmic grid | `start`, `stop`, `num`, `base?` | `values[]` | XP-061; S12–S18 |
| `POST /v1/deriv` | Finite-difference derivative | `f[]`, `dh` | `df[]` | XP-072; S19–S22 |
| `POST /v1/fermi` | Fermi–Dirac occupation | `x`, `beta` | `value` | XP-108; S23–S26 |

Invalid → **400** `application/problem+json` (ADR-002).

## 7. File Touchpoints

### Files to CREATE

- `SciFor.sln` — solution
- `src/SciFor.Domain/**` — ports, domain services, validation for four helpers
- `src/SciFor.Api/Program.cs` — composition root, endpoint mapping
- `src/SciFor.Api/**` — DTOs, HTTP adapters, Problem Details mapping
- `tests/SciFor.Domain.Tests/**` — unit + port contract tests
- `tests/SciFor.Api.Tests/**` — HTTP integration tests (`WebApplicationFactory`)
- `tests/SciFor.Parity.Tests/**` or under Api/Domain tests — Phase P live-T1 / recorded-stdout comparisons (implementer chooses layout)

### Files to MODIFY

- `README.md` — only via Documenter/status after completion; Implementer does not change design prose unless story follow-up requires it

### Files to leave UNCHANGED

- `docs/modernization/paths/**`, inventories, impedance, assessment snapshots — analysis immutability
- Legacy `libscifor.a` / NR / MKL sources — never copy into `src/` (DEC-007)
- Oracle harness productization (XP-386–XP-389 as services) — out of scope

## 8. Acceptance Criteria Checklist

### Phase A: Ported behavior

- [x] **A1** — Linear grid happy path and defaults
  - `POST /v1/linspace` with `{start:0,stop:1,num:5}` returns 200 and five values matching the both-endpoints formula; omitted optionals use Fortran defaults
  - Evidence: `tests/SciFor.Api.Tests/LinspaceEndpointTests.cs::S1_S2_linspace_defaults_200`
  - Covers: `XP-060`, `S1`, `S2`

- [x] **A2** — Linear grid optional mesh and endpoint flag combinations
  - `mesh:true` returns numeric spacing; start-only / end-only / neither match XP-060 formulas; decreasing and constant grids succeed
  - Evidence: `tests/SciFor.Domain.Tests/LinearGridTests.cs::endpoint_and_mesh_edges`
  - Covers: `XP-060`, `S3`, `S4`, `S5`, `S6`, `S7`, `S8`

- [x] **A3** — Linear grid client errors
  - `num<0`, both-endpoints `num<2`, and DEF-001 (`num=0` with endpoint excluded) return **400** Problem Details
  - Evidence: `tests/SciFor.Api.Tests/LinspaceEndpointTests.cs::S9_S11_linspace_400`
  - Covers: `XP-060`, `S9`, `S10`, `S11`; DEF-001

- [x] **A4** — Logarithmic grid happy path and defaults
  - `POST /v1/logspace` `{start:1,stop:1000,num:5}` returns 200; omitted `base` is 10
  - Evidence: `tests/SciFor.Api.Tests/LogspaceEndpointTests.cs::S12_S13_logspace_defaults_200`
  - Covers: `XP-061`, `S12`, `S13`

- [x] **A5** — Logarithmic grid DEF-002 zero rewrite
  - Exact-zero start/stop rewritten to `1e-12` before log; success response (no warning required)
  - Evidence: `tests/SciFor.Domain.Tests/LogarithmicGridTests.cs::S14_zero_rewrite`
  - Covers: `XP-061`, `S14`; DEF-002

- [x] **A6** — Logarithmic grid client errors
  - Negative endpoints, invalid base, `num<0`, `num<2` return **400**
  - Evidence: `tests/SciFor.Api.Tests/LogspaceEndpointTests.cs::S15_S18_logspace_400`
  - Covers: `XP-061`, `S15`, `S16`, `S17`, `S18`; DEF-003, DEF-004

- [x] **A7** — Finite-difference derivative behavior
  - Stencil and length-2 edge match XP-072; HTTP returns `df` same length as `f`
  - Evidence: `tests/SciFor.Domain.Tests/FiniteDifferenceTests.cs::S20_stencil`
  - Covers: `XP-072`, `S19`, `S20`

- [x] **A8** — Finite-difference client errors
  - `f.length < 2` and `dh == 0` return **400** (DEF-005, DEF-006)
  - Evidence: `tests/SciFor.Api.Tests/DerivEndpointTests.cs::S21_S22_deriv_400`
  - Covers: `XP-072`, `S21`, `S22`

- [x] **A9** — Fermi–Dirac occupation
  - Scalar endpoint returns formula results; `x*beta > 100` → exactly 0; `x*beta == 100` uses formula; `beta=0` → 0.5
  - Evidence: `tests/SciFor.Api.Tests/FermiEndpointTests.cs::S23_S26_fermi`
  - Covers: `XP-108`, `S23`, `S24`, `S25`, `S26`

### Phase P: Parity

Acceptance data: **live T1** `scifor-fidelity` (XP-388) on oracle.md v1 pin, or a recorded stdout copy from that pin. Provisional FIX names below are TBD until `/build-oracle build` — do not invent FIX file contents.

- [x] **P1** — XP-060 linspace T1 case matches under path test-plan rule
  - Oracle: live T1 / XP-388 `linspace(0,1,5)` values; provisional `FIX-060` TBD
  - Comparison: tolerance-based — `|a-b| <= max(1e-12, 1e-12 * max(|a|,|b|))` ([`XP-060-tests.md`](../modernization/test-plans/XP-060-tests.md) §2)
  - Defect decision: none on happy path; DEF-001 covered in A3 (not parity-to-crash)
  - Evidence: `tests/SciFor.Parity.Tests/Xp060ParityTests.cs::parity_XP060_P1`

- [x] **P2** — XP-061 logspace T1 case matches under path test-plan rule
  - Oracle: live T1 / XP-388 `logspace(1,1000,5)`; provisional `FIX-061` TBD
  - Comparison: same tolerance-based bound ([`XP-061-tests.md`](../modernization/test-plans/XP-061-tests.md) §2)
  - Defect decision: DEF-002 reproduce-faithfully on zero-rewrite edge (A5); H1 has no zero endpoints
  - Evidence: `tests/SciFor.Parity.Tests/Xp061ParityTests.cs::parity_XP061_P2`

- [x] **P3** — XP-072 deriv T1 case matches under path test-plan rule
  - Oracle: live T1 / XP-388 `deriv(y, x(2)-x(1))` on `xy2.data` parsed `dy`; provisional `FIX-072` TBD
  - Comparison: tolerance-based on parsed numbers (not list-directed text) ([`XP-072-tests.md`](../modernization/test-plans/XP-072-tests.md) §2)
  - Defect decision: DEF-005/006 in A8 (not OOB/div0 parity)
  - Evidence: `tests/SciFor.Parity.Tests/Xp072ParityTests.cs::parity_XP072_P3`

- [x] **P4** — XP-108 fermi T1 case matches under path test-plan rule
  - Oracle: live T1 / XP-388 five-point `beta=100` sample; provisional `FIX-108` TBD
  - Comparison: tolerance-based ([`XP-108-tests.md`](../modernization/test-plans/XP-108-tests.md) §2)
  - Defect decision: none (saturation is correct behavior)
  - Evidence: `tests/SciFor.Parity.Tests/Xp108ParityTests.cs::parity_XP108_P4`

### Phase Y: Binding & stack compliance

- [x] **Y1** — **(binding)** HTTP adapters integrate real domain ports (no mock of owned boundary)
  - Each of the four endpoint integration tests hits the real domain implementation via `WebApplicationFactory`
  - Evidence: `tests/SciFor.Api.Tests/LinspaceEndpointTests.cs`, `LogspaceEndpointTests.cs`, `DerivEndpointTests.cs`, `FermiEndpointTests.cs`

- [x] **Y2** — **(binding)** ADR-003 / DEC-003: no P/Invoke or wrap of `libscifor.a`
  - Solution references and native imports contain no `libscifor` / Fortran wrap
  - Evidence: `tests/SciFor.Api.Tests/ContainmentTests.cs::no_libscifor_pinvoke` (or review grep cited in End-of-Session Summary)

- [x] **Y3** — **(binding)** ADR-002 invalid inputs return 400 Problem Details (not 500)
  - Invalid inputs yield **400** `application/problem+json`, not 500
  - Evidence: A3 `tests/SciFor.Api.Tests/LinspaceEndpointTests.cs::S9_S11_linspace_400`; A6 `tests/SciFor.Api.Tests/LogspaceEndpointTests.cs::S15_S18_logspace_400`; A8 `tests/SciFor.Api.Tests/DerivEndpointTests.cs::S21_S22_deriv_400`; `tests/SciFor.Api.Tests/FermiEndpointTests.cs::missing_required_fields_400`

### Phase Z: Quality Gates

- [x] **Z1** — `dotnet build` passes with zero build/type errors
  - Evidence: `dotnet build SciFor.sln` (exit 0)

- [x] **Z2** — `dotnet format --verify-no-changes` passes (or only has pre-existing warnings)
  - Evidence: `dotnet format --verify-no-changes` (exit 0)

- [x] **Z3** — Configured type policy passes (no unjustified `any`-equivalent looseness in new/modified C#)
  - Evidence: static — C# / .NET 8 type policy; `dotnet build SciFor.sln` with zero type errors (no `dynamic` / unjustified looseness in new code)

- [x] **Z4** — Shared type import policy: N/A for this C# solution (no `@shared/types` alias); satisfied by project references
  - Evidence: static — project references (`SciFor.Api` → `SciFor.Domain`); no shared-types alias in profile

- [x] **Z5** — New or modified code includes appropriate logging for errors and significant operations per README logging guidance
  - Evidence: static — adapter / exception-handler logging inspection (`SciFor.Api` JSON console logger; `Program.cs` unhandled-exception log; HTTP adapters log validation failures)

- [x] **Z6** — `/review-story SF-1` satisfies the configured review gate, including zero high or critical `TEST-#`, `SEC-#`, `REL-#`, `API-#`, `MODEL-#`, `PAR-#`, or `PROV-#` findings
  - Evidence: `docs/features/SF-1-review.md` REVIEW SUMMARY `result=Pass` (all `*-critical=0` / `*-high=0`)

- [x] **Z7** — `/review-story SF-1` satisfies the configured model-fidelity gate (zero high/critical `MODEL-#`)
  - Evidence: `docs/features/SF-1-review.md` REVIEW SUMMARY `MODEL-critical=0 MODEL-high=0`

- [x] **Z8** — `/verify-parity SF-1` satisfies the configured parity gate and writes the configured parity report
  - Evidence: `docs/modernization/parity/SF-1-parity.md` PARITY SUMMARY `result=Pass`

## 8a. Test Plan

| # | Level | File::test name | Covers AC | Covers Sn | Covers XP | Notes |
|---|-------|------------------|-----------|-----------|-----------|-------|
| 1 | integration | `LinspaceEndpointTests.cs::S1_S2_linspace_defaults_200` | A1 | S1, S2 | XP-060 | HTTP happy + defaults |
| 2 | unit | `LinearGridTests.cs::endpoint_and_mesh_edges` | A2 | S3–S8 | XP-060 | formulas / edges |
| 3 | contract | `LinearGridPortContractTests.cs` | A1–A3 | S1–S11 | XP-060 | port contract |
| 4 | integration | `LinspaceEndpointTests.cs::S9_S11_linspace_400` | A3, Y3 | S9–S11 | XP-060 | DEF-001 + num guards; ADR-002 400 Problem Details |
| 5 | integration | `LogspaceEndpointTests.cs::S12_S13_logspace_defaults_200` | A4 | S12, S13 | XP-061 | |
| 6 | unit | `LogarithmicGridTests.cs::S14_zero_rewrite` | A5 | S14 | XP-061 | DEF-002 |
| 7 | contract | `LogarithmicGridPortContractTests.cs` | A4–A6 | S12–S18 | XP-061 | |
| 8 | integration | `LogspaceEndpointTests.cs::S15_S18_logspace_400` | A6, Y3 | S15–S18 | XP-061 | DEF-003/004; ADR-002 400 Problem Details |
| 9 | unit | `FiniteDifferenceTests.cs::S20_stencil` | A7 | S19, S20 | XP-072 | |
| 10 | contract | `FiniteDifferencePortContractTests.cs` | A7–A8 | S19–S22 | XP-072 | |
| 11 | integration | `DerivEndpointTests.cs::S21_S22_deriv_400` | A8, Y3 | S21, S22 | XP-072 | DEF-005/006; ADR-002 400 Problem Details |
| 12 | integration | `FermiEndpointTests.cs::S23_S26_fermi` | A9 | S23–S26 | XP-108 | |
| 13 | contract | `FermiDiracPortContractTests.cs` | A9 | S23–S26 | XP-108 | |
| 14 | parity | `Xp060ParityTests.cs::parity_XP060_P1` | P1 | S1 | XP-060 | live T1 / FIX-060 TBD |
| 15 | parity | `Xp061ParityTests.cs::parity_XP061_P2` | P2 | S12 | XP-061 | live T1 / FIX-061 TBD |
| 16 | parity | `Xp072ParityTests.cs::parity_XP072_P3` | P3 | S19 | XP-072 | live T1 / FIX-072 TBD |
| 17 | parity | `Xp108ParityTests.cs::parity_XP108_P4` | P4 | S23 | XP-108 | live T1 / FIX-108 TBD |
| 18 | integration | four `*EndpointTests` suites | Y1 | S1–S26 | all | real adapters |
| 19 | integration | `ContainmentTests.cs::no_libscifor_pinvoke` | Y2 | — | — | DEC-003 |
| 20 | integration | `FermiEndpointTests.cs::missing_required_fields_400` | Y3 | — | XP-108 | ADR-002 missing required x/beta → 400 |
| 21 | command | `dotnet build SciFor.sln` | Z1 | — | — | exit 0; zero build/type errors |
| 22 | command | `dotnet format --verify-no-changes` | Z2 | — | — | exit 0 |
| 23 | static | type policy / `dotnet build SciFor.sln` | Z3 | — | — | no unjustified looseness in new C# |
| 24 | static | project references (no `@shared/types`) | Z4 | — | — | N/A shared-types alias; Api→Domain refs |
| 25 | static | adapter / exception-handler logging inspection | Z5 | — | — | README logging guidance |
| 26 | review | `docs/features/SF-1-review.md` REVIEW SUMMARY Pass | Z6 | — | — | zero high/critical TEST/SEC/REL/API/MODEL/PAR/PROV |
| 27 | review | `docs/features/SF-1-review.md` MODEL-critical=0 MODEL-high=0 | Z7 | — | — | model-fidelity gate |
| 28 | parity-report | `docs/modernization/parity/SF-1-parity.md` PARITY SUMMARY Pass | Z8 | — | — | Phase P gate |

## 8b. Parity Plan

Copied from the path test plans. Do not invent a tighter or looser rule here.

| XP ID | Evidence grade | Oracle / fixture | Comparison rule | Defect decision | Acceptance data gap |
|-------|----------------|------------------|-----------------|-----------------|---------------------|
| XP-060 | E1 (H1 values) | Live T1 / XP-388; provisional FIX-060 TBD | tolerance-based `max(1e-12, 1e-12·max\|a\|,\|b\|)`; errors semantic 400 | DEF-001 fix-now | FIX file not written; Human accepts live T1 |
| XP-061 | E1 (H1 values) | Live T1 / XP-388; provisional FIX-061 TBD | same numeric rule; errors semantic 400 | DEF-002 reproduce; DEF-003/004 fix-now | FIX file not written; Human accepts live T1 |
| XP-072 | E1 (driver runs) | Live T1 / XP-388 parsed `dy`; provisional FIX-072 TBD | same numeric rule on parsed numbers (not format text) | DEF-005/006 fix-now | FIX file not written; Human accepts live T1 |
| XP-108 | E1 (H1 values) | Live T1 / XP-388; provisional FIX-108 TBD | same numeric rule | none (saturation OK) | FIX file not written; Human accepts live T1 |

## 9. Risks & Tradeoffs

| Risk | Impact | Mitigation | Evidence |
|------|--------|------------|----------|
| RISK-002 parity vs profile hint / goldens | high | Use path test-plan bounds + live T1 only | E2 ASSESSMENT; DEC-005 |
| RISK-003 T1 host-only | medium | Document pin; vN+1 if CI differs | E1 oracle.md |
| FIX files missing | medium | Human accepts live T1 for Phase P this POC | E2 this story DoR note |
| RISK-004 NR/MKL in snapshot archive | high | Never copy into C# tree (Y2) | E1 oracle §3; DEC-007 |

## Implementation Order

1. Create failing Phase P tests (P1–P4) against live T1 / recorded stdout expectations; observe fail for missing implementation.
2. Create solution, `SciFor.Domain` ports/interfaces and domain services with validation (DEF policies).
3. Create `SciFor.Api` composition root and four HTTP adapters (ADR-002/004).
4. Add port contract + HTTP integration tests (A*, Y1).
5. Satisfy containment check (Y2); run build/lint; then `/review-story` and `/verify-parity`.

## 10. Completion Metadata

| Field | Value |
|-------|-------|
| Completed by | Implementer (SF-1) / Docs-PM handoff |
| Completion ref | TBD: not committed |
| Review ref | `docs/features/SF-1-review.md` (Pass) |
| QA ref | QA PASS 2026-08-25 |
| Parity ref | `docs/modernization/parity/SF-1-parity.md` (Pass) |

## 11. Post-complete Follow-up Ledger

| ID | Date | Change class | Intent | Files touched | Verification | Change ref | Review ref | Docs impact | AC impact |
|----|------|--------------|--------|---------------|--------------|------------|------------|-------------|-----------|
| — | — | — | — | — | — | — | — | — | — |

*Created: 2026-08-25 | Port story template*

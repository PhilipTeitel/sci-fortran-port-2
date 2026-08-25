<!--
Per-story review contract:
- This file is produced by /review-story (or /review-diff for arbitrary base refs).
- It is a focused, lightweight audit limited to the changed surface — not a full-repo audit.
- Save using the configured story-review or diff-review path (defaults: `docs/features/{STORY-ID}-review.md` or `docs/reviews/diff-...md`).
- The auditor agent owns this template.
- The first non-comment line MUST use the configured machine-checkable review summary label and format (default "REVIEW SUMMARY") so QA and the configured quality gate can grep it.
- Use configured category-specific finding ID prefixes. Defaults are `TEST-#` for Test Coverage, `REL-#` for Reliability, `SEC-#` for Security, `API-#` for API Contracts, and `MODEL-#` for Model Fidelity. When both a full audit and a per-story review exist, IDs may overlap but are scoped to their own file (no cross-file renumbering).
- Findings list must use bullets per finding, not a single table — same as `audit-template.md` Detailed Findings. Every finding subsection item must be headed as `#### PREFIX-#. { Short title }` using that section's prefix.
- Severity is required on every finding; confidence is required on every non-`TEST-#` finding.
- `None.` is an exclusive empty-state marker: include it only when a subsection has no findings, and do not include it alongside findings, evidence summaries, or positive assertions.
-->

REVIEW SUMMARY: result=Pass TEST-critical=0 TEST-high=0 SEC-critical=0 SEC-high=0 REL-critical=0 REL-high=0 API-critical=0 API-high=0 MODEL-critical=0 MODEL-high=0 PAR-critical=0 PAR-high=0 PROV-critical=0 PROV-high=0

# Story Review: SF-1 — S1 numeric helpers HTTP/JSON

**Reviewed against:** `docs/features/SF-1-s1-numeric-helpers.md`
**Date:** 2026-08-25
**Mode:** `/review-story`
**Gate result:** `Pass`

---

## Scope

- Story ID: SF-1
- Purpose artifact: `docs/PURPOSE.md` — present
- Domain artifact: `docs/DOMAIN.md` — present
- Domain terms/entities in scope:
  - `LinearGrid` — from `docs/DOMAIN.md`
  - `LogarithmicGrid` — from `docs/DOMAIN.md`
  - `SampledSeries` / `FiniteDifferenceDerivative` — from `docs/DOMAIN.md`
  - `FermiDiracOccupation` — from `docs/DOMAIN.md`
  - Endpoint inclusion, grid spacing, overflow cutoff — from `docs/DOMAIN.md`
- Linked refined requirements (Sn IDs in scope): REQ-001 `S1`–`S26`
- Files in scope (from Section 7 "Files to CREATE/MODIFY" intersected with working-tree additions):
  - `SciFor.sln` — created
  - `src/SciFor.Domain/**` — created (ports, services, models, validation)
  - `src/SciFor.Api/Program.cs` — created
  - `src/SciFor.Api/**` — created (adapters, DTOs, host config)
  - `tests/SciFor.Domain.Tests/**` — created
  - `tests/SciFor.Api.Tests/**` — created
  - `tests/SciFor.Parity.Tests/**` — created
  - `README.md` — modified (Section 7 MODIFY allowed)
- Tests in scope (from Section 8a Test Plan):
  - `LinspaceEndpointTests.cs::S1_S2_linspace_defaults_200`, `S9_S11_linspace_400`
  - `LinearGridTests.cs::endpoint_and_mesh_edges`
  - `LinearGridPortContractTests.cs`
  - `LogspaceEndpointTests.cs::S12_S13_logspace_defaults_200`, `S15_S18_logspace_400`
  - `LogarithmicGridTests.cs::S14_zero_rewrite`
  - `LogarithmicGridPortContractTests.cs`
  - `FiniteDifferenceTests.cs::S20_stencil`
  - `FiniteDifferencePortContractTests.cs`
  - `DerivEndpointTests.cs::S19_deriv_happy_same_length`, `S21_S22_deriv_400`
  - `FermiEndpointTests.cs::S23_S26_fermi`, `missing_required_fields_400`
  - `FermiDiracPortContractTests.cs`
  - `Xp060ParityTests.cs::parity_XP060_P1` … `Xp108ParityTests.cs::parity_XP108_P4`
  - `ContainmentTests.cs::no_libscifor_pinvoke`
- Adapters in scope (from Section 4b):
  - `LinearGridHttpAdapter` for port `ILinearGrid`
  - `LogarithmicGridHttpAdapter` for port `ILogarithmicGrid`
  - `FiniteDifferenceHttpAdapter` for port `IFiniteDifference`
  - `FermiDiracHttpAdapter` for port `IFermiDirac`

### Out-of-plan changes

- `global.json` — SDK pin not listed in Section 7; skeleton tooling only
- `.gitignore` — not listed in Section 7; tooling only
- `.cursorignore` — not listed in Section 7; tooling only
- Prior-lane docs under `docs/decisions/**`, `docs/requirements/**`, `docs/modernization/**`, `docs/PURPOSE.md`, `docs/DOMAIN.md` — not Section 7 implement surface

---

## Findings

### Test Coverage {`TEST-#`}

None.

### Reliability {`REL-#`}

None.

### Security {`SEC-#`}

None.

### API Contracts {`API-#`}

None.

### Model Fidelity {`MODEL-#`}

None.

### Parity {`PAR-#`}

None.

### Provenance {`PROV-#`}

None.

---

## Required actions before QA

---

## Notes

### Prior Block remediation (verified)

- **API-1:** `FermiRequest.X`/`Beta` are `double?`; `FermiDiracHttpAdapter` rejects null with 400 Problem Details (`src/SciFor.Api/Adapters/FermiDiracHttpAdapter.cs:29-32`). Regression: `FermiEndpointTests.missing_required_fields_400`. Same nullable+required pattern applied to linspace/logspace/deriv adapters.
- **TEST-1:** `S1_S2_linspace_defaults_200` plus `// @scenario S1 @scenario S2` (`tests/SciFor.Api.Tests/LinspaceEndpointTests.cs:20-22`).
- **TEST-2:** Section 8a rows 4, 8, 11, and 20 list `Y3` under **Covers AC**.

### Rubric checks (this pass)

- **AC coverage:** A1–A9, P1–P4, Y1–Y3 each appear in Section 8a **Covers AC**; cited test files/names exist and are non-skipped `[Fact]`/`[Theory]` methods.
- **Adapter integration:** Four `*EndpointTests` suites use `WebApplicationFactory` with real domain registrations in `Program.cs`; no mocks of owned boundaries.
- **Scenario traceability:** S1–S26 appear in Section 8a **Covers Sn** and in test names and/or in-body scenario annotations (including S3–S8, S24–S25 comments).
- **Parity:** P1–P4 cite path-plan tolerance via `NumericTolerance`; E1 recorded T1 / XP-388 values (and XP-072 fixtures); DEF-001–006 reconciled; Human-accepted live-T1 gap documented.
- **Provenance:** Story legacy touchpoints and slice prerequisites use singular E1–E3 grades; no unresolved E4/E5 or open DEP/DEC/DEF in the prerequisite table.
- **Model fidelity:** HTTP rewrite of four helpers matches PURPOSE thesis; domain ports/services use DOMAIN terms; wire `istart`/`iend`/`mesh` mapped in adapters per ADR-004.
- Leave Z6/Z7 for Implementer/orchestrator to check after this Pass; next process step is `/verify-parity SF-1` (Z8).

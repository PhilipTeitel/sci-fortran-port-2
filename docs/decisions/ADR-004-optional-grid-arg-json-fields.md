# ADR-004: Optional grid-argument JSON field names

**Status:** Accepted
**Date:** 2026-08-25

---

## Context

XP-060 optional Fortran args `istart`, `iend`, and `mesh`, and XP-061 optional `base`, must appear on the product surface. Product owner resolved: include as optional JSON with Fortran defaults when omitted (both endpoints true; base 10). Exact wire names needed a binding ADR so REQ scenarios stay route-agnostic while implementers share one schema (REQ-001 resolved Q3; migration-plan recommended ADR).

---

## Decision

On **`POST /v1/linspace`** (ADR-002):

| JSON field | Required? | Type | Default when omitted | Meaning |
|------------|-----------|------|----------------------|---------|
| `istart` | no | boolean | `true` | Include start endpoint |
| `iend` | no | boolean | `true` | Include stop endpoint |
| `mesh` | no | boolean | `false` | When `true`, response includes numeric `mesh` equal to the step used |

On **`POST /v1/logspace`**:

| JSON field | Required? | Type | Default when omitted | Meaning |
|------------|-----------|------|----------------------|---------|
| `base` | no | number | `10` | Logarithm base |

Legacy Fortran `mesh` was an optional intent-out real; on the wire, request `mesh` is a **boolean request flag**, and response `mesh` is the **numeric spacing** when requested. Domain model still treats spacing as the derived step (DOMAIN.md `LinearGrid.spacing`).

---

## Consequences

**Positive**

- Matches Fortran default behavior for omitted optionals.
- Keeps XP-388-style default calls as minimal JSON (`start`, `stop`, `num` only).

**Negative / costs**

- Request vs response reuse of the name `mesh` (boolean in, number out) must be documented in OpenAPI/clients; implementers must not treat request `mesh` as a numeric input.

---

## Alternatives considered

| Alternative | Why not chosen |
|---------------|----------------|
| Require all optional fields always | Noisier clients; contradicts PO “defaults when omitted” |
| Rename to `includeStart` / `includeMesh` only | Domain prefers those terms, but PO asked to keep boring Fortran-shaped names on the wire; domain mapping stays in adapters |
| Always return spacing | Changes default response shape vs “when requested” |

---

## Explicit non-decisions

- Non-default `base` parity fixtures (path test plan may add FIX cases later).
- Whether clients may send `null` vs omit (treat both as omitted / default if the serializer allows; malformed types → 400 per ADR-002).

---

## Links

- Requirements: [`docs/requirements/REQ-001-s1-numeric-helpers.md`](../requirements/REQ-001-s1-numeric-helpers.md)
- Related design doc section: API Contract; Key Design Decisions
- ADR-002: [`ADR-002-s1-http-json-contract.md`](ADR-002-s1-http-json-contract.md)
- Supersedes / superseded by: none

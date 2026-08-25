# ADR-002: S1 HTTP/JSON contract

**Status:** Accepted
**Date:** 2026-08-25

---

## Context

REQ-001 and migration-plan RISK-001 / IMP-004 require a binding HTTP contract for XP-060, XP-061, XP-072, and XP-108: routes, JSON field names, and how invalid inputs (including DEF-001, DEF-003–DEF-006 and legacy `num<0` / both-endpoints `num<2`) map to client errors instead of process stop, NaN, or OOB. Product-owner preference is boring REST with camelCase JSON.

---

## Decision

### Routes and bodies

| Operation | Method / path | Request JSON | Success response JSON |
|-----------|---------------|--------------|------------------------|
| Linear grid (XP-060) | `POST /v1/linspace` | `start` (number), `stop` (number), `num` (integer), optional `istart` (boolean), `iend` (boolean), `mesh` (boolean — when `true`, include spacing in response) | `values` (number[]); optional `mesh` (number, spacing) when requested |
| Logarithmic grid (XP-061) | `POST /v1/logspace` | `start`, `stop`, `num`, optional `base` (number) | `values` (number[]) |
| Finite-difference derivative (XP-072) | `POST /v1/deriv` | `f` (number[]), `dh` (number) | `df` (number[]) |
| Fermi–Dirac occupation (XP-108) | `POST /v1/fermi` | `x` (number), `beta` (number) — **scalar only** for S1 | `value` (number) |

JSON property names are camelCase as above. Fortran optional defaults when omitted: `istart`/`iend` → `true`; `base` → `10`; `mesh` request flag → `false` (spacing omitted from response). See ADR-004.

### Invalid input status

Return **HTTP 400** with ASP.NET Core **Problem Details** (`application/problem+json`), not 500, for:

- `num < 0` (linspace / logspace)
- `num < 2` with both endpoints (linspace defaults or explicit; logspace always both-endpoints underneath)
- DEF-001: `num = 0` with an endpoint excluded
- DEF-003: negative (non-zero) logspace endpoints
- DEF-004: `base <= 0` or `base == 1`
- DEF-005: `f.length < 2`
- DEF-006: `dh == 0`
- Malformed JSON / missing required fields / type mismatches

DEF-002 (exact-zero logspace endpoints → `1e-12`) is success-path behavior, not 400.

---

## Consequences

**Positive**

- Implementers and parity tests share one wire contract.
- Unsafe legacy UB becomes deterministic client errors (defect ledger fix-now).

**Negative / costs**

- Fortran callers must be adapted to HTTP; field names are not a 1:1 ABI.
- Scalar-only fermi defers elemental arrays to a later decision.

---

## Alternatives considered

| Alternative | Why not chosen |
|---------------|----------------|
| GET with query strings for arrays | Awkward for `f` and grids; POST bodies are clearer |
| 422 / 500 for domain validation | Owner preference: 400 for invalid inputs |
| Reproduce Fortran `stop` as process crash | Unsafe for HTTP; IMP-004 / DEF fix-now |

---

## Explicit non-decisions

- Exact Problem Details `type` / `title` / `detail` string catalog beyond requiring structured 400 bodies.
- Auth, rate limiting, OpenAPI publication policy.
- Elemental / array fermi API (later slice or follow-up ADR).
- Numeric parity tolerances (path test plans; DEC-005).

---

## Links

- Requirements: [`docs/requirements/REQ-001-s1-numeric-helpers.md`](../requirements/REQ-001-s1-numeric-helpers.md)
- Related design doc section: API Contract
- Defect ledger: [`docs/modernization/defect-ledger.md`](../modernization/defect-ledger.md)
- Supersedes / superseded by: none

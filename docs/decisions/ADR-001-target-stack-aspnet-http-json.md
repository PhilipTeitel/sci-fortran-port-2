# ADR-001: Target stack ASP.NET Core HTTP/JSON

**Status:** Accepted
**Date:** 2026-08-25

---

## Context

The modernization assessment and decision register pin the SciFor port target as C# / .NET 8 / ASP.NET Core HTTP/JSON services (DEC-001). The product is a methodology-evaluation POC that rewrites selected numeric helpers for HTTP clients — not a Fortran ABI drop-in (DEC-002, DEC-003; IMP-001).

---

## Decision

Use **C# / .NET 8** with an **ASP.NET Core** host that exposes **HTTP/JSON** operations for first-slice helpers. Callers invoke REST endpoints; the process boundary is the ASP.NET Core app, not `libscifor.a` or Fortran modules.

---

## Consequences

**Positive**

- Aligns with profile `modernization.targetStack` and DEC-001–DEC-003.
- Enables per-path parity against T1 via ordinary HTTP clients and JSON parsers (DEC-004, DEC-005).

**Negative / costs**

- No binary compatibility with existing Fortran callers; clients must speak HTTP/JSON.
- Host and serialization overhead versus in-process library calls (acceptable for this POC).

---

## Alternatives considered

| Alternative | Why not chosen |
|---------------|----------------|
| P/Invoke / wrap `libscifor.a` | Contradicts DEC-002, DEC-003, IMP-001 rewrite-not-wrap |
| Fortran ABI / `.mod` drop-in | Explicitly out of product intent (PURPOSE anti-thesis) |
| Non-HTTP (CLI-only) surface | DEC-001 and DEC-006 require HTTP/JSON product services for S1 |

---

## Explicit non-decisions

- This ADR does not define routes, JSON field names, or status codes (see ADR-002).
- This ADR does not choose hexagonal project layout details (see ADR-003).
- Out of scope: auth, persistence, multi-tenant hosting, production SciFor redistribution.

---

## Links

- Requirements: [`docs/requirements/REQ-001-s1-numeric-helpers.md`](../requirements/REQ-001-s1-numeric-helpers.md)
- Related design doc section: Technical Stack; Key Design Decisions
- DEC-001, DEC-002, DEC-003: [`docs/modernization/decision-register.md`](../modernization/decision-register.md)
- Supersedes / superseded by: none

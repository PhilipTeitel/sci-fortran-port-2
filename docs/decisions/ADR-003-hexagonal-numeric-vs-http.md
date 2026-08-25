# ADR-003: Hexagonal numeric domain vs HTTP adapters

**Status:** Accepted
**Date:** 2026-08-25

---

## Context

DEC-002 and DEC-003 forbid wrapping `libscifor.a` or exposing a Fortran ABI. IMP-001 requires rewrite of XP-060, XP-061, XP-072, and XP-108 in C#. House rules require hexagonal port/adapter pairing so HTTP transport stays replaceable and numeric semantics stay testable without a web host. Domain language lives in [`docs/DOMAIN.md`](../DOMAIN.md).

---

## Decision

Structure S1 as a **hexagonal** .NET solution:

1. **Domain library** (`SciFor.Domain`) owns pure numeric helpers and domain validation (linear grid, logarithmic grid, finite-difference derivative, Fermi–Dirac occupation) using domain terms from DOMAIN.md. No ASP.NET, no HTTP types, no P/Invoke to `libscifor.a`.
2. **HTTP adapter host** (`SciFor.Api`) is the composition root: maps ADR-002 DTOs ↔ domain inputs/outputs, returns 400 Problem Details on domain validation failures, and delegates computation to domain services/ports.
3. **Ports** are C# interfaces (or equivalent application-facing abstractions) for the four operations, implemented in the domain (or thin application layer) and consumed by the HTTP adapters. Contract tests cover each port; integration tests cover the HTTP adapter against a real test server.

Parity and characterization tests may call the domain directly and/or the HTTP API; both must obey path test-plan comparison rules.

---

## Consequences

**Positive**

- Numeric parity can be proven without HTTP noise where useful.
- Prevents accidental Fortran wrap or leaking transport into domain logic.
- Matches methodology port/adapter test gating for later port stories.

**Negative / costs**

- Slightly more projects/types than a single-file Minimal API POC.
- Requires discipline to keep DTOs out of the domain.

---

## Alternatives considered

| Alternative | Why not chosen |
|---------------|----------------|
| P/Invoke to `libscifor.a` from controllers | DEC-002, DEC-003, IMP-001 |
| All logic inside controllers | Couples domain to ASP.NET; hard to parity-test cleanly |
| Separate gRPC / message bus for S1 | Out of scope; ADR-001 is HTTP/JSON |

---

## Explicit non-decisions

- Exact interface names and folder nesting beyond the project split in the design doc.
- Whether a separate `SciFor.Application` project is introduced later (allowed; not required for S1 if ports live next to domain implementations).
- Persistence, auth, or external numeric libraries beyond BCL `double` math (DEC-007 containment stands).

---

## Links

- Requirements: [`docs/requirements/REQ-001-s1-numeric-helpers.md`](../requirements/REQ-001-s1-numeric-helpers.md)
- Related design doc section: High-Level Architecture; Project Structure
- DEC-002, DEC-003: [`docs/modernization/decision-register.md`](../modernization/decision-register.md)
- Supersedes / superseded by: none

<!--
Refined-requirements contract:
- This file is produced by /refine-feature (architect in Discovery / Refinement Mode).
- Save it under the configured requirements directory using the configured requirement naming pattern (default `docs/requirements/REQ-NNN-short-slug.md`). Use sequential three-digit `NNN` unless the profile overrides the pattern.
- Do not delete or renumber existing REQ files; append the next number.
- Every Gherkin scenario must have an ID matching the configured scenario ID pattern (default `Sn`). The architect references those IDs from story Test Plans so each scenario traces to a concrete test.
- Unresolved questions are listed under `Open questions` — they are blocking. Do not write design or stories until they are resolved (architect should stop and re-ask).
-->

# REQ-001: S1 first-slice numeric helpers (HTTP/JSON)

**Source material:**
- [`docs/modernization/paths/XP-060-linspace.md`](../modernization/paths/XP-060-linspace.md)
- [`docs/modernization/paths/XP-061-logspace.md`](../modernization/paths/XP-061-logspace.md)
- [`docs/modernization/paths/XP-072-deriv.md`](../modernization/paths/XP-072-deriv.md)
- [`docs/modernization/paths/XP-108-fermi.md`](../modernization/paths/XP-108-fermi.md)
- [`docs/PURPOSE.md`](../PURPOSE.md)
- [`docs/DOMAIN.md`](../DOMAIN.md)
- [`docs/modernization/decision-register.md`](../modernization/decision-register.md) (DEC-001–DEC-007)
- [`docs/modernization/defect-ledger.md`](../modernization/defect-ledger.md) (DEF-001–DEF-006)
- [`docs/modernization/ASSESSMENT.md`](../modernization/ASSESSMENT.md)
- [`docs/modernization/migration-plan.md`](../modernization/migration-plan.md) (S1)
- [`docs/modernization/oracle.md`](../modernization/oracle.md)
- Product-owner resolutions supplied with `/refine-feature` (2026-08-25)
**Date:** 2026-08-25
**Status:** Draft

---

## 1. Goals

- Expose four SciFor-derived first-slice numeric helpers — linear grid (XP-060), logarithmic grid (XP-061), finite-difference derivative (XP-072), and Fermi–Dirac occupation (XP-108) — as HTTP/JSON operations a scientific/numeric client can invoke (DEC-001, DEC-002, DEC-006; PURPOSE north-star).
- Return parsed numeric results that match the bounded T1 / XP-388 observed cases for those paths within each path’s comparison rule (DEC-004, DEC-005; oracle.md v1).
- Preserve rewritten numeric semantics for the happy path and for DEF-002’s zero-endpoint rewrite, while rejecting unsafe legacy undefined behaviors (DEF-001, DEF-003–DEF-006) and invalid `num` with HTTP client errors (4xx) instead of crash, NaN, or out-of-bounds reads (defect ledger; IMP-004).
- Keep the product a methodology-evaluation POC rewrite — not a Fortran ABI, not a `libscifor.a` wrap, and not a production SciFor redistribution (DEC-002, DEC-003, DEC-007; PURPOSE anti-thesis; LICENSE).

## 2. Non-goals

- Numutils CLIs (XP-370–XP-385) as product services.
- Wrapping `libscifor.a` or exposing a Fortran ABI / `.mod` drop-in (DEC-002, DEC-003).
- FFT, LAPACK, MATRIX, gnuplot, or `SYSTEM` paths in this slice (DEC-006).
- Oracle harnesses XP-386, XP-387, XP-389 as product services (DEC-004, DEC-006); XP-388 remains oracle tooling only.
- Production SciFor redistribution (LICENSE.md; PURPOSE anti-thesis).
- Byte-identical Fortran format text, `fidelity/golden/` / XP-387 Python goldens as the observed corpus, or any system-wide numeric tolerance (DEC-005; oracle.md).
- Treating the Fermi clamp `x*beta > 100` → `0` as a defect (path XP-108; defect ledger refused).

## 3. Personas / actors

- **Scientific/numeric HTTP client** — Invokes the four HTTP/JSON numeric-helper operations (not a Fortran ABI caller). Cares about correct parsed numbers under each path’s comparison rule, predictable client errors on invalid input, and optional grid arguments that default like Fortran when omitted.

## 4. User scenarios (Gherkin)

Exact routes and JSON field names are ADR scope; scenarios name the services and domain inputs/outputs.

### S1 — Linear grid happy path (T1 / XP-388 defaults)

```gherkin
Given a scientific/numeric client can invoke the linspace service
And   start is 0, stop is 1, and num is 5
And   optional endpoint-inclusion and spacing fields are omitted (Fortran defaults: both endpoints included)
When  the client requests a linear grid
Then  the service returns a success response with five values
And   those values match 0, 0.25, 0.5, 0.75, 1 within the path’s comparison rule
```

### S2 — Linear grid optional args omitted use Fortran defaults

```gherkin
Given the client invokes the linspace service with required start, stop, and num only
When  optional include-start, include-stop, and spacing-request fields are omitted
Then  both endpoints are included
And   spacing uses (stop-start)/(num-1) when num >= 2
And   the response does not require the client to have sent those optional fields
```

### S3 — Linear grid optional spacing reported when requested

```gherkin
Given the client requests a linear grid with both endpoints included and num >= 2
And   the client asks for the grid spacing to be returned
When  the service computes the linear grid
Then  the response includes the spacing equal to the step used
And   the values follow start + (i)*step under the both-endpoints formula
```

### S4 — Linear grid start-only endpoint exclusion

```gherkin
Given start, stop, and num with num > 0
And   include-start is true and include-stop is false
When  the client requests a linear grid
Then  step is (stop-start)/num
And   values are start + (i-1)*step for i in 1..num
And   the last value is not stop
```

### S5 — Linear grid end-only endpoint exclusion

```gherkin
Given start, stop, and num with num > 0
And   include-start is false and include-stop is true
When  the client requests a linear grid
Then  step is (stop-start)/num
And   values are start + i*step for i in 1..num
And   the first value is not start
```

### S6 — Linear grid neither endpoint included

```gherkin
Given start, stop, and num with num > 0
And   include-start is false and include-stop is false
When  the client requests a linear grid
Then  step is (stop-start)/(num+1)
And   values are start + i*step for i in 1..num
```

### S7 — Linear grid decreasing when stop < start

```gherkin
Given stop is less than start and num >= 2 with both endpoints included
When  the client requests a linear grid
Then  the service returns a success response
And   the grid decreases (negative step) with no extra validation error
```

### S8 — Linear grid constant when start equals stop

```gherkin
Given start equals stop, num >= 2, and both endpoints included
When  the client requests a linear grid
Then  every returned value equals start
```

### S9 — Linear grid rejects num < 0

```gherkin
Given the client supplies num less than 0 to the linspace service
When  the client requests a linear grid
Then  the service responds with an HTTP client error (4xx)
And   it does not crash or return a partial grid
```

### S10 — Linear grid rejects num < 2 with both endpoints

```gherkin
Given both endpoints are included (explicitly or by default)
And   num is 0 or 1
When  the client requests a linear grid
Then  the service responds with an HTTP client error (4xx)
```

### S11 — Linear grid rejects num = 0 with an endpoint excluded (DEF-001 fix-now)

```gherkin
Given num is 0 and at least one endpoint is excluded
When  the client requests a linear grid
Then  the service responds with an HTTP client error (4xx)
And   it does not divide by zero or crash (DEF-001)
```

### S12 — Logarithmic grid happy path (T1 / XP-388)

```gherkin
Given a scientific/numeric client can invoke the logspace service
And   start is 1, stop is 1000, and num is 5
And   optional base is omitted (Fortran default 10)
When  the client requests a logarithmic grid
Then  the service returns a success response with five values
And   those values match 1, 5.62341325190349117, 31.6227766016837926, 177.827941003892278, 1000 within the path’s comparison rule
```

### S13 — Logarithmic grid optional base omitted defaults to 10

```gherkin
Given the client invokes the logspace service with required start, stop, and num only
When  optional base is omitted
Then  the grid is computed with base 10
And   both endpoints are included in log-space (as the legacy wrapper does)
```

### S14 — Logarithmic grid rewrites exact-zero endpoints to 1e-12 (DEF-002)

```gherkin
Given start is exactly 0, or stop is exactly 0, or both
And   num >= 2 and base is valid (default or positive and not 1)
When  the client requests a logarithmic grid
Then  the service silently rewrites each exact-zero endpoint to 1e-12 before taking log
And   the returned values match that rewritten computation within the path’s comparison rule
And   no warning is required (DEF-002 reproduce-faithfully)
```

### S15 — Logarithmic grid rejects negative endpoints (DEF-003 fix-now)

```gherkin
Given start or stop is negative (and not the exact-zero rewrite case)
When  the client requests a logarithmic grid
Then  the service responds with an HTTP client error (4xx)
And   it does not require NaN parity with legacy unguarded log (DEF-003)
```

### S16 — Logarithmic grid rejects invalid base (DEF-004 fix-now)

```gherkin
Given base is less than or equal to 0, or base equals 1
When  the client requests a logarithmic grid
Then  the service responds with an HTTP client error (4xx) (DEF-004)
```

### S17 — Logarithmic grid rejects num < 0

```gherkin
Given the client supplies num less than 0 to the logspace service
When  the client requests a logarithmic grid
Then  the service responds with an HTTP client error (4xx)
```

### S18 — Logarithmic grid rejects num < 2

```gherkin
Given num is 0 or 1 (both-endpoints log-space grid underneath)
When  the client requests a logarithmic grid
Then  the service responds with an HTTP client error (4xx)
```

### S19 — Finite-difference derivative happy path (T1 / XP-388)

```gherkin
Given a scientific/numeric client can invoke the deriv service
And   the sampled series and uniform abscissa spacing match the bounded T1 / XP-388 case already observed for XP-072
When  the client requests the finite-difference derivative
Then  the service returns a success response with a same-length derivative sequence
And   the parsed numbers match that T1 observation within the path’s comparison rule
```

### S20 — Finite-difference derivative stencil and length-2 edge

```gherkin
Given a sampled series of length L >= 2 and nonzero uniform spacing dh
When  the client requests the finite-difference derivative
Then  the first sample uses a forward difference (f[2]-f[1])/dh
And   interior samples use centered differences (f[i+1]-f[i-1])/(2*dh)
And   the last sample uses a backward difference (f[L]-f[L-1])/dh
And   when L is 2, both ends equal the one-sided slope (f[2]-f[1])/dh
And   result length equals L
```

### S21 — Finite-difference derivative rejects short series (DEF-005 fix-now)

```gherkin
Given a sampled series with fewer than 2 samples
When  the client requests the finite-difference derivative
Then  the service responds with an HTTP client error (4xx)
And   it does not reproduce out-of-bounds reads (DEF-005)
```

### S22 — Finite-difference derivative rejects dh = 0 (DEF-006 fix-now)

```gherkin
Given uniform abscissa spacing dh is 0
And   the series length is otherwise valid
When  the client requests the finite-difference derivative
Then  the service responds with an HTTP client error (4xx) (DEF-006)
```

### S23 — Fermi–Dirac occupation happy path (T1 / XP-388)

```gherkin
Given a scientific/numeric client can invoke the fermi service
And   beta is 100 and x takes the values -2, -1, 0, 1, 2
When  the client requests Fermi–Dirac occupation for each x
Then  the returned values match 1, 1, 0.5, 3.72007597602083562e-44, 0 within the path’s comparison rule
```

### S24 — Fermi–Dirac occupation saturates when x*beta > 100

```gherkin
Given x * beta is strictly greater than 100
When  the client requests Fermi–Dirac occupation
Then  the service returns exactly 0 without requiring exp evaluation
And  that saturation is treated as correct product behavior (not a defect)
```

### S25 — Fermi–Dirac occupation at x*beta == 100 uses the formula

```gherkin
Given x * beta equals 100 exactly
When  the client requests Fermi–Dirac occupation
Then  the service evaluates 1/(1+exp(beta*x))
And   it does not apply the early-zero cutoff (cutoff is strict >)
```

### S26 — Fermi–Dirac occupation at beta = 0 is 0.5

```gherkin
Given beta is 0 and x is finite
When  the client requests Fermi–Dirac occupation
Then  the returned value is 0.5
```

## 5. Constraints

- Target surface is HTTP/JSON on C# / .NET 8 / ASP.NET Core; callers are not Fortran ABI clients (DEC-001, DEC-002).
- First-slice helpers are rewritten in C#, not wrapped via `libscifor.a` (DEC-003).
- Numeric parity oracle is the bounded T1 pin in oracle.md v1; comparison is per-path parsed numbers (DEC-004, DEC-005). Path test plans own numeric bounds later; REQ scenarios only require “within the path’s comparison rule.”
- No system-wide numeric tolerance; profile `0.000001` is not a gate (DEC-005).
- Optional linear-grid endpoint/spacing fields and logarithmic `base` are included as optional JSON with Fortran defaults when omitted (both endpoints true; base 10). Exact JSON names are ADR-only.
- Invalid `num<0` (and both-endpoints `num<2`) map to HTTP client error (4xx), not legacy process stop (IMP-004 / PO resolution).
- DEF-001, DEF-003, DEF-004, DEF-005, DEF-006 are fix-now → 4xx client error.
- DEF-002 is reproduce-faithfully → zero endpoints rewrite to `1e-12` before log; parity expects that.
- Do not copy Numerical Recipes (DEP-006) or Intel Confidential MKL headers (DEP-004) into the C# tree (DEC-007).
- Out of slice: Numutils CLIs; FFT/LAPACK/MATRIX/gnuplot/`SYSTEM`; XP-386/387/389 as product; production redistribution.

## 6. Resolved questions

| # | Question | Resolution | Source |
|---|----------|------------|--------|
| 1 | Who is the actor / caller? | Scientific/numeric client invoking HTTP/JSON services (not Fortran ABI). | product owner (2026-08-25) |
| 2 | What is the product surface for S1? | Four HTTP/JSON operations for XP-060 linspace, XP-061 logspace, XP-072 deriv, XP-108 fermi. Exact routes/field names = ADR (not invented as binding paths in REQ prose); scenarios may say “the linspace service” etc. | product owner (2026-08-25) |
| 3 | Are optional Fortran args `istart`/`iend`/`base`/`mesh` on the product surface? | Include as optional JSON fields with Fortran defaults when omitted (both endpoints true; base 10). Exact names = ADR. | product owner (2026-08-25) |
| 4 | How is happy-path parity stated in REQ vs later test plans? | Parsed numbers vs bounded T1 / XP-388 cases already observed; comparison per path (DEC-005). Numeric bound for each path test plan later; REQ scenarios state “within the path’s comparison rule.” | product owner (2026-08-25); DEC-005 |
| 5 | How should DEF-001, DEF-003, DEF-004, DEF-005, DEF-006 be handled? | fix-now → HTTP client error (4xx), not crash / NaN / OOB. | product owner (2026-08-25); defect ledger |
| 6 | How should DEF-002 (logspace zero rewrite) be handled? | reproduce-faithfully → zero endpoints rewrite to `1e-12` before log; parity expects that. | product owner (2026-08-25); defect ledger |
| 7 | What is explicitly out of scope for this feature? | Numutils CLIs as product; wrap `libscifor.a`; FFT/LAPACK/MATRIX/gnuplot; XP-386/387/389 as product; production SciFor redistribution (LICENSE). | product owner (2026-08-25); DEC-002, DEC-003, DEC-006, DEC-007 |
| 8 | How should invalid `num<0` for linspace/logspace be handled? | Reject with client error (maps IMP-004 / legacy error+stop). | product owner (2026-08-25) |
| 9 | Is Fermi clamp `x*beta>100` a defect? | No — reproduce (saturation), not a defect. | product owner (2026-08-25); defect ledger refused |
| 10 | Is there a system-wide numeric tolerance? | No. | product owner (2026-08-25); DEC-005 |

## 7. Open questions

_(none)_

## 8. Suggested ADR triggers

| Trigger | Why it likely needs an ADR | Related Sn |
|---------|----------------------------|------------|
| HTTP/JSON contract (routes, JSON field names, success and 4xx status codes for invalid input) | Binding transport contract for all four operations; IMP-004 / RISK-001; maps legacy `error`/`stop` and fix-now DEFs to client errors without inventing routes in REQ prose | S1–S26 |
| Hexagonal numeric-domain vs HTTP-adapter boundary | Rewritten helpers must stay separable from HTTP; no Fortran ABI port (DEC-002, DEC-003); migration-plan recommended ADR | S1–S26 |
| Optional grid-argument JSON presence and names (`istart`/`iend`/`mesh`/`base` equivalents) | Product already decided include-with-Fortran-defaults; ADR only names fields and wire shape | S2, S3, S4–S6, S11, S13, S14, S16 |

## 9. Links

- Source material: see header
- Related REQ files: none (first refined REQ)
- Related ADRs (if any already exist): none yet
- Purpose: [`docs/PURPOSE.md`](../PURPOSE.md)
- Domain: [`docs/DOMAIN.md`](../DOMAIN.md)
- Migration plan S1: [`docs/modernization/migration-plan.md`](../modernization/migration-plan.md)
- Defect ledger: [`docs/modernization/defect-ledger.md`](../modernization/defect-ledger.md)
- Decision register: [`docs/modernization/decision-register.md`](../modernization/decision-register.md)

---

*Created: 2026-08-25 | Refined by: architect in Discovery Mode*

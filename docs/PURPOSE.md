<!--
Purpose artifact contract:
- Produced by /recover-domain (modeler in Legacy recovery mode) for first-slice recovery.
- Purpose is canonical for product intent. Requirements, design, domain modeling, stories, reviews, and QA must not contradict it silently.
- Status is Draft until Human approval at greenfield purpose/domain gates.
-->

# Purpose: SciFor Numeric Helpers Port (S1 POC)

**Source material:**
- `README.md` (§ What this is; License)
- `LICENSE.md`
- `.cursor/workflow.config.yml` (`modernization.targetStack`, `oracleTier`, parity rules)
- `docs/modernization/ASSESSMENT.md`
- `docs/modernization/decision-register.md` (DEC-001–DEC-007)
- `docs/modernization/migration-plan.md` (S1)
- `docs/modernization/paths/XP-060-linspace.md`
- `docs/modernization/paths/XP-061-logspace.md`
- `docs/modernization/paths/XP-072-deriv.md`
- `docs/modernization/paths/XP-108-fermi.md`
- Binding product-intent summary supplied with `/recover-domain` (E2 owner / DEC / profile)

**Date:** 2026-08-25  
**Status:** Draft

---

## Thesis

This product is a methodology-evaluation POC that rewrites selected SciFor numeric helpers as C# / .NET 8 ASP.NET Core HTTP/JSON services and proves per-path numeric parity against a bounded Fortran T1 oracle — not a production SciFor redistribution and not a Fortran ABI drop-in.

## The job it does

An ADD / port evaluator (or a service caller in that evaluation setting) needs four SciFor-derived numeric operations — linear grid, logarithmic grid, finite-difference derivative, and Fermi–Dirac occupation — available as HTTP/JSON so that rewritten behavior can be exercised and compared to live T1 outputs. The job matters because the slice exists to evaluate Artifact-Driven Development on a real brownfield numeric port, not to ship a SciFor replacement.

## North-star outcome

XP-060, XP-061, XP-072, and XP-108 are served as HTTP/JSON operations whose parsed numeric results pass per-path comparison against the bounded T1 executable pin (oracle.md v1 / DEC-004, DEC-005, DEC-006).

## Trade-off rule

When goals conflict, optimize for **per-path numeric parity and rewritten-helper fidelity on the T1 pin** over **Fortran ABI compatibility, wrapping `libscifor.a`, global format-text sameness, or inventory completeness**.

This ordering matches DEC-002, DEC-003, DEC-005, and DEC-006: success is four rewritten HTTP/JSON operations with parsed-number parity, not a drop-in library or whole-system port.

## Anti-thesis

Tempting but wrong shapes for this product:

- A Fortran `.mod` / `libscifor.a` ABI drop-in, or HTTP that only wraps `libscifor.a` (DEC-002, DEC-003 / IMP-001).
- A production SciFor/SciFortran redistribution or operational replacement (LICENSE.md).
- Byte-identical Fortran format text, or treating `fidelity/golden/` / XP-387 Python goldens as the observed T1 corpus (DEC-005).
- Declaring first-slice success by shipping Numutils CLIs (XP-370–XP-385), oracle harnesses (XP-386–XP-389), or FFT/LAPACK/MATRIX/gnuplot/`SYSTEM` paths (DEC-006).
- Copying Numerical Recipes (DEP-006) or Intel Confidential MKL headers (DEP-004) into the C# tree (DEC-007).

## Success signals

- Four first-slice operations are invokable as HTTP/JSON (linear grid, logarithmic grid, finite-difference derivative, Fermi–Dirac occupation) — E2 DEC-001, DEC-006; README What this is.
- Parsed numbers for those operations pass per-path tolerance vs live T1 / XP-388 stdout on the pinned host — E2 DEC-004, DEC-005.
- The C# tree does not contain DEP-006 or DEP-004 materials — E2 DEC-007.
- Use of the tree remains methodology evaluation only — E2 LICENSE.md.

## Open purpose questions

None that change thesis, job, north-star, trade-off rule, or anti-thesis for S1. Remaining first-slice work (path test plans; Architect HTTP ADR for routes, JSON names, invalid-input status) is design/contract work, not unresolved purpose.

Wire presence of optional linear-grid endpoint flags and logarithmic-grid base is ADR scope (ASSESSMENT.md §4; migration-plan recommended ADR), not an open owner purpose TBD.

## Links

- Related requirements: none yet (`/refine-feature` not run for this slice)
- Related domain model: `docs/DOMAIN.md`
- Supersedes / superseded by: none (first recovery)

---

*Created: 2026-08-25 | Modeled by: modeler in Legacy recovery mode*

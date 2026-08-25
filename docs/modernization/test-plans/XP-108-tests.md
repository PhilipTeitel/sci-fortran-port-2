<!-- Path test plan contract:
- Per execution path. This is the source for port-story Phase P and 8b Parity Plan.
- Comparison rules are decided here. Do not defer to a global numeric default.
- If no fixture exists, do not invent a parity criterion. Say how the path will be verified instead.
- One evidence grade per oracle/source row.
- Omit unused scenario groups. Do not write N/A rows.
- QA verifies via /verify-parity; QA does not edit this file.
-->

# Test Plan: XP-108 FUNCTIONS::fermi

**Execution path:** `XP-108`
**Oracle tier:** `T1 executable`
**Date:** `2026-08-25`
**Owner:** Architect (`/plan-path-tests`)

## Summary

Verify the Fermi–Dirac occupation HTTP/JSON rewrite by comparing **parsed numbers** to the bounded T1 / XP-388 five-point sample at `beta=100` and to E3 formula edges (saturation guard, equality-at-100, `beta=0`). Numeric rule for this plan is **tolerance-based**: relative `1e-12` or absolute `1e-12`, whichever is larger for the value (DEC-005; not the workflow-profile hint). The clamp `x*beta > 100` → `0` is **correct saturation**, not a defect. No DEF-NNN binds this path. **Fixture not yet written** — provisional `FIX-108` TBD; until `/build-oracle build`, verify against live T1 stdout (or recorded copy on the same pin).

---

## 1. Oracle strategy

| Question | Answer |
|----------|--------|
| How is correctness verified for this path? | Live T1 executable (`scifor-fidelity` / XP-388) on the oracle.md v1 pin; optional recorded stdout once captured |
| Fixture / data | **Not yet written.** Provisional `FIX-108` → TBD harness path after `/build-oracle build`. Until then: live T1 stdout for H1; E3 path formulas for edges |
| Linked scenarios | REQ-001 `S23`–`S26` |

Do not claim a stronger oracle than `docs/modernization/oracle.md`.

## 2. Comparison rules

| Output kind | Rule | Bound / notes | Decision |
|-------------|------|---------------|----------|
| numeric (occupation) | tolerance-based | Pass if `|a-b| <= max(1e-12, 1e-12 * max(\|a\|, \|b\|))`. Exact `0` from the saturation guard must match exactly (still within this bound). This plan’s rule; not a profile gate. | DEC-005; this plan |

There is no project-wide numeric tolerance gate. If a rule is missing, this path is not implementation-ready.

## 3. Happy path

| ID | Input | Expected output | Source | Evidence |
|----|-------|-----------------|--------|----------|
| H1 | `beta=100`; `x` in `{-2,-1,0,1,2}` | `1`, `1`, `0.5`, `3.72007597602083562e-44`, `0` within §2 numeric rule | XP-388 / oracle.md §1; provisional FIX-108 TBD | E1 `docs/modernization/oracle.md` §1 |

## 4. Edge cases

| ID | Case | Expected legacy behavior | Source | Evidence |
|----|------|--------------------------|--------|----------|
| E1 | `x*beta > 100` (e.g. `x=2`, `beta=100`) | Exactly `0` without evaluating `exp` (saturation; not a defect) | XP-108 §6; REQ S24 | E1 oracle.md §1; E3 `src/FUNCTIONS.f90:243-246` |
| E2 | `x*beta == 100` (e.g. `x=1`, `beta=100`) | Formula `1/(1+exp(100))` — no early zero (cutoff is strict `>`) | XP-108 §6; REQ S25 | E1 oracle.md §1; E3 `src/FUNCTIONS.f90:243-247` |
| E3 | `beta=0`, finite `x` | `0.5` | XP-108 §6; REQ S26 | E3 `src/FUNCTIONS.f90:243-247` |
| E4 | Large negative `x*beta` (e.g. `x=-2`, `beta=100`) | Tends to `1` (observed `1`) | XP-108 §6; H1 subset | E1 oracle.md §1; E3 `src/FUNCTIONS.f90:247` |

## 5. Error and failure scenarios

_(omitted — no client-error failure modes for this path in REQ-001 or the defect ledger)_

## 6. Defects that bind this plan

None.

## 7. Open test-planning questions

_(none)_

## Links

- Path detail: [`docs/modernization/paths/XP-108-fermi.md`](../paths/XP-108-fermi.md)
- Oracle: [`docs/modernization/oracle.md`](../oracle.md)
- Defect ledger: [`docs/modernization/defect-ledger.md`](../defect-ledger.md)
- Requirements: [`docs/requirements/REQ-001-s1-numeric-helpers.md`](../../requirements/REQ-001-s1-numeric-helpers.md)
- Port story: TBD

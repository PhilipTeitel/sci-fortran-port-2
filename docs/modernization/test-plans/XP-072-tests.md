<!-- Path test plan contract:
- Per execution path. This is the source for port-story Phase P and 8b Parity Plan.
- Comparison rules are decided here. Do not defer to a global numeric default.
- If no fixture exists, do not invent a parity criterion. Say how the path will be verified instead.
- One evidence grade per oracle/source row.
- Omit unused scenario groups. Do not write N/A rows.
- QA verifies via /verify-parity; QA does not edit this file.
-->

# Test Plan: XP-072 TOOLS::deriv

**Execution path:** `XP-072`
**Oracle tier:** `T1 executable`
**Date:** `2026-08-25`
**Owner:** Architect (`/plan-path-tests`)

## Summary

Verify the finite-difference derivative HTTP/JSON rewrite by comparing **parsed derivative numbers** to the bounded T1 / XP-388 case (`deriv(y, x(2)-x(1))` on `numutils/test/xy2.data`). Numeric rule for this plan is **tolerance-based**: relative `1e-12` or absolute `1e-12`, whichever is larger for the value (DEC-005; not the workflow-profile hint). Do **not** require byte-identical list-directed Fortran text (oracle.md §4). DEF-005/DEF-006 assert HTTP 4xx. **Fixture not yet written** — provisional `FIX-072` TBD; until `/build-oracle build`, parse live T1 stdout derivative column (or a recorded copy on the same pin) for H1.

---

## 1. Oracle strategy

| Question | Answer |
|----------|--------|
| How is correctness verified for this path? | Live T1 executable (`scifor-fidelity` / XP-388) on the oracle.md v1 pin; parse numeric `dy` values from driver output (not format-text sameness) |
| Fixture / data | **Not yet written.** Provisional `FIX-072` → TBD harness path after `/build-oracle build` (inputs: series from `xy2.data` + `dh=x(2)-x(1)`; expected: parsed `dy`). Until then: live T1 stdout |
| Linked scenarios | REQ-001 `S19`–`S22` |

Do not claim a stronger oracle than `docs/modernization/oracle.md`.

## 2. Comparison rules

| Output kind | Rule | Bound / notes | Decision |
|-------------|------|---------------|----------|
| numeric (derivative sequence) | tolerance-based | Pass if `|a-b| <= max(1e-12, 1e-12 * max(\|a\|, \|b\|))` on each parsed element. Ignore Fortran list-directed formatting. This plan’s rule; not a profile gate. | DEC-005; this plan |
| error / rejection | semantic | HTTP client error (4xx); do not reproduce OOB or div0 | DEF-005, DEF-006; REQ-001; this plan |

There is no project-wide numeric tolerance gate. If a rule is missing, this path is not implementation-ready.

## 3. Happy path

| ID | Input | Expected output | Source | Evidence |
|----|-------|-----------------|--------|----------|
| H1 | `f` = `y` from `numutils/test/xy2.data`; `dh = x(2)-x(1)` (XP-388) | Same-length `dy`; each element within §2 numeric rule vs T1 / XP-388 parsed values | XP-388 / oracle.md §1; provisional FIX-072 TBD | E1 `docs/modernization/oracle.md` §1 (two identical runs) |

## 4. Edge cases

| ID | Case | Expected legacy behavior | Source | Evidence |
|----|------|--------------------------|--------|----------|
| E1 | `size(f)=2`, `dh ≠ 0` | Both ends equal one-sided slope `(f(2)-f(1))/dh`; interior empty | XP-072 §6; REQ S20 | E3 `src/TOOLS.f90:181-185` |
| E2 | Stencil for `L>=3` | Forward at first; centered interior; backward at last | XP-072 §1 steps 3–5; REQ S20 | E3 `src/TOOLS.f90:181-185` |

## 5. Error and failure scenarios

| ID | Failure | Expected legacy behavior | Source | Evidence |
|----|---------|--------------------------|--------|----------|
| F1 | `size(f) < 2` (DEF-005) | Legacy: OOB reads. **Port asserts:** HTTP 4xx | DEF-005; REQ S21 | E3 `src/TOOLS.f90:180-185` |
| F2 | `dh = 0` (DEF-006) | Legacy: division by zero. **Port asserts:** HTTP 4xx | DEF-006; REQ S22 | E3 `src/TOOLS.f90:181-185` |

## 6. Defects that bind this plan

| DEF ID | Decision | Effect on expected output |
|--------|----------|---------------------------|
| DEF-005 | fix-now | F1 asserts HTTP 4xx; no OOB reproduction |
| DEF-006 | fix-now | F2 asserts HTTP 4xx; no div0 reproduction |

## 7. Open test-planning questions

_(none)_

## Links

- Path detail: [`docs/modernization/paths/XP-072-deriv.md`](../paths/XP-072-deriv.md)
- Oracle: [`docs/modernization/oracle.md`](../oracle.md)
- Defect ledger: [`docs/modernization/defect-ledger.md`](../defect-ledger.md)
- Requirements: [`docs/requirements/REQ-001-s1-numeric-helpers.md`](../../requirements/REQ-001-s1-numeric-helpers.md)
- Port story: TBD

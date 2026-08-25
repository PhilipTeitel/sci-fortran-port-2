<!-- Path test plan contract:
- Per execution path. This is the source for port-story Phase P and 8b Parity Plan.
- Comparison rules are decided here. Do not defer to a global numeric default.
- If no fixture exists, do not invent a parity criterion. Say how the path will be verified instead.
- One evidence grade per oracle/source row.
- Omit unused scenario groups. Do not write N/A rows.
- QA verifies via /verify-parity; QA does not edit this file.
-->

# Test Plan: XP-060 TOOLS::linspace

**Execution path:** `XP-060`
**Oracle tier:** `T1 executable`
**Date:** `2026-08-25`
**Owner:** Architect (`/plan-path-tests`)

## Summary

Verify the linear-grid HTTP/JSON rewrite by comparing **parsed numbers** to the bounded T1 / XP-388 case `linspace(0,1,5)` and to E3 formula edges from the path detail. Numeric rule for this plan is **tolerance-based**: relative `1e-12` or absolute `1e-12`, whichever is larger for the value (DEC-005; not the workflow-profile hint). Error cases assert HTTP client error (4xx), including DEF-001 fix-now. **Fixture not yet written** — provisional `FIX-060` TBD under a future harness dir; until `/build-oracle build`, verify against live T1 `scifor-fidelity` stdout (or a recorded copy of that stdout on the same pin).

---

## 1. Oracle strategy

| Question | Answer |
|----------|--------|
| How is correctness verified for this path? | Live T1 executable (`scifor-fidelity` / XP-388) on the oracle.md v1 pin; optional recorded stdout of that probe once captured |
| Fixture / data | **Not yet written.** Provisional `FIX-060` → TBD harness path after `/build-oracle build`. Until then: live T1 stdout for H1; E3 path formulas for edges |
| Linked scenarios | REQ-001 `S1`–`S11` |

Do not claim a stronger oracle than `docs/modernization/oracle.md`.

## 2. Comparison rules

| Output kind | Rule | Bound / notes | Decision |
|-------------|------|---------------|----------|
| numeric (grid values, optional spacing) | tolerance-based | Pass if `|a-b| <= max(1e-12, 1e-12 * max(\|a\|, \|b\|))` on each parsed element. This plan’s rule; not a profile gate. | DEC-005; this plan |
| error / rejection | semantic | HTTP client error (4xx); do not assert Fortran `stop` text or crash | DEF-001; REQ-001; this plan |

There is no project-wide numeric tolerance gate. If a rule is missing, this path is not implementation-ready.

## 3. Happy path

| ID | Input | Expected output | Source | Evidence |
|----|-------|-----------------|--------|----------|
| H1 | `start=0`, `stop=1`, `num=5`, defaults (both endpoints) | `0, 0.25, 0.5, 0.75, 1` within §2 numeric rule | XP-388 / oracle.md §1; provisional FIX-060 TBD | E1 `docs/modernization/oracle.md` §1 |

## 4. Edge cases

| ID | Case | Expected legacy behavior | Source | Evidence |
|----|------|--------------------------|--------|----------|
| E1 | Optional flags omitted | Both endpoints included; step `(stop-start)/(num-1)` | XP-060 §1 steps 3–4; REQ S2 | E3 `src/tools_grids.f90:8-14` |
| E2 | Spacing requested | `mesh`/spacing equals step used | XP-060 §1 step 8; REQ S3 | E3 `src/tools_grids.f90:28` |
| E3 | Start only (`istart` true, `iend` false) | `step=(stop-start)/num`; last ≠ stop | XP-060 §1 step 5; REQ S4 | E3 `src/tools_grids.f90:16-18` |
| E4 | End only | `step=(stop-start)/num`; first ≠ start | XP-060 §1 step 6; REQ S5 | E3 `src/tools_grids.f90:20-22` |
| E5 | Neither endpoint | `step=(stop-start)/(num+1)` | XP-060 §1 step 7; REQ S6 | E3 `src/tools_grids.f90:24-26` |
| E6 | `stop < start`, both endpoints, `num>=2` | Decreasing grid; success | XP-060 §6; REQ S7 | E3 `src/tools_grids.f90:13-14` |
| E7 | `start == stop`, both endpoints, `num>=2` | All values equal `start` | XP-060 §6; REQ S8 | E3 `src/tools_grids.f90:13-14` |

## 5. Error and failure scenarios

| ID | Failure | Expected legacy behavior | Source | Evidence |
|----|---------|--------------------------|--------|----------|
| F1 | `num < 0` | Legacy: `error` + `stop`. **Port asserts:** HTTP 4xx | XP-060 §6; REQ S9 | E3 `src/tools_grids.f90:7` |
| F2 | `num < 2` with both endpoints | Legacy: `error` + `stop`. **Port asserts:** HTTP 4xx | XP-060 §6; REQ S10 | E3 `src/tools_grids.f90:11-12` |
| F3 | `num = 0` with an endpoint excluded (DEF-001) | Legacy: unguarded div0 / empty. **Port asserts:** HTTP 4xx (fix-now; not crash) | DEF-001; REQ S11 | E3 `src/tools_grids.f90:16-26` |

## 6. Defects that bind this plan

| DEF ID | Decision | Effect on expected output |
|--------|----------|---------------------------|
| DEF-001 | fix-now | F3 asserts HTTP client error; do not reproduce div0/crash |

## 7. Open test-planning questions

_(none)_

## Links

- Path detail: [`docs/modernization/paths/XP-060-linspace.md`](../paths/XP-060-linspace.md)
- Oracle: [`docs/modernization/oracle.md`](../oracle.md)
- Defect ledger: [`docs/modernization/defect-ledger.md`](../defect-ledger.md)
- Requirements: [`docs/requirements/REQ-001-s1-numeric-helpers.md`](../../requirements/REQ-001-s1-numeric-helpers.md)
- Port story: TBD

<!-- Path test plan contract:
- Per execution path. This is the source for port-story Phase P and 8b Parity Plan.
- Comparison rules are decided here. Do not defer to a global numeric default.
- If no fixture exists, do not invent a parity criterion. Say how the path will be verified instead.
- One evidence grade per oracle/source row.
- Omit unused scenario groups. Do not write N/A rows.
- QA verifies via /verify-parity; QA does not edit this file.
-->

# Test Plan: XP-061 TOOLS::logspace

**Execution path:** `XP-061`
**Oracle tier:** `T1 executable`
**Date:** `2026-08-25`
**Owner:** Architect (`/plan-path-tests`)

## Summary

Verify the logarithmic-grid HTTP/JSON rewrite by comparing **parsed numbers** to the bounded T1 / XP-388 case `logspace(1,1000,5)` (default base 10) and to E3 edges from the path detail. Numeric rule for this plan is **tolerance-based**: relative `1e-12` or absolute `1e-12`, whichever is larger for the value (DEC-005; not the workflow-profile hint). DEF-002 zero-endpoint rewrite is **reproduce-faithfully**. DEF-003/DEF-004 and invalid `num` assert HTTP 4xx. **Fixture not yet written** — provisional `FIX-061` TBD; until `/build-oracle build`, verify H1 against live T1 stdout (or recorded copy on the same pin).

---

## 1. Oracle strategy

| Question | Answer |
|----------|--------|
| How is correctness verified for this path? | Live T1 executable (`scifor-fidelity` / XP-388) on the oracle.md v1 pin; optional recorded stdout once captured |
| Fixture / data | **Not yet written.** Provisional `FIX-061` → TBD harness path after `/build-oracle build`. Until then: live T1 stdout for H1; E3 formulas for edges/DEF-002 |
| Linked scenarios | REQ-001 `S12`–`S18` |

Do not claim a stronger oracle than `docs/modernization/oracle.md`.

## 2. Comparison rules

| Output kind | Rule | Bound / notes | Decision |
|-------------|------|---------------|----------|
| numeric (grid values) | tolerance-based | Pass if `|a-b| <= max(1e-12, 1e-12 * max(\|a\|, \|b\|))` on each parsed element. This plan’s rule; not a profile gate. | DEC-005; this plan |
| error / rejection | semantic | HTTP client error (4xx); no NaN parity for DEF-003/004 | DEF-003, DEF-004; REQ-001; this plan |

There is no project-wide numeric tolerance gate. If a rule is missing, this path is not implementation-ready.

## 3. Happy path

| ID | Input | Expected output | Source | Evidence |
|----|-------|-----------------|--------|----------|
| H1 | `start=1`, `stop=1000`, `num=5`, base omitted (10) | `1`, `5.62341325190349117`, `31.6227766016837926`, `177.827941003892278`, `1000` within §2 numeric rule | XP-388 / oracle.md §1; provisional FIX-061 TBD | E1 `docs/modernization/oracle.md` §1 |

## 4. Edge cases

| ID | Case | Expected legacy behavior | Source | Evidence |
|----|------|--------------------------|--------|----------|
| E1 | Optional `base` omitted | Base 10; both endpoints in log-space then `base**` | XP-061 §1; REQ S13 | E3 `src/tools_grids.f90:40-46` |
| E2 | Exact-zero `start` and/or `stop` (DEF-002) | Silent rewrite to `1e-12` before log; no warning; parity expects rewritten result | XP-061 §6; DEF-002; REQ S14 | E3 `src/tools_grids.f90:42-43` |

## 5. Error and failure scenarios

| ID | Failure | Expected legacy behavior | Source | Evidence |
|----|---------|--------------------------|--------|----------|
| F1 | Negative (non-zero) endpoint (DEF-003) | Legacy: unguarded `log` → NaN/UB. **Port asserts:** HTTP 4xx | DEF-003; REQ S15 | E3 `src/tools_grids.f90:42-44` |
| F2 | `base <= 0` or `base == 1` (DEF-004) | Legacy: unguarded. **Port asserts:** HTTP 4xx | DEF-004; REQ S16 | E3 `src/tools_grids.f90:44` |
| F3 | `num < 0` | Legacy: `error` + `stop`. **Port asserts:** HTTP 4xx | XP-061 §6; REQ S17 | E3 `src/tools_grids.f90:41` |
| F4 | `num < 2` | Legacy: linspace both-endpoints abort. **Port asserts:** HTTP 4xx | XP-061 §6; REQ S18 | E3 `src/tools_grids.f90:45,11-12` |

## 6. Defects that bind this plan

| DEF ID | Decision | Effect on expected output |
|--------|----------|---------------------------|
| DEF-002 | reproduce-faithfully | E2 expects `0` → `1e-12` rewrite before log; no warning required |
| DEF-003 | fix-now | F1 asserts HTTP 4xx; no NaN parity |
| DEF-004 | fix-now | F2 asserts HTTP 4xx |

## 7. Open test-planning questions

_(none)_

## Links

- Path detail: [`docs/modernization/paths/XP-061-logspace.md`](../paths/XP-061-logspace.md)
- Oracle: [`docs/modernization/oracle.md`](../oracle.md)
- Defect ledger: [`docs/modernization/defect-ledger.md`](../defect-ledger.md)
- Requirements: [`docs/requirements/REQ-001-s1-numeric-helpers.md`](../../requirements/REQ-001-s1-numeric-helpers.md)
- Port story: TBD

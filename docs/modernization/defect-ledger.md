# Defect Ledger

**Legacy repo:** `/Users/philipteitel/code/ADD-migrations/sci-fortran-legacy`
**Date:** 2026-08-25
**Owner:** Archaeologist (`/ledger-defects`); human decides

## Summary

Six S1 defects are recorded (DEF-001–DEF-006); **none remain open**. They bind XP-060, XP-061, and XP-072 only. Five are `fix-now` (unguarded `num=0` with a dropped endpoint; unguarded non-positive log endpoints other than the zero rewrite; unguarded `base`; `size(f)<2`; `dh==0`). DEF-002 (`logspace` silent `0` → `1.d-12`) is `reproduce-faithfully`. XP-108 Fermi clamp `x*beta > 100` was **not** filed (observed saturation). A system-wide numeric tolerance was **refused** (DEC-005 already: comparison is per path). HTTP status mapping for `fix-now` rows belongs in the HTTP ADR / path test plans, not here.

---

## 1. Defect decisions

| ID | Defect / mismatch | Affected XP | Evidence | Decision | UAT impact | Story / DEC |
|----|-------------------|-------------|----------|----------|------------|-------------|
| DEF-001 | With an endpoint excluded, `num=0` skips `N<2` and divides by zero on start-only / end-only. | XP-060 | E3 `src/tools_grids.f90:16-26`; E3 `docs/modernization/paths/XP-060-linspace.md` §6 | fix-now | Port must reject this input with a defined HTTP error, not reproduce Fortran stop/div0. | TBD (HTTP ADR / path test plan) |
| DEF-002 | `start==0` or `stop==0` silently becomes `1.d-12` with no warning. | XP-061 | E3 `src/tools_grids.f90:42-43`; E3 `docs/modernization/paths/XP-061-logspace.md` §6; E3 `docs/DOMAIN.md` §9 | reproduce-faithfully | Parity fixture must expect the rewrite for zero endpoints. | TBD (path test plan) |
| DEF-003 | Negative (or otherwise non-positive after rewrite) endpoints can take `log` unguarded → NaN / abort. | XP-061 | E3 `src/tools_grids.f90:42-44`; E3 `docs/modernization/paths/XP-061-logspace.md` §6 | fix-now | Defined HTTP error; do not require NaN parity. | TBD (HTTP ADR / path test plan) |
| DEF-004 | Invalid `base` (`base<=0` or `base==1`) is unguarded. | XP-061 | E3 `src/tools_grids.f90:44`; E3 `docs/modernization/paths/XP-061-logspace.md` §6 | fix-now | Defined HTTP error. | TBD (HTTP ADR / path test plan) |
| DEF-005 | `size(f)<2` leads to out-of-bounds reads. | XP-072 | E3 `src/TOOLS.f90:180-185`; E3 `docs/modernization/paths/XP-072-deriv.md` §6; E3 `docs/DOMAIN.md` §9 | fix-now | Defined HTTP error; do not reproduce OOB. | TBD (HTTP ADR / path test plan) |
| DEF-006 | `dh==0` divides by zero. | XP-072 | E3 `src/TOOLS.f90:181-185`; E3 `docs/modernization/paths/XP-072-deriv.md` §6; E3 `docs/DOMAIN.md` §9 | fix-now | Defined HTTP error. | TBD (HTTP ADR / path test plan) |

## 2. Reproduce faithfully

| DEF ID | Expected port behavior | Parity fixture | Rationale |
|--------|------------------------|----------------|-----------|
| DEF-002 | Bug-compatible: exact-zero `start`/`stop` rewritten to `1e-12` before log; no warning. | TBD (path test plan) | Human 2026-08-25: profile `reproduce-then-backlog`; intentional-looking domain clamp; document and parity-test. |

## 3. Fix now

| DEF ID | Corrected expectation | Acceptance criterion | Approval source |
|--------|-----------------------|----------------------|-----------------|
| DEF-001 | Map invalid `num` (including `num=0` with an endpoint dropped) to a defined client HTTP error; do not crash or emit non-deterministic failure. | TBD (HTTP ADR / path test plan) | Human 2026-08-25 (product-owner test double): UB unsafe for HTTP/JSON |
| DEF-003 | Reject unguarded non-positive endpoints (other than DEF-002’s zero rewrite) with a defined client HTTP error; do not require NaN parity. | TBD (HTTP ADR / path test plan) | Human 2026-08-25: UB unsafe for HTTP service |
| DEF-004 | Reject `base<=0` or `base==1` with a defined client HTTP error. | TBD (HTTP ADR / path test plan) | Human 2026-08-25: UB unsafe for HTTP service |
| DEF-005 | Reject short series (`size(f)<2`) with a defined client HTTP error; do not reproduce OOB. | TBD (HTTP ADR / path test plan) | Human 2026-08-25: UB unsafe for HTTP service |
| DEF-006 | Reject `dh==0` with a defined client HTTP error. | TBD (HTTP ADR / path test plan) | Human 2026-08-25: UB unsafe for HTTP service |

## 4. Fix later

None.

## 5. Open defect decisions

None.

## Refused / not filed

- XP-108 `FUNCTIONS::fermi` clamp `x*beta > 100` → `0`: observed intentional saturation, not a defect. E3 `src/FUNCTIONS.f90:243-246`; E3 `docs/modernization/paths/XP-108-fermi.md` §6.
- System-wide numeric tolerance: methodology has no system-wide answer. Already DEC-005 (comparison is per path). Bounds belong in each path test plan.

## Links

- Path details: [`docs/modernization/paths/XP-NNN-*.md`](paths/)
- Decision register: [`docs/modernization/decision-register.md`](decision-register.md)
- Oracle: [`docs/modernization/oracle.md`](oracle.md)
- Migration plan: [`docs/modernization/migration-plan.md`](migration-plan.md)

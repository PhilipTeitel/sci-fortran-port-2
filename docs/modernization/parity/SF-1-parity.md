PARITY SUMMARY: result=Pass PAR-critical=0 PAR-high=0 PROV-critical=0 PROV-high=0 mismatches=0 blocked=0 oracleTier=T1

<!-- Parity report contract:
- First non-comment line must be PARITY SUMMARY.
- One row per Phase P criterion and covered XP-NNN.
- Comparison rules come from the path test plan. Do not apply a global numeric default when a path rule is missing; mark BLOCKED.
- Do not claim parity above the configured oracle tier.
-->

# Parity Report: SF-1 — S1 numeric helpers HTTP/JSON

**Date:** `2026-08-25`
**Oracle tier:** `T1 executable`
**Result:** `Pass`
**Execution paths:** `XP-060, XP-061, XP-072, XP-108`

## Summary

All four Phase P criteria (P1–P4) match the accepted T1 / XP-388 oracle pin under each path test plan’s tolerance rule `|a-b| <= max(1e-12, 1e-12 * max(|a|,|b|))`. Acceptance data is the embedded E1 recorded probe values from oracle.md v1 / `.oracle-sandbox/probe-out/run1.out` (Human-accepted until FIX files are written). No unreconciled mismatches; happy-path DEF decisions do not apply (DEF-001/003–006 are fix-now HTTP 400 on error paths; DEF-002 is reproduce-faithfully on a non-H1 edge covered outside P2).

---

## 1. Scope

| Story criterion | Path | Oracle source | Comparison rule | New-system command / test |
|-----------------|------|---------------|-----------------|---------------------------|
| P1 | `XP-060` | Live T1 / XP-388 `linspace(0,1,5)`; provisional FIX-060 TBD; embedded E1 from `run1.out` `=== linspace-5 ===` | `|a-b| <= max(1e-12, 1e-12 * max(|a|,|b|))` (XP-060-tests.md §2) | `tests/SciFor.Parity.Tests/Xp060ParityTests.cs::parity_XP060_P1` |
| P2 | `XP-061` | Live T1 / XP-388 `logspace(1,1000,5)`; provisional FIX-061 TBD; embedded E1 from `run1.out` `=== logspace-5 ===` | same bound (XP-061-tests.md §2) | `tests/SciFor.Parity.Tests/Xp061ParityTests.cs::parity_XP061_P2` |
| P3 | `XP-072` | Live T1 / XP-388 parsed `dy` on `xy2.data`; provisional FIX-072 TBD; fixtures from probe `=== deriv-xy2 ===` | same bound on parsed numbers, not list-directed text (XP-072-tests.md §2) | `tests/SciFor.Parity.Tests/Xp072ParityTests.cs::parity_XP072_P3` |
| P4 | `XP-108` | Live T1 / XP-388 five-point `beta=100`; provisional FIX-108 TBD; embedded E1 from `run1.out` `=== fermi-beta100 ===` | same bound (XP-108-tests.md §2) | `tests/SciFor.Parity.Tests/Xp108ParityTests.cs::parity_XP108_P4` |

## 2. Results matrix

| XP | AC | Fixture / data | Comparison | Legacy result | New result | Result | Defect decision |
|----|----|----------------|------------|---------------|------------|--------|-----------------|
| XP-060 | P1 | Embedded E1 `0, 0.25, 0.5, 0.75, 1` (probe `linspace-5`; FIX-060 TBD) | tolerance-based 1e-12 | `.oracle-sandbox/probe-out/run1.out` linspace-5 | `LinearGridService.Compute(0,1,5)` via `parity_XP060_P1` | match | none (DEF-001 is A3 error path) |
| XP-061 | P2 | Embedded E1 es24.17 probe values (FIX-061 TBD) | tolerance-based 1e-12 | `.oracle-sandbox/probe-out/run1.out` logspace-5 | `LogarithmicGridService.Compute(1,1000,5)` via `parity_XP061_P2` | match | none on H1 (DEF-002 zero-rewrite is A5) |
| XP-072 | P3 | `Fixtures/xp072-{x,y,dy-expected}.txt` (1024 pts; FIX-072 TBD) | tolerance-based 1e-12 on parsed `dy` | probe `deriv-xy2` second column | `FiniteDifferenceService.Compute(y, dh)` via `parity_XP072_P3` | match | none (DEF-005/006 are A8 error paths) |
| XP-108 | P4 | Embedded E1 five-point sample (FIX-108 TBD) | tolerance-based 1e-12 | `.oracle-sandbox/probe-out/run1.out` fermi-beta100 | `FermiDiracService.Evaluate` via `parity_XP108_P4` | match | none (saturation correct) |

## 3. Mismatches

None.

## 4. Provenance gaps

None. FIX-060/061/072/108 files are not yet written; Human accepts live T1 / embedded recorded probe values from the oracle.md v1 pin as Phase P acceptance data. Tests cite those E1 values explicitly; grade remains E1 verified (not E4/E5).

## 5. Commands and evidence

| Command / inspection | Result | Evidence excerpt |
|----------------------|--------|------------------|
| `DOTNET_ROOT=$HOME/.dotnet dotnet test tests/SciFor.Parity.Tests/SciFor.Parity.Tests.csproj --no-build` | pass | `Passed! - Failed: 0, Passed: 4, Skipped: 0, Total: 4, Duration: 5 ms - SciFor.Parity.Tests.dll (net8.0)` |
| Inspect `NumericTolerance.NearlyEqual` | pass | Implements path-plan rule `max(1e-12, 1e-12 * max(|a|,|b|))` (DEC-005; not profile 1e-6 hint) |
| Compare P1/P2/P4 expected arrays to `.oracle-sandbox/probe-out/run1.out` | pass | linspace-5 / logspace-5 / fermi-beta100 match embedded expectations |
| Compare `Fixtures/xp072-dy-expected.txt` to probe `deriv-xy2` `$NF` | pass | 1024 lines; `diff` empty vs probe second column |

*Created: 2026-08-25*

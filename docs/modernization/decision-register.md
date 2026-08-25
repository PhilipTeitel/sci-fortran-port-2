# Decision Register

**Date:** 2026-08-25
**Owner:** Migration Strategist (`/record-decision`); human decides

## Summary

Seven decisions are `decided` (DEC-001–DEC-007). All bind the first product slice XP-060, XP-061, XP-072, XP-108. None are `open`. First-slice HTTP/JSON contract is ADR-001–004. Per-path parsed-numeric bounds live in the path test plans. Slice prerequisites are in the migration plan. Gate M1 is accepted.

---

## 1. Decisions

| ID | Question | Answer | Status | Evidence | Date | Affects |
|----|----------|--------|--------|----------|------|---------|
| DEC-001 | What is the target stack for this port? | C# / .NET 8 / ASP.NET Core HTTP/JSON services. | decided | E2 `.cursor/workflow.config.yml` `modernization.targetStack` | 2026-08-25 | first slice XP-060, XP-061, XP-072, XP-108; IMP-001 |
| DEC-002 | Is the product a Fortran ABI / `libscifor.a` drop-in? | No. Callers invoke HTTP/JSON. Not a Fortran `.mod` / `libscifor.a` drop-in. | decided | E2 owner; IMP-001; profile target framework | 2026-08-25 | IMP-001; XP-060, XP-061, XP-072, XP-108 |
| DEC-003 | Wrap `libscifor.a` or rewrite the first-slice helpers? | Rewrite XP-060, XP-061, XP-072, XP-108 in C# behind those HTTP/JSON services. Do not wrap `libscifor.a` for the POC product. | decided | E2 owner; IMP-001 candidate pattern rewrite | 2026-08-25 | IMP-001; XP-060, XP-061, XP-072, XP-108 |
| DEC-004 | Which legacy oracle is binding for first-slice numeric parity? | Accept the bounded T1 pin in oracle.md v1: Darwin 25.5.0 arm64 / Homebrew gfortran 16.1.0 / OpenBLAS 0.3.34 / `FFT_BACKEND=NR`. Preserve XP-386, XP-387, XP-388 as oracle tooling, not product services. | decided | E2 owner (oracle.md v1 pin; profile `oracleTier: T1 executable`) | 2026-08-25 | RISK-003; IMP-006; XP-060, XP-061, XP-072, XP-108; XP-386, XP-387, XP-388 |
| DEC-005 | Global numeric tolerance vs per-path parsed numbers? | Comparison is per path. Parsed numbers within a per-path tolerance. Not byte-identical Fortran format text. Not a system-wide tolerance. Do not use `fidelity/golden/` or `fidelity.sh` Python goldens as the observed corpus. Profile hint `0.000001` is not a gate. | decided | E2 profile `modernization.parity.comparisonRules: per-path`; house rule comparison is per path | 2026-08-25 | RISK-002; IMP-002, IMP-005; XP-060, XP-061, XP-072, XP-108 |
| DEC-006 | What is in the first product slice vs out of slice? | Success is XP-060, XP-061, XP-072, XP-108 as HTTP/JSON with that numeric parity. Numutils CLIs XP-370–XP-385 are not product services in this slice. XP-386, XP-387, XP-389 are not product services. FFT/LAPACK/MATRIX/gnuplot/`SYSTEM` paths are out of this slice. | decided | E2 owner; profile port library/CLI entry points that callers would consume as services | 2026-08-25 | first slice; XP-370–XP-389 |
| DEC-007 | May the C# tree include Numerical Recipes or Intel Confidential MKL headers? | No. Do not copy DEP-006 or DEP-004 into the C# tree. First-slice execution of the four helpers does not require those units. Snapshot-only oracle containment stands. | decided | E2 owner (oracle.md §3; `LICENSE.md` methodology-eval POC) | 2026-08-25 | RISK-004; DEP-006, DEP-004; IMP-013 |

Status `open` means an owner decision has not been made. That is not `no-route`.

Recommended ADR (Architect writes; not a DEC): HTTP routes, JSON field names, and invalid-input status for XP-060, XP-061, XP-072, XP-108 (RISK-001 / IMP-004).

## 2. Open decisions that bind a slice

None.

## Links

- Migration plan: [`docs/modernization/migration-plan.md`](migration-plan.md)
- Defect ledger: [`docs/modernization/defect-ledger.md`](defect-ledger.md)
- ADRs: [`docs/decisions/`](../decisions/)
- Assessment: [`docs/modernization/ASSESSMENT.md`](ASSESSMENT.md)

# SciFor Fortran Port

This repository is a port of SciFor/SciFortran from Fortran 90/95 (static library `libscifor.a`) to C# / .NET 8 / ASP.NET Core HTTP/JSON services. Assessment verdict is `go-with-conditions` on a bounded `T1 executable` oracle; candidate first slice is XP-062, XP-063, XP-074, XP-110 (harness XP-223–XP-225). See [`docs/modernization/ASSESSMENT.md`](docs/modernization/ASSESSMENT.md).

## Table of Contents

- [Modernization](#modernization)
- [Requirements](#requirements)
- [Backlog Items](#backlog-items)
- [License](#license)

## Modernization

### What this is

This repo ports SciFor/SciFortran, a Fortran 90/95 numeric library (`libscifor.a`, no application framework), to C# / .NET 8 ASP.NET Core HTTP/JSON services. Legacy behavior is evidence for recovery and parity, not an automatic product requirement. The POC first slice rewrites `linspace`, `logspace`, `fermi`, and `deriv` as those services and compares parsed numbers to a bounded T1 oracle. Tensions that remain (harness goldens, HTTP contract ADR) live in the assessment.

- Legacy repo: `/Users/philipteitel/code/ADD-migrations/sci-fortran-legacy` (`e586903a26cc50ca8942f20ca3bccbd8814e6252`)
- Source stack: Fortran 90/95 free-form modules / static library `libscifor.a` (no application framework)
- Target stack: C# / .NET 8 / ASP.NET Core (HTTP/JSON services)
- Assessment: [`docs/modernization/ASSESSMENT.md`](docs/modernization/ASSESSMENT.md)

### Lane status

| Field | Value |
|-------|-------|
| Phase | Assessment |
| Verdict | go-with-conditions |
| Oracle tier | T1 executable (bounded) |
| Gate | M1 pending |
| First / current slice | XP-062, XP-063, XP-074, XP-110 as HTTP/JSON; harness XP-223, XP-224, XP-225 |
| Next command | `/record-decision` (owner answers in ASSESSMENT.md §4; assign DEC-NNN) |

Phase stays `Assessment` or `Planning` here. Recovery and delivery progress show up in the Artifact index (path details, test plans) and in **Backlog Items**. Do not add a third progress narrative.

### Artifact index

| Artifact | Owner | Status | Link |
|----------|-------|--------|------|
| Execution path inventory | Archaeologist | v1 Snapshot | [`docs/modernization/execution-path-inventory.md`](docs/modernization/execution-path-inventory.md) |
| Dependency inventory | Migration Strategist | v2 Snapshot | [`docs/modernization/dependency-inventory.md`](docs/modernization/dependency-inventory.md) |
| Dependency graph | Archaeologist | v1 Snapshot | [`docs/modernization/dependency-graph.md`](docs/modernization/dependency-graph.md) |
| Impedance analysis | Migration Strategist | v2 Snapshot | [`docs/modernization/impedance-analysis.md`](docs/modernization/impedance-analysis.md) |
| Oracle | Implementer | v1 Snapshot | [`docs/modernization/oracle.md`](docs/modernization/oracle.md) |
| Assessment | Migration Strategist | present | [`docs/modernization/ASSESSMENT.md`](docs/modernization/ASSESSMENT.md) |
| Decision register | Migration Strategist | missing | [`docs/modernization/decision-register.md`](docs/modernization/decision-register.md) |
| Path details | Archaeologist | missing | [`docs/modernization/paths/`](docs/modernization/paths/) |
| Purpose | Modeler | missing | [`docs/PURPOSE.md`](docs/PURPOSE.md) |
| Domain model | Modeler | missing | [`docs/DOMAIN.md`](docs/DOMAIN.md) |
| Defect ledger | Archaeologist | missing | [`docs/modernization/defect-ledger.md`](docs/modernization/defect-ledger.md) |
| Migration plan | Migration Strategist | missing | [`docs/modernization/migration-plan.md`](docs/modernization/migration-plan.md) |
| Path test plans | Architect | missing | [`docs/modernization/test-plans/`](docs/modernization/test-plans/) |
| Port stories | Architect | missing | [`docs/features/`](docs/features/) |
| Parity reports | QA | missing | [`docs/modernization/parity/`](docs/modernization/parity/) |

## Requirements

**Purpose and domain model**

- Purpose: TBD (`docs/PURPOSE.md` not written; `/recover-domain` / `/define-purpose` have not run)
- Domain model: TBD (`docs/DOMAIN.md` not written)

**Architecture decisions included for traceability for backlog alignment**

None yet.

## Backlog Items

None yet. Architect creates epic/story rows after `/plan-migration` and `/plan-port-story`.

## License

See [`LICENSE.md`](LICENSE.md). This tree is **never** to be used except to evaluate the Artifact-Driven Development methodology. It is not MIT-licensed and is not a production SciFor port.

# test-modernization

Drive a Human-gated test of the Artifact-Driven Development modernization-port lane. This command is **project-local** (`.cursor/commands/test-modernization.md`). Before acting, resolve the workflow profile (target project `.cursor/workflow.config.yml`, then user `~/.cursor/workflow.config.yml`) and use its command directory for ADD role commands, modernization paths, `modernizationPort` lane, agent bindings, assessment statuses, and target/source stack pins.

**Command-agent binding:** This command is role-bound to `agents.productOwner` (this project's `.cursor/agents/sci-fortran-product-owner.md`). Before executing any step, load that agent definition and follow it as binding role context. If `agents.productOwner` is missing, stop and tell the user to add that mapping; do not silently play Human as another role.

This command is a **workflow test**. The product owner is a test double for the Human, not a production stakeholder. Role agents still own their artifacts. The product owner answers gates and `DEC-NNN` / `DEF-NNN` questions so the lane can proceed, and reports whether the methodology held.

The methodology docs live at `~/code/Artifact-Driven-Development` (`MODERNIZATION.md`, `PROCESS.md`, `ROLES.md`).

## Command-Agent Binding And Delegation

Before executing the sequence, load the configured house rules, this command spec, the product-owner agent definition, and the active workflow profile. For each modernization-port phase, read the phase command spec from `cursor.commandsDir` and its configured agent definition before acting:

| Phase command | Agent binding |
|---|---|
| `/inventory-paths` | `agents.archaeologist` |
| `/inventory-dependencies` | `agents.migrationStrategist` |
| `/map-dependency-graph` | `agents.archaeologist` |
| `/analyze-impedance` | `agents.migrationStrategist` |
| `/build-oracle probe` | `agents.implementer` |
| `/assess-modernization` (synthesis) | `agents.migrationStrategist` |
| `/record-decision` | `agents.migrationStrategist` (records); `agents.productOwner` (decides) |
| `/plan-migration` | `agents.migrationStrategist` |
| `/document-legacy` | orchestrator: Archaeologist + Modeler |
| `/trace-path` | `agents.archaeologist` |
| `/recover-domain` | `agents.modeler` |
| `/ledger-defects` | `agents.archaeologist` (records); `agents.productOwner` (decides) |
| `/refine-feature` | `agents.architect` |
| `/plan-path-tests` | `agents.architect` |
| `/design-application` | `agents.architect` |
| `/plan-project` | `agents.architect` |
| `/plan-port-story` | `agents.architect` |
| `/complete-port-story` | orchestrator: Implementer, Auditor, QA, Docs-PM |

When subagent delegation is available, run each role phase in the configured role subagent and pass the legacy repo path, target stack, active workflow profile, command spec, loaded agent definition, and in-scope `XP-NNN` IDs as binding context. Keep Human-gate answers in this product-owner chat. If subagent delegation is unavailable, continue in the current chat only after loading the same agent definition and state in the output which phases used this fallback.

Always pass the product-owner answer into `/record-decision` and `/ledger-defects`. Those ADD commands do not pull from this agent.

## Inputs

- Optional start point: `from: <command or gate>` (default: current README **Lane status** next command, or `/assess-modernization` if the hub is missing).
- Optional stop point: `until: <command or gate>` (default: through `/plan-port-story` for the first slice unless the user asked for `/complete-port-story` or a full replay).
- Optional slice or `XP-NNN` scope. If omitted, use the assessment's candidate first slice.
- The configured legacy repo path unless overridden.

## Preconditions

- `agents.productOwner` is configured and readable.
- The target repo is where artifacts will be written.
- The legacy repo is treated as read-only.
- The user asked to test, exercise, dry-run, or automate the modernization workflow (this command counts as that ask).

## Sequence

1. Resolve the workflow profile. Read `workflow.lanes.modernizationPort.commands`, `modernization.*` pins, and README **Lane status** if present.
2. Load `~/code/Artifact-Driven-Development/MODERNIZATION.md` enough to know gates M1–M5.
3. Decide resume vs replay. Do not wipe existing snapshots. Resume at the next incomplete command unless the user asked for a full replay.
4. For each remaining command until the stop point:
   1. Delegate the command to the bound role as in the table above.
   2. On Human gate M1–M5, or an open `DEC-NNN` / `DEF-NNN`, answer using the product-owner judgment policy.
   3. Have the owning role record the answer (`/record-decision`, `/ledger-defects`) with the answer included. Do not write those artifacts from this role.
   4. Log methodology findings (ownership violations, skipped gates, compound evidence grades, global-as-slice-gate, invented requirements, snapshot edits).
5. Do not catalog the entire legacy system before the first slice. Scope `/document-legacy` and later recovery to the first-slice `XP-NNN` IDs.
6. Stop and ask the user on non-product-owner questions (role-contract conflicts, missing legacy path, legal, changing binding profile pins, `no-go` that would end the test).

## Hard rules

- Do not skip M1–M5. A test that bypasses a Human gate is a methodology defect.
- Do not let the product owner write role-owned artifacts.
- Do not treat this test double as a production sign-off.
- Do not invent IDs. Use IDs the owning agents assigned.
- Do not use `blocked` as a dependency disposition.

## Output

Return the product owner's **Workflow Test Report**:

- Mode and scope.
- Lane position (last command, current gate, next command).
- Owner answers given (`DEC-NNN`, `DEF-NNN`, gates).
- Delegation vs fallback phases.
- Methodology findings (`WTF-NNN` rows, or `None.`).
- Suggested next ADD command.

## Examples

- `/test-modernization`
- `/test-modernization from: /assess-modernization until: M1`
- `/test-modernization from: /record-decision until: /plan-port-story`
- `/test-modernization until: /complete-port-story`

This command is available in chat with `/test-modernization`.
It expects an optional start and stop point; otherwise it resumes the current modernization lane.

---
name: project-memory
description: >-
  Persistent compressed memory for RELIC HAULERS. Read references/memory-snapshot.md and
  references/sprint-tracker.md at the start of a session instead of re-reading all 16 GDD
  files (~160 KB) or scanning the repository. Update sprint-tracker.md after finishing a slice
  or architectural decision so future turns and sessions consume minimal quota.
---

# Project Memory Protocol — RELIC HAULERS

> Goal: Give any AI coding session 95% of project context in **< 4 KB** of reads instead of burning quota on `kich-ban/00..15` (`~160 KB`).

## Mandatory Session Workflow

### 1. Start of Session (Max 2 Short Reads)
1. Read [references/sprint-tracker.md](./references/sprint-tracker.md) to see what Sprint/task is active, which C#/scene files already exist, and any recent decisions or bugs solved.
2. Read [references/memory-snapshot.md](./references/memory-snapshot.md) for locked invariants, formulas, folder layout, and the exact **GDD Lookup Map** (which tells you the exact file and section to open *only* when implementing that system).
3. **DO NOT** read unrelated `kich-ban/*.md` files "just to be thorough".

### 2. During Implementation (On-Demand Lookup Only)
- Open a `kich-ban/*.md` file **only** for the system you are currently coding, using `Select-String` / `rg -n` first and reading only the matching section (`StartLine..EndLine`, `<= 120` lines).
- Do not re-read `memory-snapshot.md` in the same session unless compacted.

### 3. End of Milestone / Sprint Slice (Write Back Memory)
Update [references/sprint-tracker.md](./references/sprint-tracker.md) **only when meaningful state changes** (keep entries terse, bullet-only, `< 150` lines total):
- Move completed items from `In Progress` / `Next` to `Done` with exact file paths created/modified.
- Record any new architectural decision, ScriptableObject ID, or bug fix invariant in `Locked Decisions` so the next agent does not repeat the investigation.
- Keep `sprint-tracker.md` compact; prune obsolete notes instead of appending logs forever.

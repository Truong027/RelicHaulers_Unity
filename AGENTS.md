# AGENTS.md — Workspace Rules (RELIC HAULERS)

1. **Save Quota First**: At the start of any coding session, activate `project-memory` (read `references/sprint-tracker.md` + `references/memory-snapshot.md`) and `quota-saver-coding`. **Never** bulk-read all `kich-ban/*.md` files (`00..15`) when starting a task.
2. **Respect Memory-First Lookup**: Open individual `kich-ban/*.md` files only by targeted line range for the specific system you are implementing.
3. **Never Read Unity Bloat**: Exclude `Library/`, `Temp/`, `Logs/`, `obj/`, `.meta`, binary assets, and huge raw `.unity`/`.prefab` YAML files; use Unity MCP tools and `Assets/_Project/Scripts` searches instead.
4. **Respect Production Gate Scope**: Only implement items allowed in the active gate of `.agents/skills/project-memory/references/sprint-tracker.md` (**Gate 1: Greybox** → **Gate 2: Vertical Slice** → **Production**).
5. **Preserve Locked Invariants**: Do not violate physics separation (`VisualRoot`), `Gadget Slot Cap = 2`, Pet `Zero Self-Collision`, `Sky-Aquarium` Pod-Quota ban, Revive `CollapseTimer <= 0` block, or `inkle/ink` zero dead-ends (`kich-ban/ink/relic_haulers_story.ink`).
6. **Update Memory on Milestone Completion**: Record completed Gate slices and new files concisely in `.agents/skills/project-memory/references/sprint-tracker.md`.

---
name: narrative-designer
description: Project-local narrative design & branching Ink (.ink) dialogue skill for RELIC HAULERS. Enforces 3-Act structure, world bible consistency, NPC character voices, UI character limits, and zero-dead-end verification compatible with inkle/ink and ink-unity-integration.
---

# Narrative Designer Skill — RELIC HAULERS (`inkle/ink`)

Use this skill whenever writing, expanding, or reviewing story progression, 3-Act endings, Secret Logs, or NPC branching dialogues in `kich-ban/ink/` or `Assets/_Project/Narrative/`.

## 1. Files in This Skill
1. **[world-bible.md](world-bible.md)** — Canonical lore, Sky-Shards, factions, currencies, 6 Biomes, 14 Relics, Survival/Ranch/Aquarium terms, and Revive Tether rules. Never invent contradicting lore.
2. **[character-voices.md](character-voices.md)** — Personality, pronouns (*xưng hô*), catchphrases, secrets, and attitude triggers for **VEX**, **Bác Kael**, **Pip**, **Giáo Sư Myla**, and **The Hauler**.
3. **[template.ink](template.ink)** — Standard `.ink` scaffold with project `VAR` flags, `EXTERNAL` C# bindings for `ink-unity-integration`, and safe divert/termination patterns.

## 2. Hard Narrative & Technical Rules

### A. 3-Act & 3-Ending Structure
- **Act 1 (`act == 1`, Chapters 1–2)**: Rookie optimism, learning physics hauling, mining/fishing, first encounters with anomalies and VEX's tether revive.
- **Act 2 (`act == 2`, Chapters 3–4)**: Discovering that the Hauler Guild is harvesting Sky-Anchors for an ancient machine and *Golem Warden* guards the last core.
- **Act 3 (`act == 3`, Chapters 5–6)**: VEX confesses being the consciousness backup of the First Expedition Captain; climax leads strictly into one of **3 verified endings**:
  - `END_GUILD_CONTRACT` (Default/Pragmatic)
  - `END_BREAK_CORE` (Kael & Myla Preservation)
  - `END_TRUE_VEX_SKY` (Requires `logs_collected >= 12` + freeing VEX)

### B. Variable Naming Conventions (`VAR` in `.ink`)
Always prefix persistent Ink variables strictly by role:
- `flag_*` (`bool`): Story decisions & milestones (e.g., `flag_trusted_kael`, `flag_myla_preserve_pact`, `flag_vex_confessed`, `flag_pip_vip_deal`).
- `stat_*` (`int`): Counters synced from `SaveSystem` / `DialogueBlackboard` (e.g., `stat_logs_collected`, `stat_revives_used_total`, `stat_revives_this_run`, `stat_broken_relics_last_run`, `stat_museum_count`, `stat_guild_tokens`).
- `state_*` (`string`): Runtime context (e.g., `state_last_run_outcome`, `state_archetype`, `state_next_biome`).

### C. UI Line Length Budgets (Prevent Text Overflow)
- **HUD Radio Toast (`VEX` mid-run)**: **Max `120` characters**, `1–2` sentences max, no player choices (`*`/`+`).
- **Hub / Shop Dialogue Box (`Kael`, `Pip`, `Myla`)**: **Max `220` characters per beat** (split long lore into sequential lines or player choice follow-ups).
- **Choice Button Pills (`*` / `+`)**: **Max `48` characters** (short, expressive Vietnamese).

### D. Zero Dead-End & Anti-Logic-Bug Verification (Inky & Script Check)
Before finishing any `.ink` edit:
1. **No empty falls / missing diverts**: Every knot (`=== knot_name ===`) and stitch (`= stitch_name`) must explicitly divert (`-> target`, `-> DONE`, or `-> END`). Never leave a dangling block.
2. **Always provide a fallback choice**: If choices inside a hub/shop menu use conditions (`{ condition }`), include at least one unconditional exit choice (`+ [Rời đi] -> DONE`).
3. **One-time vs Repeatable choices**: Use `*` (sticky-off / once-only) for story confessions and quest rewards (`give_item`, `unlock_log`), and `+` (sticky) only for repeatable menus/shop browsing so players cannot farm rewards infinitely!
4. **Sync Revive state honestly**: Never trigger `Contract Failed` or `First-Time Death Bonus` dialogue when `stat_revives_this_run > 0` ended in a Victory/Escape; react specifically to Tether Revives (`flag_vex_tether_used`) instead.

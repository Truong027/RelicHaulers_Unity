# Memory Snapshot — RELIC HAULERS (1-Page Brain)

> Last synced with GDD v1.0 (`kich-ban/00..15` + `kich-ban/ink/relic_haulers_story.ink`). Use this instead of reading all 16 GDD files.

## 1. Core Specs & Engine Target
- **Genre**: 3D Stylized Physics Extraction + Roguelite + Light Survival (`1P` Solo default, `1–4P` Co-op ready).
- **Engine / Style**: Unity 6 / 2022.3 LTS (**URP**), Stylized Low-Poly 3D, Humanoid/Mixamo rig (`1.6m` height), `PAL_Master_Stylized.png` (256x256) + `SG_StylizedToonLit` + `SG_OcclusionFade`.
- **Currencies**: `$` (**Pod Value** = stage quota), `¢` (**Surplus Credits** = excess `$` spent in `02_AirshipShop`), `GT` (**Guild Tokens** = meta-currency; keep `50%` on `Contract Failed`).

## 2. Non-Negotiable Logic Invariants (Never Break These!)
1. **Physics Separation**: Never scale `Transform` containing `Collider`/`Rigidbody`; apply Squash & Stretch only to `VisualRoot`. Keep `FloatingCapsule` `RideHeight = 0.35m`, `FixedUpdate`, `Interpolate`.
2. **Input Priority (`E` & `RMB`)**: `E` prioritizes `Interactable` (Pod/NPC/Shrine/Chest) without dropping held relics; blocks picking up objects the player is standing on (`IsGroundedOn`). Holding `RMB` (throw aim) cancels safely via `LMB` or `E`. Sprint (`Hold Shift`) costs `0` Stamina; Dash (`Tap Shift`/`V`/`Mouse4`) costs `25` Stamina (`22/s` regen).
3. **Loot / Fragility**: Relics deal impact damage above `breakThreshold`, min value `25%` while intact, shatter into `Scrap Shard` (`1kg`, `15% baseValue`) at `0 HP`. Spawn value per island always `>= 1.8x Quota`.
4. **Survival Anti-Exploit Rules**:
   - Mining/Fishing live in `OrePouch` (`20` slots, `+10` upgraded); on Run fail retain `50%` (`75%` with Tech Tree).
   - `Sky-Ranch` (4 species) & `Sky-Aquarium` (12 fish) progress strictly by **Run Tick** (`1 stage = 1 Growth Tick`, max `3` stored) — **zero real-time AFK timers**.
   - **Aquarium-raised fish CANNOT be submitted into Pod Quota** (Galley buff / Fish Scales / Museum / small GT sale only).
   - **Tactical Gadgets** capped at **`2` slots per Run** (`Q`/`4`/`5`, Gamepad `Y`).
   - **Pet Companion**: Max `1` Pet active; **`Zero Self-Collision`** layer (never pushes Player or breaks Relics).
5. **Revive Anti-Bug Rules**:
   - Solo Hard Cap **`2` revives/stage** (`Second Wind Tether` `-15s` Collapse Timer / `gad_revive_capsule` `-5s` Timer, max `1`/Run).
   - **Block Revive when `CollapseTimer <= 0` or `LeftBehindByPod`** (preserve unused capsule, go straight to `Contract Failed`).
   - Always teleport revived player to cached `LastSafeGroundPosition` (updated every `0.5s`), reset `rb.linearVelocity = Vector3.zero`, grant `3.0s` I-Frame, drop held relic safely (no dupe/no destroy), and **do NOT** reset `Utility_Slot`, Gadgets, or award `First-Time Death Bonus`.
6. **Narrative (`inkle/ink`)**:
   - Real-time barks via `DialogueDirector`; 3-Act story, NPC choices, post-revive reactions & **3 Endings** (`END_GUILD_CONTRACT`, `END_BREAK_CORE`, `END_TRUE_VEX_SKY`) via `InkNarrativeManager` reading `kich-ban/ink/relic_haulers_story.ink`. Every knot/stitch must divert cleanly (`badDiverts = 0`).

## 3. Targeted GDD Lookup Map (Read ONLY Needed Lines)
| When Coding... | Open File (`E:\GAMETEST\kich-ban\`) | Key Sections / IDs |
| :--- | :--- | :--- |
| Characters, Art, Tri Budgets | `01_THE_GIOI_NHAN_VAT_VA_ART_STYLE.md` | 3 Archetypes, 4 Modular Slots, Blender Export rules |
| Movement, IK, Ragdoll, Grab/Throw | `02_CORE_GAMEPLAY_VA_PHYSICS_ANIMATION.md` | Floating Capsule, RigBuilder 3 IKs, Fragility formula |
| Run Loop, Story Acts, Side Contracts | `03_HE_THONG_MAN_CHOI_VA_TIEN_TRINH.md` | 5-Stage Branching Loop, 4 Side Contracts, 6-Ch Pacing |
| Unity Folders, SO Schemas, 6 Sprints | `04_KIEN_TRUC_UNITY_VA_KE_HOACH_AI_AGENT.md` | `Assets/_Project/`, `IGrabbable`/`IKnockbackable`, Sprint 1–6 |
| Relics, Upgrades, Shop, Gear IDs | `05_DANH_MUC_RELIC_UPGRADE_VA_SHOP.md` | 14 Relics (`rel_*`), 16 Upgrades (`upg_*`), 7 Shop items |
| Enemies, Boss, Traps, Wave Director | `06_CHI_TIET_QUAI_VAT_BOSS_VA_LEVEL_DESIGN.md` | `PebbleImp`, `EchoStalker`, `SpringMimic`, `GustWisp`, `Crusher`, `GolemWarden` |
| Edge Cases, Save JSON, Input, Assets | `07_LUAT_CHI_TIET_UI_AM_THANH_VA_ASSET_MANIFEST.md` | Save schema, PC/Gamepad map, VEX toasts, FBX/VFX/SFX IDs |
| 12 Stage Layouts, 2 Hubs, Endless | `08_BAN_DO_VA_MAN_CHOI_MO_RONG.md` | `01_SkyDockHub`, `02_AirshipShop`, Stages 1-1..6-2, Endless |
| Synergies, Overcharge, Events, Tech | `09_CO_CHE_GIU_CHAN_VA_THU_HUT_NGUOI_CHOI.md` | 6 Synergies, 4 Events, Museum, Guild Tech Tree |
| Colors, Lighting, HUD Wireframe | `10_CAU_TRUC_HINH_ANH_VA_ART_BIBLE.md` | Biome Hex palettes, URP lights, HUD anchors, QA Gate |
| Dynamic Barks & Cross-NPC Banter | `11_HE_THONG_HOI_THOAI_VA_DAN_NPC.md` | `DialogueBlackboard`, Priority Queue, `T` Salute |
| Utility Slot, Knowledge Shortcuts | `12_BO_SUNG_CO_CHE_HAY_TU_CAC_GAME_NOI_TIENG.md` | `Utility_Slot` (`C`), 3 Shrines, Radar Flicker |
| Room Graph, WeakSpot Mining, Fishing | `13_HE_THONG_SINH_TON_CAU_CA_DAO_QUANG_VA_MAP_PHONG_NOI_PHONG.md` | 10 Room Archetypes, `RoomGraphGenerator`, 6 Ores, 12 Fish, 6 Meals |
| Alloy Crafting, Ranch, Aquarium | `14_CHE_TAO_QUANG_CHAN_NUOI_VA_NUOI_CA.md` | 4 Alloys, 6+1 Gadgets, 5 Permanent Gear, 4 Ranch, Aquarium |
| Revive Logic & Ink Architecture | `15_CO_CHE_HOI_SINH_VA_COT_TRUYEN_INK.md` | Solo/Co-op Revive, `EXTERNAL` C# API, 3 Endings |
| Playable Ink Branching Script | `ink/relic_haulers_story.ink` | Knots, `VAR`, `EXTERNAL`, 3 Endings |

# Sprint Tracker & Living Project State — RELIC HAULERS

> Keep this file concise (`< 150` lines). Update at the end of every milestone.

## Current Phase & Active Hard Gate
- **GĐ 1 — Concept & GDD**: COMPLETE (`kich-ban/00..15` + `kich-ban/ink/relic_haulers_story.ink` verified).
- **ACTIVE GATE**: **Cổng Khóa Cứng 1 — GĐ 2: Prototype / Greyboxing** (NOT STARTED — Unity project not yet initialized).
- **Strict Scope Rule**: Build ONLY the active Gate items below. Do **NOT** implement Post-Gate-2 features (Fishing, Ranch, Aquarium, Revive, full Ink, Ch 2–6) until Cổng 1 & Cổng 2 are passed!

## 7-Stage Gate Queue (Ref: `04_KIEN_TRUC_UNITY_VA_KE_HOACH_AI_AGENT.md`)
- [ ] **Cổng Khóa Cứng 1 (GĐ 2: Prototype / Greyboxing)** — *Movement, Carry/Bag & Extraction Loop*:
  - [x] **Slice 1A (Movement & Temp Hub)**: `Packages/manifest.json` (`com.unity.cinemachine: 3.1.7`), `Assets/_Project/` layout, `HaulerInputReader.cs`, `FloatingCapsuleController.cs` (fixed spring `260/28` + snappy `~1.8m` jump), `SquashStretchVisual.cs`, `GreyboxCameraRig.cs` (XZ lock + smooth Y), `GreyboxHudOverlay.cs`, `ScenePortalPad.cs`, `GreyboxPrototypeBuilder.cs` → playtested & confirmed smooth.
  - [ ] **Slice 1B (Carry/Throw + 1 Chest)**: Implemented `IInteractable.cs`, `IGrabbable.cs`, `RelicItem.cs`, `GreyboxChest.cs`, `CarrySystem.cs`, updated `FloatingCapsuleController` (`GroundCollider` anti-fly, carry speed weight, Shoulder Bash), `ScenePortalPad`, `GreyboxHudOverlay`, `GreyboxPrototypeBuilder` — awaiting Unity menu rebuild (`RelicHaulers/Gate 1/Build Prototype Scenes (Slice 1A + 1B)`) & playtest.
  - [ ] **Slice 1C (1 Ore Vein + OrePouch Grid)**: `OreVein.cs` (`WeakSpotRing`), `MiningSystem.cs`, `OrePouch` bag UI.
  - [ ] **Slice 1D (Extraction Loop)**: `ExtractionPod.cs`, Collapse Timer, placeholder summary, `1` test `Pebble Imp` Ragdoll.
  - **LOCKED**: All auxiliary mechanics (Fishing, Ranch, Aquarium, Revive) & Ink (text placeholders only).
- [ ] **Cổng Khóa Cứng 2 (GĐ 3: Vertical Slice `10–15m`)** — *URP Art Pipeline, Data-Driven & Ink*:
  - `1` complete Biome (`2–3` Room Archetypes + Runtime NavMesh) + `01_SkyDockHub` + Humanoid Rig/IK/URP Toon Lit.
  - `3` Chest tiers, `2` Ore Veins, `1` Relic (`rel_crystal_vase`), `2` Upgrades (`SO`), `3` Craftables + Basic Forge + JSON Save/Load.
  - Ink connected to `1` main NPC (`Bác Kael`) + `1` Relic story flag.
- [ ] **Sau Cổng 2 (GĐ 4: Production)** — *Modular Scale-Out*:
  - Full `6` Biomes / `12` Stages / `10` Room Archetypes; `14` Relics, `16` Upgrades, `18` Craftables; Fishing, `Sky-Ranch`, `Sky-Aquarium`, Solo/Co-op `PlayerReviveSystem`; full `relic_haulers_story.ink` & `3` Endings.
- [ ] **GĐ 5–7 (Alpha Blind Playtest → Beta Juice `<0.1s` & Profiling → Release/Demo)**.

## Installed Workspace Skills (`E:\GAMETEST\.agents\skills\`)
- `project-memory`, `quota-saver-coding`, `narrative-designer`, `unity-mcp-game-development`, `unity-physics-anim`, `unity-stylized-character-animation`, `tools-unity-animation`, `blender-mcp-ai`, `blender-3d-asset-generation`.

## Locked Architectural Decisions & Recent Fixes
- **2026-11-14 (Production Gates Locked)**:
  - Restructured execution into **7 Stages + 2 Hard Gates (`Greybox` → `Vertical Slice` → `Production`)** to enforce *Kill Your Darlings* and avoid building half-finished systems early.
  - Earlier GDD invariants remain locked for Cổng 2 & Production: `VisualRoot` physics separation, `Gadget Slot Cap = 2`, Pet `Zero Self-Collision`, `Sky-Aquarium` Pod-Quota ban, Revive `CollapseTimer <= 0` block, `relic_haulers_story.ink` zero dead-ends.

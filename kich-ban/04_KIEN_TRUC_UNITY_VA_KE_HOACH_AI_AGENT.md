# 04. Kiến Trúc Kỹ Thuật Unity & Lộ Trình Thực Thi Cho AI Agent

## 1. Cấu Trúc Thư Mục Dự Án Unity Đề Xuất

```text
Assets/
└── _Project/
    ├── Art/
    │   ├── Characters/          # Hauler rig (Humanoid), Armor slots (Head/Pack/Gloves)
    │   ├── Enemies/             # PebbleImp, EchoStalker, SpringMimic, GolemWarden
    │   ├── Relics/              # CrystalVase, ReactorCore, WindBell, MagnetCube, VoidCrown
    │   ├── Environment/         # Modular Sky-Shards, Windmills, Gears, Citadel pieces
    │   ├── Materials/           # Palette materials, Toon Lit, OcclusionFade URP ShaderGraph
    │   └── Animations/          # Mixamo/KayKit clips, BlendTrees, AvatarMasks, RigProfiles
    ├── Audio/
    ├── Data/                    # ScriptableObjects (Archetypes, Relics, Upgrades, Biomes) & Narrative (.ink)
    ├── Prefabs/
    │   ├── Player/
    │   ├── Enemies/
    │   ├── Relics/
    │   ├── Pod/
    │   └── VFX/                 # Dust, Spark, Shockwave, Shatter
    ├── Scenes/
    │   ├── 00_Bootstrap.unity
    │   ├── 01_SkyDockHub.unity
    │   ├── 02_AirshipShop.unity
    │   ├── Stages_Ch1/          # Stage_1_1_WindmillOutskirts, Stage_1_2_GaleRidge
    │   ├── Stages_Ch2/          # Stage_2_1_BounceWoods, Stage_2_2_HollowCanopy
    │   ├── Stages_Ch3/          # Stage_3_1_Gearworks, Stage_3_2_SmeltingCore
    │   ├── Stages_Ch4/          # Stage_4_1_SlickIce, Stage_4_2_MirrorCavern
    │   ├── Stages_Ch5/          # Stage_5_1_TarBasin, Stage_5_2_OvergrownLab
    │   └── Stages_Ch6/          # Stage_6_1_TwilightAscent, Stage_6_2_WardensSanctum
    └── Scripts/
        ├── Core/                # GameManager, RunState, QuotaManager, SaveSystem
        ├── Player/              # FloatingCapsuleController, SquashStretch, CarrySystem, RagdollController, PlayerReviveSystem, LastSafeGroundTracker, DownedBeacon
        ├── Animation/           # HaulerAnimatorDriver, ProceduralIKController, EquipmentRetargeter
        ├── Relics/              # RelicItem, ExtractionPod, FragilityHandler
        ├── WorldGen/            # RoomGraphGenerator, RoomModule, DoorwaySocket, RuntimeNavMeshBaker
        ├── Survival/            # OreVein, MiningSystem, FishingSpot, FishingMinigame, CraftingManager, RanchManager, AquariumManager, PetCompanion
        ├── Enemies/             # EnemyBase, NavMeshChase, HoverSteeringAI, EnemyRagdollHit
        ├── Upgrades/            # UpgradeDataSO, UpgradeManager, OrbitingRelicEffect, RiftShrine
        ├── Dialogue/            # DialogueDirector, DialogueBlackboard, DialogueRuleSO, NPCInteractable, InkNarrativeManager
        └── UI/                  # HUDController, UpgradeDraftModal, DialogueChoiceModal, FishingBarUI, WorldItemTooltip
```

---

## 2. Thiết Kế Dữ Liệu Cốt Lõi (ScriptableObjects & Interfaces)

### Interfaces Chuẩn
- `IGrabbable`: `Grab(Transform socket)`, `Release(Vector3 throwVelocity)`, `float Mass`, `Transform LeftHandAnchor`, `Transform RightHandAnchor`.
- `IKnockbackable`: `ApplyKnockback(Vector3 impulse, bool triggerRagdoll)`.
- `IDamageable`: `TakeDamage(float amount, Vector3 hitPoint, Vector3 hitNormal)`.

### ScriptableObjects Chính
1. `CharacterArchetypeSO`: `archetypeName`, `moveSpeed`, `jumpForce`, `dashForce`, `carrySpeedPenaltyMultiplier`, `fragilityResistance`, `startingToolPrefab`.
2. `RelicDataSO`: `relicId`, `displayName`, `baseValue`, `massKg`, `maxDurability`, `noiseRadiusOnImpact`, `breakThresholdVelocity`, `prefab`.
3. `UpgradeDataSO`: `upgradeId`, `title`, `description`, `icon`, `rarity`, `maxStacks` (`0` = vô hạn), `statModifiers[]` hoặc `effectBehaviour`.
4. `BiomeConfigSO` / `StageConfigSO`: `stageId`, `biomeName`, `sceneName`, `collapseDurationSeconds`, `quotaTarget`, `roomCountRange`, `roomPool[]`, `relicSpawnTable[]`, `oreSpawnTable[]`, `fishLootTable[]`, `enemySpawnTable[]`, `availableModifiers[]`.

---

## 3. Lộ Trình 7 Giai Đoạn & 2 Cổng Khóa Cứng Cho AI Agent

> **Quy tắc bất biến cho AI Agent**: Tuyệt đối không nhảy cóc hoặc nhồi tính năng của giai đoạn sau vào giai đoạn trước (*Kill Your Darlings*). Trước mỗi giai đoạn/Sprint, đọc `project-memory` + skill tương ứng trong `.agents/skills/`, kiểm tra zero lỗi Console Unity trước khi qua Cổng.

### Bảng Cổng Khóa Cứng (Hard Gates Matrix)

| Tiêu chí | Cổng Khóa Cứng 1 (GĐ 2: Prototype) | Cổng Khóa Cứng 2 (GĐ 3: Vertical Slice) | Sau Cổng 2 (GĐ 4: Production) |
| :--- | :--- | :--- | :--- |
| **Trọng tâm** | Cảm giác di chuyển, túi đồ & vòng lặp thoát hiểm | Pipeline Art URP, Data-Driven & tích hợp Ink | Nhân bản nội dung hàng loạt |
| **Bản đồ & Môi trường** | `1` Map Greybox (khối hộp xám) + `1` Hub tạm | `1` Biome hoàn chỉnh (`2–3` Archetype phòng) + Quán trọ/Trạm Hub (`01_SkyDockHub`) | Mở rộng đủ `6` Biome / `12` màn & `10` Archetype phòng |
| **Vật phẩm & Kinh tế** | `1` loại Rương + `1` mạch Quặng + Túi đồ lưới (`OrePouch` / Carry) | `3` cấp Rương, `2` mạch Quặng, `1` Cổ vật, `2` Upgrades, `3` món Chế tạo | Mở khóa đủ `14` Cổ vật, `16` Upgrades, `18` món Chế tạo |
| **Cơ chế phụ trợ** | Khóa toàn bộ | Chỉ mở Rèn/Chế tạo cơ bản + Save/Load JSON | Mở khóa Câu cá, Chăn nuôi (`Sky-Ranch`), Bể cá (`Sky-Aquarium`), Hồi sinh Solo/Co-op |
| **Kịch bản (`.ink`)** | Khóa toàn bộ (chỉ dùng text placeholder) | Kết nối `1` NPC chủ chốt + `1` biến cờ Cổ vật | Tích hợp toàn bộ [relic_haulers_story.ink](./ink/relic_haulers_story.ink) & `3` Ending |

---

### Chi Tiết Triển Khai Từng Giai Đoạn (Ánh Xạ Sprint 1–6)

#### Giai Đoạn 1 — Concept & Living GDD *(ĐÃ HOÀN TẤT)*
- Chốt Core Loop, Hook, bảng thông số SO & kịch bản nguồn tại `kich-ban/00..15` + `ink/relic_haulers_story.ink`.

#### Giai Đoạn 2 — Prototyping & Greyboxing (**Mốc Cổng Khóa Cứng 1** — *Sprint 1 lõi*)
- **Skills**: `unity-mcp-game-development`, `unity-physics-anim`.
- **Phạm vi khóa cứng**:
  - `1` Map Greybox khối hộp xám (dốc nghiêng, cầu nhảy, khu Extraction Pod) + `1` Hub tạm chuyển cảnh.
  - `FloatingCapsuleController` (`FixedUpdate`, `Interpolate`, `RideHeight`, Coyote Time, Jump Buffer, Sprint miễn phí, Dash) + `CarrySystem` + `ExtractionPod` + Túi đồ lưới (`OrePouch`) + `1` loại Rương + `1` mạch Quặng test + `1` quái placeholder (`Pebble Imp` đẩy lực).
  - **Khóa toàn bộ**: Art 3D/Shader hoàn chỉnh, Câu cá, Chăn nuôi, Bể cá, Hồi sinh, và `.ink` (chỉ hiện text placeholder).
- **Điều kiện qua Cổng 1**: Chạy/nhảy/dash, nhặt/ném/hủy ném `RMB`, đào mạch quặng vào túi lưới và rút qua Pod thấy **thực sự vui trên khối hộp xám**.

#### Giai Đoạn 3 — Vertical Slice `10–15 phút` (**Mốc Cổng Khóa Cứng 2** — *Sprint 2 + Lát cắt Sprint 3/4/5*)
- **Skills**: `blender-mcp-ai`, `blender-3d-asset-generation`, `unity-stylized-character-animation`, `tools-unity-animation`, `narrative-designer`.
- **Phạm vi khóa cứng (đạt 100% chất lượng thương mại cho 1 lát cắt)**:
  - **Môi trường & Art URP**: `1` Biome hoàn chỉnh (`Biome 1 — Windmill Meadow`, nối `2–3` Archetype phòng bằng `RoomGraphGenerator` + Runtime `NavMeshSurface`) + Quán trọ/Trạm Hub `01_SkyDockHub`, Rig Humanoid + 2D Blend Tree + Foot/Carry IK + `SquashStretchVisual` + `SG_StylizedToonLit` & `SG_OcclusionFade`.
  - **Dữ liệu Data-Driven**: `3` cấp Rương, `2` mạch Quặng, `1` Cổ vật hoàn chỉnh (`rel_crystal_vase`), `2` thẻ Upgrades (`ScriptableObject`), `3` món Chế tạo cơ bản + Save/Load JSON.
  - **Kịch bản Ink**: Cài `inkle/ink-unity-integration`, kết nối `InkNarrativeManager` với **`1` NPC chủ chốt** (*Bác Kael*) và **`1` biến cờ Cổ vật**.
  - **Audio/VFX mẫu**: Phản hồi va chạm, âm thanh bước/nứt vỡ/Pod, HUD gọn.
- **Điều kiện qua Cổng 2**: Chơi trọn vẹn `10–15 phút` từ Hub → đảo Biome 1 → nhặt/rèn/nộp Pod → lưu JSON → quay về Hub nghe NPC Ink phản ứng đúng cờ; đo xong thời gian làm 1 màn/pipeline Blender→Unity.

#### Giai Đoạn 4 — Production: Nhân Bản Nội Dung Theo Module (*Mở rộng Sprint 3–5 đầy đủ*)
- Chỉ mở sau khi qua Cổng 2. Nhân bản hoàn toàn theo Data-Driven (`ScriptableObject` / JSON / `.ink`):
  - **Bản đồ**: Đủ `6` Biome / `12` màn (`08_BAN_DO_VA_MAN_CHOI_MO_RONG.md`) + đủ `10` Archetype phòng (`13`).
  - **Kinh tế & Chiến đấu**: Đủ `14` Cổ vật, `16` Upgrades + `6` Synergies, `4` quái + `Clockwork Crusher` + `Golem Warden` (3 Phase), `18` món Chế tạo từ quặng.
  - **Cơ chế phụ trợ**: Mở khóa Câu cá (`FishingMinigame`, `12` loài), Chăn nuôi (`RanchManager`, `4` loài + Pet `Zero Self-Collision`), Bể cá (`AquariumManager` theo Run-Tick), Hồi sinh (`PlayerReviveSystem` Solo/Co-op, `LastSafeGroundTracker`, `DownedBeacon`), Shop Pip + Bếp Lò, Endless.
  - **Kịch bản Ink**: Tích hợp đầy đủ `relic_haulers_story.ink` (4 NPC, 12 Nhật Ký, Post-Revive reactions, **3 Kết Thúc**).

#### Giai Đoạn 5 — Alpha & Blind Playtest (`Feature Freeze`)
- **Khóa tính năng tuyệt đối**: Game chơi được từ đầu tới 3 Ending; không thêm cơ chế mới.
- Cho `5–10` người chơi lạ chơi thử không giải thích hộ; ghi lại mọi điểm bị kẹt đường Room-Graph, nhầm nút `E`/`RMB`/`Q`, hoặc hiểu sai Quota/Tether để sửa UX.

#### Giai Đoạn 6 — Beta, Game Feel (`Juice < 0.1s`) & Tối Ưu Hóa (*Sprint 6*)
- Đảm bảo phản hồi hình ảnh + âm thanh + chuyển động dưới `0.1s` (Hitstop `0.04s`, Camera Shake, Particle theo bề mặt, SFX theo trọng lượng).
- Tinh chỉnh 15 phút Onboarding đầu game; chạy Profiler kiểm tra RAM, VRAM, Draw Calls, GC Alloc & Object Pooling.

#### Giai Đoạn 7 — Release & Live Ops
- Tung Demo / Playtest cộng đồng trước, theo dõi tỷ lệ rớt/bỏ cuộc theo từng Biome & cân bằng `ScriptableObject` (Quota, HP quái, giá Chế tạo) qua các bản vá nhỏ.

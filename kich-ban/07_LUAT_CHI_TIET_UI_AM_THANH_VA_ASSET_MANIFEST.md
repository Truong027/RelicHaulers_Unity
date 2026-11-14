# 07. Luật Edge Case, Điều Khiển, UI Text, Âm Thanh & Asset Manifest

## 1. Xử Lý Các Tình Huống Ngoại Lệ (Edge Cases) & Lưu Dữ Liệu

1. **Vỡ Quá Nhiều Cổ Vật Khiến Không Đủ Quota**:
   - Đảo luôn spawn tổng giá trị cổ vật `>= 1.8x Quota` (VD: Màn 1-1 Quota `$450` thì đảo có tổng `~$850` cổ vật).
   - Ngoài ra, mỗi `45s` Pod có thể quét ra 1 *Tín Hiệu Tiếp Tế (Bonus Relic Crate)* ở nhánh phụ nếu tổng giá trị đồ còn lại trên đảo xuống dưới `1.1x` phần Quota còn thiếu.
2. **Mắc Kẹt Cổ Vật Dưới Khe Hoặc Rớt Vực**:
   - Mọi Cổ Vật có `Y < -25m` tự động dịch chuyển về *Vị Trí Spawn Gần Nhất Còn Đứng Được*, giảm `25%` HP và phát hiệu ứng sáng nhấp nháy `3s` để người chơi tìm lại.
3. **Chế Độ Solo vs Co-op (Tương Lai / Mở Rộng)**:
   - Thiết kế mặc định chạy mượt **Single-Player (Solo)** trước để AI Agent hoàn thiện core loop nhanh, nhưng tách biệt `PlayerInput` và `PlayerEntity` khỏi `GameManager` để sẵn sàng mở rộng `1–4` người chơi (Netcode/Split) mà không phải đập code cũ.
   - Khi HP về `0` ở chế độ Solo: Hệ thống kiểm tra **`PlayerReviveSystem`** trước (chi tiết tại [File 15](./15_CO_CHE_HOI_SINH_VA_COT_TRUYEN_INK.md)) — nếu còn lượt `Emergency Tether` hoặc `gad_revive_capsule` **VÀ `CollapseTimer > 0s`**, hồi sinh ngay tại `LastSafeGroundPosition` (trừ `15s` / `5s` đồng hồ); nếu hết lượt hồi sinh hoặc đảo đã sụp/bị bỏ lại, mới chuyển sang **Run Summary (Contract Failed)**, giữ lại `50%` Guild Tokens đã tích lũy. Ở Co-op chuyển qua **Downed Beacon (`20s`) → Hauler Dog-Tag (`3kg`)**.
4. **Save System (JSON tại `Application.persistentDataPath/save_data.json`)**:
   ```json
   {
     "guildTokens": 240,
     "unlockedArchetypes": ["Scout", "Brawler", "Tinker"],
     "unlockedEquipmentIds": ["head_miner", "pack_rope", "gloves_leather", "tool_hook"],
     "equippedLoadout": {
       "head": "head_miner",
       "backpack": "pack_rope",
       "gloves": "gloves_leather",
       "tool": "tool_hook"
     },
     "bestRunScore": 4250,
     "highestChapterCleared": 6,
     "oreInventory": { "ore_sky_copper": 4, "ore_ancient_iron": 2 },
     "craftedMaterials": { "mat_wind_bronze": 1, "byprod_cloud_egg": 2 },
     "equippedGadgets": ["gad_repair_spray", "gad_cloud_net"],
     "ranchAnimals": [{ "id": "pet_tamed_imp", "fed": true, "readyToHarvest": false }],
     "aquariumFish": ["fish_cloud_wing", "fish_lantern_shroom"],
     "activePetId": "pet_tamed_imp",
     "fishCatalog": ["fish_cloud_wing", "fish_lantern_shroom"],
     "toolLevels": { "pickaxe": 2, "fishingRod": 1, "orePouch": 1 },
     "totalRevivesUsed": 0,
     "unlockedEndings": [],
     "inkStoryStateJson": ""
   }
   ```

---

## 2. Bảng Phím Điều Khiển (Unity Input System — Đã Tinh Chỉnh Không Xung Đột)

| Hành Động | Bàn Phím & Chuột (PC) | Tay Cầm (Gamepad) | Ghi Chú Chống Lỗi Control |
| :--- | :--- | :--- | :--- |
| Di chuyển | `W A S D` | `Left Stick` | — |
| Xoay Camera | `Mouse Delta` | `Right Stick` | Có thanh chỉnh độ nhạy trong Pause |
| Nhảy / Bung Dù Lướt | `Space` | `Button South (A / Cross)` | Hỗ trợ Coyote Time & Jump Buffer |
| Chạy Nhanh (Sprint) | Giữ `Left Shift` | `L3 (Left Stick Press)` | **Không tốn Stamina** |
| Dash / Né Đòn | Nhấp `Left Shift` (hoặc `V` / `Mouse4`) | `Button East (B / Circle)` | Tránh dùng `Ctrl`/`Alt` gây dính phím OS |
| Nhặt / Đặt Nhẹ / Tương Tác | `E` (Tap) | `Button West (X / Square)` | **Ưu tiên Interactable** (không làm rơi đồ khi bấm vào Pod/NPC/Shrine); khóa nhặt vật đang đứng lên |
| Đánh Đẩy / Skill Cổ Vật / Húc Vai | `Left Mouse` | `Right Bumper (RB)` | Tay trống = Đánh; Đang cầm vật có Skill = Kích hoạt Skill; Vật thường = Húc vai |
| Gồng & Ném Parabol | Giữ `Right Mouse` → Thả | Giữ `Right Trigger (RT)` → Thả | — |
| **Hủy Ném An Toàn** | Bấm `Left Mouse` / `E` lúc đang giữ `RMB` | Bấm `LT` / `B` lúc đang giữ `RT` | Thu tay lại, giữ nguyên cổ vật không ném |
| Dùng Gadget Chiến Thuật (`Slot 1 / Slot 2`) | `4` / `5` (hoặc `Q` xoay chọn) | `Button North (Y / Triangle)` | Tối đa `2` Slot Gadget mỗi Run; bấm `Q` để dùng Keo Hàn / Gadget đang chọn |
| Dùng Tiện Ích Địa Hình (`Utility_Slot`) | `C` | `D-Pad Down` | Đặt Đệm Nấm / Cọc Neo / Phao |
| Đổi nhanh Tool (Chiến đấu / Cuốc / Cần Câu) | `1` / `2` / `3` (hoặc tự động theo ngữ cảnh) | `D-Pad Left` (xoay vòng) | Smart Context tự chuyển khi nhìn Quặng / Điểm Câu |
| Bật / Tắt Đèn Mũ Thợ Mỏ | `F` | `D-Pad Right` | Tự động bật trong hang tối, bấm tay bất kỳ lúc nào |
| Ping Đánh Dấu Mục Tiêu (`8s`) | `G` (hoặc `Middle Mouse`) | `Left Bumper (LB)` | Đánh dấu Cổ Vật / Quái / Lối tắt |
| Hô Khẩu Hiệu / Chào NPC (`Salute`) | `T` | `D-Pad Up` | Hồi `15` Stamina + kích hoạt NPC đối đáp |
| Tạm Dừng (Pause) | `Esc` | `Start / Menu` | — |

---

## 3. Kịch Bản Lời Thoại Radio (Dispatcher VEX) & UI Copy

Tất cả hiển thị dưới dạng **Toast Radio nhỏ góc trên phải (`3.5s`)** để không che khuất tầm nhìn khi nhảy:
- **Bắt đầu Màn 1-1**: *"VEX: Đảo Gió sắp mất lực nâng. Thu hồi đủ `$450` cổ vật về Pod trước khi lõi tắt nhé!"*
- **Khi nhặt đồ dễ vỡ lần đầu**: *"VEX: Cẩn thận! Bình Pha Lê đó giòn lắm — đừng có ném bừa như cục đá!"*
- **Ném Trick Shot vào Pod**: *"VEX: Quá đỉnh! Cú ném bật nảy đẹp mắt — thưởng nóng +25% tiền thưởng!"*
- **Đạt 50% Quota**: *"VEX: Đã thu một nửa chỉ tiêu! Chọn lấy một bản nâng cấp từ Pod đi."*
- **Đạt 100% Quota**: *"VEX: Đủ Quota rồi! Bấm nút Xanh Launch Pod để rút quân ngay, hoặc bấm nút Đỏ Overcharge để đánh cược 2.5x!"*
- **Còn 60 giây sụp đảo**: *"CẢNH BÁO: Từ trường đảo đang tan rã! Quái vật đang mất kiểm soát — mau quay về Pod!"*
- **Khi gặp Golem Warden (Màn 6-2)**: *"VEX: Phát hiện Golem Bảo Hộ! Đánh vào lõi phát sáng sau lưng nó để làm nó quỳ gối!"*
- **Hoàn thành Run (Victory)**: *"VEX: Tuyệt vời! Toàn bộ di vật đã an toàn trên tàu. Nghỉ ngơi ở Sky-Dock và chọn đồ mới thôi!"*

---

## 4. Master Asset Manifest (Danh Sách Tài Nguyên Cần Tạo / Import)

AI Agent dùng bảng này làm **Checklist sản xuất tài nguyên** kết hợp [10_CAU_TRUC_HINH_ANH_VA_ART_BIBLE.md](./10_CAU_TRUC_HINH_ANH_VA_ART_BIBLE.md) bằng `blender-mcp-ai`, `blender-3d-asset-generation`, KayKit/Quaternius CC0 và `Roboticela/Animator`:

### A. Mô Hình 3D (`.fbx` / `.blend`)
- **Nhân vật, NPC & Trang bị** (Rig Humanoid chuẩn):
  - `CHAR_Hauler_Base.fbx`, `CHAR_Hauler_VEXSkin.fbx` (Skin bí mật mở từ 12 Nhật Ký).
  - NPCs: `NPC_Kael_Smith.fbx` (chân cơ khí), `NPC_Pip_Merchant.fbx` (mũ phi công lớn), `NPC_Myla_Curator.fbx`.
  - `EQ_Head_Miner.fbx`, `EQ_Head_Pilot.fbx`, `EQ_Head_Wizard.fbx`, `EQ_Head_Golem.fbx`, `EQ_Head_GoldCrown.fbx`.
  - `EQ_Pack_Rope.fbx`, `EQ_Pack_Jet.fbx`, `EQ_Pack_Crystal.fbx`.
  - `EQ_Gloves_Leather.fbx`, `EQ_Gloves_Rubber.fbx`, `EQ_Gloves_MagGold.fbx`.
  - `EQ_Tool_Hook.fbx`, `EQ_Tool_Hammer.fbx`, `EQ_Tool_Stabilizer.fbx`.
  - Tiện ích địa hình, Gadget & Sinh tồn: `UTIL_PocketBouncePad.fbx`, `UTIL_AnchorSpike.fbx`, `UTIL_LiftBalloon.fbx`, `TOOL_Pickaxe.fbx`, `TOOL_FishingRod.fbx`, `GAD_ImpulseGrenade.fbx`, `GAD_ClockworkDecoy.fbx`, `GAD_CloudCatchNet.fbx`, `GAD_FoamWrap.fbx`, `GAD_ReviveCapsule.fbx`, `ITEM_DogTag.fbx`.
- **Quái Vật, Thú Nuôi, Sự Kiện & Boss**:
  - `EN_PebbleImp.fbx`, `EN_GoldenImp.fbx`, `EN_EchoStalker.fbx`, `EN_SpringMimic.fbx`, `EN_GustWisp.fbx`.
  - `MINI_ClockworkCrusher.fbx`, `BOSS_GolemWarden.fbx`.
  - Thú nuôi trang trại & Pet: `PET_CloudPuffHen.fbx`, `PET_TamedImp.fbx`, `PET_CrystalLamb.fbx`, `PET_GearBeetle.fbx`.
- **Cổ Vật (14 món) + Quặng & Cá**:
  - `REL_ClayJar`, `REL_BrassGear`, `REL_StoneIdol`, `REL_CrystalVase`, `REL_WindBell`, `REL_MagnetCube`, `REL_SpringOrb`, `REL_ReactorCore`, `REL_FrostPrism`, `REL_HornEcho`, `REL_GravityAnchor`, `REL_ChronoHourglass`, `REL_MimicCore`, `REL_VoidCrown` + `REL_ScrapShard`.
  - Quặng & Cá: `ORE_Vein_Common/Rare/Giant.fbx`, `ORE_Chunk_Small.fbx`, `FISH_Set_12Species.fbx`, `PROP_CookingGalley.fbx`, `PROP_ForgeAnvil.fbx`, `PROP_RanchPen.fbx`, `PROP_AquariumPond.fbx`.
- **Môi Trường, Phòng Nối Phòng, Hub & Cơ Khí (Đủ 6 Chương)**:
  - Hub & Sự kiện: `PROP_ExtractionPod.fbx`, `PROP_UpgradeChest.fbx`, `PROP_RiftShrine.fbx`, `PROP_BrokenScoutBot.fbx`, `PROP_RelicMuseumPedestal.fbx`, `PROP_SkyShardLog.fbx`, `PROP_RelicMeteorCapsule.fbx`, `PROP_SkyPeddlerShip.fbx`, `ENV_SkyDock_Hub.fbx`, `ENV_AirshipShop.fbx`.
  - Bộ Phòng Modular (10 Archetypes x Biến thể Biome): `RM_StartPod`, `RM_CorridorBridge`, `RM_TJunction_Split`, `RM_Crossroad_Plaza`, `RM_Vertical_Shaft`, `RM_Cavern_Mining`, `RM_Cliff_FishingPier`, `RM_Hazard_Gauntlet`, `RM_Vault_Secret`, `RM_Boss_Arena` + các module bẫy (`ENV_Windmill`, `ENV_SwingingBridge`, `ENV_BounceMushroom`, `ENV_GiantGear`, `ENV_ConveyorBelt`, `ENV_Piston`, `ENV_JumpPad`, `ENV_LavaPit`, `ENV_IceFloe`, `ENV_ThinIce`, `ENV_TarPool`, `ENV_AirBubble`, `ENV_GravityPortal`, `ENV_GravityZone`, `ENV_CrumblingTile`, `ENV_LightBridge`, `ENV_CrystalPillar`).

### B. Animation Clips (Mixamo / KayKit / Roboticela Animator)
- **Hauler**: `Idle`, `Walk`, `Run`, `JumpStart`, `FallLoop`, `Land`, `Dash`, `CarryIdle`, `CarryWalk`, `ShoulderBash`, `ThrowWindup`, `ThrowRelease`, `MeleeSwing`, `MineSwing`, `FishCast`, `FishReelLoop`, `FishCatchCelebrate`, `GetUp_FaceUp`, `GetUp_FaceDown`, `GlideLoop`.
- **Quái & Boss**: `Imp_Run`, `Imp_Charge`, `Stalker_Sneak`, `Stalker_Pounce`, `Mimic_IdlePop`, `Crusher_Spin`, `Golem_Walk`, `Golem_Slam`, `Golem_Charge`, `Golem_StaggerLoop`.

### C. Shader, Textures, UI Icons, VFX & SFX
- **Textures & UI Icons**: `PAL_Master_Stylized.png`, `TEX_CrystalCrack_Mask.png`, `TEX_Noise_Dissolve.png`, `UI_Icons` (16 Upgrades, 6 Synergies, 3 Archetypes, 5 Run Awards, 6 Shop Items, 6 Ores, 12 Fish, 6 Meals).
- **Shaders (URP Shader Graph)**:
  - `SG_StylizedToonLit`, `SG_OcclusionFade`, `SG_CrystalCrack`, `SG_IceSlip`, `SG_ForceFieldAndPortal`.
- **VFX (Particle System)**: `VFX_FootDust`, `VFX_LandingRing`, `VFX_DashTrail`, `VFX_RelicShatter`, `VFX_ShockwaveRing`, `VFX_PodBeam`, `VFX_ReviveBeam`, `VFX_NoisePulseRing`, `VFX_LightningStrike`, `VFX_MeteorStomp`, `VFX_OreSparkCrit`, `VFX_WaterCloudSplash`.
- **SFX (Audio Clips)**: `SFX_Footstep_Grass/Stone/Metal`, `SFX_Jump`, `SFX_Land_Soft/Heavy`, `SFX_Dash`, `SFX_Grab`, `SFX_Throw`, `SFX_TrickShot`, `SFX_CrystalCrack`, `SFX_CrystalBreak`, `SFX_PodDeposit`, `SFX_PodQuotaReady`, `SFX_ReviveTether`, `SFX_PickaxeHit`, `SFX_PickaxeCrit`, `SFX_FishingCast`, `SFX_FishingSplash`, `SFX_FishingReel`, `SFX_CookSizzle`, `SFX_ImpChirp`, `SFX_GolemSlam`, `SFX_UpgradePick`, `SFX_SynergyUnlock`.
- **BGM (7 Track nhẹ)**: `BGM_SkyDockHub`, `BGM_Biome1_Windmill`, `BGM_Biome2_Canopy`, `BGM_Biome3_Foundry`, `BGM_Biome4_Glacier`, `BGM_Biome5_Swamp`, `BGM_Biome6_BossCollapse`.

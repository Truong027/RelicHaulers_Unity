# Kịch Bản Game 3D: RELIC HAULERS (Biệt Đội Vớt Cổ Vật)

> **Tài liệu dành cho AI Agent & Developer**
> Phiên bản: `1.0` | Engine mục tiêu: **Unity 6 / Unity 2022.3 LTS (URP)** | Art Style: **Stylized Low-Poly 3D**

---

## 1. Ý Tưởng Cốt Lõi (High Concept)

**RELIC HAULERS** là game hành động – sinh tồn – thu thập **3D Stylized Physics Extraction + Roguelite** (chơi đơn 1 người hoặc Co-op 1–4 người), kết hợp xu hướng game 3D indie nổi bật giai đoạn 2025–2026:
- **Vật lý hài hước & Nhặt/Vác Cổ Vật dễ vỡ** (cảm hứng từ *R.E.P.O.* & *PEAK*): Đồ vật có khối lượng, độ bền và tương tác vật lý thật; quái vật cũng chịu lực đẩy, va chạm và ragdoll.
- **Di chuyển nảy mượt & Build sức mạnh cộng dồn** (cảm hứng từ *Risk of Rain 2* & *Megabonk*): Nhân vật co giãn hoạt họa (*Squash & Stretch*), leo trèo/nhảy vượt địa hình tàn tích nổi và tích lũy Perk tạo combo điên rồ khi đưa cổ vật về Pod.

### Câu Tóm Tắt (Elevator Pitch)
> Vào vai những **Hauler** nhỏ bé mang ba lô phản lực mini, bạn phải thâm nhập **6 Quần Đảo Di Tích Nổi (12 Màn Chơi + Chế Độ Vô Tận)** đang sụp đổ, giữ thăng bằng để vác những **Cổ Vật Cổ Đại** nặng và dễ vỡ xuyên qua địa hình dốc, bẫy cơ học và đàn quái vật bảo hộ, ném chúng vào **Khoang Thu Hồi (Extraction Pod)** để đổi lấy nâng cấp sức mạnh trước khi hòn đảo rơi vào hư không.

---

## 2. Thứ Tự Đọc Cho AI Agent

Khi AI Agent bắt đầu thiết kế asset, dựng scene hoặc viết code C# cho dự án này, hãy đọc tuần tự toàn bộ 15 file kịch bản & file nguồn `.ink` trong thư mục `kich-ban/`:

1. **[01_THE_GIOI_NHAN_VAT_VA_ART_STYLE.md](./01_THE_GIOI_NHAN_VAT_VA_ART_STYLE.md)** — Bối cảnh, 6 Biome, Archetype nhân vật, tổng quan Quái/Cổ vật & quy chuẩn Export Blender → Unity URP.
2. **[02_CORE_GAMEPLAY_VA_PHYSICS_ANIMATION.md](./02_CORE_GAMEPLAY_VA_PHYSICS_ANIMATION.md)** — Cơ chế điều khiển Floating Capsule, Squash & Stretch, IK tay cầm đồ & chân bám dốc, Ragdoll, cơ chế Nhặt/Ném & Upgrade.
3. **[03_HE_THONG_MAN_CHOI_VA_TIEN_TRINH.md](./03_HE_THONG_MAN_CHOI_VA_TIEN_TRINH.md)** — Kịch bản cốt truyện 3 Hồi, nhiệm vụ phụ tự chọn, cấu trúc vòng lặp 1 Run, rẽ nhánh màn chơi, Quota, Shop trạm trung chuyển, UI/UX & Âm thanh.
4. **[04_KIEN_TRUC_UNITY_VA_KE_HOACH_AI_AGENT.md](./04_KIEN_TRUC_UNITY_VA_KE_HOACH_AI_AGENT.md)** — Cấu trúc thư mục Unity, kiến trúc Script/ScriptableObject, Interfaces và lộ trình triển khai 6 Sprint.
5. **[05_DANH_MUC_RELIC_UPGRADE_VA_SHOP.md](./05_DANH_MUC_RELIC_UPGRADE_VA_SHOP.md)** — Bảng thông số đầy đủ của 14 Relics, 16 Roguelite Upgrades (kèm công thức cộng dồn), 6 món Airship Shop & 13 Trang bị Bone Retargeting.
6. **[06_CHI_TIET_QUAI_VAT_BOSS_VA_LEVEL_DESIGN.md](./06_CHI_TIET_QUAI_VAT_BOSS_VA_LEVEL_DESIGN.md)** — Thông số nhân vật, State Machine chi tiết của 4 quái + Mini-Boss + Boss Golem Warden 3 Phase, các bẫy môi trường và Wave Director.
7. **[07_LUAT_CHI_TIET_UI_AM_THANH_VA_ASSET_MANIFEST.md](./07_LUAT_CHI_TIET_UI_AM_THANH_VA_ASSET_MANIFEST.md)** — Xử lý Edge Cases, Save JSON Schema, bảng phím điều khiển PC/Gamepad, toàn bộ lời thoại Radio VEX và Master Asset Manifest.
8. **[08_BAN_DO_VA_MAN_CHOI_MO_RONG.md](./08_BAN_DO_VA_MAN_CHOI_MO_RONG.md)** — Thiết kế chi tiết toàn bộ **6 Biomes (12 Màn Chơi)**, 2 map Hub/Shop, lộ trình rẽ nhánh Standard Run và chế độ **Endless Sky-Abyss**.
9. **[09_CO_CHE_GIU_CHAN_VA_THU_HUT_NGUOI_CHOI.md](./09_CO_CHE_GIU_CHAN_VA_THU_HUT_NGUOI_CHOI.md)** — 6 Synergy Combo tiến hóa, nút đánh cược *Overcharge Pod*, 4 Sự kiện ngẫu nhiên (*Golden Imp*, *Sky Peddler*...), Bảo Tàng 3D, Guild Tech Tree, 12 Nhật ký bí mật & Snapshot/Danh hiệu hài hước cuối trận.
10. **[10_CAU_TRUC_HINH_ANH_VA_ART_BIBLE.md](./10_CAU_TRUC_HINH_ANH_VA_ART_BIBLE.md)** — Cấu trúc hình ảnh chuẩn Art Bible: Mã màu tương tác gameplay, bảng Hex & URP Lighting cho 6 Biomes, tỷ lệ nhân vật/Camera, UI Wireframe, cấu trúc Texture/Shader và Visual QA Gate.
11. **[11_HE_THONG_HOI_THOAI_VA_DAN_NPC.md](./11_HE_THONG_HOI_THOAI_VA_DAN_NPC.md)** — Kiến trúc hội thoại động (`DialogueBlackboard`, Priority Queue, Variant Pool), 4 NPC có cá tính (*VEX*, *Bác Kael*, *Pip*, *Giáo Sư Myla*), hội thoại rẽ nhánh mặc cả & nút hô khẩu hiệu (`T`).
12. **[12_BO_SUNG_CO_CHE_HAY_TU_CAC_GAME_NOI_TIENG.md](./12_BO_SUNG_CO_CHE_HAY_TU_CAC_GAME_NOI_TIENG.md)** — Cơ chế tinh hoa từ *Deep Rock Galactic*, *Outer Wilds*, *Risk of Rain 2*, *Hades*, *Lethal Company* (Tiện ích địa hình, Lối tắt tri thức, Đài Nguyện Rift Shrine, Thưởng lần đầu chết, Radar nhiễu sóng).
13. **[13_HE_THONG_SINH_TON_CAU_CA_DAO_QUANG_VA_MAP_PHONG_NOI_PHONG.md](./13_HE_THONG_SINH_TON_CAU_CA_DAO_QUANG_VA_MAP_PHONG_NOI_PHONG.md)** — Kiến trúc **Map Phòng Nối Phòng Đa Dạng Cấu Trúc** (`RoomGraphGenerator`, 10 Archetype phòng, Dead-End Cap, Runtime NavMesh) + hệ thống sinh tồn **Đào Quặng (`WeakSpot`)**, **Câu Cá Mây/Biome**, **Rèn Công Cụ** & **Nấu Ăn Buff**.
14. **[14_CHE_TAO_QUANG_CHAN_NUOI_VA_NUOI_CA.md](./14_CHE_TAO_QUANG_CHAN_NUOI_VA_NUOI_CA.md)** — Hệ thống **18 Sản Phẩm Chế Tạo Từ Quặng** (Hợp kim, 6+1 Gadget chiến thuật giới hạn 2 slot, 5 Trang bị nền, 3 Công trình Hub), **Chăn Nuôi 4 Loài Sinh Vật Đảo Nổi + Pet Đồng Hành** và **Bể Nuôi Cá Mây** theo chu kỳ *Run Tick* (chống AFK & chống phá kinh tế).
15. **[15_CO_CHE_HOI_SINH_VA_COT_TRUYEN_INK.md](./15_CO_CHE_HOI_SINH_VA_COT_TRUYEN_INK.md)** — Hệ thống **Hồi Sinh Nhân Vật (`Solo Emergency Tether`, `gad_revive_capsule`, Co-op `Downed & Dog-Tag`)** kèm 6 luật chống lỗi logic & kiến trúc **Kịch bản phân nhánh 3 Hồi – 3 Kết Thúc chuẩn `inkle/ink`** ([ink/relic_haulers_story.ink](./ink/relic_haulers_story.ink)).

---

## 3. Bản Đồ Ánh Xạ Tới Agent Skills Đã Cài Đặt

AI Agent phải kích hoạt đúng skill tương ứng trong `.agents/skills/` khi thực hiện từng phần của kịch bản:

| Hạng Mục Trong Game | Skill Cần Kích Hoạt | Tham Chiếu Quan Trọng |
| :--- | :--- | :--- |
| Dựng model stylized, vật liệu toon/procedural, auto-rig trong Blender | `blender-mcp-ai` | Style presets, 67 procedural materials, export Unity URP zeroed transform |
| Kiểm tra topology, UV, scale, xuất FBX/GLB chuẩn game | `blender-3d-asset-generation` | `scripts/audit_blender_asset.py`, `references/game-ready-assets.md` |
| Kiểm tra xương, gắn/test nhanh animation Mixamo/Humanoid | `unity-stylized-character-animation` | `Roboticela/Animator`, KayKit / Quaternius CC0 |
| Character Controller co giãn, bụi, shader xuyên tường | `unity-stylized-character-animation` | `joebinns/stylised-character-controller` |
| Kết hợp Keyframe + Procedural + Animation Rigging IK + Ragdoll | `unity-stylized-character-animation` & `unity-physics-anim` | `fierc3/UnityCharacterAnimations` |
| Đổi mũ/giáp/ba lô (Bone Retargeting), Blend Tree, Inventory | `unity-stylized-character-animation` | `murenne/3DLowPolyRPG_` |
| Tối ưu Animator State Machine, Animancer/Playables, Rigidbody | `tools-unity-animation` & `unity-physics-anim` | `FixedUpdate` interpolation, Rig Layers, memory budget |
| Dựng Scene, Prefab, kiểm tra Console & PlayMode qua Unity MCP | `unity-mcp-game-development` | Observe → Reversible Slice → Compile/Console Check |
| Mở rộng cốt truyện 3 Hồi, đa kết thúc & thoại nhánh chuẩn `inkle/ink` | `narrative-designer` | `world-bible.md`, `character-voices.md`, `template.ink` (zero dead-ends) |
| Tránh đọc lại toàn bộ GDD giữa các phiên code Unity | `project-memory` | `memory-snapshot.md` & `sprint-tracker.md` (< 4 KB tổng) |
| Giảm tiêu hao Quota / Token khi code C# & điều khiển Unity | `quota-saver-coding` | Slice read, batched edits, loại trừ `Library/` `.meta` YAML |

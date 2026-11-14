# 01. Thế Giới, Nhân Vật, Quái Vật & Quy Chuẩn Art Style

## 1. Bối Cảnh & Cốt Truyện Ngắn

Hàng trăm năm sau sự kiện **Đại Phân Tách (The Great Rift)**, lục địa cũ vỡ thành hàng ngàn **Quần Đảo Nổi (Sky-Shards)** trôi lơ lửng trên biển mây. Các nền văn minh cổ đại để lại vô số **Cổ Vật Năng Lượng** nằm rải rác trên những hòn đảo đang dần mất lực nâng và sắp rơi xuống Vực Thẳm.

Người chơi là thành viên của **Hội Vớt Cổ Vật (The Hauler Guild)** — những nhà thám hiểm mặc giáp nhẹ gắn động cơ đẩy mini, nhận hợp đồng từ **Trạm Chỉ Huy Khí Cầu (Sky-Dock)** để nhảy xuống các hòn đảo sắp sập, thu hồi đủ hạn mức cổ vật trước khi hòn đảo sụp đổ hoàn toàn.

### Cấu Trúc 6 Biome Chính (12 Màn Chơi)
*(Chi tiết từng màn xem tại [08_BAN_DO_VA_MAN_CHOI_MO_RONG.md](./08_BAN_DO_VA_MAN_CHOI_MO_RONG.md))*

1. **Biome 1 — Quần Đảo Gió Lộng (Windmill Meadow — Màn 1-1 & 1-2)**
   - **Tone màu**: Xanh ngọc – Vàng nắng – Đá xám ấm.
   - **Đặc trưng**: Đảo cỏ bậc thang, cầu gỗ lắc lư, cối xay gió & luồng gió thổi đứng đẩy nhân vật.
2. **Biome 2 — Rừng Nấm Phát Quang (Bioluminescent Canopy — Màn 2-1 & 2-2)**
   - **Tone màu**: Lam ngọc – Tím dạ quang – Gỗ rêu.
   - **Đặc trưng**: Mũ nấm đàn hồi như bạt nhún vật lý, thân cây cổ thụ nhiều tầng thẳng đứng, tán lá rậm kích hoạt shader `OcclusionFade`.
3. **Biome 3 — Xưởng Đúc Cổ Đại (Clockwork Foundry — Màn 3-1 & 3-2)**
   - **Tone màu**: Cam đồng – Tím than – Xanh lân tinh.
   - **Đặc trưng**: Bánh răng khổng lồ xoay liên tục, băng chuyền, cầu nâng lò xo, hố dung nham năng lượng và Mini-Boss *Clockwork Crusher*.
4. **Biome 4 — Vịnh Băng Pha Lê (Frostglass Glacier — Màn 4-1 & 4-2)**
   - **Tone màu**: Xanh băng – Trắng tuyết – Cực quang tím.
   - **Đặc trưng**: Sàn băng trơn ma sát thấp (trượt người & đẩy cổ vật theo quán tính), gió tuyết theo chu kỳ và sàn băng mỏng nứt theo tải trọng.
5. **Biome 5 — Đầm Lầy Trọng Lực (Abyssal Rift Swamp — Màn 5-1 & 5-2)**
   - **Tone màu**: Tím sẫm – Xanh axit – Đá nổi dị thường.
   - **Đặc trưng**: Vũng bùn hút chậm, bong bóng khí nâng, cổng dịch chuyển 2 chiều và vùng trọng lực thấp/cao xen kẽ.
6. **Biome 6 — Thánh Điện Mây Vỡ (Shattered Spire Citadel — Màn 6-1 & 6-2)**
   - **Tone màu**: Tím hoàng hôn – Vàng kim – Pha lê phát sáng.
   - **Đặc trưng**: Đảo vỡ thẳng đứng, cầu ánh sáng chu kỳ, sàn nứt sau 1.8 giây và đấu trường **Boss Golem Warden (3 Phase)**.

---

## 2. Thiết Kế Nhân Vật & Hệ Thống Trang Bị (Bone Retargeting)

### Nhân Vật Chính: The Hauler
- **Tỷ lệ stylized**: Chiều cao `1.6m` (1.6 Unity units), đầu hơi to stylized (tỷ lệ ~1:4.5), tay chân rõ khớp để Animation Rigging IK và Squash & Stretch trông sinh động.
- **Chuẩn xương**: **Unity Humanoid / Mixamo Compatible** (tương thích trực tiếp với KayKit / Quaternius CC0 và Mixamo animations).
- **4 Lớp Trang Bị Thay Thế Được (Modular Armor / Bone Retargeting theo mẫu `murenne/3DLowPolyRPG_`, chi tiết ở `05_DANH_MUC_RELIC_UPGRADE_VA_SHOP.md`)**:
  1. `Head_Slot`: Mũ thợ mỏ, Nón phi công mây, Mũ phù thủy pha lê, Mũ giáp Golem, Vương miện vàng.
  2. `Backpack_Slot`: Ba lô dây thừng, Bình hơi phản lực, Lồng tinh thể sáng.
  3. `Gloves_Slot`: Găng da Hauler, Găng cao su nảy, Găng từ trường vàng.
  4. `Tool_Slot` (tay phải/hai tay): Gậy móc thu hồi / Búa xung kích / Máy ổn định từ.

### 3 Archetype Khởi Đầu (Chọn ở Sky-Dock)
| Archetype | Điểm Mạnh | Điểm Yếu | Vũ Khí / Công Cụ Riêng |
| :--- | :--- | :--- | :--- |
| **Scout (Trinh Sát)** | Chạy nhanh, nhảy cao, Dash hồi nhanh | Sức vác thấp (giảm tốc mạnh khi vác đồ nặng) | Dây móc kéo đồ từ xa |
| **Brawler (Cơ Bắp)** | Vác đồ nặng không giảm tốc, đẩy quái ragdoll xa | Nhảy thấp hơn, hitbox lớn hơn | Búa xung kích đẩy văng diện rộng |
| **Tinker (Kỹ Sư)** | Giảm 50% sát thương vỡ cổ vật, Pod hút đồ rộng hơn | Máu thấp | Tia ổn định từ trường (giữ cổ vật không lắc) |

---

## 3. Danh Mục Cổ Vật (Relics) & Tương Tác Vật Lý

Mỗi cổ vật là 1 Prefab có `Rigidbody`, `Collider` và component `RelicItem`:

| Tên Cổ Vật | Trọng Lượng | Độ Bền | Hành Vi Vật Lý Đặc Biệt |
| :--- | :--- | :--- | :--- |
| **Bình Pha Lê Cổ** | Nhẹ (`2 kg`) | Rất dễ vỡ (`100 HP`) | Rơi từ độ cao > 1.5m bị nứt, giảm giá trị 30% hoặc vỡ tan nếu hết HP. |
| **Lõi Lò Phản Ứng** | Nặng (`18 kg`) | Bền (`300 HP`) | Làm nhân vật đi chậm 35%; phát ánh sáng và rung nhẹ hút quái ở gần. |
| **Chuông Gió Rỗng** | Trung bình (`8 kg`) | Trung bình (`160 HP`) | Khi va chạm hoặc chạy nhanh phát sóng âm (*Noise Pulse*), thu hút quái tuần tra. |
| **Khối Đá Nam Châm** | Nặng (`15 kg`) | Rất bền (`400 HP`) | Hút nhẹ các mảnh xu / vật kim loại nhỏ xung quanh khi đang được vác. |
| **Vương Miện Hư Không (Siêu Cổ Vật)** | Nặng (`20 kg`) | Bền (`500 HP`) | Chỉ rơi ra hoặc mở khóa khi hạ / vượt qua Golem Warden ở cuối Biome; nhân đôi tiền thưởng Run. |

---

## 4. Danh Mục Quái Vật (Tất Cả Đều Hỗ Trợ Ragdoll & Knockback)

1. **Pebble Imp (Tiểu Yêu Đá)** — Quái nhỏ, chạy đàn, thích lao vào húc để làm rơi cổ vật trên tay người chơi. Khi bị đánh vào trạng thái **Ragdoll**, người chơi có thể nhặt nó lên ném vào quái khác!
2. **Echo Stalker (Kẻ Rình Rập)** — Bị thu hút bởi tiếng ồn của *Chuông* hoặc tiếng bước chân chạy nước rút; lao vồ chính xác nếu không né.
3. **Spring Mimic (Rương Bật Lò Xo)** — Giả dạng rương cổ vật; khi người chơi đến gần nhặt sẽ bật ra cắn và hất tung người chơi lên không trung.
4. **Gust Wisp (Tinh Linh Gió)** — Quái bay lơ lửng không phụ thuộc mặt đất, bắn cầu gió đẩy lệch hướng khi nhảy qua vực.
5. **Golem Warden (Boss Chung Kết — Màn 6-2)** — To lớn (`3.2m`), bước đi nặng nề rung camera, đánh choáng diện rộng; điểm yếu là lõi phát sáng sau lưng — phải dùng đòn đánh hoặc ném vật nặng vào lõi để làm Golem quỳ gối (*Stagger*) rồi mới mở khóa **Vương Miện Hư Không**.

---

## 5. Quy Chuẩn Kỹ Thuật Asset (Blender → Unity URP)

Tuân thủ chặt chẽ quy trình trong skills `blender-mcp-ai` và `blender-3d-asset-generation`:
- **Đơn vị & Trục**: Blender dùng `METRIC` (`scale_length = 1.0`). Mặt trước nhân vật/prop hướng `-Y` trong Blender (tương ứng `+Z` trong Unity).
- **Transform khi Export**:
  - Prop tĩnh / môi trường: Scale `(1,1,1)`, Rotation `(0,0,0)`, Location `(0,0,0)`, `unity_static_preset=True`, Origin đặt dưới đáy (`Z=0`) cho vật đứng sàn, hoặc tâm hình học cho vật lơ lửng.
  - Nhân vật có xương: `unity_static_preset=False`, `primary_bone_axis="Y"`, `secondary_bone_axis="X"`, `add_leaf_bones=False`.
- **Ngân sách Tam Giác (Triangle Budget)**:
  - Nhân vật chính + phụ kiện: `3,000 – 7,500 tris`.
  - Quái thường: `1,500 – 4,000 tris`.
  - Cổ vật / Prop: `300 – 1,500 tris`.
  - Boss: `8,000 – 14,000 tris`.
- **Material / Shader (URP)** *(Bảng màu Hex, Lighting, Silhouette & UI Layout xem chi tiết tại [10_CAU_TRUC_HINH_ANH_VA_ART_BIBLE.md](./10_CAU_TRUC_HINH_ANH_VA_ART_BIBLE.md))*:
  - Dùng bảng màu Palette Texture (`PAL_Master_Stylized.png`, 256x256) + Stylized Toon/Lit URP shader để giảm Draw Call.
  - Bắt buộc có **Occlusion Fade Shader** (tham khảo `joebinns/stylised-character-controller`) cho tường/cột/cây khi che mất nhân vật trước camera.

# 08. Hệ Thống Bản Đồ Đầy Đủ (6 Biomes, 12 Màn Chơi & Endless Mode)

> Tài liệu chi tiết toàn bộ bản đồ, màn chơi, bẫy đặc trưng, bảng spawn quái/cổ vật và lộ trình rẽ nhánh cho `BiomeConfigSO` & `StageConfigSO`.

---

## 1. Cấu Trúc Tiến Trình Map (Branching Expedition Map)

Thay vì đi thẳng 1 đường nhàm chán, game hỗ trợ **2 chế độ chọn màn** từ bảng hợp đồng ở `01_SkyDockHub`:
1. **Standard Run (Chọn Nhánh — 5 Màn / Run, ~25–30 phút)**:
   - **Chặng 1**: Màn `1-1` (mặc định lần đầu; có thể chọn `1-1` hoặc `1-2` sau khi đã mở khóa `1-2`).
   - **Chặng 2 (Rẽ nhánh)**: Chọn giữa **Chương 2 (Rừng Nấm)** hoặc **Chương 3 (Xưởng Đúc)**
   - **Chặng 3 (Rẽ nhánh)**: Chọn giữa **Chương 4 (Vịnh Băng)** hoặc **Chương 5 (Đầm Lầy Trọng Lực)**
   - **Chặng 4**: **Chương 6 — Màn 6-1** (Bậc Thang Hoàng Hôn)
   - **Chặng 5 (Chung Kết)**: **Chương 6 — Màn 6-2** (Đỉnh Ngai Golem — Boss Finale)
2. **Expedition / Select Stage**: Chơi riêng từng màn đã mở khóa để luyện tập hoặc săn kỷ lục.
3. **Endless Sky-Abyss (Mở sau khi thắng 6-2)**: Random map từ 12 màn kèm **Stage Modifiers** cộng dồn, Quota nhân `1.35x` mỗi tầng.
   - **Cân bằng chống Softlock ở Endless**: `RelicValueMultiplier` đồng thời tăng `1.25x` mỗi tầng và tăng tỷ lệ cổ vật Tier 2/3 để tổng giá trị đồ trên đảo luôn đảm bảo `>= 1.8x Quota`.
   - Nếu bốc trúng Màn `6-2` trong Endless Mode, nộp **Vương Miện Hư Không** cộng ngay **`50%` Quota hiện tại** (tối thiểu `$2,500`) thay vì bỏ qua toàn bộ Quota.

---

## 2. Hai Map Hub & Trung Chuyển

| Scene Name | Tên Map | Chức Năng & Khu Vực Bên Trong |
| :--- | :--- | :--- |
| `01_SkyDockHub` | **Trạm Chỉ Huy Khí Cầu** | 1) Lò Rèn Chế Tạo (18 công thức từ quặng) & Tủ Đổi Trang Bị của Bác Kael; 2) Bảo Tàng Cổ Vật & **Bể Nuôi Cá Mây (`Sky-Aquarium`)** của Myla; 3) **Trang Trại Mây (`Sky-Ranch` 4 loài thú nuôi & Pet)**; 4) **Cầu Tàu Câu Cá Mây & Bếp Lò**; 5) Bãi tập vật lý, Bảng Hợp Đồng & Cửa nhảy (*Drop Hatch*). Chi tiết xem [File 14](./14_CHE_TAO_QUANG_CHAN_NUOI_VA_NUOI_CA.md). |
| `02_AirshipShop` | **Khoang Tàu Nghỉ Giữa Chặng** | Xuất hiện giữa các chặng: Quầy máy bán 6 món Shop Item của Pip (có mặc cả), **Bếp Lò Nấu Cá Buff**, trạm hồi HP, bảng chọn nhánh màn tiếp theo. |

> **Cấu trúc Map Màn Chơi**: Toàn bộ 12 Màn Chơi bên dưới được sinh động theo **kiến trúc Phòng Nối Phòng Đa Dạng (`RoomGraphGenerator`)** — mỗi màn ưu tiên các loại Phòng đặc trưng của Biome kết hợp *Phòng Tháp Đứng*, *Hang Quặng* và *Ban Công Câu Cá* (chi tiết xem [13_HE_THONG_SINH_TON_CAU_CA_DAO_QUANG_VA_MAP_PHONG_NOI_PHONG.md](./13_HE_THONG_SINH_TON_CAU_CA_DAO_QUANG_VA_MAP_PHONG_NOI_PHONG.md)).

---

## 3. Chi Tiết 6 Biomes & 12 Màn Chơi

### CHƯƠNG 1 — Quần Đảo Gió Lộng (Biome 1: Windmill Meadow)
*Tone màu*: Xanh cỏ – Vàng nắng – Đá ấm | *Nhạc*: Nhẹ nhàng, phiêu lưu

#### Màn 1-1: Bãi Cỏ Cối Xay (`Stage_1_1_WindmillOutskirts`)
- **Mục tiêu**: Làm quen nhảy Floating Capsule, giữ thăng bằng qua cầu treo và ném đồ vào Pod.
- **Quota / Thời gian**: `$450` | `4 phút 00 giây` | Tổng giá trị đồ spawn: `~$850` (8–9 món Tier 1).
- **Bố cục 3 khu vực**:
  - *Bãi Đáp Trung Tâm*: Pod đặt trên đồi thấp, tầm nhìn thấy 2 cối xay gió lớn.
  - *Nhánh Đông (Cầu Treo)*: Cầu dây `HingeJoint` bắc qua khe hẹp nối sang kho gốm cũ.
  - *Nhánh Tây (Tháp Cối Xay)*: Đường dốc xoắn quanh tháp để lấy *Bình Pha Lê* và *Chuông Gió*.
- **Cạm bẫy**: Gió nhẹ trên cầu treo; cánh quạt cối xay quay chậm (đứng trên được hoặc bị hất nếu va cạnh).
- **Quái**: `Pebble Imp` (4–5 con), `Echo Stalker` (1 con ở tháp Tây).

#### Màn 1-2: Thung Lũng Lốc Xoáy (`Stage_1_2_GaleRidge`)
- **Mục tiêu**: Kết hợp **Luồng Gió Thổi Đứng (`Wind Updraft`)** để bay qua đảo tách rời trong khi vẫn giữ cổ vật.
- **Quota / Thời gian**: `$700` | `4 phút 15 giây` | Tổng giá trị đồ: `~$1,300` (Tier 1 + 2 món Tier 2).
- **Bố cục**: 4 đảo đá nhỏ lơ lửng cách nhau `8–12m`, nối bằng cột luồng gió đứng và 1 chiếc cầu gỗ hẹp.
- **Cạm bẫy**: Luồng gió theo chu kỳ (bật `6s` / nghỉ `2s`, có vệt gió báo trước); đồ nhẹ (`<5kg`) thả ra trong gió sẽ bay bổng lên trên.
- **Quái**: `Pebble Imp` (5), `Echo Stalker` (2), `Gust Wisp` (1).

---

### CHƯƠNG 2 — Rừng Nấm Phát Quang (Biome 2: Bioluminescent Canopy)
*Tone màu*: Xanh lam ngọc – Tím dạ quang – Nâu gỗ rêu | *Shader nổi bật*: `OcclusionFade` xuyên tán lá dày

#### Màn 2-1: Rừng Nấm Đàn Hồi (`Stage_2_1_BounceWoods`)
- **Mục tiêu**: Lợi dụng mũ nấm đàn hồi như bạt nhún vật lý để vận chuyển cổ vật lên cao nhanh chóng.
- **Quota / Thời gian**: `$950` | `4 phút 30 giây` | Tổng giá trị đồ: `~$1,800` (có *Quả Cầu Lò Xo*, *Tù Và Khí Lưu*).
- **Bố cục**: Lòng chảo rừng rậm nhiều gốc cây rỗng và cụm nấm phát sáng xếp bậc thang tự nhiên.
- **Cạm bẫy**: Mũ nấm nảy theo góc tiếp xúc (ném cổ vật lên nấm có thể chuyền đồ qua tầng trên hoặc làm nảy văng mất nếu lệch góc); sương mù bào tử làm hẹp tầm nhìn nhẹ ở đáy thung lũng.
- **Quái**: `Pebble Imp` (5), `Spring Mimic` (2 rương giả lẫn giữa rương thật), `Echo Stalker` (2).

#### Màn 2-2: Tổ Rễ Cổ Thụ (`Stage_2_2_HollowCanopy`)
- **Mục tiêu**: Leo trèo thẳng đứng (Verticality) dọc theo thân cây khổng lồ cao `45m`, thả đồ từ trên cao rơi xuống khu Pod có lưới đệm nấm.
- **Quota / Thời gian**: `$1,250` | `4 phút 30 giây` | Tổng giá trị đồ: `~$2,300` (thêm *Lăng Kính Băng*, *Lõi Rương Bật*).
- **Bố cục**: Thân cây khổng lồ ở giữa làm trục xoắn ốc; Pod nằm ở gốc rễ trung tâm bên dưới các tán nấm hứng đồ.
- **Cạm bẫy**: Nhánh cây mục gãy rơi sau `2.5s` đứng lên; cơn gió tán lá thổi ngang tầng đỉnh.
- **Quái**: `Gust Wisp` (3 bay quanh thân cây), `Spring Mimic` (2), `Pebble Imp` (6).

---

### CHƯƠNG 3 — Xưởng Đúc Cổ Đại (Biome 3: Clockwork Foundry)
*Tone màu*: Cam đồng – Xám thép – Xanh lân tinh | *Nhạc*: Nhịp cơ khí rộn ràng

#### Màn 3-1: Dây Chuyền Bánh Răng (`Stage_3_1_Gearworks`)
- **Mục tiêu**: Giữ vị trí trên bánh răng khổng lồ đang xoay và tận dụng băng chuyền để đẩy cổ vật về gần Pod.
- **Quota / Thời gian**: `$1,550` | `4 phút 45 giây` | Tổng giá trị đồ: `~$2,800` (nhiều *Bánh Răng Đồng*, *Khối Nam Châm*).
- **Bố cục**: 3 cụm bánh răng lớn xoay ngược chiều nhau + hệ thống băng chuyền nối về khu Pod trung tâm.
- **Cạm bẫy**: Băng chuyền (`3.5 m/s`), piston đẩy ngang nhịp `4s/lần` (trúng đòn bật Ragdoll nhẹ), hố nhiệt nhẹ.
- **Quái**: `Pebble Imp` (6), `Spring Mimic` (2), `Gust Wisp` (2).

#### Màn 3-2: Lò Luyện Hư Không (`Stage_3_2_SmeltingCore`)
- **Mục tiêu**: Vượt qua khu đúc dung nham năng lượng và hạ/né **Mini-Boss Clockwork Crusher** để lấy *Lõi Lò Phản Ứng*.
- **Quota / Thời gian**: `$1,900` | `5 phút 00 giây` | Tổng giá trị đồ: `~$3,400`.
- **Bố cục**: Đảo chính hình chữ U bao quanh hồ năng lượng nóng sáng, ở giữa có cần cẩu từ trường xoay chậm làm cầu nối tắt.
- **Cạm bẫy**: Hố năng lượng (chạm vào mất `25 HP` & bắn nảy lên không trung); bàn bật lò xo (`Jump Pad`).
- **Quái & Mini-Boss**: `Mini-Boss Clockwork Crusher` (`HP 550`, cầm búa xoay đẩy ragdoll mạnh, rơi `Lõi Lò Phản Ứng` giá trị cao khi gục) + `Echo Stalker` (2) + `Gust Wisp` (2).

---

### CHƯƠNG 4 — Vịnh Băng Pha Lê (Biome 4: Frostglass Glacier)
*Tone màu*: Xanh băng – Trắng tuyết – Ánh cực quang tím | *Vật lý nổi bật*: Ma sát thấp (`friction = 0.04`)

#### Màn 4-1: Sông Băng Trơn Trượt (`Stage_4_1_SlickIce`)
- **Mục tiêu**: Làm chủ quán tính trượt băng — đẩy cổ vật nặng trượt dài trên băng về Pod mà không cần vác bộ cả quãng đường!
- **Quota / Thời gian**: `$2,300` | `5 phút 00 giây` | Tổng giá trị đồ: `~$4,100` (nhiều *Lăng Kính Băng*, *Khối Nam Châm*).
- **Bố cục**: Mặt hồ đóng băng rộng lớn có các mỏm đá nhám làm điểm phanh và cầu băng hẹp.
- **Cạm bẫy**: Sàn băng trơn (`friction = 0.04`); gió tuyết thổi ngang mỗi `20s` (kéo dài `5s`, đẩy nhẹ vật và quái trượt trên băng).
- **Quái**: `Pebble Imp` (7 — chúng cũng bị trượt băng hài hước khi húc hụt!), `Echo Stalker` (3).

#### Màn 4-2: Hang Động Gương Pha Lê (`Stage_4_2_MirrorCavern`)
- **Mục tiêu**: Quản lý tải trọng trên **Sàn Băng Mỏng** (không vác đồ quá nặng đứng lâu quá `3s` trên băng nứt) và tìm cổ vật giá trị cao ẩn trong hốc pha lê.
- **Quota / Thời gian**: `$2,700` | `5 phút 15 giây` | Tổng giá trị đồ: `~$4,800` (có *Đồng Hồ Cát Thời Gian*).
- **Bố cục**: Hang động 2 tầng: tầng trên là cầu băng mỏng nhìn xuyên xuống tầng hầm chứa kho báu bên dưới.
- **Cạm bẫy**: Sàn băng nứt theo tổng khối lượng đứng trên (`Player + Relic > 85kg` nứt nhanh gấp đôi); cột nhũ băng rơi khi có tiếng nổ lớn.
- **Quái**: `Gust Wisp` (3), `Spring Mimic` (3), `Echo Stalker` (2).

---

### CHƯƠNG 5 — Đầm Lầy Trọng Lực (Biome 5: Abyssal Rift Swamp)
*Tone màu*: Tím sẫm – Xanh axit – Đá nổi lơ lửng | *Vật lý nổi bật*: Trọng lực biến đổi & Vũng bùn

#### Màn 5-1: Vũng Bùn Hút Lực (`Stage_5_1_TarBasin`)
- **Mục tiêu**: Nhanh tay cứu cổ vật khỏi vũng bùn đen trước khi bị chìm, dùng bong bóng khí nâng người vượt đầm lầy.
- **Quota / Thời gian**: `$3,200` | `5 phút 15 giây` | Tổng giá trị đồ: `~$5,600` (có *Mỏ Neo Trọng Lực*).
- **Bố cục**: Đầm lầy nhiều gò đá rải rác; các cổ vật quý nằm ở các đảo chìm giữa vũng bùn.
- **Cạm bẫy**: Vũng bùn giảm `45%` tốc độ chạy và kéo cổ vật đặt xuống chìm dần sau `10s` (rồi tự respawn lên gò đá gần nhất); bong bóng khí đẩy lên cao định kỳ.
- **Quái**: `Pebble Imp` (6), `Echo Stalker` (3), `Gust Wisp` (3).

#### Màn 5-2: Trạm Thí Nghiệm Đổ Nát (`Stage_5_2_OvergrownLab`)
- **Mục tiêu**: Dùng **Cổng Dịch Chuyển Vật Lý** (ném hoặc bước qua cổng để chuyển cổ vật từ phòng thí nghiệm xa về gần Pod) và thích nghi giữa vùng trọng lực thấp/cao.
- **Quota / Thời gian**: `$3,700` | `5 phút 30 giây` | Tổng giá trị đồ: `~$6,500`.
- **Bố cục**: Khu tàn tích phòng thí nghiệm vỡ làm 3 khối đảo tách biệt nối nhau bởi 2 cặp cổng dịch chuyển màu Xanh/Cam.
- **Cạm bẫy**: Vùng trọng lực thấp (`0.45g`) và vùng trọng lực nặng (`1.6g` — đồ vật nảy ít hơn nhưng rơi dễ vỡ gấp đôi!).
- **Quái**: `Spring Mimic` (3), `Echo Stalker` (3), `Gust Wisp` (4).

---

### CHƯƠNG 6 — Thánh Điện Mây Vỡ (Biome 6: Shattered Spire Citadel)
*Tone màu*: Tím hoàng hôn – Vàng kim – Pha lê rực sáng | *Nhạc*: Hùng tráng, cao trào

#### Màn 6-1: Bậc Thang Hoàng Hôn (`Stage_6_1_TwilightAscent`)
- **Mục tiêu**: Thử thách tổng hợp kỹ năng di chuyển, né bẫy và chống chọi wave quái dày đặc khi leo lên cổng Thánh Điện.
- **Quota / Thời gian**: `$4,300` | `5 phút 30 giây` | Tổng giá trị đồ: `~$7,500`.
- **Bố cục**: Con đường dốc lớn đứt đoạn với cầu ánh sáng bật/tắt theo nhịp `5s` và nhiều cụm sàn pha lê nứt (`1.8s`).
- **Cạm bẫy**: Sàn nứt, cầu ánh sáng chu kỳ, luồng gió đẩy dọc tháp.
- **Quái**: Hỗn hợp cả 4 loại quái thường với tần suất hồi nhanh hơn (`18s/đợt`).

#### Màn 6-2: Đỉnh Ngai Golem (`Stage_6_2_WardensSanctum` — Boss Finale)
- **Mục tiêu**: Vừa thu thập cổ vật rải quanh đấu trường vừa đánh/ném vật vào lõi sau lưng **Boss Golem Warden (3 Phase)** để phá Khiên Ngai (mở khi Boss vào Phase 3 `<30% HP`, bị Stagger lần 2, hoặc bị hạ hẳn), cướp **Vương Miện Hư Không (`$1,200`)** và thoát thân trước khi đấu trường sụp đổ.
- **Điều kiện mở khóa Launch Pod**: Đạt **`$5,000` Pod Value** **HOẶC** nộp thành công **Vương Miện Hư Không** vào Pod (mở khóa Launch Pod ngay lập tức) | `6 phút 00 giây`.
- **Bố cục**: Đấu trường tròn 2 tầng; Pod nằm trên bệ nâng rìa Nam; 4 cột đá cổ (có thể dụ Golem Phase 3 húc vào để tự Stagger `3.0s`).
- **Cạm bẫy**: Sóng xung kích của Boss, sàn rìa ngoài bắt đầu nứt vỡ khi Golem vào Phase 3 hoặc lúc Collapse Timer `<= 60s`.

---

## 4. Biến Thể Endless Sky-Abyss (Stage Modifiers)

Mỗi tầng trong chế độ Endless bốc ngẫu nhiên **1 Modifier** (từ tầng 3 trở đi bốc **2 Modifiers**):
1. **Low Gravity Chaos**: Toàn bản đồ giảm trọng lực còn `0.6g`, lực đẩy Ragdoll tăng `1.5x`.
2. **Fragile Cargo**: Toàn bộ cổ vật giảm `30%` HP tối đa nhưng giá trị bán tăng `+40%`.
3. **Noise Storm**: Không có âm thanh bước chân nhưng cứ mỗi `15s` phát sóng ồn ngẫu nhiên dụ *Echo Stalker*.
4. **Imp Swarm**: Tăng gấp đôi số *Pebble Imp* nhưng giảm `30%` HP của chúng.
5. **Golden Rush**: Thời gian Collapse giảm còn `3 phút`, nhưng mọi cổ vật tăng `+50%` giá trị.
6. **Slippery Ruins**: Toàn mặt đất có độ trơn nhẹ như băng (`friction = 0.25`).

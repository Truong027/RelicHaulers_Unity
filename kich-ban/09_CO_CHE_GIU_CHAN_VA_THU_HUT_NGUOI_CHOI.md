# 09. Cơ Chế Thu Hút, Gây Nghiện & Giữ Chân Người Chơi (Player Engagement & Retention)

> Tài liệu thiết kế các yếu tố **Dopamine Loop, Synergy Combo, Sự Kiện Bất Ngờ, Meta-Progression và Khoảnh Khắc Viral** để người chơi luôn muốn bấm *"Chơi thêm 1 lượt nữa"* (One More Run).

---

## 1. Hệ Thống Tiến Hóa Combo (Upgrade Synergy Evolutions)

Khi người chơi sở hữu **cả 2 Upgrade đúng cặp** (mỗi món tối thiểu `1 stack`), hệ thống tự động mở khóa **Synergy Evolution** — kèm hiệu ứng thông báo vàng rực trên HUD:

| Cặp Upgrade Cần Có | Tên Synergy | Hiệu Ứng Bùng Nổ |
| :--- | :--- | :--- |
| `Giày Lò Xo` + `Lõi Xung Kích` | **Meteor Stomp (Thiên Thạch Giáng)** | Rơi từ độ cao `> 4m` tạo vụ nổ thiên thạch bán kính `5m`, gây `80` dmg và hất toàn bộ quái quanh đó thành Ragdoll! |
| `Tinh Thể Quỹ Đạo` + `Tia Sét Truyền Dẫn` | **Tesla Orbit (Vòng Bão Điện Từ)** | Các tinh thể xoay quanh người tự phóng tia sét liên hoàn mỗi `1.2s` vào quái trong `6m`. |
| `Găng Từ Trường` + `Cánh Tay Máy Ném` | **Railgun Toss (Ném Xuyên Phá)** | Cổ vật ném với lực gồng tối đa bay đường thẳng không rơi trọng lực trong `1.0s`, xuyên qua quái nhỏ với `2.5x` sát thương và **được trường từ bao bọc giảm 80% sát thương vỡ khi va chạm**! |
| `Đế Giày Êm` + `Đèn Hiệu Pod` | **Ghost Hauler (Bóng Ma Thu Hồi)** | Vô hiệu hóa 100% tiếng ồn khi chạy/nhảy và cho phép ném cổ vật vào tia hút Pod từ khoảng cách gấp đôi. |
| `Balo Giảm Trọng` + `Giáp Cao Su Nảy` | **Juggernaut Bounce (Xe Ủi Đàn Hồi)** | Khi đang vác cổ vật nặng (`>=12kg`), chạy nhanh húc thẳng vào quái nhỏ sẽ hất văng quái mà không bị dừng lại! |
| `Bọc Khí Pha Lê` + `Máy Nhân Bản Mini` | **Prismatic Vault (Két Sắt Pha Lê)** | Nếu cổ vật có khiên bọc khí còn nguyên vẹn lúc đưa vào Pod, **chắc chắn 100%** được nhân `1.5x` giá trị. |

---

## 2. Cơ Chế "Lòng Tham vs An Toàn" & Cao Trào Cuối Màn

### A. Nút Đánh Cược: Overcharge Pod (Tăng Ca Tham Lam)
- Khi đạt `100% Quota`, trên thân Pod hiện **2 lựa chọn rõ ràng**:
  1. **Nút Xanh (`Launch Pod`)**: Rút quân an toàn ngay sau `15s` giữ vị trí.
  2. **Nút Đỏ (`OVERCHARGE GREED`)**: Kích hoạt chế độ Tăng Ca!
     - **Rủi ro**: Thời gian Collapse còn lại bị chia đôi (tối thiểu `45s`), mật độ quái tăng `1.8x`.
     - **Phần thưởng**: Mọi cổ vật vớt thêm được nhân **2.5x giá trị tiền** và ngay lập tức thả xuống đảo **1 Rương Upgrade Epic**!
- *Tâm lý*: Tạo khoảnh khắc tranh cãi/cân não cực căng thẳng: *"Rút về cho chắc hay bấm nút Đỏ làm cú chót?"*

### B. Khoảnh Khắc Nghẹt Thở (Clutch Extraction 15s)
- Trong 15 giây Pod khởi động cất cánh:
  - Loa Pod hú còi nhịp nhanh dần, bầu trời tối sầm, luồng sáng xanh bốc thẳng lên trời.
  - Ở `3 giây cuối`, người chơi vẫn có thể đứng từ xa **ném cổ vật bay vào cột sáng xanh** của Pod để tính tiền khẩn cấp; nhưng khi đồng hồ về `00:00`, nhân vật **bắt buộc phải đứng trong Safe Zone (`r = 3.5m`) hoặc bám trên thân Pod** mới tính thoát hiểm thành công (đứng ngoài sẽ bị bỏ lại trên đảo → Contract Failed).

---

## 3. Sự Kiện Ngẫu Nhiên Trong Màn (Dynamic World Events)

Mỗi màn có `40%` xác suất kích hoạt **1 Sự Kiện Bất Ngờ** ở phút thứ `01:30` hoặc `02:30` để không có 2 lượt chơi nào giống nhau:

1. **Golden Imp (Tiểu Yêu Trộm Kho Báu)**:
   - Một con Imp màu vàng óng ôm sẵn 1 Cổ Vật Tier 2/3 chạy trốn cực nhanh khắp đảo; có dấu mũi tên vàng chỉ hướng.
   - Đánh trúng hoặc ném đồ làm nó bật Ragdoll để cướp lại cổ vật trước khi nó chui xuống hố biến mất sau `40s`.
2. **Thương Nhân Khinh Khí Cầu (Sky Peddler)**:
   - Một chiếc thuyền bay mini thả thang dây xuống rìa đảo trong `50s`.
   - Cho phép **đổi 1 Cổ Vật đang cầm** (giá trị `>= $200`) lấy **1 thẻ Upgrade Rare/Epic** ngay tại chỗ!
3. **Cơn Mưa Di Tích (Relic Meteor Shower)**:
   - Bầu trời rơi xuống `3` viên nang sáng rực ở 3 góc đảo, chứa cổ vật giá trị cao nhưng tỏa nhiệt thu hút quái bu quanh.
4. **Bão Tĩnh Điện (Static Storm)**:
   - Cứ `12s` xuất hiện `3` vòng tròn cảnh báo trên mặt đất và giáng sét sau `2.0s` — người chơi khéo léo có thể dụ *Golem Warden* hoặc đàn quái đứng vào vòng sét để gây Stagger & Ragdoll diện rộng!

---

## 4. Hệ Thống Giữ Chân Dài Hạn (Meta-Progression & "Thất Bại Vẫn Vui")

### A. Bảo Tàng Cổ Vật Tại Sky-Dock (3D Relic Museum)
- Trong sảnh `01_SkyDockHub` có một hành lang trưng bày **14 Bệ Kính 3D**:
  - Mỗi loại Cổ Vật lần đầu đưa được về Pod sẽ hiện nguyên mô hình 3D xoay nhẹ trên bệ kính kèm **thẻ kỷ lục vui**:
    - *Tổng số lần thu hồi* | *Số lần làm vỡ tan tành* | *Cú ném xa nhất vào Pod*.
  - Thu thập đủ 14/14 cổ vật mở khóa danh hiệu & **Mũ Vương Miện Vàng** cho nhân vật.

### B. Cây Công Nghệ Hội Hauler (Guild Tech Tree — Nâng Vĩnh Viễn Bằng Guild Tokens)
Dù thắng hay thua (vẫn giữ `50%` Guild Tokens kiếm được), người chơi luôn có thứ để nâng cấp ngay khi quay về Hub:
- **Nhánh Thể Chất**:
  - *Bốt Khởi Động*: Tăng `+10 / +20` Max HP khởi đầu.
  - *Bình Hơi Dự Phòng*: Tăng `+15%` tốc độ hồi Stamina.
  - *Dây Neo An Toàn*: Giảm `30%` sát thương khi rớt vực.
  - *Kén Tái Sinh Khẩn Cấp (`Emergency Tether`)*: Mở khóa `1` lần tự động Hồi Sinh mỗi chặng với `50% HP` (trừ `15s` Collapse Timer; chi tiết tại [File 15](./15_CO_CHE_HOI_SINH_VA_COT_TRUYEN_INK.md)).
- **Nhánh Kinh Tế & Pod**:
  - *Thẻ Hội Viên VIP*: Giảm `15%` giá mua đồ ở Airship Shop.
  - *Két Bảo Hiểm*: Nếu chết giữa Run, giữ lại `15%` giá trị cổ vật đã nộp quy đổi thành Guild Tokens.
  - *Máy Quét Cổ Vật*: Đầu mỗi màn tự động đánh dấu vị trí `1` Cổ Vật hiếm xuyên tường trong `12s`.

### C. Mảnh Nhật Ký Bí Mật & Độ Khó Rift Hazard (1–5)
- **12 Mảnh Nhật Ký Cổ (`Sky-Shard Logs`)**: Giấu tại các vị trí hiểm trở nhất ở 12 màn chơi (1 mảnh/màn). Thu thập đủ `12/12` mở khóa:
  - Kết thúc thật sự (**True Ending `END_TRUE_VEX_SKY`** trong hệ thống phân nhánh `inkle/ink` 3 Kết Thúc tại [File 15](./15_CO_CHE_HOI_SINH_VA_COT_TRUYEN_INK.md)) giải mã nguyên nhân *Đại Phân Tách*.
  - **Skin Bí Mật `VEX-Prototype`**: Hóa thân thành robot Điều Phối Viên phát sáng với hiệu ứng giọng nói & dấu chân điện tử!
- **Rift Hazard Level (`1 → 5`)**: Sau khi phá đảo, mở khóa từng cấp độ khó tăng thêm (quái nhanh hơn, nhiều bẫy hơn) đổi lại thưởng `Guild Tokens x1.25 / cấp` và huy hiệu khung tên mới.

---

## 5. Thiết Kế Khoảnh Khắc Viral & Tổng Kết Đáng Nhớ

### A. Danh Hiệu Vui Cuối Trận (Playful Run Awards)
Tại màn hình Tổng Kết (Victory / Contract Failed), hệ thống tự động chấm điểm hành vi của người chơi để trao 1–2 huy hiệu hài hước:
- **Kẻ Phá Hoại Pha Lê**: Làm vỡ `>= 3` cổ vật trong 1 Run.
- **Xạ Thủ Vận Chuyển**: Ném cổ vật bay vào Pod từ khoảng cách `>= 10m` (hoặc ném trúng hạ quái từ `>= 12m`).
- **Nghệ Nhân Ragdoll**: Bị hất văng Ragdoll `>= 5` lần nhưng vẫn sống sót thoát hiểm.
- **Kẻ Tham Lam Bất Tử**: Bấm nút *Overcharge Pod* và thu vượt `>= 200% Quota`.
- **Vận Động Viên Không Lưu**: Ở trên không trung tổng cộng `> 35%` thời gian màn chơi.

### B. Thẻ Khoảnh Khắc Đỉnh Cao (Highlight Snapshot Card)
- Trong lúc chơi, `HighlightRecorder` tự động chụp 1 khung hình (`ScreenCapture` lưu tạm trên RAM) ở sự kiện có biên độ vật lý cao nhất (lúc làm vỡ cổ vật đắt tiền, lúc ném hạ Boss, hoặc lúc 4 con quái cùng bay Ragdoll).
- Cuối Run, ảnh này được đóng khung thành **Tấm Ảnh Kỷ Niệm Hợp Đồng** ngay bên cạnh bảng điểm — tạo động lực chụp chia sẻ cho bạn bè!

### C. Hợp Đồng Thử Thách Hằng Ngày (Daily Seed Contract)
- Dùng `System.DateTime.UtcNow.Date` làm Random Seed để tạo **1 thử thách cố định mỗi ngày** (1 Màn ngẫu nhiên + 2 Stage Modifiers + 1 Archetype bắt buộc), cho phép người chơi đua điểm cao nhất trong ngày!

# 14. Hệ Thống Chế Tạo Quặng Đa Dạng, Chăn Nuôi & Nuôi Cá (Chống Lỗi Logic)

> Mở rộng chiều sâu cho chế độ sinh tồn tại `01_SkyDockHub` và `02_AirshipShop`: **18 công thức chế tạo từ Quặng + Nguyên liệu Chăn nuôi/Nuôi cá**, **4 loài sinh vật thuần hóa**, và **Bể Nuôi Cá Mây** — được thiết kế kèm các **Luật Chống Lỗi Logic (Anti-Exploit Rules)** để không phá hỏng gameplay Extraction Roguelite.

---

## 1. Các Nguyên Tắc Vàng Tránh Lỗi Logic Ngớ Ngẩn

1. **Tiến Độ Theo Lượt Chơi (`Run Tick`), KHÔNG Theo Thời Gian Thực (Anti-AFK)**:
   - Chuồng chăn nuôi và Bể nuôi cá **chỉ lớn lên / sinh sản / tạo nguyên liệu khi bạn hoàn thành hoặc tham gia 1 chặng nhảy xuống đảo (`1 chặng = 1 Growth Tick`)**.
   - Kho chứa thu hoạch tại Hub tích tối đa **`3` Tick** — ép người chơi phải ra trận mới thu hoạch tiếp được, tuyệt đối không có lỗi đứng AFK farm vô hạn ở Hub.
2. **Tách Rạch Ròi Kho Chứa & Luật Khi Chết**:
   - **Thắng / Rút quân qua Pod**: Giữ `100%` Quặng & Cá trong `OrePouch` mang về Hub.
   - **Thất bại giữa màn (`Contract Failed`)**: Giữ lại `50%` (làm tròn xuống) số Quặng nhỏ & Cá thường; nâng cấp Tech Tree *Két Bảo Hiểm* tăng lên `75%`. (Riêng *Khối Quặng Khổng Lồ* nặng `12kg` chỉ tính nếu đã ném vào Pod trước lúc rút).
3. **Giới Hạn Trang Bị Mang Vào Màn (`Gadget Slot Cap = 2`)**:
   - Các món chế tạo tiêu hao chiến thuật (*Bom xung kích, Lưới hứng, Keo hàn, Bọc chống sốc...*) chỉ được mang tối đa **`2` ô Gadget** mỗi Run (mỗi ô `1–2` cái tùy món), không thể chất đầy balo 20 quả bom phá hỏng cân bằng màn chơi!
4. **Pet Đồng Hành Không Gây Lỗi Vật Lý (`Zero Self-Collision`)**:
   - Nếu chọn `1` thú cưng đã thuần hóa đi cùng xuống đảo, Pet sử dụng Layer va chạm riêng **không đẩy nhân vật và không va chạm làm vỡ Cổ Vật của chủ**, tự dịch chuyển theo chủ nếu bị rớt vực.

---

## 2. Hệ Thống Chế Tạo Đa Dạng Từ Quặng (`Sky-Dock Alloy Forge`)

### A. Nhóm 1 — Nấu Hợp Kim & Tinh Chế (4 Công Thức Trung Gian)
Tại **Lò Luyện Hợp Kim của Bác Kael**, người chơi kết hợp các loại quặng từ nhiều Chương để đúc nguyên liệu cao cấp:

| ID Hợp Kim | Nguyên Liệu Đầu Vào | Dùng Để Chế Tạo |
| :--- | :--- | :--- |
| `mat_wind_bronze` (Đồng Gió Rèn) | `2` Đồng Tầng Mây + `1` Vảy Cá Mây | Gadget cơ bản, Cuốc/Cần Cấp 2, Đệm Nấm. |
| `mat_lumina_alloy` (Hợp Kim Phát Quang) | `2` Hổ Phách + `1` Sắt Cổ Đại | Đèn siêu sáng, Pháo quét, Bọc chống sốc. |
| `mat_cryo_steel` (Thép Băng Lạnh) | `2` Thạch Anh Băng + `2` Sắt Cổ Đại | Giày bám băng, Keo hàn cao cấp, Lưới hứng. |
| `mat_void_ingot` (Thỏi Hư Không) | `2` Tinh Thạch Hư Không + `1` Thép Băng + `1` Dầu Bọ Cạp | Trang bị cuối, Cuốc/Cần Cấp 3, Tech Tree cuối. |

### B. Nhóm 2 — 6 Thiết Bị Tiêu Hao Chiến Thuật (`Tactical Gadgets` — Tối Đa 2 Slot/Run)
Phím sử dụng nhanh **`4` / `5`** (hoặc bấm giữ **`Q`** xoay chọn giữa Slot 1 & Slot 2; trên Gamepad là `Y` dùng Slot đang chọn):

| Tên Gadget | Công Thức Chế Tạo | Số Lượng/Slot | Công Dụng Trong Màn |
| :--- | :--- | :---: | :--- |
| **1. Lựu Đạn Xung Kích** | `1` Đồng Gió Rèn + `1` Dầu Bọ Cạp | `2` quả | Ném nổ tạo sóng đẩy văng đàn quái thành Ragdoll (`r=5m`), **không gây hại Cổ Vật**. |
| **2. Mồi Nhử Chuông Rung** | `2` Đồng Tầng Mây + `1` Mảnh Da/Lông | `2` cái | Đặt xuống phát tiếng ồn `25m` trong `12s`, dụ toàn bộ *Echo Stalker* & *Pebble Imp* tránh xa Pod. |
| **3. Bình Keo Hàn Pha Lê** | `1` Thạch Anh Băng + `1` Lông Pha Lê | `2` lần xịt | Sửa lại `+50%` HP cho Cổ Vật đang cầm trên tay (dùng cực sướng khi làm nứt Bình Pha Lê). |
| **4. Lưới Hứng Đệm Mây** | `2` Trứng Mây Đàn Hồi + `1` Đồng Tầng Mây | `1` tấm | Trải xuống sàn (`3m x 3m`, tồn tại `40s`): Cổ Vật ném từ tầng cao rơi trúng lưới **chịu 0 sát thương va chạm**! |
| **5. La Bàn Cộng Hưởng** | `1` Hợp Kim Phát Quang + `1` Vảy Cá Sáng | `2` lần quét | Quét xuyên tường `12s` toàn bộ Mạch Quặng hiếm, Cổ Vật Tier 2–3 và *Nhật Ký Bí Mật*. |
| **6. Gói Bọc Bọt Biển** | `2` Lông Pha Lê + `1` Hổ Phách | `1` bộ | Bọc trực tiếp lên `1` Cổ Vật bất kỳ đang cầm: cấp thêm lớp giáp **`+100 HP` hấp thụ va chạm**! |
| **7. Viên Nang Tái Sinh** (`gad_revive_capsule` — *Đặc biệt*) | `1` Hợp Kim Phát Quang + `1` Tinh Thạch Hư Không + `2` Vảy Cá Sáng | `1` viên (tối đa `1`/Run) | Tự động hồi sinh với `75%` HP khi ngã xuống trong lúc `CollapseTimer > 0` (chỉ trừ `5s` đồng hồ — xem [File 15](./15_CO_CHE_HOI_SINH_VA_COT_TRUYEN_INK.md)). |

### C. Nhóm 3 — 5 Trang Bị Chiến Thuật Mặc Vĩnh Viễn (Rèn Mở Khóa 1 Lần, Thay Tại Tủ Kael)
*(Không chồng lấn với các thẻ Upgrade nhặt trong màn, chỉ cộng chỉ số nền nhẹ cân bằng)*:
1. **Giày Đinh Bám Băng** (`2` Thép Băng + `2` Sắt Cổ Đại): Giảm `45%` độ trơn trượt ở Chương 4 & khi đứng trên bánh răng xoay.
2. **Mũ Đèn Pha Pha Lê** (`2` Hợp Kim Phát Quang + `1` Đèn Cá Lồng): Tăng gấp đôi tầm chiếu sáng trong hang tối, làm *Echo Stalker* lộ viền mờ sớm hơn `2m`.
3. **Balo Hấp Thụ Sốc** (`2` Lông Pha Lê + `2` Đồng Gió Rèn): Giảm `20%` sát thương rơi từ trên cao & tăng thêm `+5` sức chứa `OrePouch`.
4. **Găng Tay Mỏ Chuyên Dụng** (`2` Thép Băng + `1` Dầu Bọ Cạp): Tăng `+25%` kích thước vòng `WeakSpotRing` khi đào quặng & giảm `10%` thời gian kéo cần câu.
5. **Thắt Lưng Ngăn Giữ Lạnh** (`1` Thỏi Hư Không + `2` Thạch Anh Băng): Giảm `15%` sát thương từ Hố Dung Nham và đạn gió *Gust Wisp*.

### D. Nhóm 4 — 3 Công Trình Nâng Cấp Tại Sky-Dock Hub (Rèn Từ Quặng)
1. **Mở Rộng Lò Luyện Tự Động**: Giảm `25%` lượng quặng cần để đúc Hợp Kim.
2. **Nâng Cấp Chuồng Trại Mây (`Cấp 1 → 3`)**: Mở từ `2 → 4 → 6` ô nuôi thú và thêm **Hộp Thu Hoạch Tự Động** (gom sản phẩm sau mỗi Run).
3. **Nâng Cấp Bể Cá Mây (`Cấp 1 → 3`)**: Mở từ `4 → 8 → 12` ô cá, thêm máy lọc khí giúp cá sinh sản nhanh hơn (`2 chặng → 1 chặng/lứa`).

---

## 3. Hệ Thống Chăn Nuôi Sinh Vật Đảo Nổi (`Sky-Ranch` Tại Hub)

Tại khu vực sân cỏ phía Đông của `01_SkyDockHub`, người chơi dựng một **Trang Trại Mây Nhỏ** để nuôi các sinh vật thuần hóa tìm được khi thám hiểm:

### Cách Tìm Con Giống (Không Mua Hết Bằng Tiền Để Tạo Động Lực Khám Phá)
- Khi đào quặng, mở rương bí mật, hoặc câu cá ở các Chương, người chơi có cơ hội nhặt được **Trứng / Con Non Đảo Nổi** (lưu an toàn trong ngăn đặc biệt, không bị mất kể cả khi chết giữa màn!).

### 4 Loài Sinh Vật Nuôi & Sản Phẩm Thu Hoạch (Mỗi `1–2` Chặng = `1` Lượt Thu)

| Loài Nuôi | Nguồn Gốc | Thức Ăn | Sản Phẩm Tạo Ra Mỗi Chu Kỳ | Kỹ Năng Nếu Chọn Làm `1` Pet Đi Cùng |
| :--- | :--- | :--- | :--- | :--- |
| **1. Gà Mây Lông Bông** (`Cloud-Puff Hen`) | Nở từ Trứng nhặt ở Chương 1–2 | Cá nhỏ hoặc Hạt Rêu | **Trứng Mây Đàn Hồi** (`1–2` quả/chặng) → làm *Lưới Hứng* & món ăn tăng nhảy. | Nhảy theo sau; khi chủ rơi gần vực, vỗ cánh tạo đệm gió nhỏ giảm `20%` sát thương rơi. |
| **2. Cún Đá Imp Thuần Hóa** (`Tamed Pebble Imp`) | Câu nhầm lên bờ hoặc nhặt *Trứng Đá* ở Chương 1–3 | Mảnh Đồng / Sắt vụn | **Bới ra `2–3` Mảnh Quặng ngẫu nhiên** (theo loại thức ăn bón cho nó!). | Tự chạy nhặt *Mảnh Quặng nhỏ* rơi vãi (`<6m`) và **sủa báo hiệu** nếu đứng gần *Spring Mimic*! |
| **3. Cừu Pha Lê** (`Crystal Lamb`) | Tìm thấy ở hốc băng Chương 4 | Thạch Anh Băng vụn | **Lông Pha Lê** (`1–2` bó/chặng) → rèn *Keo Hàn*, *Bọc Chống Sốc*, *Balo*. | Tỏa ánh sáng ấm áp xung quanh bán kính `3m`, làm chậm độ đóng băng/nứt sàn băng mỏng đi `25%`. |
| **4. Bọ Cạp Cơ Khí Mini** (`Mini-Gear Beetle`) | Tìm thấy ở Chương 3 & Chương 5 | Sắt Cổ Đại | **Dầu Bôi Trơn Cơ Khí** (`1–2` bình/chặng) → rèn *Bom Xung Kích*, *Găng Tay Mỏ*. | Tự động sửa nhẹ `+2 HP/giây` cho Cổ Vật đang cầm khi đứng yên nghỉ chân (tối đa hồi `25 HP`/cổ vật). |

> **Chốt cân bằng Pet**: Chỉ mang được tối đa **`1` Pet** xuống màn; Pet hoàn toàn miễn nhiễm va chạm vật lý với Cổ Vật và Player (chỉ chạy visual theo cạnh bên, không đẩy ngã chủ!).

---

## 4. Hệ Thống Bể Nuôi Cá Mây (`Sky-Aquarium` Tại Hub)

Ngay bên cạnh Bảo Tàng của Giáo Sư Myla và Cầu Tàu Câu Cá là **Hồ Kính Nuôi Cá Mây** — giải quyết triệt để mối liên hệ giữa *Câu Cá – Nuôi Cá – Nấu Ăn – Rèn Đồ*:

### Cơ Chế Hoạt Động Chống Lỗi Kinh Tế
1. **Phân Biệt Cá Giống & Cá Nguyên Liệu**:
   - Mỗi con cá câu được có thể chọn:
     - **[Đưa vào Kho Bếp]**: Dùng để nấu **6 Món Ăn Buff** (file `13`) hoặc bán thẳng ở Pod khi cần tiền Quota.
     - **[Thả vào Bể Nuôi Cá]** (nếu bể còn trống ô): Cá trở thành **Cá Bố Mẹ (`Breeder Fish`)**.
2. **Sinh Sản & Thu Hoạch Cá Nuôi**:
   - Với **1 cặp cùng loài** (hoặc `1` cá + dùng `1` Thức Ăn Cá mua từ Pip), sau mỗi **`2` chặng nhảy đảo** (giảm còn **`1` chặng** nếu đã nâng Bể Cá Cấp 3), cặp cá sinh ra **`1` Cá Con trưởng thành cùng loài** + **`1–2` Vảy Cá Phát Sáng** (dùng để rèn *Hợp Kim* & *La Bàn*).
   - **Giới hạn chống lạm dụng**: Bể chỉ tự động xuất tối đa `1` con mới mỗi loài mỗi chu kỳ, và **cá nuôi trong bể KHÔNG được tính là Cổ Vật mang xuống Pod để gian lận Quota** (chỉ được nấu Meal Buff, lấy Vảy rèn đồ, hoặc bán lấy **Guild Tokens** giá trị nhỏ `10–25 GT/con` tại Shop của Pip!).
3. **Tác Dụng Thẩm Mỹ & Kỷ Lục Tại Hub**:
   - Toàn bộ các loài trong `12` loài cá khi thả vào Bể Kính Khổng Lồ của Myla sẽ bơi lội 3D trực tiếp trong sảnh Hub; sưu tầm đủ `12/12` loài mở khóa danh hiệu **Thủy Thủ Tầng Mây** & **Cần Câu Vàng Vĩnh Viễn**!

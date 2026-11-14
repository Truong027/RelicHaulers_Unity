# 13. Hệ Thống Phòng Nối Phòng Đa Dạng, Đào Quặng & Câu Cá Sinh Tồn

> Bổ sung hai trụ cột mở rộng giúp game vừa có **bản đồ sinh ngẫu nhiên dạng phòng nối phòng đa dạng cấu trúc** (cảm hứng *R.E.P.O.* + *Lethal Company* + *Deep Rock Galactic*), vừa có **tính năng Đào Quặng & Câu Cá kiểu game sinh tồn** gắn chặt vào vòng lặp Extraction & Craft tại Hub.

---

## PHẦN A: KIẾN TRÚC MAP PHÒNG NỐI PHÒNG ĐA DẠNG (PROCEDURAL ROOM-GRAPH)

Thay vì dùng 1 mặt đảo tĩnh lặp đi lặp lại hay khối vuông Voxel kiểu Minecraft, mỗi màn chơi được tạo từ **các Phòng Đảo Nổi Stylized Low-Poly nối tiếp nhau qua Cổng Kết Nối (`DoorwaySocket`)** theo dạng đồ thị phân nhánh (*Branching Room Graph*).

### 1. Bộ 10 Loại Phòng Đa Dạng Cấu Trúc (Không Chỉ Là Phòng Vuông!)
Mỗi Biome sở hữu biến thể hình học & vật liệu riêng cho 10 Archetype phòng sau để tạo cấu trúc cực kỳ phong phú:

| Mã Phòng | Hình Dáng & Cấu Trúc Không Gian | Số Cổng (`Sockets`) | Vai Trò Trong Màn Chơi |
| :--- | :--- | :---: | :--- |
| `RM_StartPod` | Sân đáp trung tâm hình bát giác rộng có mái che & cầu cảng | `2 – 3` (ra các nhánh) | Điểm spawn người chơi & **Extraction Pod**. |
| `RM_CorridorBridge` | Cầu treo dây lắc / Đường ray băng chuyền / Cầu ánh sáng hẹp | `2` (thẳng hoặc uốn cong 45°) | Phòng chuyển tiếp tạo kịch tính di chuyển & ném đồ qua khe. |
| `RM_TJunction_Split` | Ngã ba chữ T / chữ Y có độ cao chênh lệch 2m (dốc hoặc bậc) | `3` | Phân nhánh đường đi an toàn vs đường tắt mạo hiểm. |
| `RM_Crossroad_Plaza` | Quảng trường ngã tư lớn có cột che & tầng gác lửng | `4` | Nút giao chính, thường chứa bẫy hoặc *Đài Nguyện Rift Shrine*. |
| `RM_Vertical_Shaft` | **Phòng Tháp Đứng 2–3 Tầng** (dốc xoắn / thang nhảy / luồng gió / nấm bật) | `2–3` (khác cao độ `Y`!) | Tạo chiều sâu thẳng đứng kiểu *PEAK*; có giếng thả đồ thẳng xuống tầng dưới. |
| `RM_Cavern_Mining` | **Hang Động Mạch Quặng** nhiều vách đá nhô ra | `2` | Chứa `2–3` **Mạch Quặng lớn** và tường đá nứt bí mật. |
| `RM_Cliff_FishingPier` | **Ban Công Mép Mây / Hồ Băng / Đầm Lầy** mở rộng tầm nhìn | `1–2` | Chứa **Điểm Câu Cá (`FishingSpot`)**, rương nhỏ, tầm nhìn ra biển mây. |
| `RM_Hazard_Gauntlet` | Phòng cơ khí dày đặc bẫy (*Bánh răng xoay / Sàn băng mỏng / Sàn nứt*) | `2` | Độ rủi ro cao, thưởng Cổ Vật Tier 2–3 ở giữa hoặc đầu bên kia. |
| `RM_Vault_Secret` | Phòng kho báu kín bị chặn bởi **Tường Đá Nứt** hoặc **Công Tắc Đối Trọng** | `1` (ẩn) | Chứa Rương Upgrade, Nhật Ký Bí Mật & Cổ Vật giá trị cao. |
| `RM_Boss_Arena` | Đấu trường tròn 2 tầng rộng lớn (chỉ spawn ở `3-2` Mini-Boss & `6-2` Boss) | `1–2` | Nơi chiến đấu với *Clockwork Crusher* hoặc *Golem Warden*. |

### 2. Thuật Toán Sinh Map Chống Lỗi (`RoomGraphGenerator.cs`)
1. **Đặt `RM_StartPod` ở gốc `(0, 0, 0)`**, đánh dấu độ sâu `Depth = 0`.
2. **Mở rộng theo nhánh (Branching Depth `3 → 6` tùy Chương)**:
   - Chương 1: `7 – 9` phòng | Chương 2–3: `9 – 12` phòng | Chương 4–6: `11 – 15` phòng.
   - Bắt buộc đảm bảo mỗi màn có tối thiểu: `1` *RM_Vertical_Shaft*, `1` *RM_Cavern_Mining*, `1` *RM_Cliff_FishingPier*, và `1` *RM_Vault_Secret*.
3. **Kiểm tra Va Chạm Không Gian (`Bounds Overlap Check`)**:
   - Trước khi đặt phòng mới vào `DoorwaySocket`, tính `BoxCollider / Bounds` (thu nhỏ biên `0.5m` để cho phép sát tường). Nếu đụng phòng đã đặt → thử xoay/chọn phòng khác trong pool; nếu không khớp → đánh dấu socket đó là **Dead-End**.
4. **Xử Lý Cổng Dư (`Dead-End Cap` Thông Minh)**:
   - Không bao giờ để cửa cụt vô nghĩa! Mọi `DoorwaySocket` chưa nối sẽ tự gắn 1 trong 3 nắp kết thúc:
     - *Ban Công Câu Cá Mây* (`FishingPier Cap`).
     - *Hốc Mạch Quặng* (`MiningAlcove Cap`).
     - *Lan Can Gỗ / Đá + Bình Cổ Vật* (`LootBalcony Cap`).
5. **Tạo Vòng Lặp Đi Tắt (`Shortcut Loop` Chống Chạy Ngược Xa)**:
   - Nếu hai phòng thuộc 2 nhánh khác nhau có cổng cách nhau `< 10m` và hướng đối mặt, tự động thả `1` **Cầu Nối Tắt / Dây Đu** nối thông nhau để người chơi vác đồ về Pod qua đường vòng nhanh hơn!
6. **Bake `NavMesh` Runtime (`NavMeshSurface.BuildNavMesh()`)**:
   - Ngay khi sinh xong toàn bộ phòng (mất `< 0.3s` với low-poly), chạy `NavMeshSurface` đồng bộ để quái đi bộ di chuyển mượt qua mọi mối nối phòng!

---

## PHẦN B: TÍNH NĂNG ĐÀO QUẶNG & CÂU CÁ SINH TỒN (LIGHT SURVIVAL)

> **Triết lý thiết kế**: Không có thanh Đói/Khát gây phiền phức. Đào Quặng và Câu Cá là **hoạt động sinh tồn tự do** giúp kiếm thêm tiền bù Quota khi vỡ đồ, mở khóa công thức Rèn Trang Bị tại Sky-Dock, và Nấu Món Ăn Buff chỉ số cho Run!

### 1. Chuyển Đổi Công Cụ Thông Minh (Không Rối Phím)
- Người chơi dùng **1 nút chính (`Left Mouse`) + Ngữ cảnh thông minh (`Smart Context Tool`)** — hoặc bấm phím `1 / 2 / 3` để chọn thủ công:
  - Bình thường: Cầm **Tool Chiến Đấu** (`Gậy Móc / Búa / Máy Ổn Định`).
  - Khi tâm ngắm hướng vào **Mạch Quặng** (`<= 2.6m`): Tự động chuyển tay sang **Cuốc Khai Khoáng** → nhấp/giữ `Left Mouse` để đào!
  - Khi đứng ở **Điểm Câu Cá** (bờ hồ hoặc ban công mây có biển báo phao): Bấm `E` hoặc `Left Mouse` để tung **Cần Câu**!

---

### 2. Hệ Thống Đào Quặng (`Ore Mining`)

#### Cơ Chế Đào Có Kỹ Năng (Kiểu *Deep Rock Galactic* + *Fortnite Weak-Spot*)
- Mỗi Mạch Quặng có `3 – 5` nấc HP (`MiningHits`).
- Khi đập nhát đầu tiên, trên bề mặt tảng đá hiện **Vòng Điểm Yếu Phát Sáng (`WeakSpotRing`)**:
  - Đập trúng vòng sáng → tính **Mining Crit (`2x` tốc độ vỡ + tiếng *choang* vang dội + văng thêm `1` mảnh quặng bonus)**.
- Khi mạch quặng vỡ, các cục quặng văng ra vật lý:
  - **Mảnh Quặng Nhỏ (`Small Ore Chunk`)**: Nhặt vào **`OrePouch` (Túi Nguyên Liệu Riêng, sức chứa 20 ô — KHÔNG chiếm tay vác Cổ Vật!)**. *Luật chống mất trắng:* Khi Extract thành công giữ `100%`; nếu chết giữa chặng vẫn giữ **`50%`** lượng nguyên liệu trong `OrePouch` (nâng lên **`75%`** qua Tech Tree, làm tròn xuống — chi tiết tại [File 14](./14_CHE_TAO_QUANG_CHAN_NUOI_VA_NUOI_CA.md)).
  - **Khối Quặng Khổng Lồ (`Giant Crystal Cluster`, `15%` tỷ lệ rơi)**: Nặng `12kg`, phải vác bằng 2 tay như cổ vật; nộp vào Pod nhận ngay **`$220 – $400` Pod Value** (hoặc ôm lên Pod mà không bấm nộp Quota để tách thành `4` viên quặng hiếm mang về Hub!).

#### 6 Loại Quặng Theo Chương & Công Dụng
| ID Quặng | Tên Quặng | Nơi Xuất Hiện | Giá Trị Nếu Nộp Pod (`$`) | Công Dụng Rèn Tại Sky-Dock (Bác Kael) |
| :--- | :--- | :--- | :---: | :--- |
| `ore_sky_copper` | Đồng Tầng Mây | Chương 1, 2 | `35 / viên` | Nâng cấp Cuốc & Cần Câu Cấp 1–2; rèn Đệm Nấm/Cọc Neo. |
| `ore_lumen_amber` | Hổ Phách Phát Quang | Chương 2, 5 | `55 / viên` | Rèn Đèn Mũ sáng xa hơn + Bình Hơi Phản Lực. |
| `ore_ancient_iron` | Sắt Cổ Đại | Chương 3, 6 | `65 / viên` | Rèn Giáp/Găng tăng HP & giảm sát thương rơi. |
| `ore_frost_quartz` | Thạch Anh Băng | Chương 4 | `80 / viên` | Rèn Keo Hàn Pha Lê miễn phí trước mỗi Run. |
| `ore_void_shard` | Tinh Thạch Hư Không | Chương 5, 6 | `120 / viên` | Rèn các món cuối của **Guild Tech Tree** & Skin VEX. |
| `ore_giant_prism` | Khối Quặng Khổng Lồ | Mọi Chương (hiếm) | `220–400` (nộp Pod) | Nếu mang được lên Pod thoát hiểm mà không tiêu, tách thành `4` viên quặng hiếm! |

---

### 3. Hệ Thống Câu Cá Mây & Hồ Dị Thường (`Sky-Sea & Biome Fishing`)

#### Nơi Câu Cá
1. **Tại `01_SkyDockHub` (Cầu Tàu Phía Nam)**: Câu thư giãn vô hạn thời gian để kiếm nguyên liệu nấu ăn trước khi vào trận hoặc sưu tầm đủ Bộ Sưu Tập Cá cho Bảo Tàng của Myla!
2. **Trong Màn Chơi (`RM_Cliff_FishingPier` & Hồ Biome)**: Nước/Mây ở mỗi Biome cho ra loài cá đặc trưng riêng; có độ rủi ro nhẹ vì **đồng hồ Collapse vẫn chạy** và cần câu tạo sóng nhỏ có thể thu hút quái ở gần!

#### Minigame Câu Cá Nhanh (`3 – 6 giây / lần`)
1. Quăng phao (`Left Mouse` / `E`).
2. Chờ `1.5 – 3.5 giây` đến khi phao chìm mạnh + dấu `!` vàng + âm thanh *tùm!* (tránh bị gián đoạn giữa chừng bởi quái).
3. Giữ/nhấp `Left Mouse` để giữ **con trỏ Cá nằm trong vùng Thanh Xanh (`Sweet Spot`)** trong `2.5 giây` là kéo lên! (Có nút hủy nhanh bằng `Right Mouse` hoặc `Space` nếu thấy quái lao tới!).

#### Bảng Loài Cá & Vật Phẩm Câu Trúng (12 Loài + Sự Kiện Vui)
- **Tỷ lệ câu tại Điểm Câu Trong Màn**: `68%` Cá Biome (trong đó `75%` là **Cá Nấu Ăn / Cá Khô** vào `OrePouch` và `25%` là **Cá Giống Sống (`Breeder Fish`)** để thả vào **Bể Nuôi Cá Mây** tại Hub!) | `18%` **Rương/Cổ Vật Trôi Dạt** (cứu nguy cực tốt khi thiếu Quota!) | `8%` Giày Rách / Bánh Răng Gỉ (`$15`) | `6%` **Câu Nhầm Quái (`Pebble Imp` ướt sũng hoặc `Baby Mimic` nhảy thẳng lên bờ!)**.
- **Luật Chống Exploit Nuôi Cá**: Cá sinh sản trong `Sky-Aquarium` chỉ được dùng để **nấu Meal Buff**, **thu hoạch `Vảy Cá Phát Sáng`**, hoặc **hoàn thiện Bảo Tàng Myla** — **KHÔNG được mang xuống màn chơi để nộp vào Pod Quota**! (Chi tiết xem [File 14](./14_CHE_TAO_QUANG_CHAN_NUOI_VA_NUOI_CA.md)).
- **12 Loài Cá Theo Biome**:
  1. *Cá Cánh Chuồn* & *Cá Bông Mây* (Sky-Dock & Chương 1).
  2. *Cá Đèn Lồng Nấm* & *Lươn Bào Tử* (Chương 2).
  3. *Cá Vảy Đồng* & *Tôm Cơ Khí* (Chương 3).
  4. *Cá Tuyết Pha Lê* & *Bạch Tuộc Băng* (Chương 4).
  5. *Cá Bùn Trọng Lực* & *Cá Mắt Tím* (Chương 5).
  6. *Cá Vàng Thánh Điện* & *Cá Rồng Hư Không* (Chương 6 — cực hiếm!).

---

### 4. Rèn Trang Bị, Chế Tạo Đa Dạng & Bếp Nấu Ăn Sinh Tồn (Tại `01_SkyDockHub` & `02_AirshipShop`)

#### A. Bếp Lò Của Pip (`Airship Galley`) — Nấu Cá Lấy Buff Chặng Sau
Trước khi nhảy xuống chặng tiếp theo (tối đa mang `1` **Meal Buff** hiệu lực suốt cả màn đó):

| Món Ăn | Nguyên Liệu Cá | Hiệu Ứng Buff Suốt Màn |
| :--- | :--- | :--- |
| **Cháo Cá Mây Nóng** | `2` Cá Chương 1 / Hub | `+25 Max HP` & hồi `30 HP` ngay lập tức. |
| **Súp Nấm Dạ Quang** | `1` Cá Chương 2 + `1` Cá bất kỳ | `+15%` Tốc độ chạy & `+20%` tốc độ hồi Stamina. |
| **Cá Nướng Giòn Cơ Khí** | `1` Cá Chương 3 + `1` *Đồng Tầng Mây* | Tăng `+1` lượt dùng **Tiện Ích Địa Hình (`Utility_Slot`)**. |
| **Gỏi Cá Băng Chống Trơn** | `1` Cá Chương 4 | Miễn nhiễm `70%` hiệu ứng trượt băng & giảm `35%` sát thương vỡ cổ vật. |
| **Lẩu Bùn Trọng Lực** | `1` Cá Chương 5 | Nhảy cao hơn `+15%` & giảm `50%` sát thương rơi/rớt vực. |
| **Tiệc Cá Rồng Hoàng Kim** | `1` Cá Chương 6 | Khởi đầu màn với `+1` **Thẻ Upgrade Ngẫu Nhiên** ngay lúc đáp xuống đảo! |

#### B. Lò Rèn Bác Kael (`Sky-Dock Forge`), Chăn Nuôi & Nuôi Cá (`File 14`)
- **Công Cụ Sinh Tồn**:
  - **Cuốc Khai Khoáng (`Cấp 1 → 3`)**: Tăng sát thương đào & xác suất rơi *Khối Quặng Khổng Lồ* (`15% → 30%`).
  - **Cần Câu Mây (`Cấp 1 → 3`)**: Tăng độ rộng thanh `Sweet Spot`, giảm thời gian chờ cá cắn câu (`-35%`), tăng tỷ lệ câu trúng *Cổ Vật Trôi Dạt* và *Cá Giống (`Breeder Fish`)*.
  - **Túi Nguyên Liệu Lớn (`OrePouch +10 Slot`)**: Nâng sức chứa quặng & cá mang theo trong mỗi Run (từ `20` lên `30` ô).
- **Chế Tạo Nâng Cao, Trang Trại Mây (`Sky-Ranch`) & Bể Cá Mây (`Sky-Aquarium`)**: Xem đầy đủ **18 công thức luyện hợp kim / Gadget / trang bị vĩnh viễn**, **4 loài vật nuôi & Pet**, **hệ thống nuôi cá sinh sản theo Run-Tick**, và **4 luật chống lỗi logic** tại [14_CHE_TAO_QUANG_CHAN_NUOI_VA_NUOI_CA.md](./14_CHE_TAO_QUANG_CHAN_NUOI_VA_NUOI_CA.md).

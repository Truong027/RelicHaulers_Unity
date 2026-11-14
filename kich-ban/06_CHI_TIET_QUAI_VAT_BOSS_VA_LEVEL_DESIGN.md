# 06. Chi Tiết Quái Vật, Boss, Cạm Bẫy & Thiết Kế Màn Chơi

## 1. Thông Số Nhân Vật Cơ Bản (Player Baseline)

- **Max HP**: `100` (Scout `90`, Brawler `130`, Tinker `85`).
- **Tốc độ di chuyển & Stamina**:
  - Walk `4.5 m/s` | Sprint `7.2 m/s` (**Sprint không tốn Stamina**).
  - Carry Penalty: `speed *= 1 - clamp((mass / 40f) * carrySpeedPenaltyMultiplier, 0, 0.55f)` (với `Scout = 1.3f`, `Brawler = 0.0f`, `Tinker = 0.85f`).
  - Stamina tối đa `100`, tự hồi `22/s` sau `0.6s` không tiêu hao (`Dash = 25`, `Shoulder Bash = 15`, `Sky Glide = 10/s`).
- **Nhảy & Vật lý**:
  - `RideHeight`: `0.35m` | `JumpHeight`: `2.2m` | `CoyoteTime`: `0.12s` | `JumpBuffer`: `0.15s`.
  - `Dash` (`Left Shift` nhấp / `V`): Tốc độ `15 m/s` trong `0.22s`, i-frame `0.16s`, Cooldown `1.4s`.
  - **Sát thương rơi**: Rơi ở vận tốc dọc `> 16 m/s` chịu `(v - 16) * 4` dmg và bật **Ragdoll `0.8s`**.
  - **Rớt Vực (Void Fall)**: Rơi dưới `Y < -25m` mất `30` HP (hoặc dùng Safety Harness), respawn tại điểm đứng vững gần nhất (`LastSafeGroundPos`) sau `1.0s`; cổ vật rơi vực sẽ respawn tại `SafeRespawnAnchor` (điểm đặt trên đất liền gần nhất, không phải mép vực) với `-25%` độ bền.

---

## 2. State Machine & Thông Số Quái Vật (Enemies & Boss)

Các quái đi bộ sử dụng `NavMeshAgent` khi điều hướng bình thường và **tạm tắt `NavMeshAgent` chuyển sang `Rigidbody` + `Ragdoll`** khi nhận lực `ApplyKnockback(impulse, triggerRagdoll=true)`. Riêng **Gust Wisp** sử dụng **3D Hover Steering** (raycast giữ cao độ + né vật cản) để bay tự do qua vực thẳm mà không phụ thuộc NavMesh mặt đất.

### 1. Pebble Imp (Tiểu Yêu Đá)
- **Vai trò**: Quái bầy đàn gây rối, chuyên húc làm người chơi loạng choạng hoặc rớt cổ vật.
- **Thông số**: `HP = 45` | `Speed = 5.5 m/s` | `Tầm nhìn = 14m` | `Kháng đẩy = Thấp (0.7x)`.
- **State Machine**:
  - `Patrol`: Đi lang thang quanh cụm cổ vật.
  - `Chase`: Chạy theo người chơi gần nhất trong tầm nhìn.
  - `WindupCharge (0.45s)`: Co người, nhấp nháy cam trước khi lao húc.
  - `DashBite (0.3s)`: Lao tới gây `12` dmg + lực hất `6 m/s` (làm rơi cổ vật nếu không có Mag-Gloves stack 2).
  - `RagdollStun (1.2s)`: Bị bật ngửa khi trúng Dash hoặc cổ vật ném trúng; **trong trạng thái này người chơi có thể bấm `E` nhặt nó lên ném như vật nặng `8kg`!**

### 2. Echo Stalker (Kẻ Rình Rập Âm Thanh)
- **Vai trò**: Trừng phạt người chơi chạy ẩu hoặc mang *Chuông Gió* gây tiếng ồn lớn.
- **Thông số**: `HP = 95` | `Patrol Speed = 3.2 m/s` | `Hunt Speed = 8.4 m/s` | `Tầm nghe = 22m` | `Kháng đẩy = Trung bình (1.0x)`.
- **State Machine**:
  - `IdleListen`: Tai vểnh lên, xoay đầu về nguồn tiếng ồn gần nhất (`NoiseSystem`).
  - `Investigate`: Tiến nhanh tới vị trí phát ra `NoisePulse`.
  - `Pounce (0.5s windup)`: Nhảy vồ tầm `5m`, gây `22` dmg + đẩy ngã nhẹ.

### 3. Spring Mimic (Rương Bật Lò Xo)
- **Vai trò**: Bẫy bất ngờ giả dạng Rương Cổ Vật (có dấu hiệu nhận biết nhỏ: nắp rương khẽ thở phập phồng mỗi `3s`).
- **Thông số**: `HP = 140` | `Speed = 4.8 m/s` | `Tầm kích hoạt = 2.4m` | `Kháng đẩy = Cao (1.3x)`.
- **State Machine**:
  - `Disguised`: Nằm yên như rương loot.
  - `AmbushPop`: Khi người chơi chạm vào, bật lò xo hất tung lên cao (`Y + 9 m/s`), gây `20` dmg + Ragdoll `1.0s`.
  - `HopChase`: Nhảy tưng tưng đuổi theo người chơi; khi chết rơi ra **Lõi Rương Bật (`$450`)**.

### 4. Gust Wisp (Tinh Linh Gió — Từ Chương 1 Màn 1-2 trở đi)
- **Vai trò**: Quái bay tầm trung dùng **3D Hover Steering**, bắn đạn gió đẩy lệch hướng nhảy ở mép vực.
- **Thông số**: `HP = 65` | `Hover Speed = 4.2 m/s` | `Tầm bắn = 12m`.
- **Hành vi**: Lơ lửng cách mặt đất `2.5m`, nạp `0.9s` rồi bắn quả cầu gió gây `10` dmg + lực đẩy ngang `10 m/s`.

### 5. MINI-BOSS: Clockwork Crusher (Chương 3 — Màn 3-2)
- **Kích thước**: Cao `2.5m` | `HP = 550` | `Speed = 4.6 m/s` | `Kháng đẩy = Cao (miễn nhiễm Ragdoll trừ khi dính Piston/Lõi Ném)`.
- **Hành vi**:
  - *Whirlwind Hammer (`2.2s`)*: Xoay búa 360° truy đuổi chậm, đẩy văng người chơi & cổ vật (`16` dmg + Ragdoll).
  - *Overheat Pause (`1.8s`)*: Sau khi xoay xong bị quá nhiệt lộ khe hở sau lưng, nhận `2x` sát thương.
  - Khi chết rơi ra **Lõi Lò Phản Ứng (`$350`)**.

### 6. BOSS: Golem Warden (Chương 6 — Màn 6-2 Shattered Spire Citadel)
- **Kích thước**: Cao `3.2m` | `HP = 1,200` | `Kháng đẩy = Miễn nhiễm Ragdoll trừ khi Stagger`.
- **Cơ chế điểm yếu & Vương Miện Hư Không**:
  - Sau lưng Golem có **Lõi Pha Lê Lộ Thiên (`CoreHitbox`)** nhận `2.5x` sát thương và tích thanh **Stagger (`0 → 100`)**.
  - **Vương Miện Hư Không (`$1,200`)** đặt trên **Ngai Bảo Hộ** ngay cạnh đấu trường, được bọc **Khiên Năng Lượng**. Khiên này chỉ tắt khi **Boss vào Phase 3 (`< 30% HP`) HOẶC khi Boss bị Stagger lần thứ 2** (hoặc khi Boss bị hạ hẳn) → tạo 2 cách chơi: đánh hạ Boss để lấy an toàn, **HOẶC cướp nóng Vương Miện giữa lúc Boss nổi điên chạy về Pod!**
- **3 Phase Chiến Đấu**:
  - **Phase 1 (`100% – 65% HP`)**:
    - *Ground Slam*: Đập 2 tay xuống đất tạo vòng sóng xung kích (`r = 5m`, nhảy lên để né!).
    - *Sweeping Arm*: Quét ngang tầm gần (`18` dmg + hất văng Ragdoll).
  - **Phase 2 (`65% – 30% HP`)**:
    - Thêm chiêu *Boulder Toss*: Nhổ tảng đá ném theo đường cong về phía người chơi (`25` dmg).
    - Triệu hồi `2 Pebble Imp` mỗi `25s`.
  - **Phase 3 (`< 30% HP` hoặc khi Khiên Vương Miện bị phá & người chơi nhặt Vương Miện)**:
    - *Laser Sweep / Shock Charge*: Lao húc thẳng (`0.8s` báo hiệu vệt đỏ trên sàn), nếu đâm trúng cột đá sẽ tự bị choáng `3.0s`!
  - **Trạng thái Stagger (`3.5s`)**: Quỳ xuống, lõi sau lưng mở to; đánh hoặc ném vật nặng vào lõi gây sát thương cực lớn. Hạ Boss rơi thêm `2` Cổ Vật Tier 2 (`$260–$350`).

---

## 3. Bẫy & Tương Tác Môi Trường Theo 6 Biome

*(Thông số map chi tiết từng màn xem tại [08_BAN_DO_VA_MAN_CHOI_MO_RONG.md](./08_BAN_DO_VA_MAN_CHOI_MO_RONG.md))*

| Biome | Yếu Tố Môi Trường | Thông Số Vật Lý & Gameplay |
| :--- | :--- | :--- |
| **Chương 1: Đồi Cối Xay** | **Luồng Gió & Cầu Dây Treo** | Gió thổi lên `Y + 11 m/s` (theo chu kỳ `6s/2s` ở Màn 1-2); cầu ván nối `HingeJoint` lắc theo trọng lượng. |
| **Chương 2: Rừng Nấm** | **Mũ Nấm Đàn Hồi & Nhánh Mục** | Nấm bật vật lý như bạt nhún (hỗ trợ ném **Trick Shot**); nhánh mục sụp sau `2.5s` đứng lên. |
| **Chương 3: Xưởng Đúc** | **Bánh Răng, Băng Chuyền & Jump Pad** | Bánh răng xoay `25°/s` (thừa kế vận tốc tiếp tuyến); băng chuyền `3.5 m/s`; Jump Pad phóng `18 m/s`; hố năng lượng `-25 HP`. |
| **Chương 4: Vịnh Băng** | **Sàn Băng Trơn & Băng Mỏng** | `friction = 0.04` trượt theo quán tính; băng mỏng nứt nhanh gấp đôi khi tổng tải `Player + Relic > 85kg`. |
| **Chương 5: Đầm Lầy** | **Vũng Bùn & Cổng Dịch Chuyển** | Vũng bùn giảm `45%` tốc chạy & hút chìm đồ sau `10s`; cổng dịch chuyển truyền vật lý nguyên vận tốc; trọng lực `0.45g / 1.6g`. |
| **Chương 6: Thánh Điện** | **Sàn Pha Lê Nứt & Cầu Ánh Sáng** | Sàn pha lê rung `1.8s` rồi rơi (hồi sau `6.0s`); cầu ánh sáng bật/tắt theo nhịp `5s`. |

---

## 4. Cấu Trúc Bản Đồ Phòng Nối Phòng & Wave Director

### Bố Cục Phòng Nối Phòng Đa Dạng (`RoomGraphGenerator`)
*(Chi tiết 10 loại phòng & thuật toán nối phòng xem tại [13_HE_THONG_SINH_TON_CAU_CA_DAO_QUANG_VA_MAP_PHONG_NOI_PHONG.md](./13_HE_THONG_SINH_TON_CAU_CA_DAO_QUANG_VA_MAP_PHONG_NOI_PHONG.md))*
- **Phòng Gốc (`RM_StartPod`)**: Nơi đặt **Extraction Pod**, kết nối `2 – 3` nhánh phòng mở rộng theo chiều ngang lẫn chiều thẳng đứng (`RM_Vertical_Shaft`).
- **Phân tầng rủi ro theo độ sâu nhánh (`Depth 1 → 5`)**:
  - *Nhánh Gần (`Depth 1–2`)*: Đường dốc & cầu nối dễ đi, chứa Cổ Vật Tier 1 (`$80–$180`) + *Ban Công Câu Cá (`RM_Cliff_FishingPier`)*.
  - *Nhánh Trung Bình (`Depth 2–4`)*: Tháp đứng / Phòng bẫy cơ khí / Hang động Mạch Quặng (`RM_Cavern_Mining`), chứa Cổ Vật Tier 2 (`$260–$350`) + Rương Upgrade.
  - *Nhánh Sâu (`Depth 4–5`)*: Kho báu kín `RM_Vault_Secret` hoặc Đấu trường Boss (`RM_Boss_Arena`), chứa Cổ Vật Tier 3 (`$450–$1,200`) + *Shortcut Loop* quay nhanh về Pod.
- **Navigation**: Bake `NavMeshSurface` runtime ngay sau khi ghép phòng xong (`< 0.3s`), kết hợp **3D Hover Steering** cho *Gust Wisp*.

### Luật Điều Phối Quái (Enemy Wave Director)
- **Pha Thu Thập (`Collapse Timer > 60s`)**: Giới hạn tối đa `5–8` quái hoạt động cùng lúc; hồi quái mỗi `25s` ở các điểm xa người chơi (`>22m`).
- **Pha Khẩn Cấp (`Collapse Timer <= 60s`)**: Bầu trời chuyển đỏ cam, nhạc dồn dập; cứ mỗi `15s` xuất hiện thêm 1 tốp quái săn đuổi và rìa ngoài đảo bắt đầu rung nứt.
- **Pha Sụp Đổ (`Collapse Timer == 0s`)**: Không thua ngay lập tức! Một **Bão Hư Không** dâng lên và các mảng sàn ngoài rìa vỡ dần vào trong mỗi `10s`; nếu người chơi kịp kích hoạt Launch Pod trong lúc hỗn loạn vẫn tính thắng Run!

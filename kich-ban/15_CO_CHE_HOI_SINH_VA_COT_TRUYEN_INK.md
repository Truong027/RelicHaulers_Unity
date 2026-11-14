# 15. Cơ Chế Hồi Sinh Nhân Vật & Kịch Bản Phân Nhánh `inkle/ink` (3 Hồi – 3 Kết Thúc)

> Tài liệu khóa chặt **logic Hồi Sinh / Tái Sinh** (Solo & Co-op, chống loop chết/bug vật lý/lạm dụng kinh tế) và **hệ thống cốt truyện phân nhánh chuẩn `inkle/ink`** chạy trực tiếp trong Unity (`ink-unity-integration`) kèm bộ skill nội bộ tại [.agents/skills/narrative-designer/](../.agents/skills/narrative-designer/SKILL.md).

---

## 1. Hệ Thống Hồi Sinh Nhân Vật (`PlayerReviveSystem.cs`)

### A. Cơ Chế Hồi Sinh Chế Độ Solo (`Emergency Tether` + `gad_revive_capsule`)
Thay vì `HP = 0` là thua ngay tức khắc, người chơi Solo có cơ hội sống lại có trả giá rõ ràng (tối đa **Hard Cap = `2` lần/chặng**):

| Nguồn Hồi Sinh | Cách Sở Hữu | Giá Trả Khi Kích Hoạt | Trạng Thái Sau Khi Hồi Sinh |
| :--- | :--- | :--- | :--- |
| **1. Tia Tái Sinh Pod (`Second Wind Tether`)** | Mở sẵn `1` lần/chặng sau khi mở nút đầu tiên trên **Guild Tech Tree (*Kén Tái Sinh*)** (hoặc mặc định `1` lần từ Run 2). | Trừ **`-15 giây`** trên *Collapse Timer* (Pod trích năng lượng kéo thân thể về). | Đứng dậy tại `LastSafeGroundPosition` với **`50%` Max HP**, **`100%` Stamina**, `3.0s` Bất tử (`Revive I-Frame`) + sóng đẩy `4m` hất văng *Pebble Imp*. |
| **2. Viên Nang Tái Sinh (`gad_revive_capsule`)** | Rèn tại Lò Kael (`1 mat_lumina_alloy + 1 ore_void_shard + 2 Vảy Cá Sáng`) hoặc mua ở Shop Pip (`400 ¢`), chiếm **1 ô Gadget** (tối đa `1` cái/Run). | Tiêu hao viên nang + chỉ trừ **`-5 giây`** *Collapse Timer*. | Tương tự trên nhưng hồi **`75%` Max HP** + làm choáng quái trong bán kính `5m`. |

### B. Cơ Chế Hồi Sinh Chế Độ Co-op (1–4 Người Chơi)
1. **Pha 1 — Ngã Gục (`Downed Beacon`, `20 giây`)**:
   - Khi `HP = 0`, nhân vật chuyển sang tư thế quỳ gục (bật cột sáng đỏ + phát sóng cầu cứu khi bấm `T`), có thể bò chậm (`1.5 m/s`).
   - Đồng đội đứng cạnh giữ **`E` trong `2.5s`** để kéo dậy với `40% HP` (miễn phí, không trừ đồng hồ).
2. **Pha 2 — Thẻ Sinh Mệnh (`ITEM_DogTag`)**:
   - Nếu hết `20s` chưa được kéo (hoặc bị đánh ngắt), nhân vật chuyển sang **Spectator Drone** bay theo đồng đội và để lại **Thẻ Sinh Mệnh (`3kg`, không thể vỡ)** tại `LastSafeGroundPosition`.
   - Đồng đội nhặt Thẻ ném vào tia hút **Extraction Pod** → hồi sinh người chết ngay cạnh Pod với `50% HP`!
   - Nếu đồng đội thoát hiểm qua màn mà chưa kịp ném Thẻ, người chết vẫn **tự động hồi sinh miễn phí tại `02_AirshipShop`** đầu chặng tiếp theo với `50% HP`.
3. **Total Party Wipe**: Chỉ khi **tất cả người chơi** đều ngã gục cùng lúc và không còn `Tether / gad_revive_capsule` khả dụng mới chuyển sang `Contract Failed`.

### C. 6 Luật Vàng Chặn Lỗi Logic Khi Hồi Sinh (Anti-Bug Checklist)
1. **Cấm hồi sinh khi thế giới đã sập (`CollapseTimer <= 0s` hoặc `LeftBehindByPod`)**:
   - Nếu đồng hồ sụp đảo đã về `0` hoặc Pod đã bay đi bỏ bạn lại, **TUYỆT ĐỐI KHÔNG** kích hoạt Tether/Viên nang (bảo toàn viên nang chưa dùng và chuyển thẳng sang `Contract Failed`), tránh vòng lặp hồi sinh rồi chết ngay trên đảo đã biến mất!
2. **Tọa độ hồi sinh an toàn (`LastSafeGroundPosition`)**:
   - Cứ `0.5s` khi `IsGrounded && !OnThinIce && !OnLavaOrTar && !InVoid`, lưu lại `LastSafeGroundPosition`.
   - Nếu chết do rơi vực (`Y < -25m`), hố dung nham Chương 3, hoặc kẹt dưới bánh răng *Crusher*, luôn dịch chuyển về `LastSafeGroundPosition` (fallback: `PodSafeZone`), tuyệt đối không hồi sinh ngay tại điểm chết.
3. **Bảo toàn Cổ Vật đang cầm (Không mất, Không nhân đôi)**:
   - Nếu chết trên đất liền lúc đang cầm Cổ Vật: thả nhẹ Cổ Vật xuống cạnh nhân vật (miễn sát thương va chạm `0.5s` lúc buông tay để không tự vỡ oan).
   - Nếu chết do rơi vực lúc đang cầm Cổ Vật: áp dụng luật dịch chuyển cổ vật lên bờ trừ `25%` HP ở [File 07](./07_LUAT_CHI_TIET_UI_AM_THANH_VA_ASSET_MANIFEST.md).
4. **Reset Ragdoll & Vận Tốc Sạch Sẽ**:
   - Trước khi trả quyền điều khiển sau hồi sinh, bắt buộc đặt `rb.linearVelocity = Vector3.zero`, tắt `RagdollMode`, khôi phục xương từ `Bone Snapshot` rồi bật `FloatingCapsuleController` kèm `3.0s` I-Frame.
5. **Chặn Lạm Dụng Hồi Sinh Để Farm Kinh Tế**:
   - Hồi sinh **KHÔNG** nạp lại `Utility_Slot` (`C`), **KHÔNG** reset Gadget đã tiêu, **KHÔNG** tính là chết để nhận *First-Time Death Bonus (`+30 GT`)* — phần thưởng đó chỉ mở khi thật sự thua cả Run (`Contract Failed`).
6. **Gắn Kết Với Cốt Truyện (`VEX Tether Lore`)**:
   - Mỗi lần dùng `Tether Hồi Sinh` sẽ tăng biến `stat_revives_this_run` và `stat_revives_total` sang hệ thống Ink: VEX giải thích tia cứu hộ dùng chung năng lượng **Lõi Neo Trời**, lý giải vì sao dùng Tether làm giảm `15s` tuổi thọ đảo và dần khơi lại ký ức của Đội Tiên Phong!

---

## 2. Kiến Trúc Kịch Bản Phân Nhánh `inkle/ink` (3 Hồi – 3 Kết Thúc)

### A. Phân Vai Giữa `DialogueDirector` Và `InkNarrativeManager`
Để không làm khựng nhịp hành động khi đang chạy nhảy né quái:
- **`DialogueDirector.cs` (Real-time Barks — Không chặn điều khiển)**: Phát Toast Radio ngắn (`3.5s`) của VEX lúc nhặt đồ, làm vỡ bình, Trick Shot, hoặc tiếng hô `T`.
- **`InkNarrativeManager.cs` (`inkle/ink-unity-integration` — Kịch bản phân nhánh chính)**:
  - File nguồn chuẩn: **[ink/relic_haulers_story.ink](./ink/relic_haulers_story.ink)** (kiểm tra dead-end trực tiếp trên **Inky Editor**).
  - Quản lý toàn bộ **Cốt truyện 3 Hồi**, **Hội thoại có lựa chọn tại Hub/Shop** (Kael, Myla, Pip), **12 Mảnh Nhật Ký Bí Mật**, **Phản ứng hậu Hồi Sinh**, và **Cảnh Kết Thúc 3 Nhánh**.

### B. Hệ Thống 3 Kết Thúc Chính Thức (`Endings Matrix`)
Tại cuối **Chương 6 — Màn 6-2 (*Đỉnh Ngai Golem*)**, sau khi chạm vào hoặc thu hồi **Vương Miện Hư Không**, kịch bản Ink rẽ vào `act3_climax_sanctum` dựa trên trạng thái thu thập & lựa chọn của người chơi:

| Mã Ending (`trigger_ending`) | Tên Kết Thúc | Điều Kiện Mở Khóa Trong `.ink` | Hệ Quả Cốt Truyện & Phần Thưởng |
| :--- | :--- | :--- | :--- |
| `END_GUILD_CONTRACT` | **Khế Ước Hội Hauler** *(Mặc định)* | Chọn giao nộp Vương Miện theo lệnh Hội (hoặc thu `< 8` Nhật Ký). | Hoàn thành hợp đồng, nhận trọn GT & mở Khóa Mũ Giáp Golem; VEX nhắc nhẹ Hội đang rục rịch kích hoạt Cỗ Máy Cũ (gợi ý chơi lại tìm đủ 12 Nhật Ký). |
| `END_BREAK_CORE` | **Phá Vỡ Lõi Neo** *(Nhánh Kael & Myla)* | Thu `>= 6` Nhật Ký **HOẶC** đã hứa với Kael/Myla (`flag_trusted_kael || flag_myla_preserve_pact`), chọn ngắt mạch máy thu hồi tại Pod để trả năng lượng lại cho đảo. | Các quần đảo ngừng sụp đổ nhanh; Kael và Myla tách khỏi sự kiểm soát của Hội, mở rộng Bảo Tàng lịch sử thật tại Sky-Dock. |
| `END_TRUE_VEX_SKY` | **Trái Tim Trên Bầu Trời Mới** *(True Ending)* | Thu đủ **`12/12` Mảnh Nhật Ký** (`stat_logs_collected >= 12`) và chọn giải phóng ý thức Thuyền Trưởng cho VEX (có biến thể thoại riêng nếu cả Run **`0` lần Hồi Sinh**). | Phá giải bí mật Đại Phân Tách, giải thoát VEX, mở khóa **Skin Bí Mật `VEX-Prototype`** và chế độ **Rift Hazard `1–5`**. |

### C. Cầu Nối Ink ↔ Unity C# (`EXTERNAL` & Variables)
- **Biến đồng bộ vào Ink trước mỗi đoạn thoại**:
  - `stat_act` (`1..3`), `stat_logs_collected` (`0..12`), `stat_revives_this_run`, `stat_revives_total`, `stat_broken_relics_last_run`, `stat_museum_count`, `stat_guild_tokens`, `state_last_run_outcome`, `state_archetype`, `state_next_biome`.
- **Hàm `EXTERNAL` Ink gọi thẳng vào C#**:
  - `give_item(item_id, amount)`: Thưởng GT, mảnh quặng hoặc gadget từ đối thoại/nhật ký.
  - `unlock_log(log_id)`: Mở khóa mục đọc lại trong Bảo Tàng Myla.
  - `adjust_haggle(delta_percent)`: Áp dụng giảm giá `-20%` hoặc phạt `+10%` ngay lên quầy Pip ở `02_AirshipShop`.
  - `trigger_ending(ending_id)`: Kích hoạt Cutscene/Màn hình kết thúc tương ứng (`END_GUILD_CONTRACT` / `END_BREAK_CORE` / `END_TRUE_VEX_SKY`) và lưu vào `SaveData.unlockedEndings`.

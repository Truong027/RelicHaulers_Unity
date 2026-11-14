# 03. Hệ Thống Màn Chơi, Tiến Trình, Cốt Truyện & UI/UX

## 1. Vòng Lặp Một Lượt Chơi (Complete Run Loop — Standard 5 Màn Rẽ Nhánh)

```
[1. Sky-Dock Hub] (Chọn Archetype, Đổi Mũ/Áo/Ba lô Bone Retargeting, Chọn Chế Độ / Hợp Đồng)
        │
        ▼
[2. Nhảy Xuống Đảo Nổi — 5 Chặng Rẽ Nhánh]
   ├── Chặng 1: Biome 1 (Màn 1-1 hoặc 1-2)
   ├── Chặng 2: Rẽ nhánh chọn Biome 2 (Rừng Nấm) HOẶC Biome 3 (Xưởng Đúc)
   ├── Chặng 3: Rẽ nhánh chọn Biome 4 (Vịnh Băng) HOẶC Biome 5 (Đầm Lầy Trọng Lực)
   ├── Chặng 4: Biome 6-1 (Bậc Thang Hoàng Hôn)
   └── Chặng 5: Biome 6-2 (Đỉnh Ngai Golem — Boss Finale)
        │  (Giữa các chặng ghé Airship Shop mua vật phẩm & sửa đồ)
        ▼
[3. Kích Hoạt Pod Rút Quân (Extraction)]
   ├── Đạt 100% Quota: Chọn Launch Pod (nút Xanh) hoặc Overcharge Pod (nút Đỏ)
   ├── Đếm ngược 15 giây: Sóng quái dồn về Pod; trong 3 giây cuối vẫn có thể ném nốt đồ vào tia hút
   └── Khi hết 15 giây: Người chơi phải đứng trong Safe Zone của Pod (r = 3.5m) hoặc bám trên thân Pod mới tính thoát hiểm thành công!
        │
        ▼
[4. Trạm Nghỉ Giữa Chặng (Airship Shop)]
   ├── Hệ thống 3 loại tiền tách biệt rõ ràng:
   │    • Pod Value ($): Tiền cổ vật tính vào Quota trong màn hiện tại.
   │    • Surplus Credits (¢): 100% phần $ vượt Quota -> tiêu tại Airship Shop giữa chặng.
   │    • Guild Tokens (GT): Tiền Meta vĩnh viễn = 10% tổng $ đã nộp + thưởng qua màn (thua giữ 50% GT).
   └── Mua vật phẩm / Perk và chọn nhánh Biome tiếp theo
        │
        ▼
[5. Kết Thúc Run (Victory / Contract Failed)]
   └── Tổng kết điểm, Danh Hiệu Vui, Highlight Snapshot, mở khóa Mũ/Giáp mới & Endless Sky-Abyss ở Sky-Dock, lưu JSON
```

---

## 2. Kịch Bản Tiến Trình, Thế Giới & Nhiệm Vụ Tự Chọn

### A. Ba Hồi Cốt Truyện Ẩn (Mở Khóa Qua 6 Chương & 12 Nhật Ký Bí Mật)
Thế giới *Sky-Shards* từng là **Liên Minh Thành Phố Mây Cổ Đại**, vận hành bởi mạng lưới **Lõi Từ Trường Neo Trời (Sky-Anchors)** do các Kiến Trúc Sư tạo ra. Sự kiện *Đại Phân Tách* xảy ra khi Hội Đồng Cổ Xưa cố rút quá nhiều năng lượng từ Vực Thẳm, làm các Lõi nứt vỡ và biến sinh vật trên đảo thành quái đá bảo hộ.
- **Hồi 1 — Bầu Trời Vỡ (Chương 1 & 2)**: Người chơi vào vai tân binh Hội Hauler, làm quen với công việc vớt cổ vật kiếm sống cùng lời động viên nửa đùa nửa thật của **VEX**, **Bác Kael** và **Giáo Sư Myla**. Bắt đầu tìm thấy dấu hiệu sinh vật & cây cối bị biến đổi bởi năng lượng lõi.
- **Hồi 2 — Lời Nói Dối Của Hội (Chương 3 & 4)**: Khi tiến sâu vào *Xưởng Đúc Clockwork* và *Vịnh Băng*, các mảnh nhật ký hé lộ Hội Hauler thực chất không chỉ "bảo tồn cổ vật" — họ đang thu gom đủ mảnh Lõi để tái khởi động một cỗ máy cổ đại chưa rõ mục đích, và **Golem Warden** thực ra được lập trình để *ngăn không cho ai rút lõi cuối cùng*!
- **Hồi 3 — Trái Tim Của VEX (Chương 5 & 6)**: Ở *Đầm Lầy Trọng Lực* và *Thánh Điện Mây Vỡ*, **VEX** thừa nhận mình là bản sao ý thức của Thuyền trưởng Đội Tiên Phong năm xưa — người đã chọn ở lại trong mạng lưới radio (và **Emergency Tether**) để dẫn đường cho thế hệ Hauler mới. Cuối Màn 6-2 rẽ nhánh bằng `inkle/ink` thành **3 Kết Thúc Chính Thức** (`END_GUILD_CONTRACT`, `END_BREAK_CORE`, `END_TRUE_VEX_SKY`) — xem chi tiết tại [15_CO_CHE_HOI_SINH_VA_COT_TRUYEN_INK.md](./15_CO_CHE_HOI_SINH_VA_COT_TRUYEN_INK.md) và [ink/relic_haulers_story.ink](./ink/relic_haulers_story.ink).

### B. Nhiệm Vụ Phụ Tự Chọn Trong Màn (Optional Side Contracts — Chống Lặp Lại)
Mỗi màn bốc ngẫu nhiên **1 Nhiệm Vụ Phụ** (không bắt buộc — hoàn thành thưởng thêm **`+50 GT`** và **`+$250` Pod Value**; không làm vẫn qua màn bình thường để không gò bó!):
1. **Giải Cứu Bot Trinh Sát**: Tìm 1 chú robot nhỏ hỏng bị mắc kẹt ở nhánh xa trên đảo và vác/ném nó về Pod như 1 cổ vật (`10kg`).
2. **Vận Chuyển Hoàn Mỹ**: Đưa ít nhất `1` Cổ Vật Tier 2+ vào Pod với **100% HP nguyên vẹn** (không một vết trầy).
3. **Nghệ Nhân Cú Ném**: Thực hiện ít nhất `1` cú **Trick Shot** hoặc hạ `2` quái bằng cách ném vật trúng đầu.
4. **Dọn Đường Cổ Đại**: Kích hoạt `1` **Đài Nguyện Rift Shrine** hoặc phá `1` **Bức Tường Bí Mật** trên đảo.

---

## 3. Luật Cân Bằng & Nhịp Độ (Pacing Tổng Quan 6 Chương)

*(Bảng Quota chi tiết từng màn 1-1 đến 6-2 xem tại [08_BAN_DO_VA_MAN_CHOI_MO_RONG.md](./08_BAN_DO_VA_MAN_CHOI_MO_RONG.md))*

| Chương / Biome | Thời Gian Collapse | Khoảng Quota | Số Cổ Vật Spawn | Áp Lực Quái |
| :--- | :---: | :---: | :---: | :--- |
| **Chương 1 (Windmill Meadow)** | `4m00s – 4m15s` | `$450 – $700` | `8 – 10 món` | Thấp (Pebble Imp, Echo Stalker) |
| **Chương 2 (Bioluminescent Canopy)** | `4m30s` | `$950 – $1,250` | `10 – 12 món` | Trung bình (+ Spring Mimic, Gust Wisp) |
| **Chương 3 (Clockwork Foundry)** | `4m45s – 5m00s` | `$1,550 – $1,900` | `11 – 13 món` | Trung bình-Cao (+ Mini-Boss Crusher) |
| **Chương 4 (Frostglass Glacier)** | `5m00s – 5m15s` | `$2,300 – $2,700` | `12 – 14 món` | Cao (Sàn băng trơn + Băng mỏng) |
| **Chương 5 (Abyssal Rift Swamp)** | `5m15s – 5m30s` | `$3,200 – $3,700` | `12 – 15 món` | Cao (Vũng bùn + Trọng lực biến đổi) |
| **Chương 6 (Shattered Citadel)** | `5m30s – 6m00s` | `$4,300 – $5,000` | `14+ món` + Boss | Rất cao (Golem Warden 3 Phase) |

- **Luật Rút Quân Sớm / Tham Lam (Risk-Reward)**:
  - Ngay khi đạt đủ `100% Quota`, nút **Rút Quân (Launch Pod)** trên Pod bật sáng xanh.
  - Người chơi có thể thoát ngay cho an toàn, **HOẶC** ở lại vớt thêm cổ vật vượt Quota để kiếm tiền mua Perk xịn ở Airship Shop trước khi *Collapse Timer* về 0!

---

## 4. Giao Diện (UI/UX) & Phản Hồi Cảm Giác (Game Feel)

### HUD Trong Trận (Tối Giản, Dễ Đọc)
- **Góc trên trái**: Thanh Máu (HP), Thể Lực Dash (Stamina), và dải icon các Perk đang sở hữu (kèm số lượng stack x2, x3...).
- **Góc trên giữa**: Thanh **Quota Pod** (`$350 / $450`) + **Đồng Hồ Đảo Sụp** (`02:45` chuyển đỏ & rung khi dưới 30s).
- **World-Space UI (Trên màn hình 3D)**:
  - Nhãn nổi trên Cổ Vật khi nhìn vào: Tên + Giá trị `$` + Thanh độ bền màu xanh/đỏ.
  - Mũi tên chỉ hướng về **Extraction Pod** ở viền màn hình khi đang vác cổ vật nặng.
  - Vòng cung dự đoán quỹ đạo khi giữ nút Ném.

### Âm Thanh & Game Feel (Juice)
- **Hitstop nhẹ (`0.04s`)** + **Camera Shake (Cinemachine Impulse)** khi đập quái văng ragdoll hoặc khi cổ vật va đập mạnh.
- **Pitch thay đổi theo trạng thái**: Âm thanh bước chân nặng hơn khi vác đồ `>12kg`; tiếng kính rạn nứt khi *Bình Pha Lê* mất máu; tiếng còi Pod vang lên khi đủ Quota.

# 05. Danh Mục Đầy Đủ: Relics, Upgrades, Shop & Trang Bị

> Tài liệu thông số dữ liệu gốc (Data Catalog) để AI Agent tạo trực tiếp các `ScriptableObject` (`RelicDataSO`, `UpgradeDataSO`, `ShopItemSO`, `EquipmentSO`) mà không cần tự bịa số liệu.

---

## 1. Danh Mục 14 Cổ Vật (Relics)

### Hạng Phổ Thông (Common — Tier 1)
| ID | Tên Cổ Vật | Giá Trị (`$`) | Khối Lượng (`kg`) | HP Độ Bền | Ngưỡng Va Chạm (`m/s`) | Hiệu Ứng Vật Lý & Cơ Chế Riêng |
| :--- | :--- | :---: | :---: | :---: | :---: | :--- |
| `rel_clay_jar` | Bình Gốm Cổ | `80` | `3` | `80` | `4.5` | Rơi vỡ dễ mất giá; ném trúng quái gây `20` dmg + vỡ vụn tạo mảnh văng nhẹ. |
| `rel_brass_gear` | Bánh Răng Đồng | `110` | `10` | `250` | `8.0` | Cứng cáp, lăn trên dốc rất nhanh nếu đặt xuống; ném gây `45` dmg + stun `1.0s`. |
| `rel_stone_idol` | Tượng Đá Nhỏ | `140` | `14` | `320` | `9.0` | Nặng tay, giảm `20%` tốc chạy; làm bệ đứng tạm cao `0.6m` để leo gờ cao (khóa nhặt khi đang đứng trực tiếp lên tượng để chống bug bay). |
| `rel_crystal_vase` | Bình Pha Lê Cổ | `180` | `2` | `100` | `3.5` | Giá trị cao nhưng cực kỳ giòn; va chạm `>3.5 m/s` mất HP rất nhanh. |
| `rel_wind_bell` | Chuông Gió Rỗng | `160` | `7` | `150` | `5.0` | Rung lắc tạo `NoisePulse` bán kính `14m` mỗi `1.2s` khi chạy/nhảy, hút *Echo Stalker*. |

### Hạng Hiếm (Rare — Tier 2)
| ID | Tên Cổ Vật | Giá Trị (`$`) | Khối Lượng (`kg`) | HP Độ Bền | Ngưỡng Va Chạm (`m/s`) | Hiệu Ứng Vật Lý & Cơ Chế Riêng |
| :--- | :--- | :---: | :---: | :---: | :---: | :--- |
| `rel_magnet_cube` | Khối Nam Châm Cổ | `260` | `15` | `380` | `9.5` | Tự hút mảnh vỡ/xu kim loại trong `6m`; dính vào *Bánh Răng Đồng* nếu đặt sát (vác 1 lần 2 món). |
| `rel_spring_orb` | Quả Cầu Lò Xo | `280` | `5` | `220` | `12.0` | Nảy tưng tưng mạnh (`bounciness = 0.85`) khi thả hoặc ném; tự giảm nảy và dừng hẳn khi tốc độ `< 1.2 m/s` để không trôi mất vô tận. |
| `rel_reactor_core` | Lõi Lò Phản Ứng | `350` | `18` | `300` | `7.0` | Giảm `35%` tốc chạy; phát sáng mạnh + tỏa nhiệt đẩy lùi *Pebble Imp* mỗi `4s` nhưng thu hút quái từ xa (đặt cạnh *Lăng Kính Băng* để làm nguội `20s`). |
| `rel_frost_prism` | Lăng Kính Băng | `320` | `6` | `140` | `4.5` | Làm chậm quái chạm vào người vác `30%`; trơn trượt khi đặt trên đất (`friction = 0.05`); ném vào hố dung nham đóng băng lối đi `15s`. |
| `rel_horn_echo` | Tù Và Khí Lưu | `300` | `9` | `240` | `7.5` | Bấm `Left Mouse` khi đang cầm để thổi luồng gió đẩy văng quái phía trước `8m` (cooldown `6s`), đồng thời tạo tiếng ồn `20m`. |

### Hạng Huyền Thoại / Đặc Biệt (Epic & Boss — Tier 3)
| ID | Tên Cổ Vật | Giá Trị (`$`) | Khối Lượng (`kg`) | HP Độ Bền | Ngưỡng Va Chạm (`m/s`) | Hiệu Ứng Vật Lý & Cơ Chế Riêng |
| :--- | :--- | :---: | :---: | :---: | :---: | :--- |
| `rel_gravity_anchor` | Mỏ Neo Trọng Lực | `520` | `24` | `500` | `12.0` | Tạo vùng trọng lực thấp `0.6x` trong bán kính `4m` xung quanh (nhảy cao hơn khi đứng gần!). |
| `rel_chrono_hourglass` | Đồng Hồ Cát Thời Gian | `580` | `4` | `120` | `4.0` | Khi còn nguyên vẹn trong tay, làm chậm tốc độ đếm ngược *Collapse Timer* đi `25%` trong tối đa `45s` tích lũy (cộng dồn với `Cuộn Dây Thời Gian` không vượt quá trần giảm tốc `45%`). |
| `rel_mimic_chest_core` | Lõi Rương Bật | `450` | `12` | `350` | `8.5` | Chỉ rớt ra khi tiêu diệt *Spring Mimic*; phát ánh sáng vàng dễ thấy. |
| `rel_void_crown` | Vương Miện Hư Không | `1200` | `20` | `600` | `10.0` | **Siêu Cổ Vật Boss Chương 6 (Màn 6-2)**. Khi nhặt lên kích hoạt đợt quái tổng lực; đưa vào Pod lập tức mở khóa `Launch Pod` (bất kể đã đủ `$5,000` hay chưa) và nhân đôi thưởng `Guild Tokens` của Run. |

> **Công thức tính Giá Trị Thực Tế khi đưa vào Pod**:
> `ActualValue = Mathf.RoundToInt(baseValue * Mathf.Clamp(currentHP / maxHP, 0.25f, 1.0f))`
> Nếu `currentHP <= 0`, cổ vật vỡ thành **Mảnh Vụn Cổ Vật (`Scrap Shard`)** giá trị cố định `15% baseValue` (`mass = 1kg`, không thể vỡ thêm).

---

## 2. Danh Mục 16 Thẻ Nâng Cấp Roguelite (Upgrades)

Người chơi nhận **1 lần Draft (chọn 1 trong 3 thẻ ngẫu nhiên)** từ các nguồn giới hạn để cân bằng kinh tế:
- Mốc Pod trong màn: **`50% Quota`**, **`100% Quota`**, và tối đa thêm **`1` lần tại `150% Quota`** (tránh spam Draft vô hạn khi Overcharge).
- Mở *Rương Nâng Cấp (Upgrade Chest)* ẩn trên đảo hoặc đổi đồ với *Thương Nhân Sky Peddler*.

| ID | Tên Upgrade | Độ Hiếm | Max Stack | Công Thức Cộng Dồn (`n` = số stack) |
| :--- | :--- | :---: | :---: | :--- |
| `upg_spring_boots` | Giày Lò Xo | Common | `5` | `+1` lần nhảy trên không mỗi stack; tiếp đất tạo sóng choáng nhẹ bán kính `1.8 + 0.4n` m. |
| `upg_feather_pack` | Balo Giảm Trọng | Common | `5` | Giảm phạt tốc độ do khối lượng cổ vật đi `1 - 0.72^n` (tối đa ~80% ở 5 stack). |
| `upg_mag_gloves` | Găng Từ Trường | Common | `4` | `+0.8m` tầm nhặt đồ & `+20%` lực ném mỗi stack; từ stack 2 trở đi không làm rơi đồ khi bị húc nhẹ. |
| `upg_bubble_wrap` | Bọc Khí Pha Lê | Common | `3` | Cổ vật đang cầm có lớp khiên chặn `1` lần va đập mạnh mỗi `18 / (1 + 0.35*(n-1))` giây. |
| `upg_turbo_dash` | Lõi Dash Phản Lực | Common | `5` | Giảm hồi chiêu Dash `15%` theo hàm mũ (`0.85^n`) và tăng lực đẩy va chạm khi Dash `+30%` mỗi stack. |
| `upg_vital_flask` | Bình Sinh Lực Cổ | Common | `0` (∞) | `+25` Max HP và hồi ngay `30` HP khi chọn. |
| `upg_shock_core` | Lõi Xung Kích | Rare | `4` | Khi Dash tạo vụ nổ bán kính `3.5 + 0.8n` m, gây `25n` dmg + bật **Ragdoll** quái nhỏ/vừa. |
| `upg_orbit_shards` | Tinh Thể Quỹ Đạo | Rare | `5` | Triệu hồi `n + 1` mảnh tinh thể xoay quanh người chơi, gây `18 + 8n` dmg/chạm + đẩy lùi nhẹ. |
| `upg_heavy_throw` | Cánh Tay Máy Ném | Rare | `4` | Cổ vật ném ra bay nhanh hơn `+25%` và gây `2x + 0.5n` sát thương va chạm lên quái. |
| `upg_silent_step` | Đế Giày Êm | Rare | `3` | Giảm `35%` bán kính tiếng ồn chạy & tiếng ồn của *Chuông Gió* mỗi stack (`0.65^n`). |
| `upg_pod_beacon` | Đèn Hiệu Pod | Rare | `3` | Mở rộng vùng hút tự động của Extraction Pod thêm `+1.5m` mỗi stack và tăng `+10%` giá trị cổ vật nộp vào. |
| `upg_ragdoll_bounce` | Giáp Cao Su Nảy | Rare | `3` | Giảm `35%` sát thương va chạm/rơi; khi đứng dậy từ Ragdoll hất văng quái trong `3m`. |
| `upg_time_coil` | Cuộn Dây Thời Gian | Epic | `2` | Giảm `18%` tốc độ chạy của *Collapse Timer* mỗi stack; tăng thêm `0.5s` thời gian Stagger của Boss. |
| `upg_chain_spark` | Tia Sét Truyền Dẫn | Epic | `4` | Mỗi đòn đánh / ném trúng quái phóng tia sét sang `n + 1` mục tiêu gần đó, gây `30n` dmg. |
| `upg_double_extract` | Máy Nhân Bản Mini | Epic | `2` | Có `18% * n` xác suất nhân đôi giá trị tiền của một Cổ Vật ngay lúc đưa vào Pod. |
| `upg_sky_glide` | Cánh Lướt Mây | Epic | `1` | Giữ phím Nhảy trên không để bung dù lướt (giảm tốc rơi còn `2.5 m/s`), cực mạnh khi vượt vực thẳm. |

---

## 3. Vật Phẩm Airship Shop (Giữa Các Chặng 1→2, 2→3, 3→4, 4→5)

Tiền sử dụng trong Shop là **Surplus Credits (`¢`)**: `100%` phần `$` vượt Quota ở chặng vừa hoàn thành (cộng dồn nếu chưa tiêu hết).

| Tên Vật Phẩm | Giá (`¢`) | Tác Dụng |
| :--- | :---: | :--- |
| **Túi Y Tế Khẩn (Medkit)** | `150` | Hồi `60%` HP tối đa ngay lập tức (không dùng để hồi sinh khi `HP = 0`). |
| **Keo Hàn Pha Lê (Repair Spray)** | `120` | Cấp `+1` заряд (`gad_repair_spray`, cộng dồn nếu đang trang bị, hoặc lắp vào 1 trong tối đa **2 ô Gadget** `Q`/`4`/`5` nếu còn trống — xem [File 14](./14_CHE_TAO_QUANG_CHAN_NUOI_VA_NUOI_CA.md)): sửa `+50%` độ bền cổ vật đang cầm. |
| **Viên Nang Tái Sinh (`gad_revive_capsule`)** | `400` | Chiếm `1` ô Gadget (`tối đa 1/Run`, xem [File 15](./15_CO_CHE_HOI_SINH_VA_COT_TRUYEN_INK.md)): tự động hồi sinh với `75%` HP khi ngã xuống (chỉ trừ `5s` Collapse Timer; không kích hoạt nếu đảo đã sụp `00:00`). |
| **Thẻ Nâng Cấp Ngẫu Nhiên (Perk Crate)** | `240` | Mở ngay 1 bảng Draft 3 thẻ Upgrade (đảm bảo tối thiểu 1 thẻ Rare+). |
| **Xúc Xắc Đổi Thẻ (Reroll Token)** | `90` | Cộng thêm `+2` lượt Reroll bảng Upgrade miễn phí (`rerollCharges` cộng dồn đến hết Run). |
| **Pháo Sáng Đánh Dấu (Flare Pack)** | `100` | Bắn sáng vị trí Cổ Vật hiếm xuyên tường trong `15s` (3 lần dùng/chặng). |
| **Bảo Hiểm Rớt Vực (Safety Harness)** | `220` | Miễn trừ 1 lần mất máu & tự dịch chuyển cổ vật đang cầm lên bờ an toàn nếu rớt vực. |

---

## 4. Trang Bị Ngoại Hình & Bone Retargeting (Sky-Dock Hub)

Các món trang bị dùng chung chuẩn xương Humanoid (`EquipmentRetargeter`), mở khóa bằng **Guild Tokens (`GT`)** tích lũy sau mỗi Run:

- **Head (`Head_Slot`)**:
  1. `Mũ Thợ Mỏ Mặc Định` (Mở sẵn — Đèn pin nhẹ phía trước).
  2. `Nón Phi Công Mây` (`100 Tokens` — Kính bảo hộ).
  3. `Mũ Phù Thủy Pha Lê` (`250 Tokens` — Đỉnh mũ nhọn lắc theo vật lý).
  4. `Mũ Giáp Golem` (`500 Tokens` — Mở khóa khi thắng Chương 6 Màn 6-2 lần đầu).
- **Backpack (`Backpack_Slot`)**:
  1. `Ba Lô Dây Thừng` (Mở sẵn).
  2. `Bình Hơi Phản Lực` (`150 Tokens` — Xì khói khi Dash).
  3. `Lồng Tinh Thể Sáng` (`350 Tokens`).
- **Gloves (`Gloves_Slot`)**:
  1. `Găng Da Hauler` (Mở sẵn).
  2. `Găng Cao Su Nảy` (`120 Tokens`).
  3. `Găng Từ Trường Vàng` (`300 Tokens`).
- **Tool (`Tool_Slot`)**:
  1. `Gậy Móc Thu Hồi` (Mở sẵn — Đánh đẩy + kéo nhẹ vật nhỏ).
  2. `Búa Xung Kích` (Mở khóa cùng Archetype Brawler).
  3. `Máy Ổn Định Từ` (Mở khóa cùng Archetype Tinker).

# 10. Cấu Trúc Hình Ảnh & Art Bible (Visual Direction, URP Lighting, UI & Shader Spec)

> Tài liệu chuẩn hóa toàn bộ **Ngôn Ngữ Hình Ảnh (Visual Language)** để `blender-mcp-ai`, `blender-3d-asset-generation` và Unity URP tạo ra hình ảnh đồng nhất, dễ đọc gameplay ở tốc độ cao và chuẩn stylized low-poly 3D.

---

## 1. Triết Lý Hình Ảnh & Độ Dễ Đọc (Visual Readability First)

1. **Phong cách chủ đạo**: **Chunky Stylized Low-Poly + Soft Cel/Toon Shading** (gần với *Risk of Rain 2*, *PEAK*, *Very Very Valet*):
   - Bề mặt khối lớn dùng **vát cạnh nhẹ (`Bevel 1 segment`)** thay vì cạnh sắc 90°, giúp bắt sáng viền đẹp dưới ánh sáng URP.
   - Không dùng texture ảnh chụp (photorealistic); toàn bộ bề mặt dùng **Flat Palette + Gradient Ramp + Edge Highlight** để nhân vật và cổ vật luôn tách bạch khỏi nền đất.
2. **Quy Tắc Tách Lớp Màu (Value & Saturation Separation)**:
   - **Môi trường / Sàn đi được**: Độ bão hòa thấp–trung bình (`Saturation 25–45%`), sáng vừa phải.
   - **Nhân vật & Quái**: Độ bão hòa cao (`Saturation 65–85%`), có **Rim Light** nhẹ để không chìm vào bóng tối.
   - **Cổ Vật & Bẫy Tương Tác**: Có màu tương phản hoặc **Vật liệu Phát Sáng (Emission)** + **Outline / Fresnel** theo quy ước màu cố định bên dưới.

---

## 2. Hệ Thống Mã Màu Tương Tác (Color-Coded Gameplay Language)

Người chơi phải nhìn 0.2 giây là hiểu vật trước mặt làm gì mà không cần đọc chữ:

| Nhóm Đối Tượng | Màu Chủ Đạo / Viền Phát Sáng | Dấu Hiệu Thị Giác Bổ Sung |
| :--- | :--- | :--- |
| **Cổ Vật Thường / Bền** | Vàng Ấm (`#FFD166`) | Hạt lấp lánh nhẹ bay lên chậm (`1–2 hạt/s`). |
| **Cổ Vật Giòn / Dễ Vỡ** | Xanh Ngọc Sáng (`#4CC9F0`) | Bề mặt bán trong suốt nhẹ; hiện **vết nứt đen phát sáng** qua shader `SG_CrystalCrack` khi mất HP. |
| **Siêu Cổ Vật / Boss Loot** | Tím Hoàng Kim (`#C77DFF` + `#FFD166`) | Cột sáng mảnh bốc thẳng lên trời (`height = 12m`). |
| **Extraction Pod (An Toàn / Đủ Quota)** | Xanh Lá Neon (`#06D6A0`) | Đèn Pod chuyển từ Vàng (`<100%`) → Xanh Lá (`>=100%`) → Đỏ Chớp (`Overcharge`). |
| **Cảnh Báo Nguy Hiểm / Đòn Đánh / Sàn Nứt** | Đỏ Cam (`#FF4D4D`) | Vòng tròn/vệt đỏ trên sàn trước khi đòn đánh hoặc sét giáng xuống; sàn nứt rung + phát khe sáng đỏ. |
| **Bàn Bật / Gió Đẩy / Dịch Chuyển** | Xanh Cyan (`#00F5D4`) | Vòng mũi tên hướng lên hoặc vòng xoáy hạt bay theo chiều lực đẩy. |

---

## 3. Bảng Màu & Thiết Lập Ánh Sáng URP Theo 6 Biome + Hub

Dùng làm thông số trực tiếp cho `Directional Light`, `Environment Fog`, `Skybox Gradient` và `Palette_Master.png`:

| Scene / Biome | Màu Nền Chính (Hex) | Màu Nhấn (Hex) | Fog & Skybox Mood | URP Post-Processing |
| :--- | :--- | :--- | :--- | :--- |
| **Sky-Dock & Shop** | Gỗ `#8D5B4C`, Thép `#4A5568` | Đồng `#E9A84C`, Xanh `#38BDF8` | Nắng chiều ấm (`#FFE5B4`), mây trắng trôi chậm | Bloom nhẹ `0.4`, ACES Tonemapping |
| **Ch.1: Đồi Cối Xay** | Cỏ `#52B788`, Đá `#8D99AE` | Gỗ `#BC6C25`, Vàng `#FFB703` | Trời xanh trong (`#72EFDD → #4EA8DE`), Fog xa `85m` | Độ tương phản tươi sáng, Saturation `+10` |
| **Ch.2: Rừng Nấm** | Vỏ cây `#3D2C4D`, Rêu `#2D6A4F` | Nấm `#00F5D4`, Tím `#9D4EDD` | Sương tím xanh dày (`45m`), đom đóm phát sáng | Bloom `0.85` cho mũ nấm dạ quang |
| **Ch.3: Xưởng Đúc** | Sắt `#343A40`, Đồng `#B5651D` | Dung nham `#FF6B00`, Lân tinh `#38B000` | Khói công nghiệp xám cam (`65m`), ánh hắt từ hố năng lượng | Bloom `0.9`, Vignette nhẹ `0.18` |
| **Ch.4: Vịnh Băng** | Băng `#A2D2FF`, Tuyết `#EDF6F9` | Pha lê `#48CAE4`, Cực quang `#B5179E` | Cực quang xanh-tím chuyển động trên bầu trời | Specular sáng cao trên băng, Bloom `0.6` |
| **Ch.5: Đầm Lầy** | Đá sẫm `#2B2D42`, Bùn `#1B1926` | Axit `#80ED99`, Hư không `#7209B7` | Sương mù nặng (`40m`), hạt năng lượng trôi ngược lên | Chromatic Aberration nhẹ `0.12` ở vùng trọng lực |
| **Ch.6: Thánh Điện** | Đá cổ `#3C2A4D`, Cẩm thạch `#E0AAFF` | Vàng kim `#FFD166`, Đỏ vỡ `#FF4D6D` | Hoàng hôn tím vàng kịch tính; **chuyển Đỏ Sẫm khi Collapse <=60s** | Bloom `1.0`, Shake + Vignette đỏ khi sụp đổ |

---

## 4. Cấu Trúc Hình Học Nhân Vật, Camera & UI Wireframe

### A. Tỷ Lệ Nhân Vật & Rig Chuẩn (`CHAR_Hauler_Base`)
- **Chiều cao tổng**: `1.60m` (Pivot đặt tại lòng bàn chân `Y = 0`).
- **Tỷ lệ stylized**: Đầu `0.40m` (dễ nhìn biểu cảm mũ/kính từ camera góc thứ 3), Thân `0.55m`, Chân `0.65m` (đủ dài để **Foot IK** bước trên bậc thang/dốc nhìn rõ ràng).
- **Khoảng cách Camera (Cinemachine Third-Person Follow)**:
  - `FOV = 60°`, `Camera Distance = 5.8m` (kéo ra `6.8m` khi vác cổ vật lớn hoặc đang ở Boss Arena), `Shoulder Offset = (0.45, 1.35, 0)`.
  - Bắt buộc gắn layer mask `EnvironmentOccluding` cho tường/cây lớn để kích hoạt **`SG_OcclusionFade`** (làm mờ bán kính `1.2m` quanh đường tia từ Camera → ngực nhân vật).

### B. Bố Cục HUD Màn Hình (16:9 Clean Layout)
```text
┌──────────────────────────────────────────────────────────────────────┐
│ [HP: █████░ 85/100]      [QUOTA: $350 / $450]       [VEX RADIO TOAST]│
│ [STAMINA: ██████░]       [COLLAPSE:  02:45 ⏱]       "Cẩn thận bình!" │
│ [Perks: 🥾x2 ⚡x1 🧲x1]  (Chuyển ĐỎ RUNG < 30s)                       │
│                                                                      │
│                          (Khu vực chơi 3D thoáng)                    │
│                        [World Tooltip trên Cổ Vật]                   │
│                          Bình Pha Lê — $180                          │
│                          HP: ██████░░ 75%                            │
│                                                                      │
│ [Q: Keo Hàn x1]                               [E: Nhặt | RMB: Ném]   │
└──────────────────────────────────────────────────────────────────────┘
```

### C. Cấu Trúc Thư Mục Art & Texture Trong Unity (`Assets/_Project/Art/`)
```text
Art/
├── Textures/
│   ├── PAL_Master_Stylized.png      # Texture palette 256x256 dùng chung 80% prop/char
│   ├── TEX_CrystalCrack_Mask.png    # Mask vết nứt 3 cấp (100% -> 60% -> 30% HP)
│   ├── TEX_Noise_Dissolve.png       # Perlin/Voronoi dùng cho OcclusionFade & Teleport
│   └── UI_Icons/                    # Icon 128x128: 16 Upgrades, 6 Synergies, 3 Archetypes, 5 Awards
├── Materials/
│   ├── MAT_Hauler_Palette.mat       # SG_StylizedToonLit
│   ├── MAT_Enemy_Palette.mat
│   ├── MAT_Relic_Fragile.mat        # SG_CrystalCrack
│   ├── MAT_Env_Occluder.mat         # SG_OcclusionFade
│   └── MAT_Env_IceSlip.mat          # SG_IceSlip (tương phản specular cao)
└── Shaders/
    ├── SG_StylizedToonLit.shadergraph
    ├── SG_OcclusionFade.shadergraph
    ├── SG_CrystalCrack.shadergraph
    └── SG_ForceFieldAndPortal.shadergraph
```

---

## 5. Quy Trình Kiểm Tra Hình Ảnh Trước Khi Đưa Vào Game (Visual QA Gate)

Mỗi model/scene tạo từ `blender-mcp-ai` hoặc import từ KayKit/Quaternius phải vượt qua 5 bước kiểm tra sau (dùng `scripts/audit_blender_asset.py` & `Roboticela/Animator`):
1. **Scale & Pivot**: Scale `(1,1,1)`, Rotation `(0,0,0)`, nhân vật đứng đúng `1.6m` ở `Y=0`.
2. **Silhouette Test**: Chụp render 3 góc (`Front`, `3/4`, `Top-Down`) kiểm tra ở kích thước thu nhỏ `128px` vẫn phân biệt được ngay loại quái/cổ vật.
3. **Normal & Backface**: Không lộn mặt (flipped normals), bật Smooth Shading theo góc (`Auto Smooth 35°`) hoặc Sharp Edges cho phong cách low-poly sắc nét.
4. **UV & Material Count**: Mỗi prefab tối đa `1–2` Materials (dùng `PAL_Master_Stylized.png` để gộp Draw Call).
5. **Rig & IK Check**: Test clip `CarryWalk` & `Ragdoll` trong `Roboticela/Animator` hoặc Unity Preview trước khi gắn vào Scene.

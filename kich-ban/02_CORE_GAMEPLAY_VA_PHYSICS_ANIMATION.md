# 02. Core Gameplay, Physics Controller & Animation Pipeline

## 1. Hệ Thống Điều Khiển Nhân Vật (Stylized Physics Controller)

Dựa trên kiến trúc **Floating Capsule Physics** (`joebinns/stylised-character-controller`) kết hợp `unity-physics-anim`:

### A. Floating Capsule & Di Chuyển
- **Collider**: Capsule được nâng lơ lửng cách mặt đất một khoảng `RideHeight` (`0.35m`) bằng lực lò xo giảm chấn (*Spring-Damper Raycast* bắn xuống từ tâm Capsule).
  - *Lợi ích*: Nhân vật bước qua bậc thang, gờ đá, địa hình gồ ghề mượt mà mà không bị khựng collider.
- **Lực di chuyển (`FixedUpdate`)**:
  - Dùng `Rigidbody` với `interpolation = RigidbodyInterpolation.Interpolate`, `collisionDetectionMode = CollisionDetectionMode.Continuous`.
  - Tăng tốc/giảm tốc theo đường cong `Acceleration` và `MaxAccelForce` để có cảm giác linh hoạt ngay cả khi vác nặng.
  - Khóa xoay X/Z bằng lực giữ thẳng đứng (*Upright Spring Torque*) khi chưa ở trạng thái Ragdoll.

### B. Nhảy, Coyote Time, Jump Buffer, Sprint & Dash
- **Coyote Time**: `0.12s` sau khi rời mép đảo vẫn cho phép nhảy.
- **Jump Buffer**: `0.15s` bấm nhảy trước khi chạm đất vẫn tự động kích hoạt khi tiếp đất.
- **Sprint Tự Do & Dash (`Left Shift`)**:
  - **Chạy Nhanh (Giữ `Left Shift`)**: **Không tốn Stamina**, giúp di chuyển và khám phá đảo rộng thoải mái không bị ức chế.
  - **Dash (Nhấp `Left Shift` hoặc `V`/`Mouse4`)**: Lướt nhanh `15 m/s` trong `0.22s` (tốn `25/100` Stamina, i-frame `0.16s`), tạo lực đẩy va chạm hất ngã quái nhỏ (*Pebble Imp*). Stamina tự hồi `22/s` sau `0.6s` không dùng.

### C. Squash & Stretch + VFX Phản Hồi
- **Tuyệt đối không scale Transform chứa `Collider`/`Rigidbody`**.
- Tạo một child node `VisualRoot` chứa mesh & rig nhân vật:
  - Khi **Nhảy lên**: kéo dãn trục Y `(0.85, 1.25, 0.85)` rồi đàn hồi về `(1, 1, 1)` theo `AnimationCurve`.
  - Khi **Tiếp đất**: nén trục Y `(1.25, 0.72, 1.25)` tỷ lệ thuận với vận tốc rơi `impactVelocity`, kèm **hiệu ứng bụi vòng tròn** (`LandingDustVFX`) và âm thanh *thump*.
  - Khi **Chạy nhanh**: phát hạt bụi nhỏ dưới gót chân (`FootstepDustVFX`).

---

## 2. Kiến Trúc Animation Hybrid (Keyframe + Procedural + IK + Ragdoll)

Dựa trên mẫu `fierc3/UnityCharacterAnimations` + `tools-unity-animation` + `unity-physics-anim`:

### A. Layer 0 — Base Locomotion (Mecanim / Blend Tree)
- **2D Freeform Directional Blend Tree** (hoặc 1D Speed + Turn):
  - Tham số Animator (cache bằng `Animator.StringToHash`): `Speed` (`0..1`), `IsGrounded` (`bool`), `VerticalVelocity` (`float`), `IsCarrying` (`bool`), `CarryWeightFactor` (`0..1`).
- Clip chuẩn Mixamo / KayKit / Quaternius: `Idle`, `Walk`, `Run`, `JumpStart`, `FallLoop`, `Land`, `Dash`, `CarryWalk`.

### B. Layer 1 — Upper Body Override (Avatar Mask)
- Dùng **Avatar Mask** cho nửa thân trên (`Spine`, `Chest`, `LeftArm`, `RightArm`, `Head`) khi nhân vật:
  - Vung vũ khí / đánh đẩy quái.
  - Giơ tay chuẩn bị ném cổ vật.

### C. Unity Animation Rigging (Procedural IK)
Đặt `RigBuilder` + 3 Rig Constraints chạy sau Animator pass:
1. **Foot IK (Two-Bone IK Constraint)**:
   - Raycast từ gót trái/phải xuống mặt đất → điều chỉnh vị trí & góc bàn chân bám sát dốc, bậc cầu thang và giữ dáng đứng tự nhiên trên địa hình lồi lõm.
2. **Hand Carry IK (Two-Bone IK + Multi-Parent Constraint)**:
   - Mỗi Cổ Vật có 2 điểm neo `LeftGrabAnchor` và `RightGrabAnchor`.
   - Khi nhặt đồ, weight của Hand IK nội suy mượt (`0 → 1` trong `0.15s`) để hai bàn tay bám chính xác vào thân cổ vật dù cổ vật to hay nhỏ.
3. **Head Look-At IK (Multi-Aim Constraint)**:
   - Đầu nhân vật tự xoay nhẹ hướng về Cổ Vật gần nhất (gợi ý cho người chơi thấy đồ loot) hoặc hướng về kẻ địch đang áp sát.

### D. Ragdoll Physics (Bật/Tắt Mượt Khi Bị Đánh Văng)
- Khi nhân vật trúng đòn hất tung từ *Golem Warden*, *Spring Mimic*, hoặc rơi từ quá cao:
  1. Tắt `Animator` & `CharacterController/FloatingCapsule`, bật `isKinematic = false` trên các xương Ragdoll.
  2. Chờ đến khi vận tốc Ragdoll ổn định (`< 0.3 m/s` trong `0.6s`).
  3. **Bone Snapshot Blend** (`0.25s`): Lưu pose hiện tại của xương Ragdoll rồi nội suy mượt sang animation `GetUp_FaceUp` hoặc `GetUp_FaceDown` tùy hướng ngực (`hips.forward.y`), sau đó bật lại điều khiển.

---

## 3. Cơ Chế Nhặt, Vác, Ném & Tự Do Vật Lý (Sandbox Freedom)

1. **Nhặt / Đặt Nhẹ / Tương Tác (`E`) — Hệ Thống Ưu Tiên Thông Minh**:
   - Khi bấm `E`, hệ thống kiểm tra theo thứ tự ưu tiên để **không bao giờ lỡ tay đánh rơi cổ vật**:
     1) Nếu tâm ngắm đang chỉ vào `Interactable` (*Nút Pod, Sky Peddler, Rift Shrine, Upgrade Chest, NPC*) → **tương tác trực tiếp** (giữ nguyên cổ vật đang cầm trên tay, có thể giao nộp/hiến tế/trao đổi thẳng!).
     2) Nếu tay trống & gần vật `IGrabbable` (`<=2.2m`) → **Nhặt** (không cho nhặt nếu nhân vật đang đứng trực tiếp lên chính vật đó `IsGroundedOn(item)` để tránh bug bay vô hạn).
     3) Nếu đang cầm vật → Hiện khung **Ghost Preview** trong suốt trước mặt và **Đặt nhẹ** xuống sàn không gây sát thương va đập.
   - Cổ vật gắn vào `CarrySocket` phía trước ngực bằng `SpringJoint` / `ConfigurableJoint` để vật **đung đưa vật lý thật** khi chạy hoặc xoay gấp.
   - **Nhặt Quái Đang Ragdoll**: Nếu một con *Pebble Imp* đang bị bật Ragdoll (`RagdollStun`), người chơi có thể bấm `E` nhặt nó lên và ném như một vật nặng `8kg` vào đàn quái hoặc dùng nó **kích hoạt bẫy *Spring Mimic*** từ xa!
2. **Đánh Đẩy / Skill Cổ Vật / Húc Vai (`Left Mouse`)**:
   - Tay trống: Vung vũ khí/tool riêng của Archetype để đánh đẩy quái hoặc phá các **mảng tường đá nứt bí mật** chứa kho báu.
   - Đang cầm Cổ Vật có *Active Ability* (VD: *Tù Và Khí Lưu*): Kích hoạt kỹ năng gió đẩy văng của chính Cổ Vật đó!
   - Đang cầm Cổ Vật thường: Thực hiện đòn **Húc Vai (Shoulder Bash, `15 Stamina`)** đẩy nhẹ *Pebble Imp* ra xa mà không cần thả đồ.
3. **Gồng Ném, Hủy Ném & Trick Shot (`Right Mouse`)**:
   - Giữ `Right Mouse` để gồng lực (hiển thị đường cong quỹ đạo Parabol dự đoán).
   - **Hủy Ném An Toàn**: Trong lúc đang giữ `Right Mouse` ngắm ném, bấm **`Left Mouse`** (hoặc `E`) để **hủy ném** mà không làm rơi cổ vật!
   - Thả `Right Mouse` để ném cổ vật vào **Khoang Thu Hồi (Pod)** hoặc ném thẳng vào quái vật.
   - **Trick Shot Bonus (`+25% $`)**: Nếu cổ vật được ném bật qua **Mũ Nấm Đàn Hồi, Jump Pad, hoặc văng tường** rồi bay thẳng vào vùng hút của Pod mà không chạm đất thường, nhận ngay **+25% Style Bonus** kèm thông báo *"TRICK SHOT!"*.
4. **Tương Tác Vật Lý Không Gò Bó (Emergent Physics Interactions)**:
   - *Bệ đứng & Đối trọng*: Đặt *Tượng Đá Nhỏ* hoặc cổ vật nặng lên công tắc áp lực / làm bệ nhảy qua tường cao; đặt *Khối Nam Châm* sát *Bánh Răng Đồng* để dính làm 1 cụm vác chung 2 món.
   - *Khử nhiệt & Đông cứng*: Đặt *Lăng Kính Băng* cạnh *Lõi Lò Phản Ứng* để làm nguội lõi `20s` (hết thu hút quái), hoặc ném *Lăng Kính Băng* xuống hố dung nham Chương 3 để đóng băng lối đi tạm `15s`.
   - *Dây chuyền nổ*: Dùng vụ nổ *Lõi Xung Kích* hoặc *Cú Ném Nặng* phá vỡ rào gỗ/tường nứt để mở đường tắt về Pod!
5. **Hư Hại Va Đập (`Fragility`)**:
   - Lắng nghe `OnCollisionEnter` trên `RelicItem`: nếu `collision.relativeVelocity.magnitude > breakThreshold`, tính sát thương `RelicDamage = Mathf.Max(0f, (relSpeed - breakThreshold) * 12f * (1f - ownerFragilityResistance))` (với `Tinker` có `fragilityResistance = 0.5f`).
   - Trừ HP cổ vật, phát hiệu ứng nứt (`SG_CrystalCrack`) + hạt mảnh vỡ, và trừ giá trị bán (tối thiểu `25%` khi chưa vỡ nát). Nếu `HP <= 0`, cổ vật nổ vỡ thành **Mảnh Vụn Cổ Vật (`Scrap Shard`)** giá trị cố định `15% baseValue` (`mass = 1kg`, không thể vỡ thêm).

---

## 4. Hệ Thống Roguelite Upgrades (Tích Lũy Trong Run)

Mỗi khi đưa cổ vật vào Pod hoặc mở Hòm Di Tích, người chơi chọn **1 trong 3 thẻ Upgrade** (lưu dưới dạng `UpgradeDataSO`, hỗ trợ cộng dồn không giới hạn như *Megabonk* / *Risk of Rain 2*):

- **Giày Lò Xo (Spring Boots)**: +1 lần Nhảy trên không, tăng lực đẩy tiếp đất gây sóng choáng nhỏ.
- **Găng Từ Trường (Mag-Gloves)**: Tăng tầm với nhặt đồ +1.5m, cổ vật không bị rơi khi bị quái nhỏ húc.
- **Lõi Xung Kích (Shock Core)**: Mỗi lần Dash tạo vụ nổ đẩy quái xung quanh vào trạng thái Ragdoll.
- **Đạn Quỹ Đạo (Orbiting Relics)**: Tạo các khối tinh thể bay vòng quanh người chơi, tự động đập quái lại gần.
- **Balo Giảm Trọng (Feather Pack)**: Giảm 40% ảnh hưởng trọng lượng cổ vật lên tốc độ chạy.
- **Bảo Hiểm Pha Lê (Bubble Wrap)**: Cổ vật đang cầm được bọc lớp khiên chịu miễn phí 1 lần va chạm mạnh mỗi 15 giây.

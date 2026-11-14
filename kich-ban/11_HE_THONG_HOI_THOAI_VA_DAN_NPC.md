# 11. Hệ Thống Hội Thoại Động (Dynamic Dialogue) & Dàn NPC Chi Tiết

> Học hỏi từ **Hades** (ghi nhớ micro-state từng lượt chơi), **Deep Rock Galactic** (contextual barks & tương tác hô khẩu hiệu) và **Portal 2** (nhịp điệu phản ứng trực tiếp với hành vi người chơi) để mọi câu thoại đều tự nhiên, đời thường, không cứng nhắc và không lặp lại nhàm chán.

---

## 1. Kiến Trúc Engine Hội Thoại Không Cứng (Reactive Dialogue System)

AI Agent triển khai kết hợp **`DialogueDirector.cs`** (barks thời gian thực trong trận) và **`InkNarrativeManager.cs` (`inkle/ink` — đọc kịch bản nguồn [ink/relic_haulers_story.ink](./ink/relic_haulers_story.ink), xem thêm [File 15](./15_CO_CHE_HOI_SINH_VA_COT_TRUYEN_INK.md))** theo 4 nguyên tắc sau thay vì viết `if-else` cứng nhắc:

1. **Micro-State Blackboard (`DialogueBlackboard`)**:
   - Lưu giữ trạng thái từ Run vừa kết thúc & hiện tại:
     - `lastRunOutcome` (`Victory`, `DiedToMimic`, `DiedToVoid`, `DiedToGolem`, `LeftBehindByPod`)
     - `brokenRelicsLastRun`, `trickShotsLastRun`, `overchargedLastRun`, `timesRagdolledLastRun`
     - `equippedHeadId`, `logsCollectedCount`, `museumDiscoveredCount`, `playerIdleSeconds`
2. **Priority & Interruption Queue**:
   - `Priority 100 (Critical)`: Cảnh báo sụp đảo `60s`, Boss xuất hiện, Pod sắp cất cánh `15s`. (Ngắt ngang câu tán gẫu thường).
   - `Priority 70 (Event)`: Nộp Vương Miện, kích hoạt Synergy, Trick Shot, gặp Golden Imp.
   - `Priority 40 (Banter)`: Phản ứng lúc nhặt đồ, làm vỡ đồ, đứng im lâu, bấm nút hô khẩu hiệu (`T`).
3. **Variant Pool & Anti-Repeat Cooldown**:
   - Mỗi `DialogueRuleSO` chứa tối thiểu **3–4 biến thể câu thoại**, lưu `lastPlayedIndex` để không bao giờ phát cùng 1 câu hai lần liên tiếp, kèm `cooldownSeconds` (`15s–45s` tùy loại).
4. **Hội Thoại Có Lựa Chọn (Branching Choice Pills)**:
   - Tại Hub & Shop, người chơi luôn có `2–3` nút trả lời thể hiện thái độ (*Chuyên nghiệp / Cà khịa / Hỏi chuyện cũ / Mặc cả*), giúp NPC phản hồi khác nhau thay vì đọc thoại một chiều.

---

## 2. Dàn Nhân Vật NPC & Tính Cách Đặc Trưng

| NPC | Vị Trí | Tính Cách & Giọng Văn | Vai Trò Gameplay |
| :--- | :--- | :--- | :--- |
| **VEX** | Radio toàn trận & Trạm Khí Cầu | AI thực dụng, hay lo tiền bảo hiểm, cà khịa duyên kiểu *GLaDOS* nhẹ, nhưng càng về sau càng lộ tình cảm và ký ức cũ. | Điều phối nhiệm vụ, cảnh báo động, phản ứng theo hành vi người chơi. |
| **Bác Kael** | Lò Rèn `01_SkyDockHub` | Cựu Hauler mất 1 chân (đeo chân đồng cơ khí), nói cộc lốc kiểu thợ già nhưng rất xót lính trẻ. | Đổi trang bị Bone Retargeting, nâng Guild Tech Tree, bình luận về cái chết của bạn. |
| **Pip** | `02_AirshipShop` & *Sky Peddler* | Sinh vật nhỏ nhanh nhẹn đội mũ phi công to gấp đôi đầu, mê đồ cổ sáng lấp lánh, nói nhanh và rất khoái mặc cả. | Bán đồ giữa chặng, đổi đồ bất ngờ giữa màn, tiết lộ tin đồn về màn sắp tới. |
| **Giáo Sư Myla** | Bảo Tàng `01_SkyDockHub` | Học giả hậu đậu đeo kính lệch, nói chuyện say mê quá đà về lịch sử đảo nổi, hay hoảng hốt khi bạn làm vỡ đồ cổ. | Trưng bày 14 Cổ Vật 3D, kể Lore từng món & giải mã 12 Mảnh Nhật Ký Bí Mật. |

---

## 3. Kịch Bản Hội Thoại Chi Tiết Theo Ngữ Cảnh (Không Cứng)

### A. Tại `01_SkyDockHub` — Bác Kael (Phản Ứng Theo Kỷ Lục & Chết Kiểu Gì)

- **Lần đầu vào game**:
  - **Kael**: *"Lại thêm một tân binh thích nhảy khỏi khí cầu à? Nhìn cái ba lô dây thừng kia đi — buộc cho chặt vào. Dưới mây không có bảo hiểm rơi tự do đâu."*
    - `[Lựa chọn 1: "Cháu quen nhảy cao rồi bác!"]` → **Kael**: *"Hừ, thằng nhóc trước nói y hệt thế xong rơi thẳng vào tổ Imp. Đeo cái mũ đèn vào đi!"*
    - `[Lựa chọn 2: "Bác từng vớt cổ vật ở dưới đó à?"]` → **Kael**: *(gõ nhẹ vào chân cơ khí)* *"Ngày xưa tôi vác hẳn hai cái Lõi Lò một lúc. Giờ thì... giữ gìn cặp giò của cậu cho cẩn thận đấy."*

- **Nếu Run trước chết vì `Spring Mimic`**:
  - **Kael** *(biến thể 1)*: *"Nghe VEX báo lại rồi. Bị cái rương cắn bay lên trời hả? Lần sau đứng cách hai mét nhìn cái nắp xem nó có 'thở' không rồi hẵng sờ vào!"*
  - **Kael** *(biến thể 2)*: *"Haha, rương biết cắn! Hồi trẻ tôi cũng bị nó hất bay mất cái mũ bảo hiểm. Lấy cây gậy chọc thử trước khi mở nhé nhóc."*

- **Nếu Run trước làm vỡ >= 3 cổ vật (`brokenRelicsLastRun >= 3`)**:
  - **Kael**: *"Này, tôi là thợ rèn chứ không phải thợ dán gốm! Cậu xuống dưới đó thu hồi cổ vật hay mở tiệm đập niêu thế hả?"*
    - `[Lựa chọn: "Tại tụi Imp húc cháu chứ bộ!"]` → **Kael**: *"Thế thì lần sau bấm `Left Mouse` húc vai vào mặt tụi nó trước, hoặc nhặt con Imp lên mà ném trả lại!"*

- **Nếu đang đội `Mũ Giáp Golem` (đã thắng Màn 6-2)**:
  - **Kael**: *(huýt sáo khẽ)* *"Chà... cái sọ đá của Golem Warden cơ đấy. Trông hợp với cậu hơn tôi tưởng. Nhưng đừng tưởng thế là bất tử — ở chế độ Rift Hazard tụi nó đập còn rát hơn nhiều."*

---

### B. Tại Bảo Tàng `01_SkyDockHub` — Giáo Sư Myla

- **Khi mới mang về `Bình Pha Lê Cổ` lần đầu**:
  - **Myla**: *"Ôi trời ơi, nguyên vẹn luôn sao?! Lớp men xanh này chỉ có ở thung lũng gió trước Đại Phân Tách... Khoan đã, cái vết xước nhỏ ở quai là cậu va vào cửa Pod đúng không? Thôi không sao, vẫn đọc được hoa văn!"*
- **Khi tỷ lệ làm vỡ của `Bình Pha Lê` nhiều hơn số lần thu hồi nguyên vẹn**:
  - **Myla**: *(ôm trán)* *"Thống kê bảo tàng ghi cậu đã làm tan tành **{{brokenCount}}** chiếc bình rồi đấy... Mỗi mảnh vỡ là một trang sử bị mất. Làm ơn mang theo Bình Keo Hàn của Pip đi mà!"*
- **Khi thu thập được `6 / 12` Mảnh Nhật Ký Bí Mật**:
  - **Myla**: *(hạ giọng)* *"Này... cậu đọc kỹ mảnh số 6 chưa? Những hòn đảo này không tự nhiên vỡ ra. Lõi Năng Lượng được chế tạo để *khóa* thứ gì đó bên dưới tầng mây... và VEX dường như biết chuyện này từ lâu rồi."*

---

### C. Tại `02_AirshipShop` — Pip & Cơ Chế Mặc Cả Có Rủi Ro

- **Lời chào ngẫu nhiên theo chương sắp tới**:
  - *(Sắp vào Chương 2 Rừng Nấm)*: *"Kính coong! Sắp đáp xuống Rừng Nấm đấy bạn hiền! Mấy cây nấm ở đó nảy còn hơn bóng cao su — mua thêm `Bảo Hiểm Rớt Vực` hay `Keo Hàn` đi, không là pha lê bay thẳng lên ngọn cây đấy!"*
  - *(Sắp vào Chương 4 Vịnh Băng)*: *"Brừ... lạnh sun râu rồi! Dưới kia sàn trơn như mỡ, đẩy cổ vật một cái là trượt tuốt luốt. Có tiền dư thì hốt nhanh không tàu sắp thả neo!"*

- **Lựa chọn Mặc Cả (1 lần mỗi lần ghé Shop)**:
  - Người chơi bấm `[Mặc cả với Pip (-20% giá hoặc +10% phạt)]`:
  - **Nếu thành công (`60%` xác suất, tăng lên `80%` nếu có thẻ VIP Tech Tree)**:
    - **Pip**: *"Ái chà, miệng lưỡi dẻo quẹo thế! Thôi được, nể mặt bạn vừa vớt vượt Quota, giảm hẳn **20%** cho món tiếp theo — cấm kể với Hội trưởng nhé!"*
  - **Nếu thất bại (`40%`)**:
    - **Pip**: *"Hả?! Trả giá bèo thế thì tự đi mà nhặt rác ngoài mây! Dỗi rồi, hôm nay tăng giá **10%** cho chừa cái thói kì kèo!"*

---

### D. Trong Trận — Radio Động Của VEX (Biến Thể & Phá Luật)

- **Khi người chơi đứng im không làm gì quá `25s`**:
  - *(Biến thể 1)*: *"VEX: Alo? Tín hiệu tim vẫn đập mà sao đứng như tượng đá thế? Hòn đảo không đợi bạn ngắm mây đâu."*
  - *(Biến thể 2)*: *"VEX: Nếu bạn đang tìm góc chụp ảnh đẹp thì báo trước, phí thuê khí cầu tính theo phút đấy."*
- **Khi người chơi nhặt 1 con `Pebble Imp` đang Ragdoll lên tay**:
  - *(Biến thể 1)*: *"VEX: Khoan... quy định Hội ghi là vớt 'Cổ Vật', không phải bắt cóc sinh vật bản địa! ...Nhưng trông buồn cười thật, ném thử vào con kia xem!"*
  - *(Biến thể 2)*: *"VEX: Pod sẽ không trả tiền cho con Imp đó đâu nhé! Ném nó ra chỗ khác đi!"*
- **Khi làm một cú `Trick Shot` bật qua nấm/Jump Pad lọt thẳng vào Pod**:
  - *(Biến thể 1)*: *"VEX: ...Tôi vừa tính toán quỹ đạo đó và kết quả là 'bất khả thi'. Thưởng nóng +25% công nghệ biểu diễn!"*
  - *(Biến thể 2)*: *"VEX: Góc nảy 10 điểm! Tiếc là bình pha lê suýt đứng tim cùng với tôi!"*
- **Khi bấm nút Đỏ `Overcharge Pod` (Tăng Ca Tham Lam)**:
  - *(Biến thể 1)*: *"VEX: Bấm nút Đỏ thật luôn?! Cảnh báo: Thời gian sụp đảo giảm một nửa, quái đang lao tới! Chúc bạn... giữ được cả đồ lẫn mạng!"*
  - *(Biến thể 2)*: *"VEX: Lòng tham đã vượt ngưỡng an toàn. Mở khóa hệ số 2.5x — chạy nhanh lên trước khi tôi phải viết báo cáo mất tích!"*
- **Khi người chơi liên tiếp thu thập đủ `12/12` Mảnh Nhật Ký (True Ending Radio Call ở cuối Màn 6-2)**:
  - **VEX**: *(giọng nhiễu sóng rồi trở nên ấm hơn)* *"Chờ đã... bạn đã ghép đủ 12 mảnh nhật ký của Đội Tiên Phong rồi sao? Thật ra... tôi không chỉ là AI điều phối. Tôi là bản sao ý thức cuối cùng của Thuyền trưởng tầng mây cũ, ở lại đây để tìm người đủ dũng cảm thu hồi toàn bộ Lõi trước khi Đại Phân Tách lặp lại. Cảm ơn vì đã không bỏ cuộc ở những cú rơi đầu tiên... Hẹn gặp trên bầu trời mới, Hauler."*
- **Khi người chơi mải Câu Cá hoặc Đào Quặng lúc đồng hồ Collapse còn `< 60s`**:
  - **VEX**: *"Này Hauler! Hòn đảo sắp rụng xuống Vực Thẳm rồi mà bạn vẫn đứng thả cần câu cá à?! Kéo nhanh con cá rồi chạy về Pod ngay!"*
- **Khi câu nhầm một con `Pebble Imp` ướt sũng lên bờ**:
  - **VEX**: *"Chúc mừng cần thủ! Bạn vừa câu được một cục đá biết cắn! Đừng nhìn nữa, húc nó xuống lại đi!"*

---

## 4. Phản Hồi Khẩu Hiệu Nhanh (Phím `T` — Kiểu *"Rock and Stone!"*)

Bấm `T` mọi lúc (hồi `15` Stamina tinh thần mỗi `12s` cooldown, giơ cao tay / đồ đang cầm):
- **Hauler hô ngẫu nhiên**:
  - *"Vì những cổ vật chưa kịp vỡ!"*
  - *"Nặng mấy cũng vác về Pod!"*
  - *"Nhảy thôi, rơi tính sau!"*
  - *"Còn thở là còn vác!"*
- **Nếu đang ở gần Kael / Pip / Myla tại Hub hoặc Shop**: NPC lập tức giơ tay hưởng ứng theo + tặng buff `+10%` tốc độ chạy nhẹ trong `2s`:
  - **Kael**: *"Khá lắm nhóc! Cứ giữ khí thế đó!"*
  - **Pip**: *"Đem về nhiều đồ sáng lấp lánh nha bạn hiền!"*
  - **Myla**: *"Nhớ... nhớ đặt nhẹ tay thôi nhé!"*

---

## 5. Hội Thoại Đa Dạng Theo Archetype, Tay Đôi Giữa NPC & Chuyện Bảo Tàng

### A. Phản Ứng Riêng Theo Archetype (Scout / Brawler / Tinker)
- **Khi chọn `Scout`**:
  - **Kael**: *"Lại chọn bộ đồ nhẹ hều nữa à? Nhanh chân thì tốt đấy, nhưng đừng có ôm Lõi Lò rồi lê lết như con rùa giữa cầu treo."*
  - **Pip**: *"Ê bạn nhanh chân! Có `Balo Giảm Trọng` trong Shop đấy — chạy như gió mà không sợ gãy lưng!"*
- **Khi chọn `Brawler`**:
  - **Kael**: *"Hảo hán! Vác cả cái Tượng Đá mà chạy phăm phăm thế kia mới ra dáng Hauler chứ. Nhớ dùng `Húc Vai` dọn tụi Imp cho bõ ghét!"*
  - **Myla**: *"Ơ... anh Brawler ơi, làm ơn đừng lấy cây Búa Xung Kích gõ thử xem Bình Pha Lê có kêu không nhé..."*
- **Khi chọn `Tinker`**:
  - **Myla**: *"Tuyệt quá! Có Kỹ sư cầm Tia Ổn Định đi cùng thì tỷ lệ bảo toàn men gốm tăng hẳn 50% luôn!"*
  - **VEX**: *"Chào mừng Tinker. Dù giáp bạn hơi mỏng, ít nhất hóa đơn đền bù cổ vật của tôi sẽ giảm một nửa."*

### B. Đối Thoại Tay Đôi Tự Nhiên Giữa Các NPC (Cross-NPC Banter Ở Sky-Dock)
Khi người chơi đi ngang khu vực Hub không nói chuyện trực tiếp, các NPC tự trò chuyện qua lại tạo cảm giác trạm khí cầu có sức sống thật:
- **Kael & VEX (Cà khịa qua loa phát thanh)**:
  - **Kael**: *"Này VEX, tuần này cậu lại cắt 5% tiền thưởng sửa chân máy của tôi vào quỹ bảo trì Pod nữa à?"*
  - **VEX**: *"Đính chính: đó là phí bảo hiểm cho việc bác cứ ném cái cờ lê vào màn hình radar mỗi khi tân binh làm vỡ bình pha lê."*
  - **Kael**: *"Chậc, cái đồ hộp biết nói..."*
- **Myla & Pip (Chuyện làm ăn & lịch sử)**:
  - **Myla**: *"Pip này, hôm qua cậu bán cho lính mới cái 'Mảnh Đá Thần Cổ Đại'... thật ra là cục gạch bếp nhà tôi đúng không?"*
  - **Pip**: *(cười hì hì)* *"Ấy, gạch nhà giáo sư cũng 200 năm tuổi rồi còn gì! Miễn nó nặng đúng 10 ký ném trúng đầu Imp là được!"*

### C. Lời Kể Chuyện Bảo Tàng Của Myla Cho Các Cổ Vật Tiêu Biểu
Mỗi món trong Bảo Tàng có nút `[Hỏi Myla về món này]` kèm lựa chọn phản ứng tự do:
- **Chuông Gió Rỗng**: *"Người dân Đảo Gió treo chiếc chuông này trước hiên để báo hiệu cơn bão sắp tới. Nghe nói tiếng ngân của nó vang xa tới tận tầng mây dưới... bảo sao lũ Echo Stalker cứ nghe thấy là lao tới!"*
  - `[Lựa chọn 1: "Thảo nào cháu bị tụi nó rượt trối chết!"]` → **Myla**: *"Hì, lần sau thử kiếm đôi `Đế Giày Êm` xem sao nhé!"*
  - `[Lựa chọn 2: "Thế bán được bao nhiêu tiền vậy cô?"]` → **Myla**: *(thở dài)* *"Sao cậu nói chuyện giống hệt Pip thế..."*
- **Mỏ Neo Trọng Lực**: *"Đây là một phần của hệ thống Neo Trời giữ hòn đảo không trôi dạt vào Vực Thẳm. Mỗi lần chúng ta rút nó khỏi đảo, hòn đảo mất cân bằng nhanh hơn — đó là lý do đồng hồ sụp đảo bắt đầu chạy."*
- **Đồng Hồ Cát Thời Gian**: *"Cát bên trong chảy ngược lên trên mỗi khi bạn giữ nó trên tay! Tiếc là năng lượng của nó chỉ cầm chân vết nứt không gian được chừng 45 giây trước khi cát lắng xuống."*

### D. Hội Thoại Chế Tạo Quặng, Trang Trại Mây (`Sky-Ranch`) & Bể Cá Mây (`Sky-Aquarium`)
*(Kết nối trực tiếp với [File 14](./14_CHE_TAO_QUANG_CHAN_NUOI_VA_NUOI_CA.md) dựa trên `DialogueBlackboard`)*:
- **Bác Kael (Lò Rèn Hợp Kim & Gadget)**:
  - *Khi mở Bàn Rèn với ≥ 3 loại quặng*: *"Quặng xịn đấy! Đừng có ném hết vào Pod lấy mấy đồng lẻ — cho vào lò luyện **Hợp Kim**, bác gò cho cái `Mồi Nhử Chuông Rung` hay `Lưới Hứng Đệm Mây` mà cứu mạng lúc sập đảo!"*
  - *Khi vừa rèn xong Trang Bị Vĩnh Viễn*: *"Nhớ quy tắc 2 mặt nhé tân binh: giáp nặng giữ cổ vật êm hơn, nhưng đừng tưởng mình bất tử mà nhảy thẳng xuống mồm Crusher!"*
- **Giáo Sư Myla (Bể Cá Mây `Sky-Aquarium`)**:
  - *Khi thả **Cá Giống (`Breeder Fish`)** mới vào bể*: *"Ôi tuyệt vời! Bể Cá Mây có dòng nước phù du đặc biệt — chỉ cần bạn hoàn thành `2` chặng nhảy đảo (`1` chặng nếu nâng cấp Bể Cấp 3), chúng sẽ sinh thêm cá con và rụng `Vảy Cá Phát Sáng` để rèn đồ! À nhớ nhé, hệ thống VEX không cho mang cá nuôi từ bể xuống nộp Quota đâu."*
  - *Khi Bể Cá đầy slot*: *"Bể chật lắm rồi! Bạn lấy bớt cá trưởng thành sang **Bếp Lò của Pip** làm súp buff, hoặc chuyển vào danh mục mẫu vật bảo tàng nhé."*
- **Pip & VEX (Trang Trại Mây `Sky-Ranch` & Pet Đồng Hành)**:
  - *Khi mang theo Pet **Cún Đá Imp Thuần Hóa***:
    - **Pip**: *"Ê ê con Imp nhỏ của bạn vừa gặm mất một con ốc dự phòng của tôi rồi... nhưng mà công nhận nó đánh hơi mạch quặng chuẩn thật! Này, mang thêm `Gà Mây` nữa là không có chuyện vướng chân nhau đâu — tụi nó biết tự né người chơi với cổ vật hết rồi."*
    - **VEX**: *"Ghi nhận sinh vật đồng hành không cản trở va chạm vật lý (`Pet Collision = Disabled`). Cảnh báo: đừng cố cho thú nuôi ăn cổ vật cổ đại."*

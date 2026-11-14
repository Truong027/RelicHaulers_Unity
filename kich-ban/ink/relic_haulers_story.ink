// ============================================================================
// RELIC HAULERS — MASTER BRANCHING NARRATIVE SCRIPT (inkle/ink)
// Tuân thủ .agents/skills/narrative-designer/SKILL.md
// Có thể mở và chạy kiểm thử Dead-End trực tiếp bằng Inky Editor!
// ============================================================================

// === EXTERNAL C# BINDINGS (ink-unity-integration) ===
EXTERNAL give_item(item_id, amount)
EXTERNAL unlock_log(log_id)
EXTERNAL adjust_haggle(delta_percent)
EXTERNAL trigger_ending(ending_id)

// === PERSISTENT WORLD VARIABLES ===
VAR stat_act = 1
VAR stat_logs_collected = 0
VAR stat_revives_this_run = 0
VAR stat_revives_total = 0
VAR stat_broken_relics_last_run = 0
VAR stat_museum_count = 0
VAR stat_guild_tokens = 0

VAR state_archetype = "Scout"
VAR state_last_run_outcome = "None"
VAR state_next_biome = "WindmillMeadow"

VAR flag_first_hub_intro = false
VAR flag_vex_tether_used = false
VAR flag_tether_secret_asked = false
VAR flag_trusted_kael = false
VAR flag_myla_preserve_pact = false
VAR flag_sided_pip_greed = false
VAR flag_vex_confessed = false
VAR flag_promised_free_vex = false
VAR flag_pip_haggled_this_visit = false

// === INKY FALLBACK STUBS ===
=== function give_item(item_id, amount) ===
~ return

=== function unlock_log(log_id) ===
~ return

=== function adjust_haggle(delta_percent) ===
~ return

=== function trigger_ending(ending_id) ===
~ return

// === ENTRY POINT FOR INKY PREVIEW ===
-> sky_dock_hub_router

// ============================================================================
// 1. TRẠM CHỈ HUY KHÍ CẦU (01_SkyDockHub) — HỘI THOẠI PHẢN ỨNG & CỐT TRUYỆN
// ============================================================================
=== sky_dock_hub_router ===
{
    - not flag_first_hub_intro:
        -> kael_first_arrival
    - stat_revives_this_run > 0 and not flag_tether_secret_asked:
        -> post_revive_tether_event
    - else:
        -> hub_npc_select
}

= kael_first_arrival
Kael: "Lại thêm một tân binh thích nhảy khỏi khí cầu à? Nhìn cái ba lô dây thừng kia đi — buộc cho chặt vào."
VEX: "Chào mừng tới Trạm Sky-Dock. Tôi là VEX. Nhắc trước: phí bảo hiểm va đập cổ vật trừ thẳng vào tiền thưởng."
* ["Cháu quen nhảy cao rồi bác Kael!"]
    Kael: "Hừ, thằng nhóc trước nói y hệt thế xong rơi thẳng vào tổ Imp. Đeo cái mũ đèn vào đi."
    ~ flag_first_hub_intro = true
    -> hub_npc_select
* ["Bác từng vớt cổ vật dưới đó ạ?"]
    ~ flag_trusted_kael = true
    ~ flag_first_hub_intro = true
    Kael: "(gõ nhẹ chân đồng cơ khí) Ngày xưa tôi vác hai Lõi Lò một lúc. Nghe đây: đừng tin hết những gì Hội Hauler viết trên bảng hợp đồng."
    -> hub_npc_select

= post_revive_tether_event
~ flag_tether_secret_asked = true
Myla: "Ơ kìa! Chỉ số sinh tồn của cậu vừa nhấp nháy tím... Tia Tái Sinh Pod vừa kéo cậu lại từ mép Vực Thẳm đúng không?!"
VEX: "Xác nhận kích hoạt Emergency Tether. Đổi lại mất 15 giây tuổi thọ từ trường đảo. Đừng chết nhiều quá, ngân sách năng lượng không miễn phí đâu."
* ["Khoan, sao Pod kéo người từ cõi chết lên được?"]
    Kael: "Nói thật đi VEX. Thứ năng lượng kéo thằng nhóc này về có phải trích từ Lõi Neo Trời không?"
    VEX: "(im lặng 2 giây) ...Hệ thống chỉ tối ưu tỷ lệ sống sót của nhân sự. Đừng hỏi quá sâu ở Hồi 1, tân binh."
    ~ flag_trusted_kael = true
    -> hub_npc_select
* ["Cảm ơn đã cứu mạng, VEX!"]
    VEX: "Không cần cảm ơn. Cứ giữ nguyên vẹn cổ vật ở lượt tới là coi như hòa."
    -> hub_npc_select

=== hub_npc_select ===
[Bạn đang đứng tại Sảnh Sky-Dock (Hồi {stat_act} | Nhật ký: {stat_logs_collected}/12 | Hồi sinh đã dùng: {stat_revives_total})]
+ [Nói chuyện với Bác Kael (Lò Rèn & Hợp Kim)] -> kael_hub_talk
+ [Nói chuyện với Giáo Sư Myla (Bảo Tàng & Bể Cá)] -> myla_hub_talk
+ [Kiểm tra Tiến Trình Nhật Ký & Cốt Truyện VEX] -> vex_act_progression
+ [Mô phỏng bước vào Airship Shop (Pip)] -> pip_airship_shop
+ [Mô phỏng tiến tới Chung Kết Màn 6-2 (Sanctum)] -> act3_climax_sanctum
+ [Kết thúc hội thoại, ra Drop Hatch nhảy đảo] -> DONE

// ============================================================================
// 2. BÁC KAEL — PHẢN ỨNG THEO ARCHETYPE, CÁI CHẾT & HỢP KIM
// ============================================================================
=== kael_hub_talk ===
{
    - state_last_run_outcome == "DiedToMimic":
        Kael: "Nghe VEX báo lại rồi. Bị cái rương cắn bay lên trời hả? Lần sau đứng cách hai mét nhìn cái nắp xem nó có thở không!"
    - state_last_run_outcome == "DiedToVoid":
        Kael: "Rớt xuống biển mây lạnh lắm phải không? Lần sau nhớ sắm Cánh Lướt Mây hoặc Gà Mây đi cùng đấy."
    - stat_broken_relics_last_run >= 3:
        Kael: "Tôi là thợ rèn chứ không phải thợ dán gốm! Chuyến vừa rồi cậu đập vỡ {stat_broken_relics_last_run} món cổ vật liền đấy!"
    - state_archetype == "Brawler":
        Kael: "Hảo hán! Vác Tượng Đá chạy phăm phăm thế mới ra dáng Hauler chứ."
    - else:
        Kael: "Lò rèn đỏ lửa rồi. Mang quặng từ dưới đảo về đây tôi gò Gadget cho."
}
* {stat_act >= 2} ["Bác mất chân dưới Xưởng Đúc phải không?"]
    Kael: "Không phải tại quái... Là lúc tôi từ chối rút Lõi Neo cuối để cứu đồng đội. Hội bỏ mặc tụi tôi ở tầng mây dưới."
    ~ flag_trusted_kael = true
    -> kael_hub_talk
+ ["Chế tạo từ quặng giúp được gì cho cháu?"]
    Kael: "Gò Keo Hàn giữ giá pha lê, Mồi Chuông dụ Stalker, hoặc Lưới Hứng Đệm Mây ném cổ vật từ tầng ba xuống vẫn nguyên vẹn!"
    -> kael_hub_talk
+ [Quay lại Sảnh Sky-Dock] -> hub_npc_select

// ============================================================================
// 3. GIÁO SƯ MYLA — BẢO TÀNG, BỂ CÁ & GIẢI MÃ NHẬT KÝ
// ============================================================================
=== myla_hub_talk ===
{
    - stat_museum_count == 0:
        Myla: "Chào mừng tới Bảo Tàng & Bể Cá Mây! Cậu cứ đem cổ vật hay cá giống hiếm về, tôi sẽ giải mã lịch sử của chúng."
    - stat_broken_relics_last_run >= 3:
        Myla: "(ôm trán) Thống kê ghi cậu vừa làm vỡ {stat_broken_relics_last_run} món đồ cổ... Mỗi mảnh vỡ là một trang sử bị mất đấy!"
    - else:
        Myla: "Hôm nay bảo tàng đã có {stat_museum_count}/14 cổ vật rồi! Cá trong Sky-Aquarium cũng bơi khỏe lắm."
}
* {stat_logs_collected >= 6} ["Cô dịch được gì từ 6 mảnh Nhật Ký chưa?"]
    Myla: "(hạ giọng) Có... Những hòn đảo không tự vỡ. Sky-Anchors khóa thứ gì đó dưới Vực Thẳm, và Golem Warden được lập trình để bảo vệ bầu trời!"
    * * ["Vậy chúng ta phải ngăn Hội rút cạn Lõi!"]
        ~ flag_myla_preserve_pact = true
        Myla: "Đúng! Nếu cậu chạm tới Thánh Điện Chương 6, hãy cân nhắc ngắt mạch thu hồi của Hội nhé!"
        -> myla_hub_talk
    * * ["Nhưng hợp đồng vẫn là tiền lương của bọn cháu..."]
        Myla: "(thở dài) Tôi hiểu... Ai cũng phải sống. Chỉ mong cậu đừng làm hại chúng quá mức."
        -> myla_hub_talk
+ ["Bể Cá Mây nuôi cá có mang nộp Pod Quota được không cô?"]
    Myla: "Không được đâu! Hệ thống VEX khóa mã rồi — cá nuôi ở Hub chỉ để nấu Meal Buff cho Pip hoặc lấy Vảy Cá Sáng rèn đồ thôi."
    -> myla_hub_talk
+ [Quay lại Sảnh Sky-Dock] -> hub_npc_select

// ============================================================================
// 4. TIẾN TRÌNH 3 HỒI & LỜI THÚ NHẬN CỦA VEX
// ============================================================================
=== vex_act_progression ===
{
    - stat_act == 1:
        VEX: "Hồi 1 — Mọi chỉ số bình thường. Cố gắng đạt Quota và thu thập thêm Nhật Ký Bí Mật ở các nhánh phòng."
        -> hub_npc_select
    - stat_act == 2:
        VEX: "Hồi 2 — Cảnh báo: Hệ thống phát hiện chữ ký sinh trắc cũ của... Đội Tiên Phong tại Xưởng Đúc và Vịnh Băng."
        -> hub_npc_select
    - stat_act >= 3 and not flag_vex_confessed:
        -> vex_act3_confession
    - else:
        VEX: "Hồi 3 — Lõi Thánh Điện đang chờ ở Màn 6-2. Quyết định cuối cùng nằm trong tay bạn, Hauler."
        -> hub_npc_select
}

= vex_act3_confession
~ flag_vex_confessed = true
VEX: "(tiếng radio rè nhẹ, giọng không còn máy móc) Dừng một chút, Hauler... Tôi cần nói thật trước khi bạn nhảy xuống Chương 6."
VEX: "Tôi không chỉ là AI. Tôi là bản sao ý thức cuối cùng của Thuyền trưởng Đội Tiên Phong năm xưa — người đã kẹt lại trong mạng lưới Sky-Anchor."
{
    - stat_revives_total > 0:
        VEX: "Mỗi lần tôi kích hoạt Emergency Tether kéo bạn về khỏi cái chết ({stat_revives_total} lần rồi đấy), tôi đều phải đánh đổi một mảnh ký ức của chính mình."
    - else:
        VEX: "Và thú thật... đến giờ bạn chưa hề phải dùng Tether kéo xác lần nào. Bạn làm tôi nhớ tới đồng đội cũ của mình."
}
* ["Tôi sẽ tìm đủ 12 Nhật Ký để giải thoát ông, Thuyền trưởng!"]
    ~ flag_promised_free_vex = true
    {
        - stat_logs_collected >= 12:
            VEX: "Bạn đã gom đủ cả 12 mảnh rồi! Hẹn gặp ở Đỉnh Ngai Golem."
        - else:
            VEX: "...Cảm ơn vì đã không coi tôi chỉ là cái hộp biết nói. Bạn đã có {stat_logs_collected}/12 mảnh rồi — hãy tìm nốt trước khi chạm Ngai Golem!"
    }
    -> hub_npc_select
* ["Ông giữ kín vậy chỉ để lợi dụng bọn tôi vớt đồ?"]
    VEX: "(im lặng) Nếu tôi nói ra sớm, Hội sẽ xóa bộ nhớ tôi ngay lập tức. Hãy tự quyết định ở cuối Màn 6-2."
    -> hub_npc_select

// ============================================================================
// 5. AIRSHIP SHOP (02_AirshipShop) — PIP & MẶC CẢ CÓ RỦI RO
// ============================================================================
=== pip_airship_shop ===
{
    - state_next_biome == "BioluminescentCanopy":
        Pip: "Kính coong! Sắp đáp Rừng Nấm rồi bạn hiền! Mua thêm Keo Hàn đi không pha lê nảy lên ngọn cây đấy!"
    - state_next_biome == "FrostglassGlacier":
        Pip: "Brừ... Vịnh Băng trơn như mỡ! Có tiền dư hốt Giày Bám hoặc Viên Nang Tái Sinh nhanh đi!"
    - else:
        Pip: "Kính coong! Surplus Credits đầy túi rồi chứ? Bếp Lò cũng sẵn sàng nấu Meal Buff nhé!"
}
* {not flag_pip_haggled_this_visit} [Mặc cả với Pip (-20% giá hoặc +10% phạt)]
    ~ flag_pip_haggled_this_visit = true
    { shuffle:
        - Pip: "Ái chà, miệng lưỡi dẻo quẹo! Giảm hẳn 20% cho lần này — cấm kể với Hội trưởng nhé!"
          ~ adjust_haggle(-20)
        - Pip: "Giảm cho bạn hiền 20% luôn! Miễn chuyến sau đem nhiều đồ sáng lấp lánh về!"
          ~ adjust_haggle(-20)
        - Pip: "Hả?! Trả giá bèo thế thì ra mây mà nhặt! Dỗi rồi, hôm nay tăng giá +10%!"
          ~ adjust_haggle(10)
    }
    -> pip_airship_shop
* ["Lấy thêm 1 món ăn buff trước khi nhảy!"]
    ~ flag_sided_pip_greed = true
    Pip: "Nấu nóng hổi xong ngay! Chúc bạn vớt vượt Quota!"
    -> hub_npc_select
+ [Rời Airship Shop] -> hub_npc_select

// ============================================================================
// 6. CAO TRÀO CHƯƠNG 6 (Màn 6-2) & 3 KẾT THÚC PHÂN NHÁNH (ZERO DEAD-ENDS)
// ============================================================================
=== act3_climax_sanctum ===
[Đỉnh Ngai Golem — Golem Warden đã quỳ gối. Vương Miện Hư Không tỏa sáng bên cạnh tia hút của Extraction Pod.]
Myla: "(qua bộ đàm) Cậu cầm được Vương Miện rồi! Nhưng nhìn bảng điều khiển Pod kìa..."
Kael: "Nếu thả Vương Miện cho Hội, máy của bọn chúng sẽ hút cạn tầng mây này giống hệt Đại Phân Tách!"
VEX: "Quỹ đạo Pod sẵn sàng. Bạn chỉ còn 10 giây trước khi Sanctum sụp đổ. Bạn chọn thế nào, Hauler?"

* {stat_logs_collected >= 12} [\[True Ending\] Nạp 12 Nhật Ký vào Vương Miện để giải phóng VEX & ổn định Lõi]
    -> ending_true_vex_sky
* {flag_trusted_kael or flag_myla_preserve_pact or stat_logs_collected >= 6} [\[Ending B\] Ngắt mạch thu hồi của Hội trên Pod, giữ cân bằng các Đảo Nổi]
    -> ending_break_core
* [\[Ending A\] Ném Vương Miện vào Pod — Hoàn thành Hợp Đồng với Hội Hauler]
    -> ending_guild_contract

=== ending_guild_contract ===
~ trigger_ending("END_GUILD_CONTRACT")
~ give_item("gt_bonus", 300)
[KẾT THÚC A — KHẾ ƯỚC HỘI HAULER]
Vương Miện được hút vào khoang tàu. Hội Hauler trả thưởng hậu hĩnh và thăng cấp cho bạn.
Nhưng đêm đó, VEX khẽ phát tín hiệu nhỏ: *"Họ vừa bật lại công tắc Cỗ Máy Cổ... Có lẽ vẫn còn những mảnh Nhật Ký bạn chưa tìm thấy."*
-> END

=== ending_break_core ===
~ trigger_ending("END_BREAK_CORE")
~ give_item("gt_bonus", 450)
[KẾT THÚC B — PHÁ VỠ LÕI NEO]
Bạn ném búa phá vỡ bộ thu khuếch đại trên thân Pod đúng khoảnh khắc Vương Miện đồng bộ ngược lại mặt đất!
Những quần đảo xung quanh ngừng rung lắc. Bác Kael mỉm cười bên lò rèn, còn Myla khóc òa vì Bảo Tàng lịch sử thật đã được cứu.
-> END

=== ending_true_vex_sky ===
~ trigger_ending("END_TRUE_VEX_SKY")
~ give_item("skin_vex_prototype", 1)
[KẾT THÚC C — TRÁI TIM TRÊN BẦU TRỜI MỚI (TRUE ENDING)]
Ánh sáng từ 12 Mảnh Nhật Ký hòa vào Vương Miện Hư Không. Lõi Neo Trời được viết lại mã nguồn hòa bình!
{
    - stat_revives_total == 0:
        VEX: "Thật không thể tin nổi... Bạn vượt qua cả bầu trời này mà không mất một lần Tether nào. Đội Tiên Phong tự hào về bạn."
    - else:
        VEX: "Những lần Tether kéo bạn trở về cuối cùng cũng đáng giá rồi... Cảm ơn vì đã không bỏ cuộc ở những cú rơi đầu tiên."
}
Ý thức Thuyền trưởng được giải phóng khỏi mạng lưới Hội, mở khóa Skin VEX-Prototype và chế độ thử thách Rift Hazard 1–5!
-> END

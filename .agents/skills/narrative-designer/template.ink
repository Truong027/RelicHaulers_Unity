// Standard Ink (.ink) Template for RELIC HAULERS
// Compatible with inkle/ink + inkle/ink-unity-integration + Inky Editor

// === EXTERNAL C# FUNCTIONS (Bound by InkNarrativeManager.cs) ===
EXTERNAL give_item(item_id, amount)
EXTERNAL unlock_log(log_id)
EXTERNAL adjust_haggle(delta_percent)
EXTERNAL trigger_ending(ending_id)

// === PERSISTENT WORLD VARIABLES (Synced with SaveSystem & DialogueBlackboard) ===
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
VAR flag_trusted_kael = false
VAR flag_myla_preserve_pact = false
VAR flag_sided_pip_greed = false
VAR flag_vex_confessed = false

// === FALLBACK DEFINITIONS FOR INKY STANDALONE PREVIEW ===
// (Ink ignores these when Unity binds real EXTERNAL functions at runtime,
//  or lets Inky run cleanly with zero compile errors.)
=== function give_item(item_id, amount) ===
~ return

=== function unlock_log(log_id) ===
~ return

=== function adjust_haggle(delta_percent) ===
~ return

=== function trigger_ending(ending_id) ===
~ return

// === SAMPLE SCENE KNOT ===
-> sample_npc_knot

=== sample_npc_knot ===
{
    - not flag_first_hub_intro:
        -> first_meeting
    - else:
        -> repeat_hub_menu
}

= first_meeting
Kael: "Lại thêm một tân binh thích nhảy khỏi khí cầu à? Nhìn cái ba lô kia đi — buộc cho chặt vào."
* ["Cháu quen nhảy cao rồi bác!"]
    Kael: "Hừ, thằng nhóc trước nói y hệt thế xong rơi thẳng vào tổ Imp."
    ~ flag_first_hub_intro = true
    -> DONE
* ["Bác từng vớt cổ vật dưới đó ạ?"]
    ~ flag_trusted_kael = true
    ~ flag_first_hub_intro = true
    Kael: "Ngày xưa tôi vác hai cái Lõi Lò một lúc. Giờ thì... giữ gìn cặp giò của cậu cho cẩn thận."
    -> DONE

= repeat_hub_menu
Kael: "Cần rèn gì hay lại làm vỡ bình pha lê của Myla nữa?"
+ {stat_revives_this_run > 0} ["Sao Pod kéo cháu lên lúc sắp chết được vậy?"]
    Kael: "Hỏi cái đồ hộp VEX ấy. Tôi chỉ biết cái tia kéo đó xài năng lượng giống hệt Lõi Đảo."
    -> repeat_hub_menu
+ ["Cháu đi chuẩn bị tiếp đây."]
    -> DONE

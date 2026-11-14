using UnityEngine;
using RelicHaulers.Player;

namespace RelicHaulers.UI
{
    /// <summary>
    /// Minimal placeholder HUD for Gate 1 Prototype (Stamina, Speed, Held Relic info, Interaction prompt).
    /// </summary>
    public class GreyboxHudOverlay : MonoBehaviour
    {
        [SerializeField] private FloatingCapsuleController player;
        [SerializeField] private CarrySystem carrySystem;
        [SerializeField] private string stageLabel = "GATE 1: GREYBOX PROTOTYPE";

        private GUIStyle headerStyle;
        private GUIStyle bodyStyle;
        private GUIStyle promptStyle;

        private void Start()
        {
            if (player == null)
            {
                player = FindFirstObjectByType<FloatingCapsuleController>();
            }
            if (carrySystem == null && player != null)
            {
                carrySystem = player.GetComponent<CarrySystem>();
            }
        }

        private void OnGUI()
        {
            EnsureStyles();

            GUI.Box(new Rect(14f, 14f, 380f, 142f), GUIContent.none);
            GUI.Label(new Rect(24f, 20f, 360f, 24f), stageLabel, headerStyle);

            if (player != null)
            {
                float staminaPct = player.MaxStamina > 0f ? player.CurrentStamina / player.MaxStamina : 0f;
                GUI.Label(new Rect(24f, 44f, 360f, 20f),
                    $"Speed: {player.HorizontalSpeed:0.0} m/s  |  Grounded: {player.IsGrounded}  |  Dash: {player.IsDashing}",
                    bodyStyle);

                GUI.Label(new Rect(24f, 66f, 120f, 20f), $"Stamina: {player.CurrentStamina:0}/{player.MaxStamina:0}", bodyStyle);
                GUI.Box(new Rect(140f, 68f, 225f, 16f), GUIContent.none);
                GUI.DrawTexture(new Rect(142f, 70f, 221f * Mathf.Clamp01(staminaPct), 12f), Texture2D.whiteTexture);
            }

            if (carrySystem != null && carrySystem.HeldItem != null)
            {
                var item = carrySystem.HeldItem;
                GUI.Label(new Rect(24f, 88f, 360f, 20f),
                    $"Carrying: {item.DisplayName}  |  {item.MassKg:0.#}kg  |  HP {item.CurrentDurability:0}/{item.MaxDurability:0}  |  ${item.CurrentValue}",
                    headerStyle);
            }
            else
            {
                GUI.Label(new Rect(24f, 88f, 360f, 20f), "Hands Empty — Look at a Relic or Chest", bodyStyle);
            }

            GUI.Label(new Rect(24f, 112f, 360f, 34f),
                "[WASD] Move | [Hold Shift] Sprint | [Space] Jump | [Tap Shift/V] Dash\n[E] Interact/Grab/Place | [RMB] Aim Throw ([LMB/E] Cancel) | [LMB] Push/Bash",
                bodyStyle);

            if (carrySystem != null && !string.IsNullOrEmpty(carrySystem.ActivePrompt))
            {
                GUI.Box(new Rect(Screen.width * 0.5f - 230f, Screen.height - 64f, 460f, 34f), GUIContent.none);
                GUI.Label(new Rect(Screen.width * 0.5f - 220f, Screen.height - 60f, 440f, 26f), carrySystem.ActivePrompt, promptStyle);
            }
        }

        private void EnsureStyles()
        {
            if (headerStyle != null)
            {
                return;
            }

            headerStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(1f, 0.85f, 0.35f) }
            };

            bodyStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 11,
                normal = { textColor = Color.white }
            };

            promptStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 13,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = new Color(0.75f, 1f, 0.85f) }
            };
        }
    }
}

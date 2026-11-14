using UnityEngine;

namespace RelicHaulers.Player
{
    /// <summary>
    /// Applies cartoon Squash & Stretch strictly to VisualRoot child transform.
    /// Collider & Rigidbody parent scale remain untouched (1,1,1) to avoid physics corruption.
    /// </summary>
    public class SquashStretchVisual : MonoBehaviour
    {
        [SerializeField] private FloatingCapsuleController controller;
        [SerializeField] private float recoverSpeed = 9f;
        [SerializeField] private Vector3 jumpScale = new Vector3(0.85f, 1.25f, 0.85f);
        [SerializeField] private Vector3 dashScale = new Vector3(0.86f, 0.86f, 1.24f);
        [SerializeField] private float maxLandImpact = 16f;

        private Vector3 currentScale = Vector3.one;

        private void Reset()
        {
            controller = GetComponentInParent<FloatingCapsuleController>();
        }

        private void OnEnable()
        {
            if (controller == null)
            {
                controller = GetComponentInParent<FloatingCapsuleController>();
            }

            if (controller != null)
            {
                controller.OnJumped += HandleJumped;
                controller.OnLanded += HandleLanded;
                controller.OnDashed += HandleDashed;
            }
        }

        private void OnDisable()
        {
            if (controller != null)
            {
                controller.OnJumped -= HandleJumped;
                controller.OnLanded -= HandleLanded;
                controller.OnDashed -= HandleDashed;
            }
        }

        private void LateUpdate()
        {
            currentScale = Vector3.Lerp(currentScale, Vector3.one, 1f - Mathf.Exp(-recoverSpeed * Time.deltaTime));
            transform.localScale = currentScale;
        }

        private void HandleJumped()
        {
            currentScale = jumpScale;
        }

        private void HandleDashed()
        {
            currentScale = dashScale;
        }

        private void HandleLanded(float impactSpeed)
        {
            float t = Mathf.Clamp01(impactSpeed / maxLandImpact);
            float ySquash = Mathf.Lerp(0.88f, 0.72f, t);
            float xzStretch = Mathf.Lerp(1.10f, 1.26f, t);
            currentScale = new Vector3(xzStretch, ySquash, xzStretch);
        }
    }
}

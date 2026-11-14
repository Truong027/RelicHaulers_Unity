using UnityEngine;

namespace RelicHaulers.Player
{
    /// <summary>
    /// Smooth collision-aware Third-Person Follow camera for Gate 1 Greyboxing.
    /// Locks XZ cleanly to the interpolated Rigidbody target and smooths only Y steps to eliminate jitter.
    /// </summary>
    public class GreyboxCameraRig : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private HaulerInputReader inputReader;
        [SerializeField] private Vector3 pivotOffset = new Vector3(0f, 1.35f, 0f);
        [SerializeField] private float distance = 6.0f;
        [SerializeField] private float minDistance = 1.6f;
        [SerializeField] private float mouseSensitivity = 0.14f;
        [SerializeField] private float minPitch = -25f;
        [SerializeField] private float maxPitch = 65f;
        [SerializeField] private float verticalSmooth = 18f;
        [SerializeField] private float zoomSmooth = 16f;
        [SerializeField] private LayerMask collisionLayers = ~0;

        private float yaw;
        private float pitch = 18f;
        private float smoothedPivotY;
        private float currentDist;

        public void SetTarget(Transform newTarget, HaulerInputReader reader)
        {
            target = newTarget;
            inputReader = reader;
            if (target != null)
            {
                yaw = target.eulerAngles.y;
                smoothedPivotY = target.position.y + pivotOffset.y;
            }
        }

        private void Start()
        {
            if (target == null)
            {
                var found = FindFirstObjectByType<FloatingCapsuleController>();
                if (found != null)
                {
                    target = found.transform;
                    inputReader = found.GetComponent<HaulerInputReader>();
                }
            }

            currentDist = distance;
            if (target != null)
            {
                yaw = target.eulerAngles.y;
                smoothedPivotY = target.position.y + pivotOffset.y;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            if (inputReader != null && Cursor.lockState == CursorLockMode.Locked)
            {
                Vector2 look = inputReader.LookInput;
                yaw += look.x * mouseSensitivity;
                pitch = Mathf.Clamp(pitch - look.y * mouseSensitivity, minPitch, maxPitch);
            }

            Vector3 rawPivot = target.position + pivotOffset;
            smoothedPivotY = Mathf.Lerp(smoothedPivotY, rawPivot.y, 1f - Mathf.Exp(-verticalSmooth * Time.deltaTime));
            Vector3 pivot = new Vector3(rawPivot.x, smoothedPivotY, rawPivot.z);

            Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 desiredDir = rot * Vector3.back;
            float targetDist = distance;

            if (Physics.SphereCast(pivot, 0.22f, desiredDir, out RaycastHit hit, distance, collisionLayers, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform != target && !hit.transform.IsChildOf(target))
                {
                    targetDist = Mathf.Clamp(hit.distance - 0.15f, minDistance, distance);
                }
            }

            currentDist = Mathf.Lerp(currentDist, targetDist, 1f - Mathf.Exp(-zoomSmooth * Time.deltaTime));

            transform.position = pivot + desiredDir * currentDist;
            transform.rotation = rot;
        }
    }
}

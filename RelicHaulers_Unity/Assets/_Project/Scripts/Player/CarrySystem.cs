using UnityEngine;
using RelicHaulers.Core;
using RelicHaulers.Relics;

namespace RelicHaulers.Player
{
    /// <summary>
    /// Smart [E] priority interaction (Interactable -> Grab -> Gentle Place with Ghost Preview),
    /// [RMB] parabolic throw aim with [LMB/E] Safe Cancel, and Shoulder Bash / Push.
    /// </summary>
    [RequireComponent(typeof(HaulerInputReader), typeof(FloatingCapsuleController), typeof(Collider))]
    public class CarrySystem : MonoBehaviour
    {
        [Header("Ranges & Socket")]
        [SerializeField] private float interactRange = 2.6f;
        [SerializeField] private float grabRange = 2.4f;
        [SerializeField] private Vector3 carryOffset = new Vector3(0f, 1.05f, 1.0f);
        [SerializeField] private float socketFollowGain = 18f;
        [SerializeField] private float maxSocketSpeed = 20f;

        [Header("Throw & Shoulder Bash")]
        [SerializeField] private float baseThrowSpeed = 14.5f;
        [SerializeField] private float throwUpwardBoost = 4.2f;
        [SerializeField] private float bashStaminaCost = 15f;
        [SerializeField] private float bashForwardBoost = 6.5f;
        [SerializeField] private float bashPushImpulse = 10f;
        [SerializeField] private int trajectorySteps = 24;
        [SerializeField] private float trajectoryTimeStep = 0.055f;

        private HaulerInputReader input;
        private FloatingCapsuleController controller;
        private Collider playerCollider;
        private Transform cameraTransform;

        private IGrabbable heldItem;
        private IInteractable hoveredInteractable;
        private IGrabbable hoveredGrabbable;

        private bool isAimingThrow;
        private bool throwCancelledUntilRelease;
        private float actionCooldown;

        private LineRenderer trajectoryLine;
        private GameObject ghostPreview;
        private Vector3 pendingPlacePosition;
        private Quaternion pendingPlaceRotation;

        public IGrabbable HeldItem => heldItem;
        public bool IsAimingThrow => isAimingThrow;
        public string ActivePrompt { get; private set; } = string.Empty;

        private void Awake()
        {
            input = GetComponent<HaulerInputReader>();
            controller = GetComponent<FloatingCapsuleController>();
            playerCollider = GetComponent<Collider>();

            CreateTrajectoryLine();
            CreateGhostPreview();
        }

        private void Start()
        {
            if (Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        private void Update()
        {
            if (actionCooldown > 0f)
            {
                actionCooldown -= Time.deltaTime;
            }

            ScanTargets();
            HandleThrowAimAndCancel();
            HandleInteractAndAttack();
            UpdateGhostPreviewVisual();
            UpdatePromptText();
        }

        private void FixedUpdate()
        {
            if (heldItem == null)
            {
                return;
            }

            if (heldItem.Body == null)
            {
                heldItem = null;
                controller.SetCarrySpeedMultiplier(1f);
                return;
            }

            Vector3 targetPos = GetCarrySocketWorldPos();
            Vector3 toTarget = targetPos - heldItem.Body.position;
            if (toTarget.sqrMagnitude > 9f)
            {
                PlaceGently();
                return;
            }

            Vector3 desiredVel = Vector3.ClampMagnitude(toTarget * socketFollowGain, maxSocketSpeed);
            heldItem.Body.linearVelocity = desiredVel;
            heldItem.Body.angularVelocity = Vector3.zero;
            heldItem.Body.MoveRotation(Quaternion.Slerp(heldItem.Body.rotation, transform.rotation, 14f * Time.fixedDeltaTime));
        }

        private void ScanTargets()
        {
            hoveredInteractable = FindBestInteractable();
            hoveredGrabbable = heldItem == null ? FindBestGrabbable() : null;
        }

        private void HandleThrowAimAndCancel()
        {
            if (heldItem == null)
            {
                isAimingThrow = false;
                throwCancelledUntilRelease = false;
                trajectoryLine.enabled = false;
                return;
            }

            if (!input.IsAimThrowHeld)
            {
                if (isAimingThrow && !throwCancelledUntilRelease)
                {
                    ExecuteThrow();
                }

                isAimingThrow = false;
                throwCancelledUntilRelease = false;
                trajectoryLine.enabled = false;
                return;
            }

            if (throwCancelledUntilRelease)
            {
                isAimingThrow = false;
                trajectoryLine.enabled = false;
                return;
            }

            // Safe Cancel Throw: LMB or E while holding RMB cancels without dropping item
            if (input.ConsumeAttackPressed() || input.ConsumeInteractPressed())
            {
                throwCancelledUntilRelease = true;
                isAimingThrow = false;
                trajectoryLine.enabled = false;
                return;
            }

            isAimingThrow = true;
            DrawTrajectoryArc();
        }

        private void HandleInteractAndAttack()
        {
            if (isAimingThrow || throwCancelledUntilRelease)
            {
                return;
            }

            if (input.ConsumeInteractPressed())
            {
                // Priority 1: Always allow Interactable (Chest, Portal, Pod) without dropping held item
                if (hoveredInteractable != null)
                {
                    hoveredInteractable.Interact(gameObject);
                    return;
                }

                // Priority 2: Grab if hands are empty
                if (heldItem == null && hoveredGrabbable != null)
                {
                    GrabItem(hoveredGrabbable);
                    return;
                }

                // Priority 3: Place gently at Ghost Preview
                if (heldItem != null)
                {
                    PlaceGently();
                    return;
                }
            }

            if (actionCooldown <= 0f && input.ConsumeAttackPressed())
            {
                if (heldItem != null)
                {
                    TryShoulderBash();
                }
                else
                {
                    LightPush();
                }
            }
        }

        private void GrabItem(IGrabbable item)
        {
            heldItem = item;
            heldItem.OnGrab(playerCollider);

            float penalty = Mathf.Clamp01(heldItem.MassKg / 28f) * 0.35f;
            controller.SetCarrySpeedMultiplier(1f - penalty);
        }

        private void PlaceGently()
        {
            if (heldItem == null)
            {
                return;
            }

            ComputeGentlePlacementPose(out Vector3 placePos, out Quaternion placeRot);
            heldItem.OnPlaceGently(placePos, placeRot, playerCollider);
            heldItem = null;
            controller.SetCarrySpeedMultiplier(1f);
            ghostPreview.SetActive(false);
        }

        private void ExecuteThrow()
        {
            if (heldItem == null)
            {
                return;
            }

            Vector3 velocity = ComputeThrowVelocity(heldItem.MassKg);
            heldItem.OnThrow(velocity, playerCollider);
            heldItem = null;
            controller.SetCarrySpeedMultiplier(1f);
            trajectoryLine.enabled = false;
            ghostPreview.SetActive(false);
        }

        private void TryShoulderBash()
        {
            if (!controller.TrySpendStamina(bashStaminaCost))
            {
                return;
            }

            actionCooldown = 0.55f;
            Vector3 fwd = GetHorizontalForward();
            controller.ApplyExternalImpulse(fwd * bashForwardBoost + Vector3.up * 1.2f);
            PushLooseRigidbodies(fwd, 2.2f, bashPushImpulse);
        }

        private void LightPush()
        {
            actionCooldown = 0.35f;
            PushLooseRigidbodies(GetHorizontalForward(), 2.2f, 6.5f);
        }

        private void PushLooseRigidbodies(Vector3 dir, float range, float impulse)
        {
            Vector3 center = transform.position + Vector3.up * 0.9f + dir * 1.0f;
            Collider[] hits = Physics.OverlapSphere(center, range * 0.65f, ~0, QueryTriggerInteraction.Ignore);
            foreach (var col in hits)
            {
                if (col == playerCollider || col.transform.IsChildOf(transform))
                {
                    continue;
                }

                if (heldItem != null && col == heldItem.MainCollider)
                {
                    continue;
                }

                Rigidbody otherRb = col.attachedRigidbody;
                if (otherRb != null && !otherRb.isKinematic)
                {
                    otherRb.AddForce((dir + Vector3.up * 0.25f).normalized * impulse, ForceMode.Impulse);
                }
            }
        }

        private IInteractable FindBestInteractable()
        {
            Vector3 origin = transform.position + Vector3.up * 0.9f;
            Collider[] hits = Physics.OverlapSphere(origin, interactRange, ~0, QueryTriggerInteraction.Collide);

            IInteractable best = null;
            float bestDistSq = float.MaxValue;

            foreach (var col in hits)
            {
                var interactable = col.GetComponentInParent<IInteractable>();
                if (interactable == null || !interactable.CanInteract(gameObject))
                {
                    continue;
                }

                float dSq = (col.ClosestPoint(origin) - origin).sqrMagnitude;
                if (dSq < bestDistSq)
                {
                    bestDistSq = dSq;
                    best = interactable;
                }
            }

            return best;
        }

        private IGrabbable FindBestGrabbable()
        {
            Vector3 origin = transform.position + Vector3.up * 1.0f;
            Vector3 aimDir = cameraTransform != null ? cameraTransform.forward : transform.forward;

            Collider[] hits = Physics.OverlapSphere(origin, grabRange, ~0, QueryTriggerInteraction.Ignore);
            IGrabbable best = null;
            float bestScore = float.MaxValue;

            foreach (var col in hits)
            {
                if (col == playerCollider || col == controller.GroundCollider)
                {
                    continue;
                }

                var grabbable = col.GetComponentInParent<IGrabbable>();
                if (grabbable == null || grabbable.IsGrabbed)
                {
                    continue;
                }

                // Anti-fly exploit check: ignore item currently directly beneath player
                if (grabbable.MainCollider == controller.GroundCollider)
                {
                    continue;
                }

                Vector3 toObj = col.ClosestPoint(origin) - origin;
                float dist = toObj.magnitude;
                float anglePenalty = dist > 0.05f ? (1f - Vector3.Dot(aimDir, toObj.normalized)) * 1.2f : 0f;
                float score = dist + anglePenalty;
                if (score < bestScore)
                {
                    bestScore = score;
                    best = grabbable;
                }
            }

            return best;
        }

        private Vector3 ComputeThrowVelocity(float massKg)
        {
            Vector3 dir = cameraTransform != null
                ? (cameraTransform.forward + Vector3.up * 0.14f).normalized
                : (transform.forward + Vector3.up * 0.18f).normalized;

            float massScale = Mathf.Lerp(1.05f, 0.65f, Mathf.Clamp01(massKg / 25f));
            return dir * (baseThrowSpeed * massScale) + Vector3.up * (throwUpwardBoost * massScale);
        }

        private void ComputeGentlePlacementPose(out Vector3 pos, out Quaternion rot)
        {
            Vector3 probeOrigin = transform.position + GetHorizontalForward() * 1.35f + Vector3.up * 1.2f;
            if (Physics.Raycast(probeOrigin, Vector3.down, out RaycastHit hit, 2.4f, ~0, QueryTriggerInteraction.Ignore))
            {
                pos = hit.point + Vector3.up * 0.32f;
            }
            else
            {
                pos = transform.position + GetHorizontalForward() * 1.25f + Vector3.up * 0.55f;
            }

            rot = Quaternion.LookRotation(GetHorizontalForward(), Vector3.up);
        }

        private void UpdateGhostPreviewVisual()
        {
            bool showGhost = heldItem != null && !isAimingThrow && hoveredInteractable == null;
            if (!showGhost)
            {
                if (ghostPreview.activeSelf)
                {
                    ghostPreview.SetActive(false);
                }
                return;
            }

            ComputeGentlePlacementPose(out pendingPlacePosition, out pendingPlaceRotation);
            if (!ghostPreview.activeSelf)
            {
                ghostPreview.SetActive(true);
            }
            ghostPreview.transform.SetPositionAndRotation(pendingPlacePosition, pendingPlaceRotation);
        }

        private void DrawTrajectoryArc()
        {
            trajectoryLine.enabled = true;
            Vector3 startPos = GetCarrySocketWorldPos();
            Vector3 vel = ComputeThrowVelocity(heldItem.MassKg);

            trajectoryLine.positionCount = trajectorySteps;
            Vector3 prev = startPos;
            trajectoryLine.SetPosition(0, startPos);

            for (int i = 1; i < trajectorySteps; i++)
            {
                float t = i * trajectoryTimeStep;
                Vector3 next = startPos + vel * t + 0.5f * Physics.gravity * (t * t);

                if (Physics.Linecast(prev, next, out RaycastHit hit, ~0, QueryTriggerInteraction.Ignore))
                {
                    if (hit.collider != playerCollider && hit.collider != heldItem.MainCollider)
                    {
                        trajectoryLine.positionCount = i + 1;
                        trajectoryLine.SetPosition(i, hit.point);
                        return;
                    }
                }

                trajectoryLine.SetPosition(i, next);
                prev = next;
            }
        }

        private void UpdatePromptText()
        {
            if (isAimingThrow)
            {
                ActivePrompt = "Release [RMB] Throw  |  [LMB] or [E] Safe Cancel";
                return;
            }

            if (hoveredInteractable != null)
            {
                ActivePrompt = hoveredInteractable.InteractionPrompt;
                return;
            }

            if (heldItem != null)
            {
                ActivePrompt = "[E] Place Gently  |  Hold [RMB] Aim Throw  |  [LMB] Shoulder Bash";
                return;
            }

            if (hoveredGrabbable != null)
            {
                ActivePrompt = $"Press [E] to Grab: {hoveredGrabbable.DisplayName} ({hoveredGrabbable.MassKg:0.#}kg | ${hoveredGrabbable.CurrentValue})";
                return;
            }

            ActivePrompt = string.Empty;
        }

        private Vector3 GetCarrySocketWorldPos()
        {
            return transform.position
                + transform.right * carryOffset.x
                + Vector3.up * carryOffset.y
                + transform.forward * carryOffset.z;
        }

        private Vector3 GetHorizontalForward()
        {
            Vector3 fwd = cameraTransform != null ? cameraTransform.forward : transform.forward;
            fwd.y = 0f;
            return fwd.sqrMagnitude > 0.001f ? fwd.normalized : transform.forward;
        }

        private void CreateTrajectoryLine()
        {
            var lineGo = new GameObject("ThrowTrajectoryLine");
            lineGo.transform.SetParent(transform, false);
            trajectoryLine = lineGo.AddComponent<LineRenderer>();
            trajectoryLine.useWorldSpace = true;
            trajectoryLine.startWidth = 0.07f;
            trajectoryLine.endWidth = 0.03f;
            trajectoryLine.positionCount = trajectorySteps;
            trajectoryLine.enabled = false;

            Shader shader = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Sprites/Default");
            trajectoryLine.material = new Material(shader) { color = new Color(0.3f, 0.95f, 1f, 0.9f) };
        }

        private void CreateGhostPreview()
        {
            ghostPreview = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ghostPreview.name = "GhostPlacementPreview";
            Destroy(ghostPreview.GetComponent<Collider>());
            ghostPreview.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);

            Shader shader = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Sprites/Default");
            var mat = new Material(shader);
            mat.color = new Color(0.25f, 0.95f, 0.55f, 0.45f);
            ghostPreview.GetComponent<Renderer>().sharedMaterial = mat;
            ghostPreview.SetActive(false);
        }
    }
}

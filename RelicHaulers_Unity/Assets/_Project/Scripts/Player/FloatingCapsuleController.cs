using System;
using UnityEngine;

namespace RelicHaulers.Player
{
    /// <summary>
    /// Stylized Floating Capsule character controller for RELIC HAULERS.
    /// Follows GDD 02 & 06: spring-damper RideHeight (0.35m), FixedUpdate + Interpolate,
    /// Coyote Time (0.12s), Jump Buffer (0.15s), free Sprint, Dash (25 stamina, 22/s regen).
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(HaulerInputReader))]
    public class FloatingCapsuleController : MonoBehaviour
    {
        [Header("Floating Spring")]
        [SerializeField] private float rayOriginHeight = 1.0f;
        [SerializeField] private float extraRayLength = 0.40f;
        [SerializeField] private float rideSpringStrength = 260f;
        [SerializeField] private float rideSpringDamper = 28f;
        [SerializeField] private LayerMask groundLayers = ~0;

        [Header("Movement & Turning")]
        [SerializeField] private float walkSpeed = 6.2f;
        [SerializeField] private float sprintSpeed = 10.0f;
        [SerializeField] private float groundAcceleration = 75f;
        [SerializeField] private float airAcceleration = 32f;
        [SerializeField] private float turnSpeedDeg = 720f;
        [SerializeField] private Transform cameraTransform;

        [Header("Jump & Fall (~1.8m Snappy Arc)")]
        [SerializeField] private float jumpVelocity = 8.4f;
        [SerializeField] private float riseGravityMultiplier = 2.0f;
        [SerializeField] private float fallGravityMultiplier = 2.8f;
        [SerializeField] private float lowJumpGravityMultiplier = 3.2f;
        [SerializeField] private float coyoteTime = 0.12f;
        [SerializeField] private float jumpBufferTime = 0.15f;

        [Header("Dash & Stamina")]
        [SerializeField] private float dashSpeed = 15.0f;
        [SerializeField] private float dashDuration = 0.22f;
        [SerializeField] private float dashStaminaCost = 25f;
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float staminaRegenRate = 22f;
        [SerializeField] private float staminaRegenDelay = 0.6f;

        [Header("Greybox Safety")]
        [SerializeField] private float voidResetY = -25f;

        public event Action OnJumped;
        public event Action<float> OnLanded;
        public event Action OnDashed;

        private Rigidbody rb;
        private HaulerInputReader input;
        private Vector3 spawnPosition;

        private bool isGrounded;
        private bool wasGrounded;
        private Vector3 groundNormal = Vector3.up;
        private float coyoteCounter;
        private float jumpBufferCounter;
        private float jumpSpringDisableTimer;

        private float currentStamina;
        private float staminaDelayCounter;
        private float dashTimer;
        private Vector3 dashDirection;
        private float lastAirMinVelocityY;
        private float carrySpeedMultiplier = 1f;

        public bool IsGrounded => isGrounded;
        public bool IsDashing => dashTimer > 0f;
        public Collider GroundCollider { get; private set; }
        public float CurrentStamina => currentStamina;
        public float MaxStamina => maxStamina;
        public float HorizontalSpeed
        {
            get
            {
                Vector3 v = rb != null ? rb.linearVelocity : Vector3.zero;
                v.y = 0f;
                return v.magnitude;
            }
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<HaulerInputReader>();

            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            currentStamina = maxStamina;
            spawnPosition = transform.position;
        }

        private void Start()
        {
            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        private void Update()
        {
            if (input.ConsumeJumpPressed())
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter = Mathf.Max(0f, jumpBufferCounter - Time.deltaTime);
            }

            if (input.ConsumeDashPressed())
            {
                TryStartDash();
            }

            if (input.ConsumeResetPressed() || transform.position.y < voidResetY)
            {
                ResetToSpawn();
            }

            TickStamina(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (jumpSpringDisableTimer > 0f)
            {
                jumpSpringDisableTimer -= Time.fixedDeltaTime;
            }

            UpdateFloatingSpring();
            HandleLandingAndCoyote();
            HandleJump();
            HandleMovement();
        }

        private void UpdateFloatingSpring()
        {
            Vector3 origin = rb.position + Vector3.up * rayOriginHeight;
            float maxDist = rayOriginHeight + extraRayLength;

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, maxDist, groundLayers, QueryTriggerInteraction.Ignore))
            {
                float targetDist = rayOriginHeight;
                float currentDist = hit.distance;

                isGrounded = jumpSpringDisableTimer <= 0f
                    && rb.linearVelocity.y <= 1.2f
                    && currentDist <= (targetDist + 0.18f);
                GroundCollider = isGrounded ? hit.collider : null;
                groundNormal = hit.normal;

                if (jumpSpringDisableTimer <= 0f && dashTimer <= 0f)
                {
                    float offset = currentDist - targetDist;
                    float velAlongDown = Vector3.Dot(rb.linearVelocity, Vector3.down);
                    float springForce = (offset * rideSpringStrength) - (velAlongDown * rideSpringDamper);
                    rb.AddForce(Vector3.down * springForce, ForceMode.Acceleration);
                }
            }
            else
            {
                isGrounded = false;
                GroundCollider = null;
                groundNormal = Vector3.up;
            }
        }

        private void HandleLandingAndCoyote()
        {
            if (!isGrounded)
            {
                lastAirMinVelocityY = Mathf.Min(lastAirMinVelocityY, rb.linearVelocity.y);
                coyoteCounter = Mathf.Max(0f, coyoteCounter - Time.fixedDeltaTime);
            }
            else
            {
                coyoteCounter = coyoteTime;
                if (!wasGrounded)
                {
                    float impactSpeed = Mathf.Abs(lastAirMinVelocityY);
                    if (impactSpeed > 1.5f)
                    {
                        OnLanded?.Invoke(impactSpeed);
                    }
                    lastAirMinVelocityY = 0f;
                }
            }

            wasGrounded = isGrounded;
        }

        private void HandleJump()
        {
            if (jumpBufferCounter > 0f && coyoteCounter > 0f)
            {
                jumpBufferCounter = 0f;
                coyoteCounter = 0f;
                jumpSpringDisableTimer = 0.20f;
                isGrounded = false;

                Vector3 vel = rb.linearVelocity;
                vel.y = jumpVelocity;
                rb.linearVelocity = vel;

                OnJumped?.Invoke();
            }

            if (!isGrounded)
            {
                float gravMult = riseGravityMultiplier;
                if (rb.linearVelocity.y < 0f)
                {
                    gravMult = fallGravityMultiplier;
                }
                else if (!input.IsJumpHeld)
                {
                    gravMult = lowJumpGravityMultiplier;
                }

                rb.AddForce(Physics.gravity * (gravMult - 1f), ForceMode.Acceleration);
            }
        }

        private void HandleMovement()
        {
            if (dashTimer > 0f)
            {
                dashTimer -= Time.fixedDeltaTime;
                rb.linearVelocity = new Vector3(
                    dashDirection.x * dashSpeed,
                    Mathf.Max(rb.linearVelocity.y, 0f),
                    dashDirection.z * dashSpeed
                );
                return;
            }

            Vector2 rawMove = input.MoveInput;
            Vector3 moveDir = GetCameraRelativeDirection(rawMove);

            float baseSpeed = input.IsSprintHeld ? sprintSpeed : walkSpeed;
            float targetSpeed = baseSpeed * carrySpeedMultiplier;
            Vector3 desiredVel = moveDir * targetSpeed;

            if (isGrounded && moveDir.sqrMagnitude > 0.001f)
            {
                desiredVel = Vector3.ProjectOnPlane(desiredVel, groundNormal).normalized * targetSpeed;
            }

            Vector3 currentVel = rb.linearVelocity;
            Vector3 currentHoriz = new Vector3(currentVel.x, 0f, currentVel.z);
            Vector3 targetHoriz = new Vector3(desiredVel.x, 0f, desiredVel.z);

            float accel = isGrounded ? groundAcceleration : airAcceleration;
            Vector3 nextHoriz = Vector3.MoveTowards(currentHoriz, targetHoriz, accel * Time.fixedDeltaTime);

            rb.linearVelocity = new Vector3(nextHoriz.x, currentVel.y, nextHoriz.z);

            if (moveDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRot, turnSpeedDeg * Time.fixedDeltaTime));
            }
        }

        private void TryStartDash()
        {
            if (dashTimer > 0f || currentStamina < dashStaminaCost)
            {
                return;
            }

            currentStamina -= dashStaminaCost;
            staminaDelayCounter = staminaRegenDelay;
            dashTimer = dashDuration;

            Vector3 inputDir = GetCameraRelativeDirection(input.MoveInput);
            dashDirection = inputDir.sqrMagnitude > 0.01f ? inputDir : transform.forward;
            dashDirection.y = 0f;
            dashDirection.Normalize();

            rb.MoveRotation(Quaternion.LookRotation(dashDirection, Vector3.up));
            OnDashed?.Invoke();
        }

        private void TickStamina(float dt)
        {
            if (staminaDelayCounter > 0f)
            {
                staminaDelayCounter -= dt;
                return;
            }

            if (currentStamina < maxStamina)
            {
                currentStamina = Mathf.Min(maxStamina, currentStamina + staminaRegenRate * dt);
            }
        }

        private Vector3 GetCameraRelativeDirection(Vector2 rawInput)
        {
            if (rawInput.sqrMagnitude < 0.001f)
            {
                return Vector3.zero;
            }

            if (cameraTransform == null)
            {
                Vector3 fallback = new Vector3(rawInput.x, 0f, rawInput.y);
                return fallback.sqrMagnitude > 1f ? fallback.normalized : fallback;
            }

            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 dir = camForward * rawInput.y + camRight * rawInput.x;
            return dir.sqrMagnitude > 1f ? dir.normalized : dir;
        }

        public void SetCarrySpeedMultiplier(float multiplier)
        {
            carrySpeedMultiplier = Mathf.Clamp(multiplier, 0.45f, 1.0f);
        }

        public bool TrySpendStamina(float amount)
        {
            if (currentStamina < amount)
            {
                return false;
            }

            currentStamina -= amount;
            staminaDelayCounter = staminaRegenDelay;
            return true;
        }

        public void ApplyExternalImpulse(Vector3 velocityDelta)
        {
            rb.linearVelocity += velocityDelta;
        }

        public void SetSpawnPosition(Vector3 pos)
        {
            spawnPosition = pos;
        }

        public void ResetToSpawn()
        {
            dashTimer = 0f;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = spawnPosition;
            transform.position = spawnPosition;
        }
    }
}

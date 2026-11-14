using System.Collections;
using UnityEngine;

namespace RelicHaulers.Relics
{
    /// <summary>
    /// Gate 1 physical Relic / Crate item implementing weight, impact fragility,
    /// collision-grace on gentle place, and conversion to indestructible Scrap Shard at 0 HP.
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class RelicItem : MonoBehaviour, IGrabbable
    {
        [Header("Relic Stats")]
        [SerializeField] private string displayName = "Bình Pha Lê Mẫu";
        [SerializeField] private int baseValue = 180;
        [SerializeField] private float massKg = 4f;
        [SerializeField] private float maxDurability = 100f;
        [SerializeField] private float breakThresholdSpeed = 4.0f;
        [SerializeField] private float damagePerSpeed = 14f;

        private Rigidbody rb;
        private Collider mainCol;
        private Renderer cachedRenderer;
        private Color originalColor = Color.white;

        private float currentDurability;
        private float collisionGraceTimer = 0.6f;
        private bool isGrabbed;
        private bool isScrapShard;
        private Collider ignoredPlayerCollider;

        public string DisplayName => isScrapShard ? $"{displayName} (Vỡ)" : displayName;
        public int CurrentValue => isScrapShard
            ? Mathf.Max(5, Mathf.RoundToInt(baseValue * 0.15f))
            : Mathf.RoundToInt(baseValue * Mathf.Clamp(currentDurability / Mathf.Max(1f, maxDurability), 0.25f, 1.0f));

        public float MassKg => isScrapShard ? 1f : massKg;
        public float CurrentDurability => currentDurability;
        public float MaxDurability => maxDurability;
        public bool IsGrabbed => isGrabbed;
        public bool IsScrapShard => isScrapShard;
        public Rigidbody Body => rb;
        public Collider MainCollider => mainCol;

        public void Configure(string itemName, int value, float weightKg, float durability, float breakSpeed)
        {
            displayName = itemName;
            baseValue = value;
            massKg = weightKg;
            maxDurability = durability;
            currentDurability = durability;
            breakThresholdSpeed = breakSpeed;
            if (rb != null)
            {
                rb.mass = Mathf.Max(1f, weightKg);
            }
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            mainCol = GetComponent<Collider>();
            cachedRenderer = GetComponentInChildren<Renderer>();
            if (cachedRenderer != null && cachedRenderer.sharedMaterial != null)
            {
                originalColor = cachedRenderer.sharedMaterial.color;
            }

            rb.mass = Mathf.Max(1f, massKg);
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            currentDurability = maxDurability;
        }

        private void Update()
        {
            if (collisionGraceTimer > 0f)
            {
                collisionGraceTimer -= Time.deltaTime;
            }
        }

        public void GrantCollisionGrace(float seconds)
        {
            collisionGraceTimer = Mathf.Max(collisionGraceTimer, seconds);
        }

        public void OnGrab(Collider playerCollider)
        {
            isGrabbed = true;
            ignoredPlayerCollider = playerCollider;
            if (playerCollider != null && mainCol != null)
            {
                Physics.IgnoreCollision(mainCol, playerCollider, true);
            }

            rb.useGravity = false;
            rb.linearDamping = 6f;
            rb.angularDamping = 8f;
        }

        public void OnPlaceGently(Vector3 worldPos, Quaternion worldRot, Collider playerCollider)
        {
            ReleasePhysicsState(playerCollider, 0.55f);
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.SetPositionAndRotation(worldPos, worldRot);
            rb.position = worldPos;
            rb.rotation = worldRot;
        }

        public void OnThrow(Vector3 velocity, Collider playerCollider)
        {
            ReleasePhysicsState(playerCollider, 0.12f);
            rb.linearVelocity = velocity;
        }

        private void ReleasePhysicsState(Collider playerCollider, float graceSeconds)
        {
            isGrabbed = false;
            rb.useGravity = true;
            rb.linearDamping = 0.05f;
            rb.angularDamping = 0.05f;
            GrantCollisionGrace(graceSeconds);

            Collider colToRestore = playerCollider != null ? playerCollider : ignoredPlayerCollider;
            if (colToRestore != null && mainCol != null)
            {
                StartCoroutine(RestorePlayerCollisionDelayed(colToRestore, 0.2f));
            }
        }

        private IEnumerator RestorePlayerCollisionDelayed(Collider playerCol, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (playerCol != null && mainCol != null)
            {
                Physics.IgnoreCollision(mainCol, playerCol, false);
            }
            if (ignoredPlayerCollider == playerCol)
            {
                ignoredPlayerCollider = null;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (isGrabbed || isScrapShard || collisionGraceTimer > 0f)
            {
                return;
            }

            if (ignoredPlayerCollider != null && collision.collider == ignoredPlayerCollider)
            {
                return;
            }

            float relSpeed = collision.relativeVelocity.magnitude;
            if (relSpeed <= breakThresholdSpeed)
            {
                return;
            }

            float dmg = (relSpeed - breakThresholdSpeed) * damagePerSpeed;
            currentDurability = Mathf.Max(0f, currentDurability - dmg);
            UpdateDamageTint();

            if (currentDurability <= 0f)
            {
                ShatterToScrap();
            }
        }

        private void UpdateDamageTint()
        {
            if (cachedRenderer == null)
            {
                return;
            }

            float ratio = Mathf.Clamp01(currentDurability / Mathf.Max(1f, maxDurability));
            Color tinted = Color.Lerp(new Color(0.85f, 0.25f, 0.25f), originalColor, ratio);
            cachedRenderer.material.color = tinted;
        }

        private void ShatterToScrap()
        {
            isScrapShard = true;
            rb.mass = 1f;
            transform.localScale *= 0.58f;
            if (cachedRenderer != null)
            {
                cachedRenderer.material.color = new Color(0.35f, 0.35f, 0.38f);
            }
        }
    }
}

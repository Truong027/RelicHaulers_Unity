using UnityEngine;

namespace RelicHaulers.Relics
{
    /// <summary>
    /// Physics carry contract for all Relics, Crates, and Ragdoll-stunned small enemies.
    /// </summary>
    public interface IGrabbable
    {
        string DisplayName { get; }
        int CurrentValue { get; }
        float MassKg { get; }
        float CurrentDurability { get; }
        float MaxDurability { get; }
        bool IsGrabbed { get; }
        Rigidbody Body { get; }
        Collider MainCollider { get; }

        void OnGrab(Collider playerCollider);
        void OnPlaceGently(Vector3 worldPos, Quaternion worldRot, Collider playerCollider);
        void OnThrow(Vector3 velocity, Collider playerCollider);
        void GrantCollisionGrace(float seconds);
    }
}

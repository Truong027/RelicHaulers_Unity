using UnityEngine;
using RelicHaulers.Core;

namespace RelicHaulers.Relics
{
    /// <summary>
    /// Gate 1 Chest (1 standard type): press [E] to pop lid and eject a sample Greybox Relic.
    /// </summary>
    public class GreyboxChest : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform lidPivot;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Material lootMaterial;
        [SerializeField] private string lootName = "Cổ Vật Rương Mẫu";
        [SerializeField] private int lootValue = 200;
        [SerializeField] private float lootMassKg = 6f;
        [SerializeField] private float lootDurability = 120f;
        [SerializeField] private float lootBreakSpeed = 4.5f;

        private bool isOpened;
        private float lidTargetAngle;
        private float lidCurrentAngle;

        public string InteractionPrompt => isOpened ? string.Empty : "Press [E] to Open Chest";

        public bool CanInteract(GameObject interactor) => !isOpened;

        public void Interact(GameObject interactor)
        {
            if (isOpened)
            {
                return;
            }

            isOpened = true;
            lidTargetAngle = -105f;
            SpawnRelic();
        }

        private void Update()
        {
            if (lidPivot == null || Mathf.Approximately(lidCurrentAngle, lidTargetAngle))
            {
                return;
            }

            lidCurrentAngle = Mathf.MoveTowards(lidCurrentAngle, lidTargetAngle, 360f * Time.deltaTime);
            lidPivot.localRotation = Quaternion.Euler(lidCurrentAngle, 0f, 0f);
        }

        private void SpawnRelic()
        {
            Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position + Vector3.up * 0.9f;
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "Relic_ChestLoot";
            go.transform.position = pos;
            go.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);

            if (lootMaterial != null)
            {
                go.GetComponent<Renderer>().sharedMaterial = lootMaterial;
            }

            var relic = go.AddComponent<RelicItem>();
            relic.Configure(lootName, lootValue, lootMassKg, lootDurability, lootBreakSpeed);
            relic.GrantCollisionGrace(0.75f);

            if (relic.Body != null)
            {
                relic.Body.linearVelocity = Vector3.up * 3.8f + transform.forward * 1.4f;
            }
        }
    }
}

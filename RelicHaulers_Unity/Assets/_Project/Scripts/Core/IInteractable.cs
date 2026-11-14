using UnityEngine;

namespace RelicHaulers.Core
{
    /// <summary>
    /// Priority-1 interaction contract (Pod, Chest, Portal, NPC) so [E] never drops held relics accidentally.
    /// </summary>
    public interface IInteractable
    {
        string InteractionPrompt { get; }
        bool CanInteract(GameObject interactor);
        void Interact(GameObject interactor);
    }
}

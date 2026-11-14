using UnityEngine;
using UnityEngine.SceneManagement;

namespace RelicHaulers.Core
{
    /// <summary>
    /// Simple Gate 1 portal pad implementing IInteractable so [E] routes cleanly through CarrySystem priority.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ScenePortalPad : MonoBehaviour, IInteractable
    {
        [SerializeField] private string targetSceneName = "Greybox_Prototype";
        [SerializeField] private string promptText = "Press [E] to travel";

        public string InteractionPrompt => promptText;

        private void Reset()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        public bool CanInteract(GameObject interactor) => true;

        public void Interact(GameObject interactor)
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }
}

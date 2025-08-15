using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public InteractionDetector interactionDetector; // Reference to the interaction detector

    private void Update()
    {
        HandleInteraction();
    }

    // ---------------- Interaction ----------------

    // Calls the interaction detector to check for interactions
    private void HandleInteraction()
    {
        if (interactionDetector != null)
            interactionDetector.OnInteract();
    }
}

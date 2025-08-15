using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange = null; // Current interactable in range
    public GameObject interactionIcon;                // UI icon to show when interaction is possible

    private void Start()
    {
        InitializeInteractionIcon();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckEnterInteractable(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CheckExitInteractable(collision);
    }

    // Called from PlayerInteraction to attempt interaction
    public void OnInteract()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactableInRange != null)
        {
            interactableInRange.Interact();
        }
    }

    // ---------------- Initialization ----------------

    // Hide the interaction icon at the start
    private void InitializeInteractionIcon()
    {
        if (interactionIcon != null)
            interactionIcon.SetActive(false);
    }

    // ---------------- Trigger Handling ----------------

    // Check if an interactable object entered the trigger
    private void CheckEnterInteractable(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactableInRange = interactable;
            if (interactionIcon != null)
                interactionIcon.SetActive(true);
        }
    }

    // Check if the interactable object exited the trigger
    private void CheckExitInteractable(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
        {
            interactableInRange = null;
            if (interactionIcon != null)
                interactionIcon.SetActive(false);
        }
    }
}

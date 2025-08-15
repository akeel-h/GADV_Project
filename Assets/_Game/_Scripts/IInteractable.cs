// Interface for objects that the player can interact with
public interface IInteractable
{
    // Called when the player performs an interaction (e.g., pressing a key)
    void Interact();

    // Returns true if the object can currently be interacted with
    bool CanInteract();
}

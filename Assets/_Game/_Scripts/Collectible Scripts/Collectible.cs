using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    [Header("Collectible Data")]
    public int ID;        // Unique collectible ID
    public string Name;   // Display name
    public string Text;   // Description or story text

    // Called when the collectible is picked up
    public virtual void Pickup()
    {
        // Get this collectible's sprite
        Sprite collectibleIcon = GetComponent<Image>().sprite;

        // Show item pickup UI if controller is available
        if (ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(Name, collectibleIcon);
        }
    }
}

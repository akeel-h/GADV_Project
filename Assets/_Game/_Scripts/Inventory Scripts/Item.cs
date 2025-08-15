using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [Header("Item Data")]
    public int ID;                    // Unique ID for the item
    public string Name;               // Display name of the item
    public float sanityReduction;     // How much this item affects sanity
    public bool addToInventory = true; // Should this item be added to inventory upon pickup

    // Called when the item is picked up
    public virtual void Pickup()
    {
        // Get the item's sprite
        Sprite itemIcon = GetComponent<Image>().sprite;

        // Show the item pickup UI if the controller exists
        if (ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(Name, itemIcon);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int ID;
    public string Name;
    public float sanityReduction;

    public bool addToInventory = true;

    public virtual void Pickup()
    {
        Sprite itemIcon = GetComponent<Image>().sprite;
        if (ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(Name, itemIcon);
        }
    }
}

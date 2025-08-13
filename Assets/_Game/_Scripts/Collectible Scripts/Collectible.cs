using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    public int ID;
    public string Name;
    public string Text;

    public virtual void Pickup()
    {
        Sprite collectibleIcon = GetComponent<Image>().sprite;
        if (ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(Name, collectibleIcon);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Serializable class used to store inventory state for saving/loading
[System.Serializable]
public class InventorySaveData
{
    // The unique ID of the item in this slot
    public int itemID;

    // The index of the slot where the item is stored
    public int slotIndex;
}

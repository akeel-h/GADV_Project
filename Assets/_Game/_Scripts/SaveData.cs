using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Stores the player's position in the world
    public Vector3 playerPosition;

    // Name or ID of the map boundary the player is currently in
    public string mapBoundary;

    // List of items saved from the player's inventory
    public List<InventorySaveData> inventorySaveData;
}

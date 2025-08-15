using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    [Header("Item Prefabs")]
    public List<Item> itemPrefabs; // List of item prefabs to populate the dictionary

    private Dictionary<int, GameObject> itemDictionary; // Maps item ID to prefab

    private void Awake()
    {
        InitializeDictionary();
    }

    // ---------------- Initialization ----------------

    // Sets up the dictionary and assigns IDs to items
    private void InitializeDictionary()
    {
        itemDictionary = new Dictionary<int, GameObject>();

        // Assign unique IDs to items
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].ID = i + 1; // IDs start at 1
            }
        }

        // Fill dictionary with item ID -> prefab mapping
        foreach (Item item in itemPrefabs)
        {
            if (item != null)
                itemDictionary[item.ID] = item.gameObject;
        }
    }

    // ---------------- Public Methods ----------------

    // Retrieve an item prefab by its ID
    public GameObject GetItemPrefab(int itemID)
    {
        itemDictionary.TryGetValue(itemID, out GameObject prefab);

        if (prefab == null)
            Debug.LogWarning($"Item with ID {itemID} not found in dictionary");

        return prefab;
    }
}

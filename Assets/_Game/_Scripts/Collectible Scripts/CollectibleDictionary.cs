using System.Collections.Generic;
using UnityEngine;

public class CollectibleDictionary : MonoBehaviour
{
    public List<Collectible> collectiblePrefabs; // List of collectible prefab scripts
    private Dictionary<int, GameObject> collectibleDictionary; // Maps ID to prefab

    private void Awake()
    {
        InitializeDictionary();
    }

    // ---------------- Initialization ----------------

    // Fill the collectible dictionary from the prefab list
    private void InitializeDictionary()
    {
        collectibleDictionary = new Dictionary<int, GameObject>();

        for (int i = 0; i < collectiblePrefabs.Count; i++)
        {
            if (collectiblePrefabs[i] != null)
            {
                collectiblePrefabs[i].ID = i; // Assign ID to collectible
                collectibleDictionary.Add(i, collectiblePrefabs[i].gameObject); // Store in dictionary
            }
        }
    }

    // ---------------- Public Methods ----------------

    // Retrieve collectible prefab by ID
    public GameObject GetCollectiblePrefab(int collectibleID)
    {
        collectibleDictionary.TryGetValue(collectibleID, out GameObject prefab);

        if (prefab == null)
            Debug.LogWarning($"Item with ID {collectibleID} not found in dictionary");

        return prefab;
    }
}

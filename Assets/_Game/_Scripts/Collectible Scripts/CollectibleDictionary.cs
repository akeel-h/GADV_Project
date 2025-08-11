using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleDictionary : MonoBehaviour
{
    public List<Collectible> collectiblePrefabs;
    private Dictionary<int, GameObject> collectibleDictionary;

    private void Awake()
    {
        collectibleDictionary = new Dictionary<int, GameObject>();

        for (int i = 0; i < collectiblePrefabs.Count; i++)
        {
            if (collectiblePrefabs[i] != null)
            {
                collectiblePrefabs[i].ID = i; 
                collectibleDictionary.Add(i, collectiblePrefabs[i].gameObject); 
            }
        }
    }

    public GameObject GetCollectiblePrefab(int collectibleID)
    {
        collectibleDictionary.TryGetValue(collectibleID, out GameObject prefab);
        if (prefab == null)
        {
            Debug.LogWarning($"Item with ID {collectibleID} not found in dictionary");
        }
        return prefab;
    }
}

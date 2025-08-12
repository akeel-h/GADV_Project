using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class CollectibleController : MonoBehaviour
{
    private CollectibleDictionary collectibleDictionary;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] collectiblePrefabs;
    // Start is called before the first frame update
    void Start()
    {
        collectibleDictionary = FindObjectOfType<CollectibleDictionary>();

        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            if (i < collectiblePrefabs.Length)
            {
                GameObject item = Instantiate(collectiblePrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
            }
        }
    }

    public bool AddCollectible(GameObject collectiblePrefabs)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newCollectible = Instantiate(collectiblePrefabs, slot.transform);
                newCollectible.transform.localScale = Vector3.one * 1f;
                newCollectible.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newCollectible;
                return true;
            }
        }

        Debug.Log("Inventory is full!");
        return false;

    }
}

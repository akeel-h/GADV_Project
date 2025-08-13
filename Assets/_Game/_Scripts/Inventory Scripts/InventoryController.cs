using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance {  get; private set; }
    private ItemDictionary itemDictionary;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();

        //for (int i = 0; i < slotCount; i++)
        //{
        //    Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
        //    if(i < itemPrefabs.Length)
        //    {
        //        GameObject item = Instantiate(itemPrefabs[i], slot.transform);
        //        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //        slot.currentItem = item;
        //    }
        //}
    }

    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                invData.Add(new InventorySaveData { itemID = item.ID, slotIndex = slotTransform.GetSiblingIndex() });
            }
        }
        return invData;
    }

    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        foreach(Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        foreach(InventorySaveData data in inventorySaveData)
        {
            if(data.slotIndex < slotCount)
            {
                Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.slotIndex);
                if(itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }
        }
    }

    public bool AddItem(GameObject itemPrefabs)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefabs, slot.transform);
                newItem.transform.localScale = Vector3.one;
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
                return true;
            }
        }

        Debug.Log("Inventory is full!");
        return false;

    }

    public GameObject GetItemPrefabFromDictionary(int id)
    {
        if (itemDictionary == null) itemDictionary = FindObjectOfType<ItemDictionary>();

        return itemDictionary.GetItemPrefab(id);
    }
}

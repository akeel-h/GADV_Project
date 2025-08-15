using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance { get; private set; } // Singleton reference
    private ItemDictionary itemDictionary; // Reference to item dictionary

    [Header("Inventory Settings")]
    public GameObject inventoryPanel; // Parent container for inventory slots
    public GameObject slotPrefab;     // Prefab for a slot
    public int slotCount;             // Number of slots in the inventory
    public GameObject[] itemPrefabs;  // Optional manual item prefab array

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        InitializeDictionary();
    }

    // ---------------- Initialization ----------------

    // Ensures this is the singleton instance
    private void InitializeSingleton()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Finds the ItemDictionary in the scene
    private void InitializeDictionary()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();
    }

    // ---------------- Inventory Management ----------------

    // Returns the list of items in inventory with their IDs and slot indices
    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                invData.Add(new InventorySaveData
                {
                    itemID = item.ID,
                    slotIndex = slotTransform.GetSiblingIndex()
                });
            }
        }

        return invData;
    }

    // Loads inventory items from save data
    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        ClearInventorySlots();
        CreateEmptySlots();
        PopulateSlotsFromSave(inventorySaveData);
    }

    // Adds an item to the first empty slot
    public bool AddItem(GameObject itemPrefab)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                AddItemToSlot(slot, itemPrefab);
                return true;
            }
        }

        Debug.Log("Inventory is full!");
        return false;
    }

    // Retrieves a prefab from the dictionary using its ID
    public GameObject GetItemPrefabFromDictionary(int id)
    {
        if (itemDictionary == null)
            InitializeDictionary();

        return itemDictionary.GetItemPrefab(id);
    }

    // ---------------- Private Helpers ----------------

    private void ClearInventorySlots()
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateEmptySlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }
    }

    private void PopulateSlotsFromSave(List<InventorySaveData> inventorySaveData)
    {
        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex < slotCount)
            {
                Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.slotIndex);
                if (itemPrefab != null)
                {
                    AddItemToSlot(slot, itemPrefab);
                }
            }
        }
    }

    private void AddItemToSlot(Slot slot, GameObject itemPrefab)
    {
        GameObject newItem = Instantiate(itemPrefab, slot.transform);
        newItem.transform.localScale = Vector3.one;
        newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        slot.currentItem = newItem;
    }
}

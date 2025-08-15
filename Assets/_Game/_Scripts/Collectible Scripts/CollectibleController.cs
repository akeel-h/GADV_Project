using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    // Reference to the dictionary containing all collectible prefabs
    private CollectibleDictionary collectibleDictionary;

    [Header("Inventory Settings")]
    public GameObject inventoryPanel;      // Parent UI panel for inventory slots
    public GameObject slotPrefab;          // Prefab for inventory slots
    public int slotCount;                   // Number of slots to create
    public GameObject[] collectiblePrefabs; // Initial collectibles to place in slots

    void Start()
    {
        InitializeInventory();
    }

    // ---------------- Initialization ----------------

    // Set up dictionary and create initial inventory slots
    private void InitializeInventory()
    {
        // Find the collectible dictionary in the scene
        collectibleDictionary = FindObjectOfType<CollectibleDictionary>();

        // Create slots and populate with starting collectibles
        CreateInventorySlots();
    }

    // Create the inventory slots and add starting items if available
    private void CreateInventorySlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();

            if (i < collectiblePrefabs.Length)
                AddStartingCollectibleToSlot(slot, collectiblePrefabs[i]);
        }
    }

    // Add a collectible to a specific slot at start
    private void AddStartingCollectibleToSlot(Slot slot, GameObject prefab)
    {
        GameObject item = Instantiate(prefab, slot.transform);
        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        slot.currentItem = item;
    }

    // ---------------- Public Methods ----------------

    // Add a collectible to the first available empty slot
    public bool AddCollectible(GameObject collectiblePrefab)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null && slot.currentItem == null)
            {
                AddCollectibleToSlot(slot, collectiblePrefab);
                return true;
            }
        }

        Debug.Log("Inventory is full!");
        return false;
    }

    // Get a collectible prefab by ID from the dictionary
    public GameObject GetCollectiblePrefabFromDictionary(int id)
    {
        if (collectibleDictionary == null)
            collectibleDictionary = FindObjectOfType<CollectibleDictionary>();

        return collectibleDictionary.GetCollectiblePrefab(id);
    }

    // ---------------- Private Helpers ----------------

    // Place a collectible prefab in a specific slot
    private void AddCollectibleToSlot(Slot slot, GameObject prefab)
    {
        GameObject newCollectible = Instantiate(prefab, slot.transform);
        newCollectible.transform.localScale = Vector3.one;
        newCollectible.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        slot.currentItem = newCollectible;
    }
}

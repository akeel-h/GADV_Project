using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using System.Collections.Generic;

public class DoorTileController : MonoBehaviour
{
    public enum DoorOrientation { Vertical, Horizontal }

    [Header("Tilemap Settings")]
    public Tilemap tilemap;
    [Tooltip("Tiles in order from bottom/left to top/right")]
    public List<TileBase> closedDoorTiles;
    public List<TileBase> openDoorTiles;
    public Vector3Int doorBottomPosition; // Starting position of the first tile
    public DoorOrientation orientation = DoorOrientation.Vertical;

    [Header("UI Prompt")]
    public TextMeshProUGUI promptText;

    [Header("Door Settings")]
    [Tooltip("IDs of items required to open this door")]
    public List<int> requiredItemIDs = new List<int>();

    public ItemDictionary itemDictionary;

    private bool playerNear = false;
    private bool isDoorOpen = false;

    // Lifecycle: call initialization
    void Start()
    {
        InitializePrompt();
    }

    // Lifecycle: call input check
    void Update()
    {
        CheckPlayerInput();
    }

    // Lifecycle: call trigger enter handling
    void OnTriggerEnter2D(Collider2D other)
    {
        HandlePlayerEnter(other);
    }

    // Lifecycle: call trigger exit handling
    void OnTriggerExit2D(Collider2D other)
    {
        HandlePlayerExit(other);
    }

    // --------------------- Methods for Lifecycle ---------------------

    // Initialize UI prompt
    void InitializePrompt()
    {
        if (promptText != null)
            promptText.text = "";
    }

    // Check for player pressing F to open door
    void CheckPlayerInput()
    {
        if (playerNear && !isDoorOpen && Input.GetKeyDown(KeyCode.F))
        {
            InventoryController inventory = InventoryController.Instance;

            if (inventory != null && HasAllItems(inventory, requiredItemIDs))
            {
                OpenDoor();
                RemoveItems(inventory, requiredItemIDs);
                ClearPrompt();
            }
            else
            {
                ShowRequiredItemsPrompt();
            }
        }
    }

    // Handle player entering trigger
    void HandlePlayerEnter(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDoorOpen)
        {
            playerNear = true;
            ShowPrompt("Press F to open door");
        }
    }

    // Handle player exiting trigger
    void HandlePlayerExit(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            ClearPrompt();
        }
    }

    // --------------------- Door Logic ---------------------

    // Open the door tiles
    void OpenDoor()
    {
        for (int i = 0; i < openDoorTiles.Count; i++)
        {
            Vector3Int tilePos = doorBottomPosition;

            if (orientation == DoorOrientation.Vertical)
                tilePos += new Vector3Int(0, i, 0);
            else
                tilePos += new Vector3Int(i, 0, 0);

            if (i < openDoorTiles.Count)
                tilemap.SetTile(tilePos, openDoorTiles[i]);
        }

        isDoorOpen = true;
    }

    // Check if inventory contains all required items
    bool HasAllItems(InventoryController inventory, List<int> itemIDs)
    {
        foreach (int id in itemIDs)
        {
            if (!HasItem(inventory, id))
                return false;
        }
        return true;
    }

    // Check if inventory contains a single item
    bool HasItem(InventoryController inventory, int itemID)
    {
        foreach (Transform slotTransform in inventory.inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                if (item != null && item.ID == itemID)
                    return true;
            }
        }
        return false;
    }

    // Remove multiple items from inventory
    void RemoveItems(InventoryController inventory, List<int> itemIDs)
    {
        foreach (int id in itemIDs)
            RemoveItem(inventory, id);
    }

    // Remove a single item from inventory
    void RemoveItem(InventoryController inventory, int itemID)
    {
        foreach (Transform slotTransform in inventory.inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                if (item != null && item.ID == itemID)
                {
                    Destroy(slot.currentItem);
                    slot.currentItem = null;
                    break;
                }
            }
        }
    }

    // --------------------- UI Prompt ---------------------

    void ShowPrompt(string message)
    {
        if (promptText != null)
            promptText.text = message;
    }

    void ClearPrompt()
    {
        if (promptText != null)
            promptText.text = "";
    }

    void ShowRequiredItemsPrompt()
    {
        if (promptText != null)
            promptText.text = GetRequiredItemsText();
    }

    // --------------------- Utility ---------------------

    public bool IsDoorOpen() => isDoorOpen;

    string GetRequiredItemsText()
    {
        List<string> itemNames = new List<string>();
        foreach (int id in requiredItemIDs)
        {
            GameObject prefab = itemDictionary.GetItemPrefab(id);
            if (prefab != null)
                itemNames.Add(prefab.name);
        }
        return string.Join(" + ", itemNames) + " required";
    }
}

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

    private bool playerNear = false;
    private bool isDoorOpen = false;

    public ItemDictionary itemDictionary;

    void Start()
    {
        if (promptText != null)
            promptText.text = "";
    }

    void Update()
    {
        if (playerNear && !isDoorOpen && Input.GetKeyDown(KeyCode.F))
        {
            InventoryController inventory = InventoryController.Instance;
            if (inventory != null && HasAllItems(inventory, requiredItemIDs))
            {
                OpenDoor();
                RemoveItems(inventory, requiredItemIDs);
                if (promptText != null) promptText.text = "";
            }
            else
            {
                if (promptText != null)
                    promptText.text = GetRequiredItemsText();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDoorOpen)
        {
            playerNear = true;
            if (promptText != null) promptText.text = "Press F to open door";
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            if (promptText != null) promptText.text = "";
        }
    }

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

    bool HasAllItems(InventoryController inventory, List<int> itemIDs)
    {
        foreach (int id in itemIDs)
        {
            if (!HasItem(inventory, id))
                return false;
        }
        return true;
    }

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

    void RemoveItems(InventoryController inventory, List<int> itemIDs)
    {
        foreach (int id in itemIDs)
            RemoveItem(inventory, id);
    }

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

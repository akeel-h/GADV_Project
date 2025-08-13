using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class DoorTileController : MonoBehaviour
{
    [Header("Tilemap Settings")]
    public Tilemap tilemap;
    public TileBase closedDoorTileBottom;
    public TileBase closedDoorTileMiddle;
    public TileBase closedDoorTileTop;
    public TileBase openDoorTileBottom;
    public TileBase openDoorTileMiddle;
    public TileBase openDoorTileTop;
    public Vector3Int doorBottomPosition;

    [Header("UI Prompt")]
    public TextMeshProUGUI promptText;

    [Header("Door Settings")]
    public int requiredItemID = 1; // ID of Silver Key in ItemDictionary

    private bool playerNear = false;
    private bool isDoorOpen = false;

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
            if (inventory != null && HasItem(inventory, requiredItemID))
            {
                OpenDoor();
                RemoveItem(inventory, requiredItemID);
                if (promptText != null) promptText.text = "";
            }
            else
            {
                if (promptText != null) promptText.text = "A Silver Key is required";
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
        Vector3Int middlePos = doorBottomPosition + new Vector3Int(0, 1, 0);
        Vector3Int topPos = doorBottomPosition + new Vector3Int(0, 2, 0);

        tilemap.SetTile(doorBottomPosition, openDoorTileBottom);
        tilemap.SetTile(middlePos, openDoorTileMiddle);
        tilemap.SetTile(topPos, openDoorTileTop);

        isDoorOpen = true;
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

    public bool IsDoorOpen()
    {
        return isDoorOpen;
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
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public bool IsOpened { get; private set; }
    public string ChestID { get; private set; }
    public GameObject itemPrefab;
    public Sprite openedSprite;
    public ItemDictionary itemDictionary;

    [Header("Optional Required Item")]
    public bool requiresItem = false;
    public int requiredItemID = 0;
    public TextMeshProUGUI promptText;

    // ---------------- IInteractable ----------------

    // Can the player interact with this chest?
    public bool CanInteract()
    {
        return !IsOpened;
    }

    // Called when player interacts with the chest
    public void Interact()
    {
        if (IsOpened) return;

        if (requiresItem && !PlayerHasRequiredItem())
        {
            ShowRequiredItemPrompt();
            return;
        }

        OpenChest();
    }

    // ---------------- Unity Lifecycle ----------------

    private void Start()
    {
        InitializeChest();
    }

    // ---------------- Chest Logic ----------------

    private void InitializeChest()
    {
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
    }

    private void OpenChest()
    {
        SetOpened(true);
        ClearPrompt();

        if (itemPrefab != null)
        {
            // Spawn item slightly below chest
            Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
        }
    }

    public void SetOpened(bool opened)
    {
        IsOpened = opened;

        if (IsOpened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }

    // ---------------- Player Item Requirement ----------------

    private bool PlayerHasRequiredItem()
    {
        InventoryController inventory = InventoryController.Instance;
        if (inventory == null) return false;

        foreach (Transform slotTransform in inventory.inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                if (item != null && item.ID == requiredItemID)
                    return true;
            }
        }
        return false;
    }

    private void ShowRequiredItemPrompt()
    {
        if (promptText == null) return;

        string itemName = "Required Item";

        if (itemDictionary != null)
        {
            GameObject prefab = itemDictionary.GetItemPrefab(requiredItemID);
            if (prefab != null) itemName = prefab.name;
        }

        promptText.text = $"You need {itemName} to open this chest!";
    }

    private void ClearPrompt()
    {
        if (promptText != null)
            promptText.text = "";
    }

    // ---------------- Player Proximity ----------------

    public void OnPlayerEnter()
    {
        if (!IsOpened && requiresItem && !PlayerHasRequiredItem())
        {
            ShowRequiredItemPrompt();
        }
    }

    public void OnPlayerExit()
    {
        ClearPrompt();
    }
}

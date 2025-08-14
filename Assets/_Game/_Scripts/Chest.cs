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


    public bool CanInteract()
    {
        return !IsOpened;
    }

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



    public void Interact()
    {
        if (IsOpened) return;

        if (requiresItem && !PlayerHasRequiredItem())
        {
            if (promptText != null)
            {
                GameObject prefab = itemDictionary.GetItemPrefab(requiredItemID);
                string itemName = prefab != null ? prefab.name : "Required Item";
                promptText.text = $"You need {itemName} to open this chest!";
            }
            return;
        }

        OpenChest();
    }



    // Start is called before the first frame update
    void Start()
    {
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
    }

    private void OpenChest()
    {
        SetOpened(true);

        if (promptText != null)
            promptText.text = "";

        if (itemPrefab)
        {
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

    public void OnPlayerEnter()
    {
        if (!IsOpened)
        {
            if (requiresItem && !PlayerHasRequiredItem())
            {
                if (promptText != null)
                {
                    string itemName = "Required Item";

                    // Only try to get prefab name if itemDictionary exists
                    if (itemDictionary != null)
                    {
                        GameObject prefab = itemDictionary.GetItemPrefab(requiredItemID);
                        if (prefab != null) itemName = prefab.name;
                    }

                    promptText.text = $"You need {itemName} to open this chest!";
                }
                return;
            }

        }
    }


    public void OnPlayerExit()
    {
        if (promptText != null)
            promptText.text = "";
    }

}

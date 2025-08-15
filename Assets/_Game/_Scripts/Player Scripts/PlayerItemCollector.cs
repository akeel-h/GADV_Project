using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;
    private CollectibleController collectibleController;

    private HashSet<GameObject> pickedUpObjects = new HashSet<GameObject>(); // Prevent double pickups

    public SanitySystem sanitySystem; // Reference to the player's sanity system

    private void Start()
    {
        InitializeReferences();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandlePickupCollision(collision);
    }

    private void LateUpdate()
    {
        ResetPickedUpObjects();
    }

    // ---------------- Initialization ----------------

    // Grab references to inventory, collectibles, and sanity system
    private void InitializeReferences()
    {
        inventoryController = FindObjectOfType<InventoryController>();
        collectibleController = FindObjectOfType<CollectibleController>();

        if (sanitySystem == null)
            sanitySystem = FindObjectOfType<SanitySystem>();
    }

    // ---------------- Pickup Handling ----------------

    // Processes collisions for picking up items or collectibles
    private void HandlePickupCollision(Collider2D collision)
    {
        if (pickedUpObjects.Contains(collision.gameObject))
            return;

        pickedUpObjects.Add(collision.gameObject);

        if (collision.CompareTag("Item"))
        {
            HandleItemPickup(collision);
        }
        else if (collision.CompareTag("Collectible"))
        {
            HandleCollectiblePickup(collision);
        }
    }

    private void HandleItemPickup(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item == null) return;

        ApplySanityReduction(item);
        PickupItemToInventory(item, collision.gameObject);
        Destroy(collision.gameObject);

        Debug.Log("Pickup triggered by " + collision.name + " at " + Time.time);
    }

    private void ApplySanityReduction(Item item)
    {
        if (sanitySystem != null && item.sanityReduction > 0f)
        {
            sanitySystem.ReduceSanity(item.sanityReduction);
            Debug.Log($"Sanity reduced by {item.sanityReduction} from {item.name}");
        }
    }

    private void PickupItemToInventory(Item item, GameObject itemObject)
    {
        if (item.addToInventory)
        {
            bool itemAdded = inventoryController.AddItem(itemObject);
            if (itemAdded)
                item.Pickup();
            else
                Debug.Log("Inventory full, item not added: " + itemObject.name);
        }
        else
        {
            item.Pickup(); // Show pickup UI even if not added to inventory
        }
    }

    private void HandleCollectiblePickup(Collider2D collision)
    {
        Collectible collectible = collision.GetComponent<Collectible>();
        if (collectible == null) return;

        bool collectibleAdded = collectibleController.AddCollectible(collision.gameObject);
        if (collectibleAdded)
        {
            collectible.Pickup();
            Destroy(collision.gameObject);

            Debug.Log("Pickup triggered by " + collision.name + " at " + Time.time);
        }
    }

    // ---------------- Utility ----------------

    // Clears the picked up objects set each frame to allow re-triggering pickups
    private void ResetPickedUpObjects()
    {
        pickedUpObjects.Clear();
    }
}

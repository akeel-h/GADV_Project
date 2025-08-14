using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;
    private CollectibleController collectibleController;

    private HashSet<GameObject> pickedUpObjects = new HashSet<GameObject>();

    public SanitySystem sanitySystem; // Reference to the player's sanity system

    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
        collectibleController = FindObjectOfType<CollectibleController>();

        // Optional: automatically find the sanity system if not assigned
        if (sanitySystem == null)
            sanitySystem = FindObjectOfType<SanitySystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pickedUpObjects.Contains(collision.gameObject)) return;
        pickedUpObjects.Add(collision.gameObject);

        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                bool itemAdded = inventoryController.AddItem(collision.gameObject);

                if (itemAdded)
                {
                    item.Pickup();

                    // Apply sanity reduction if item has a sanity effect
                    if (sanitySystem != null && item.sanityReduction > 0f)
                    {
                        sanitySystem.ReduceSanity(item.sanityReduction);
                        Debug.Log($"Sanity reduced by {item.sanityReduction} from {collision.name}");
                    }

                    Destroy(collision.gameObject);
                    Debug.Log("Pickup triggered by " + collision.name + " at " + Time.time);
                }
            }
        }

        if (collision.CompareTag("Collectible"))
        {
            Collectible collectible = collision.GetComponent<Collectible>();
            if (collectible != null)
            {
                bool collectibleAdded = collectibleController.AddCollectible(collision.gameObject);

                if (collectibleAdded)
                {
                    collectible.Pickup();
                    Destroy(collision.gameObject);
                    Debug.Log("Pickup triggered by " + collision.name + " at " + Time.time);
                }
            }
        }
    }

    private void LateUpdate()
    {
        pickedUpObjects.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;
    private CollectibleController collectibleController;

    private HashSet<GameObject> pickedUpObjects = new HashSet<GameObject>();

    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
        collectibleController = FindObjectOfType<CollectibleController>();
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

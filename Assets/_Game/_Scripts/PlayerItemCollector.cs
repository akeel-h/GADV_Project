using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;
    private CollectibleController collectibleController;
    // Start is called before the first frame update
    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
        collectibleController = FindObjectOfType<CollectibleController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if(item != null)
            {
                bool itemAdded = inventoryController.AddItem(collision.gameObject);

                if (itemAdded)
                {
                    Destroy(collision.gameObject);
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
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}

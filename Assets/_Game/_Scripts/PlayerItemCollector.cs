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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if(item != null && Input.GetKeyDown(KeyCode.F))
            {
                bool itemAdded = inventoryController.AddItem(collision.gameObject);

                if (itemAdded)
                {
                    Debug.Log("itemadd");
                    Destroy(collision.gameObject);
                }
            }
        }

        if (collision.CompareTag("Collectible"))
        {
            Collectible collectible = collision.GetComponent<Collectible>();
            if (collectible != null && Input.GetKeyDown(KeyCode.F))
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

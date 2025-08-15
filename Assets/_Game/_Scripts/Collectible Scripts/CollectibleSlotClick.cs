using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CollectibleSlotClick : MonoBehaviour, IPointerClickHandler
{
    private Collectible collectibleData; // Stores reference to collectible info

    private void Start()
    {
        InitializeCollectibleData();
    }

    // ---------------- Initialization ----------------

    // Grabs collectible component attached to this object
    private void InitializeCollectibleData()
    {
        collectibleData = GetComponent<Collectible>();
    }

    // ---------------- Public Methods ----------------

    // Manually assign collectible data from another script
    public void SetCollectible(Collectible data)
    {
        collectibleData = data;
    }

    // Handle click events on collectible slot
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && collectibleData != null)
        {
            FindObjectOfType<CollectibleUIManager>()
                .OpenBook(collectibleData.Name, collectibleData.Text);
        }
    }
}

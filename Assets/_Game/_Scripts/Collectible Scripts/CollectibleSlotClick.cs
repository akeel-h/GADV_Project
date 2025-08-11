using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CollectibleSlotClick : MonoBehaviour, IPointerClickHandler
{
    private Collectible collectibleData;

    private void Start()
    {
        collectibleData = GetComponent<Collectible>();
    }

    public void SetCollectible(Collectible data)
    {
        collectibleData = data;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            FindObjectOfType<CollectibleUIManager>()
                .OpenBook(collectibleData.Name, collectibleData.Text);
        }
    }
}

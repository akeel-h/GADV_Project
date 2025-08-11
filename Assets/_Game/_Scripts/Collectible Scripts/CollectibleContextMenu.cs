using UnityEngine;
using UnityEngine.UI;

public class CollectibleContextMenu : MonoBehaviour
{
    public Button readButton;
    private Collectible currentCollectible;

    private void Awake()
    {
        gameObject.SetActive(false);
        readButton.onClick.AddListener(OnReadClicked);
    }

    public void Show(Collectible collectible, Vector3 position)
    {
        currentCollectible = collectible;
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnReadClicked()
    {
        if (currentCollectible != null)
        {
            FindObjectOfType<CollectibleUIManager>(true)
                .OpenBook(currentCollectible.Name, currentCollectible.Text);
        }
        Hide();
    }

    private void Update()
    {
        if (gameObject.activeSelf && Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                GetComponent<RectTransform>(), Input.mousePosition))
            {
                Hide();
            }
        }
    }
}

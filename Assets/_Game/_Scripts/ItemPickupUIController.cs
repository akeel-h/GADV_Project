using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickupUIController : MonoBehaviour
{
    // Singleton instance
    public static ItemPickupUIController Instance { get; private set; }

    [Header("Popup Settings")]
    public GameObject popupPrefab;   // Prefab for item pickup UI
    public int maxPopups = 5;        // Max popups visible at once
    public float popupDuration = 3f; // How long each popup stays

    private readonly Queue<GameObject> activePopups = new();

    private void Awake()
    {
        InitializeSingleton();
    }

    // Extracted singleton initialization from lifecycle
    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple ItemPickupUIController instances detected! Destroying extra instance.");
            Destroy(gameObject);
        }
    }

    // Call this to show a popup for a picked up item
    public void ShowItemPickup(string itemName, Sprite itemIcon)
    {
        GameObject newPopup = CreatePopup(itemName, itemIcon);

        // Keep popup queue under limit
        EnqueuePopup(newPopup);

        // Start fade out coroutine
        StartCoroutine(FadeOutAndDestroy(newPopup));
    }

    // Creates the popup GameObject and sets text/icon
    private GameObject CreatePopup(string itemName, Sprite itemIcon)
    {
        GameObject newPopup = Instantiate(popupPrefab, transform);

        // Set item name text
        newPopup.GetComponentInChildren<TMP_Text>().text = itemName;

        // Set item icon if present
        Image itemImage = newPopup.transform.Find("ItemIcon")?.GetComponent<Image>();
        if (itemImage)
        {
            itemImage.sprite = itemIcon;
        }

        return newPopup;
    }

    // Adds popup to queue and removes oldest if over limit
    private void EnqueuePopup(GameObject popup)
    {
        activePopups.Enqueue(popup);
        if (activePopups.Count > maxPopups)
        {
            Destroy(activePopups.Dequeue());
        }
    }

    // Coroutine to fade out and destroy popup
    private IEnumerator FadeOutAndDestroy(GameObject popup)
    {
        // Wait for popup duration
        yield return new WaitForSeconds(popupDuration);
        if (popup == null) yield break;

        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = popup.AddComponent<CanvasGroup>();
        }

        // Fade over 1 second
        float fadeTime = 1f;
        for (float t = 0f; t < fadeTime; t += Time.deltaTime)
        {
            if (popup == null) yield break;
            canvasGroup.alpha = 1f - t / fadeTime;
            yield return null;
        }

        Destroy(popup);
    }
}

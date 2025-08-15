using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject menuCanvas;   // Reference to the pause/menu UI
    public GameObject deathScreen;  // Reference to death screen UI

    private CollectibleUIManager bookUI; // Reference to the book UI

    private void Start()
    {
        InitializeUI();
    }

    private void Update()
    {
        HandleMenuToggle();
    }

    // Initialize UI elements at start
    private void InitializeUI()
    {
        // Ensure menu is hidden at start
        if (menuCanvas != null)
            menuCanvas.SetActive(false);

        // Find the book UI (even if inactive)
        bookUI = FindObjectOfType<CollectibleUIManager>(true);
    }

    // Handles toggling the menu when Tab is pressed
    private void HandleMenuToggle()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Do not toggle menu if the book is currently open
            if (bookUI != null && bookUI.IsBookOpen)
                return;

            if (menuCanvas != null)
                menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }

    // Public method to show the death screen
    public void ShowDeathScreen()
    {
        if (deathScreen != null)
            deathScreen.SetActive(true);
    }
}

using UnityEngine;
using TMPro;

public class CollectibleUIManager : MonoBehaviour
{
    public TMP_Text titleText;      // UI element for collectible title
    public TMP_Text contentText;    // UI element for collectible description
    public GameObject bookPanel;    // Panel for displaying collectible details
    public GameObject menu;         // Main menu panel

    public bool IsBookOpen { get; private set; } // Tracks whether book panel is open

    private void Update()
    {
        HandleBookCloseInput();
    }

    // ---------------- Public Methods ----------------

    // Displays the collectible book with given title and content
    public void OpenBook(string title, string content)
    {
        titleText.text = title;
        contentText.text = content;
        bookPanel.SetActive(true);
        menu.SetActive(false);
        Time.timeScale = 0f;
        IsBookOpen = true;
    }

    // Closes the collectible book and reopens menu
    public void CloseBook()
    {
        bookPanel.SetActive(false);
        menu.SetActive(true);
        Time.timeScale = 1f;
        IsBookOpen = false;
    }

    // ---------------- Private Methods ----------------

    // Handles input for closing the book
    private void HandleBookCloseInput()
    {
        if (bookPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseBook();
        }
    }
}

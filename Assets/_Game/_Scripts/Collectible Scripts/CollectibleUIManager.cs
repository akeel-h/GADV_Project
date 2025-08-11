using UnityEngine;
using TMPro;

public class CollectibleUIManager : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text contentText;
    public GameObject bookPanel;
    public GameObject menu;

    public bool IsBookOpen {  get; private set; }

    public void OpenBook(string title, string content)
    {
        titleText.text = title;
        contentText.text = content;
        bookPanel.SetActive(true);
        menu.SetActive(false);
        Time.timeScale = 0f;
        IsBookOpen = true;
    }


    public void CloseBook()
    {
        bookPanel.SetActive(false);
        menu.SetActive(true);
        Time.timeScale = 1f;
        IsBookOpen = false;
    }

    private void Update()
    {
        if (bookPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseBook();
        }
    }
}

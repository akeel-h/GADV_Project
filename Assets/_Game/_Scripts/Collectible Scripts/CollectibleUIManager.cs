using UnityEngine;
using TMPro;

public class CollectibleUIManager : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text contentText;
    public GameObject bookPanel;
    public GameObject menu;

    public void OpenBook(string title, string content)
    {
        Debug.Log($"Opening book: {title} / {content}");
        titleText.text = title;
        contentText.text = content;
        bookPanel.SetActive(true);
        menu.SetActive(false);
        Time.timeScale = 0f;
    }


    public void CloseBook()
    {
        bookPanel.SetActive(false);
        menu.SetActive(true);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (bookPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseBook();
        }
    }
}

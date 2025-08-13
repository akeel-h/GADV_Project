using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject deathScreen;
    private CollectibleUIManager bookUI;

    void Start()
    {
        menuCanvas.SetActive(false);
        bookUI = FindObjectOfType<CollectibleUIManager>(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (bookUI != null && bookUI.IsBookOpen)
                return;

            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }

    public void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
    }
}

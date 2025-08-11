using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
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
}

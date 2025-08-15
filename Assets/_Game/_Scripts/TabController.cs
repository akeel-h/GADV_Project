using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    [Header("UI References")]
    public Image[] tabImages;  // Images representing the tabs
    public GameObject[] pages; // Pages corresponding to each tab

    private void Start()
    {
        InitializeTabs();
    }

    // Initialize the tab system by activating the first tab
    private void InitializeTabs()
    {
        ActivateTab(0);
    }

    // Activates the selected tab and deactivates others
    public void ActivateTab(int tabNo)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);        // Hide all pages
            tabImages[i].color = Color.grey;  // Set all tab icons to grey
        }

        // Activate selected tab
        pages[tabNo].SetActive(true);
        tabImages[tabNo].color = Color.white;
    }
}

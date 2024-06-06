using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public GameObject[] Tabs;
    // this won't be used for now I think
    //public Image[] TabButtons
    //public Sprite inactiveTabBg, activeTabBg
    //public Vector2 inactiveTabButtonSize, activeTabButtonSize

    public void SwitchToTab(int tabId) {
        foreach (GameObject go in Tabs) {
            go.SetActive(false);
        }
        Tabs[tabId].SetActive(true);

        /*
        foreach (Image im in TabButtons) {
            im.sprite = inactiveTabBg;
            im.rectTransform.sizeDelta = inactiveTabButtonSize;
        }
        TabButtons[tabId].sprite = activeTabBg;
        TabButtons[tabId].rectTransform.sizeDelta = activeTabButtonSize;
        */
    }
}

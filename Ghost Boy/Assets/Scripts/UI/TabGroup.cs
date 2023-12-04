using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButtons> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public void Subscribe(TabButtons button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButtons>();
        }
        tabButtons.Add(button);
    }
    public void OnTabEnter(TabButtons button)
    {
        ResetTabs();
        button.background.sprite = tabHover;
    }

    public void OnTabExit(TabButtons button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButtons button)
    {
        ResetTabs();
        button.background.sprite = tabActive; 
    }

    public void ResetTabs()
    {
        foreach(TabButtons button in tabButtons)
        {
            button.background.sprite = tabIdle;
        }
    }
}

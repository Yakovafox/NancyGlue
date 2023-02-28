using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject AccessibilityMenu;

    private void Awake()
    {
        AccessibilityMenu.SetActive(false);
    }

    public void AccessibilitySettings()
    {
        if(AccessibilityMenu.activeSelf == false)
        {
            AccessibilityMenu.SetActive(true);
        }
        else AccessibilityMenu.SetActive(false);
    }
}

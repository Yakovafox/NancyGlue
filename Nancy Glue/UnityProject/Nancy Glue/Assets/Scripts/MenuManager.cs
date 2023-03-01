using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _accessibilityMenu;
    [SerializeField] private bool _isActive;

    private void Awake()
    {
        _isActive = _accessibilityMenu.activeSelf;
        _accessibilityMenu.SetActive(_isActive);
    }

    public void AccessibilitySettings()
    {
        _isActive = !_isActive;
        _accessibilityMenu.SetActive(_isActive);
    }
}

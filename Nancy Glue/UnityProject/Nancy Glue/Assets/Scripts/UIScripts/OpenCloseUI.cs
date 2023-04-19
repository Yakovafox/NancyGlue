using System.Collections;
using System.Collections.Generic;
using Dialogue;
using UnityEngine;

public class OpenCloseUI : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [field: SerializeField] public bool IsOpen { get; private set; }
    [field: SerializeField] public bool IsHidden { get; private set; }
    private int _HideUIBoolHash;
    [SerializeField] private GameObject _inventoryUIPage;
    [SerializeField] private GameObject _inventoryGrid;
    [SerializeField] private GameObject _suspectsUIPage;
    [SerializeField] private GameObject _locationsUIPage;
    [SerializeField] private GameObject _settingsUIPage;
    [SerializeField] private GameObject _blankPage;
    [SerializeField] private GameObject _dialogueUI;
    [SerializeField] private GameObject _detailsPage;
    [SerializeField] private GameObject _mugshotPage;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _HideUIBoolHash = Animator.StringToHash("Hide");
    }

    private void Start()
    {

    }

    private void Update()
    {
        HideUnHideUI();
    }

    public void InventoryClicked()
    {
        OpenUI();
        _inventoryUIPage.SetActive(true);
        _inventoryGrid.SetActive(true);
        _suspectsUIPage.SetActive(false);
        _locationsUIPage.SetActive(false);
        _settingsUIPage.SetActive(false);
        _blankPage.SetActive(false);
    }

    public void LocationClicked()
    {
        OpenUI();
        _inventoryUIPage.SetActive(false);
        _suspectsUIPage.SetActive(false);
        _locationsUIPage.SetActive(true);
        _settingsUIPage.SetActive(false);
        _blankPage.SetActive(false);
    }

    public void SuspectClicked()
    {
        OpenUI();
        _inventoryUIPage.SetActive(false);
        _suspectsUIPage.SetActive(true);
        _locationsUIPage.SetActive(false);
        _settingsUIPage.SetActive(false);
        _blankPage.SetActive(false);
    }

    public void SettingsClicked()
    {
        OpenUI();
        _inventoryUIPage.SetActive(false);
        _suspectsUIPage.SetActive(false);
        _locationsUIPage.SetActive(false);
        _settingsUIPage.SetActive(true);
        _blankPage.SetActive(false);
    }

    private void OpenUI()
    {
        if (IsOpen) return;
        _animator.SetTrigger("OpenClose");
        IsOpen = !IsOpen;
        var text = FindObjectOfType<FontManager>();
        text.InitList();
    }

    public void CloseUI()
    {
        if (!IsOpen) return;
        _animator.SetTrigger("OpenClose");
        IsOpen = !IsOpen;
        _inventoryUIPage.SetActive(true);
        _suspectsUIPage.SetActive(true);
        _locationsUIPage.SetActive(true);
        _settingsUIPage.SetActive(true);
        _detailsPage.SetActive(false);
        _mugshotPage.SetActive(true);
        _blankPage.SetActive(true);
    }

    private void HideUnHideUI()
    {
        IsHidden = _dialogueUI.activeSelf;
        _animator.SetBool(_HideUIBoolHash, IsHidden);
    }
}

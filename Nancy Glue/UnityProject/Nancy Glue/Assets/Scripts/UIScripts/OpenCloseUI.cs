using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseUI : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _isOpen;
    [SerializeField] private GameObject _inventoryUIPage;
    [SerializeField] private GameObject _suspectsUIPage;
    [SerializeField] private GameObject _locationsUIPage;
    [SerializeField] private GameObject _settingsUIPage;
    [SerializeField] private GameObject _blackPage;
    

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void InventoryClicked()
    {
        OpenUI();
    }

    public void LocationClicked()
    {
        OpenUI();
    }

    public void SuspectClicked()
    {
        OpenUI();
    }

    public void SettingsClicked()
    {
        OpenUI();
    }

    private void OpenUI()
    {
        if (_isOpen) return;
        _animator.SetTrigger("OpenClose");
        _isOpen = !_isOpen;
    }

    public void CloseUI()
    {
        if (!_isOpen) return;
        _animator.SetTrigger("OpenClose");
        _isOpen = !_isOpen;
    }
}

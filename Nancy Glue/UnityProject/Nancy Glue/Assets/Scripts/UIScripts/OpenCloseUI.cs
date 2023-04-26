using UnityEngine;

public class OpenCloseUI : MonoBehaviour
{
    [Header("Main Ui Animator")]
    [SerializeField] private Animator _animator;
    [field: SerializeField] public bool IsOpen { get; private set; }
    [field: SerializeField] public bool IsHidden { get; private set; }
    private int _HideUIBoolHash;
    [Header("Page Game Objects")]
    [SerializeField] private GameObject _inventoryUIPage;
    [SerializeField] private GameObject _inventoryGrid;
    [SerializeField] private GameObject _suspectsUIPage;
    [SerializeField] private GameObject _locationsUIPage;
    [SerializeField] private GameObject _settingsUIPage;
    [SerializeField] private GameObject _blankPage;
    [SerializeField] private GameObject _dialogueUI;
    [SerializeField] private GameObject _detailsPage;
    [SerializeField] private GameObject _mugshotPage;

    [Header("Tab Animators")] 
    [SerializeField] private int _hiddenByDefaultHash;
    [SerializeField] private bool _closeHidden, _invHidden, _susHidden, _locHidden, _setHidden;
    [SerializeField] private Animator _closeTabAnim;
    [SerializeField] private Animator _invTabAnim;
    [SerializeField] private Animator _susTabAnim;
    [SerializeField] private Animator _locTabAnim;
    [SerializeField] private Animator _setTabAnim;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _HideUIBoolHash = Animator.StringToHash("Hide");
        _hiddenByDefaultHash = Animator.StringToHash("HiddenByDefault");
        _closeTabAnim = transform.GetChild(0).GetComponent<Animator>();
        _invTabAnim = transform.GetChild(4).GetComponent<Animator>();
        _susTabAnim = transform.GetChild(3).GetComponent<Animator>();
        _locTabAnim = transform.GetChild(2).GetComponent<Animator>();
        _setTabAnim = transform.GetChild(1).GetComponent<Animator>();
        _closeTabAnim.SetBool(_hiddenByDefaultHash, true);
        _invTabAnim.SetBool(_hiddenByDefaultHash, false);
        _susTabAnim.SetBool(_hiddenByDefaultHash, false);
        _locTabAnim.SetBool(_hiddenByDefaultHash, false);
        _setTabAnim.SetBool(_hiddenByDefaultHash, false);
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

        switch (_invHidden)
        {
            case false when _susHidden:
                _susHidden = false;
                openCloseTab(_susTabAnim);
                _invHidden = true;
                openCloseTab(_invTabAnim);
                break;
            case false when _locHidden:
                _locHidden = false;
                openCloseTab(_locTabAnim);
                _invHidden = true;
                openCloseTab(_invTabAnim);
                break;
            case false when _setHidden:
                _setHidden = false;
                openCloseTab(_setTabAnim);
                _invHidden = true;
                openCloseTab(_invTabAnim);
                break;
            case false:
                _invHidden = true;
                openCloseTab(_invTabAnim);
                break;
        }
    }

    public void LocationClicked()
    {
        OpenUI();
        _inventoryUIPage.SetActive(false);
        _suspectsUIPage.SetActive(false);
        _locationsUIPage.SetActive(true);
        _settingsUIPage.SetActive(false);
        _blankPage.SetActive(false);

        switch (_locHidden)
        {
            case false when _susHidden:
                _susHidden = false;
                openCloseTab(_susTabAnim);
                _locHidden = true;
                openCloseTab(_locTabAnim);
                break;
            case false when _invHidden:
                _locHidden = true;
                openCloseTab(_locTabAnim);
                _invHidden = false;
                openCloseTab(_invTabAnim);
                break;
            case false when _setHidden:
                _setHidden = false;
                openCloseTab(_setTabAnim);
                _locHidden = true;
                openCloseTab(_locTabAnim);
                break;
            case false:
                _locHidden = true;
                openCloseTab(_locTabAnim);
                break;
        }
    }

    public void SuspectClicked()
    {
        OpenUI();
        _inventoryUIPage.SetActive(false);
        _suspectsUIPage.SetActive(true);
        _locationsUIPage.SetActive(false);
        _settingsUIPage.SetActive(false);
        _blankPage.SetActive(false);

        switch (_susHidden)
        {
            case false when _invHidden:
                _susHidden = true;
                openCloseTab(_susTabAnim);
                _invHidden = false;
                openCloseTab(_invTabAnim);
                break;
            case false when _locHidden:
                _locHidden = false;
                openCloseTab(_locTabAnim);
                _susHidden = true;
                openCloseTab(_susTabAnim);
                break;
            case false when _setHidden:
                _setHidden = false;
                openCloseTab(_setTabAnim);
                _susHidden = true;
                openCloseTab(_susTabAnim);
                break;
            case false:
                _susHidden = true;
                openCloseTab(_susTabAnim);
                break;
        }
    }

    public void SettingsClicked()
    {
        OpenUI();
        _inventoryUIPage.SetActive(false);
        _suspectsUIPage.SetActive(false);
        _locationsUIPage.SetActive(false);
        _settingsUIPage.SetActive(true);
        _blankPage.SetActive(false);

        switch (_setHidden)
        {
            case false when _susHidden:
                _susHidden = false;
                openCloseTab(_susTabAnim);
                _setHidden = true;
                openCloseTab(_setTabAnim);
                break;
            case false when _locHidden:
                _locHidden = false;
                openCloseTab(_locTabAnim);
                _setHidden = true;
                openCloseTab(_setTabAnim);
                break;
            case false when _invHidden:
                _setHidden = true;
                openCloseTab(_setTabAnim);
                _invHidden = false;
                openCloseTab(_invTabAnim);
                break;
            case false:
                _setHidden = true;
                openCloseTab(_setTabAnim);
                break;
        }
    }

    private void OpenUI()
    {
        if (IsOpen) return;
        _animator.SetTrigger("OpenClose");
        IsOpen = !IsOpen;
        var text = FindObjectOfType<FontManager>();
        text.InitList();
        if (!_closeHidden)
        {
            openCloseTab(_closeTabAnim);
            _closeHidden = true;
        }

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
        if (_closeHidden)
        {
            openCloseTab(_closeTabAnim);
            _closeHidden = false;
        }

        if (_invHidden)
        {
            _invHidden = false;
            openCloseTab(_invTabAnim);
        }
        else if (_susHidden)
        {
            _susHidden = false;
            openCloseTab(_susTabAnim);
        }
        else if (_locHidden)
        {
            _locHidden = false;
            openCloseTab(_locTabAnim);
        }
        else if (_setHidden)
        {
            _setHidden = false;
            openCloseTab(_setTabAnim);
        }
    }

    private void openCloseTab(Animator targetTab)
    {
        targetTab.SetTrigger(_HideUIBoolHash);
    }


    private void HideUnHideUI()
    {
        IsHidden = _dialogueUI.activeSelf;
        _animator.SetBool(_HideUIBoolHash, IsHidden);
    }
}

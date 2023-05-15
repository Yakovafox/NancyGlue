using Dialogue;
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
    [SerializeField] private GameObject _controlsPage;
    [SerializeField] private GameObject _accesiPage;



    [Header("Tab Animators")] 
    [SerializeField] private int _hiddenByDefaultHash;
    [SerializeField] private bool _closeHidden, _invHidden, _susHidden, _locHidden, _setHidden;
    [SerializeField] private Animator _closeTabAnim;
    [SerializeField] private Animator _invTabAnim;
    [SerializeField] private Animator _susTabAnim;
    [SerializeField] private Animator _locTabAnim;
    [SerializeField] private Animator _setTabAnim;
    private Transform _invTab;
    private Transform _susTab;
    private Transform _locTab;
    private Transform _setTab;
    private Transform _closeTab;

    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _HideUIBoolHash = Animator.StringToHash("Hide");
        _hiddenByDefaultHash = Animator.StringToHash("HiddenByDefault");


        _closeTabAnim = transform.GetChild(0).GetComponent<Animator>();
        _closeTabAnim.SetBool(_hiddenByDefaultHash, true);
        
        _closeTab= transform.GetChild(0);
        _invTab= transform.GetChild(4);
        _susTab= transform.GetChild(3);
        _locTab= transform.GetChild(2);
        _setTab= transform.GetChild(1);

    }

    private void Start()
    {

    }

    private void FixedUpdate() //check if the ui should be hidden or not
    {
        HideUnHideUI();
    }

    public void InventoryClicked() //opens the inventory page
    {
        OpenUI();

        AudioSource audioSourceSound = GameObject.Find("Player").GetComponents<AudioSource>()[1];
        audioSourceSound.clip = Resources.Load<AudioClip>("Sfx/SoundEffects/Paper/Paper_Shuffle_001");
        audioSourceSound.pitch = 1.2f;
        audioSourceSound.Play();

        _inventoryUIPage.SetActive(true);
        _inventoryGrid.SetActive(true);
        _invTab.SetSiblingIndex(4);
        _susTab.SetSiblingIndex(3);
        _locTab.SetSiblingIndex(2);
        _setTab.SetSiblingIndex(1);
        _suspectsUIPage.SetActive(false);
        _locationsUIPage.SetActive(false);
        _settingsUIPage.SetActive(false);
        _blankPage.SetActive(false);
        _controlsPage.SetActive(false);
        _accesiPage.SetActive(false);

        
        
    }

    public void LocationClicked() //opens the locations page
    {
        OpenUI();

        AudioSource audioSourceSound = GameObject.Find("Player").GetComponents<AudioSource>()[1];
        audioSourceSound.clip = Resources.Load<AudioClip>("Sfx/SoundEffects/Paper/Paper_Shuffle_001");
        audioSourceSound.pitch = 1.2f;
        audioSourceSound.Play();
        _invTab.SetSiblingIndex(2);
        _susTab.SetSiblingIndex(3);
        _locTab.SetSiblingIndex(4);
        _setTab.SetSiblingIndex(1);
        _inventoryUIPage.SetActive(false);
        _suspectsUIPage.SetActive(false);
        _locationsUIPage.SetActive(true);
        _settingsUIPage.SetActive(false);
        _blankPage.SetActive(false);
        _controlsPage.SetActive(false);
        _accesiPage.SetActive(false);
        
    }

    public void SuspectClicked() //opens the suspects page
    {
        OpenUI();

        AudioSource audioSourceSound = GameObject.Find("Player").GetComponents<AudioSource>()[1];
        audioSourceSound.clip = Resources.Load<AudioClip>("Sfx/SoundEffects/Paper/Paper_Shuffle_001");
        audioSourceSound.pitch = 1.2f;
        audioSourceSound.Play();
        _invTab.SetSiblingIndex(3);
        _susTab.SetSiblingIndex(4);
        _locTab.SetSiblingIndex(2);
        _setTab.SetSiblingIndex(1);
        _inventoryUIPage.SetActive(false);
        _suspectsUIPage.SetActive(true);
        _locationsUIPage.SetActive(false);
        _settingsUIPage.SetActive(false);
        _blankPage.SetActive(false);
        _controlsPage.SetActive(false);
        _accesiPage.SetActive(false);
       
    }

    public void SettingsClicked() //opens the settings page
    {
        OpenUI();
        _invTab.SetSiblingIndex(1);
        _susTab.SetSiblingIndex(2);
        _locTab.SetSiblingIndex(3);
        _setTab.SetSiblingIndex(4);
        AudioSource audioSourceSound = GameObject.Find("Player").GetComponents<AudioSource>()[1];
        audioSourceSound.clip = Resources.Load<AudioClip>("Sfx/SoundEffects/Paper/Paper_Shuffle_001");
        audioSourceSound.pitch = 1.2f;
        audioSourceSound.Play();

        _inventoryUIPage.SetActive(false);
        _suspectsUIPage.SetActive(false);
        _locationsUIPage.SetActive(false);
        _settingsUIPage.SetActive(true);
        _blankPage.SetActive(false);
        _controlsPage.SetActive(false);
        _accesiPage.SetActive(false);
        
    }

    public void AcessiClicked() //opens the accessibility menu
    {
        AudioSource audioSourceSound = GameObject.Find("Player").GetComponents<AudioSource>()[1];
        audioSourceSound.clip = Resources.Load<AudioClip>("Sfx/SoundEffects/Paper/Paper_Shuffle_001");
        audioSourceSound.pitch = 1.2f;
        audioSourceSound.Play();

        _inventoryUIPage.SetActive(false);
        _suspectsUIPage.SetActive(false);
        _locationsUIPage.SetActive(false);
        _settingsUIPage.SetActive(false);
        _blankPage.SetActive(false);
        _controlsPage.SetActive(false);
        _accesiPage.SetActive(true);
    }
    public void controlsClicked() //Opens the controls menu
    {
        AudioSource audioSourceSound = GameObject.Find("Player").GetComponents<AudioSource>()[1];
        audioSourceSound.clip = Resources.Load<AudioClip>("Sfx/SoundEffects/Paper/Paper_Shuffle_001");
        audioSourceSound.pitch = 1.2f;
        audioSourceSound.Play();

        _inventoryUIPage.SetActive(false);
        _suspectsUIPage.SetActive(false);
        _locationsUIPage.SetActive(false);
        _settingsUIPage.SetActive(false);
        _blankPage.SetActive(false);
        _controlsPage.SetActive(true);
        _accesiPage.SetActive(false);
    }




    private void OpenUI() //Opens the UI
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

    public void CloseUI() //Closes the UI
    {
        if (!IsOpen) return;
        _animator.SetTrigger("OpenClose");
        IsOpen = !IsOpen;
        _inventoryUIPage.SetActive(true);
        _suspectsUIPage.SetActive(true);
        _locationsUIPage.SetActive(true);
        _settingsUIPage.SetActive(true);
        _controlsPage.SetActive(false);
        _accesiPage.SetActive(false);
        _detailsPage.SetActive(false);
        _mugshotPage.SetActive(true);
        _blankPage.SetActive(true);
        if (_closeHidden)
        {
            openCloseTab(_closeTabAnim);
            _closeHidden = false;
        }
    }

    private void openCloseTab(Animator targetTab) //Changes the target tab animator trigger
    {
        targetTab.SetTrigger(_HideUIBoolHash);
    }


    private void HideUnHideUI() //switches between hiding and un-hiding the UI
    {
        IsHidden = _dialogueUI.activeSelf;
        _animator.SetBool(_HideUIBoolHash, IsHidden);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//this script manages the main menu buttons


public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu; //Drop in mainMenu
    [SerializeField] private GameObject _accessibilityMenu; //Drop in Accessibilty menu
    [SerializeField] private GameObject _settingsMenu; //Drop in settings menu;
    [SerializeField] private bool _isActive;
    [SerializeField] private bool _isSettingsActive;
    
    SaveLoadSettings SLS;
    FontScript FS;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource SFX_AS;
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _pageSound;

    private void Awake()
    {
        Cursor.visible = true;

        //Find AccessibilityMenu and disable it. 
        _mainMenu = GameObject.Find("Main"); 
        _accessibilityMenu = GameObject.Find("AccessibilityMenu"); //Accessibility Menu needs to be enabled in Editor before startup.
        _settingsMenu = GameObject.Find("SettingsMenu");
        SLS= FindObjectOfType<SaveLoadSettings>();
        FS = FindObjectOfType<FontScript>();

        SFX_AS = this.GetComponent<AudioSource>();

        


    }

    private void Start()
    {
        _isActive = false;
        _isSettingsActive = false;
        
        NewOrLoad.isLoad = false;
        

        SLS.load();
        FS.loadSettings();
        
    }

    public void AccessibilitySettings()
    {
        SFX_AS.clip = _clickSound;
        SFX_AS.Play();
        _isActive = !_isActive;
        
        _accessibilityMenu.GetComponent<Animator>().SetTrigger("HideShow");
        _mainMenu.SetActive(!_isActive);

    }

    public void Settings()
    {
        SFX_AS.clip = _clickSound;
        SFX_AS.Play();
        _isSettingsActive = !_isSettingsActive;
        
        _settingsMenu.GetComponent<Animator>().SetTrigger("HideShow");
        _mainMenu.SetActive(!_isSettingsActive);
    }





    public void NewGame()
    {
        SFX_AS.clip = _clickSound;
        SFX_AS.Play();
        SceneManager.LoadScene("NewPlayingScene", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        SFX_AS.clip = _clickSound;
        SFX_AS.Play();
        Application.Quit();
    }

    public void LoadGame()
    {
        SFX_AS.clip = _clickSound;
        SFX_AS.Play();
        NewOrLoad.isLoad = true;
        
        SceneManager.LoadScene("NewPlayingScene", LoadSceneMode.Single);
        
    }
    
}

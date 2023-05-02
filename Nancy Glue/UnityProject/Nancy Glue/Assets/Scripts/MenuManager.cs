using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu; //Drop in mainMenu
    [SerializeField] private GameObject _accessibilityMenu; //Drop in Accessibilty menu
    [SerializeField] private GameObject _settingsMenu; //Drop in settings menu;
    [SerializeField] private bool _isActive;
    [SerializeField] private bool _isSettingsActive;
    //SaveLoadGameState _loadGameState;
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

        //settings temp 
        //tempSettingsLoader.musicVol = 50;
        //tempSettingsLoader.sens = 0;
        //tempSettingsLoader.sfxVol = 50;


    }

    private void Start()
    {
        _isActive = false;
        _isSettingsActive = false;
        //_accessibilityMenu.SetActive(_isActive);
        //_settingsMenu.SetActive(_isSettingsActive);
        NewOrLoad.isLoad = false;
        Debug.Log("initd to false");

        SLS.load();
        FS.loadSettings();
        Debug.Log("loaded settings");
    }

    public void AccessibilitySettings()
    {
        SFX_AS.clip = _clickSound;
        SFX_AS.Play();
        _isActive = !_isActive;
        //_accessibilityMenu.SetActive(_isActive);
        _accessibilityMenu.GetComponent<Animator>().SetTrigger("HideShow");
        _mainMenu.SetActive(!_isActive);

    }

    public void Settings()
    {
        SFX_AS.clip = _clickSound;
        SFX_AS.Play();
        _isSettingsActive = !_isSettingsActive;
        //_settingsMenu.SetActive(_isSettingsActive);
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
        Debug.Log("set to true");
        SceneManager.LoadScene("NewPlayingScene", LoadSceneMode.Single);
        //_loadGameState.Load();
    }
    
}

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
    private void Awake()
    {
        Cursor.visible = true;

        //Find AccessibilityMenu and disable it. 
        _mainMenu = GameObject.Find("Main"); 
        _accessibilityMenu = GameObject.Find("AccessibilityMenu"); //Accessibility Menu needs to be enabled in Editor before startup.
        _settingsMenu = GameObject.Find("SettingsMenu");
        SLS= FindObjectOfType<SaveLoadSettings>();
        FS = FindObjectOfType<FontScript>();
    }

    private void Start()
    {
        _isActive = false;
        _isSettingsActive = false;
        _accessibilityMenu.SetActive(_isActive);
        _settingsMenu.SetActive(_isSettingsActive);
        NewOrLoad.isLoad = false;
        Debug.Log("initd to false");

        SLS.load();
        FS.loadSettings();
        Debug.Log("loaded settings");
    }

    public void AccessibilitySettings()
    {
        _isActive = !_isActive;
        _accessibilityMenu.SetActive(_isActive);
        _mainMenu.SetActive(!_isActive);

    }

    public void Settings()
    {
        _isSettingsActive = !_isSettingsActive;
        _settingsMenu.SetActive(_isSettingsActive);
        _mainMenu.SetActive(!_isSettingsActive);
    }





    public void NewGame()
    {
        
        SceneManager.LoadScene("NewPlayingScene", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        NewOrLoad.isLoad = true;
        Debug.Log("set to true");
        SceneManager.LoadScene("NewPlayingScene", LoadSceneMode.Single);
        //_loadGameState.Load();
    }
    
}

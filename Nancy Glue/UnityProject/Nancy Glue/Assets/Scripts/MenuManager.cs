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
    //SaveLoadGameState _loadGameState;
    
    private void Awake()
    {
        //Find AccessibilityMenu and disable it. 
        _mainMenu = GameObject.Find("Main"); 
        _accessibilityMenu = GameObject.Find("AccessibilityMenu"); //Accessibility Menu needs to be enabled in Editor before startup.
        
        
    }

    private void Start()
    {
        _isActive = false;
        _accessibilityMenu.SetActive(_isActive);
        NewOrLoad.isLoad = false;
        Debug.Log("initd to false");
    }

    public void AccessibilitySettings()
    {
        _isActive = !_isActive;
        _accessibilityMenu.SetActive(_isActive);
        _mainMenu.SetActive(!_isActive);
    }

    public void NewGame()
    {
        
        SceneManager.LoadScene("Blockout", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        NewOrLoad.isLoad = true;
        Debug.Log("set to true");
        //_loadGameState.Load();
        //SceneManager.LoadScene("Blockout", LoadSceneMode.Single);
    }
}

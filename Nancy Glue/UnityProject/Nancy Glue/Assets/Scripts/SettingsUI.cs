using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

//this script manages settings menu buttons and sliders in game

public class SettingsUI : MonoBehaviour
{
    public Button reset;
    public Button mainMenu;
    public Button exit;
    public SaveLoadSettings SLS;
    public Slider sensSlider;
    public TMP_Text sliderValue;
    public Slider volSlider;
    public TMP_Text volSliderValue;
    public AudioMixer mixer;
    public Slider sfxVolSlider;
    public TMP_Text sfxVolSliderValue;
    public SaveLoadGameState SLGS;
    public Button closeMenu;
    public Button accesibility;
    public Button controls;
    public OpenCloseUI OCUI;
    //button listeners
    void Start()
    {
        
        reset.onClick.AddListener(resetSettings);
        mainMenu.onClick.AddListener(menu);
        exit.onClick.AddListener(exitApp);
        closeMenu.onClick.AddListener(menuClosed);
        accesibility.onClick.AddListener(Accesibility);
        controls.onClick.AddListener(Controls);
        

    }

    private void Awake()
    {
        SLS = FindObjectOfType<SaveLoadSettings>();
        SLGS = FindObjectOfType<SaveLoadGameState>();
        OCUI = FindObjectOfType<OpenCloseUI>();
        firstLoad();
        load();
    }



    public void Accesibility()
    {
        OCUI.AcessiClicked();
    }

    public void Controls(){
        OCUI.controlsClicked();
    }

    
    public void mVolChanged()
    {
        mixer.SetFloat("musicVol", Mathf.Log10(volSlider.value) * 20);
        
    }
    
    public void sfxVolChanged()
    {
        mixer.SetFloat("SFXVol", Mathf.Log10(sfxVolSlider.value) * 20);
       
    }

    //update slider values

    private void FixedUpdate()
    {
        sliderValue.text = sensSlider.value.ToString();
        volSliderValue.text = Mathf.RoundToInt((volSlider.value) * 100).ToString();
        sfxVolSliderValue.text = Mathf.RoundToInt((sfxVolSlider.value) * 100).ToString();
        

    }

    void firstLoad()
    {
        SLS.load();
    }


    void load()
    {
        //pull settings from SLS
        
        
        sensSlider.value = SLS.sensitivity;
        volSlider.value = (SLS.musicVolume);
       
        sfxVolSlider.value = SLS.sfxVolume;
        

    }

    
    void save()
    {
        SLS.sensitivity = (int)sensSlider.value;
        SLS.musicVolume = volSlider.value;

        SLS.sfxVolume = sfxVolSlider.value;

        SLS.save();
    }



    public void resetSettings()
    {
        SLS.defaultSettings();
        load();
    }

    public void menu()
    {
        save();
        SLS.save();
        SLGS.SaveGame();
        
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    public void menuClosed()
    {
        save();
        SLS.save();
        SLGS.SaveGame();
        
       
    }


    public void exitApp()
    {
        save();
        SLS.save();
        SLGS.SaveGame();
        
        Application.Quit();
    }
}

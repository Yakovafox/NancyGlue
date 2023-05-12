using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
    // Start is called before the first frame update
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
    
    public void sfxVolCHnaged()
    {
        mixer.SetFloat("SFXVol", Mathf.Log10(sfxVolSlider.value) * 20);
       
    }

    

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
        
        Debug.Log("loaded sens ui " + SLS.sensitivity);
        sensSlider.value = SLS.sensitivity;
        volSlider.value = (SLS.musicVolume);
        Debug.Log("Loaded music volume ui " + (SLS.musicVolume));
        sfxVolSlider.value = SLS.sfxVolume;
        Debug.Log("Loaded SFX volume ui " + (SLS.sfxVolume));



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
        Debug.Log("game saved to " + Application.persistentDataPath);
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    public void menuClosed()
    {
        save();
        SLS.save();
        SLGS.SaveGame();
        Debug.Log("game saved to " + Application.persistentDataPath);
       
    }


    public void exitApp()
    {
        save();
        SLS.save();
        SLGS.SaveGame();
        Debug.Log("game saved to " + Application.persistentDataPath);
        Application.Quit();
    }
}

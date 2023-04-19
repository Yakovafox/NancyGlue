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

    public int sens;
    public int sfxVol;
    public int musicVol;


    // Start is called before the first frame update
    void Start()
    {
        
        reset.onClick.AddListener(resetValues);
        mainMenu.onClick.AddListener(menu);
        exit.onClick.AddListener(exitApp);
    }

    private void Awake()
    {
        SLS = FindObjectOfType<SaveLoadSettings>();
        initLoad();
    }

    private void FixedUpdate()
    {
        sliderValue.text = sensSlider.value.ToString();

        mixer.SetFloat("musicVol", Mathf.Log10(volSlider.value) * 20);

        volSliderValue.text = Mathf.RoundToInt((volSlider.value) * 100).ToString();

        mixer.SetFloat("SFXVol", Mathf.Log10(sfxVolSlider.value) * 20);
        sfxVolSliderValue.text = Mathf.RoundToInt((sfxVolSlider.value) * 100).ToString();
    }

    void initLoad()
    {
        //pull settings from menu sliders 
        //temp for alpha
        sensSlider.value = tempSettingsLoader.sens;
        musicVol = tempSettingsLoader.musivVol;
        sfxVol = tempSettingsLoader.sfxVol;
    }

    void load()
    {
        /*
        sensSlider.value = SLS.sensitivity;
        sens = SLS.sensitivity;
        Debug.Log("sens loaded " + SLS.sensitivity);

        volSlider.value = (SLS.musicVolume) / 100;
        musicVol = (SLS.musicVolume);
        Debug.Log("Loaded music volume " + (SLS.musicVolume) / 100);
        Debug.Log("Original mv value " + musicVol);
        sfxVolSlider.value = (SLS.sfxVolume) / 100;
        sfxVol = SLS.sfxVolume;
        Debug.Log("Loaded SFX volume " + (SLS.sfxVolume) / 100);
        Debug.Log("Original sfxV value " + sfxVol);
        */
        //TEMPORARY FOR ALPHA
        sensSlider.value = tempSettingsLoader.sens;
        musicVol = tempSettingsLoader.musivVol;
        sfxVol = tempSettingsLoader.sfxVol;





    }
    public void resetValues()
    {
        SLS.defaultSettings();
        load();
    }

    public void menu()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }
    public void exitApp()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
//this script manages settings in the main menu
public class SettingsMenuManager : MonoBehaviour
{
    public Button back;
    public Button reset;
    public SaveLoadSettings SLS;
    public Slider sensSlider;
    public TMP_Text sliderValue;
    public Slider volSlider;
    public TMP_Text volSliderValue;
    public AudioMixer mixer;
    public Slider sfxVolSlider;
    public TMP_Text sfxVolSliderValue;


    private void Awake()
    {
        SLS = FindObjectOfType<SaveLoadSettings>();
        firstLoad();
        
        load();
    }
    //add listeners to buttons
    void Start()
    {
        back.onClick.AddListener(saveSettings);
        reset.onClick.AddListener(resetSettings);
        
    }
    //reset to default
    void resetSettings()
    {
        SLS.defaultSettings();
        load();
    }

    void firstLoad() 
    {
        SLS.load();



    }



    void load()
    {
        //load settings

        sensSlider.value = SLS.sensitivity;
        
        volSlider.value = (SLS.musicVolume);
        
        
        sfxVolSlider.value = SLS.sfxVolume;
       

    }

    private void FixedUpdate()
    {
        sliderValue.text = sensSlider.value.ToString();
         
        volSliderValue.text = Mathf.RoundToInt((volSlider.value)*100).ToString();

        sfxVolSliderValue.text= Mathf.RoundToInt((sfxVolSlider.value) * 100).ToString();
        
        mixer.SetFloat("SFXVol", Mathf.Log10(volSlider.value) * 20);
        mixer.SetFloat("musicVol", Mathf.Log10(volSlider.value) * 20);

        //update values 
    }

    

    void saveSettings()
    {
        
        SLS.sensitivity = (int)sensSlider.value;
        SLS.musicVolume = volSlider.value;
        
        SLS.sfxVolume= sfxVolSlider.value;
        
        SLS.save();
        
        


    }

    



}

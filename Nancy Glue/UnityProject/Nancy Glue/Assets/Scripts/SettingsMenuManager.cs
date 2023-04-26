using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

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


    //public int sliderVal;
    private void Awake()
    {
        SLS = FindObjectOfType<SaveLoadSettings>();
        firstLoad();
        //SLS.load();
        load();
    }
    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(saveSettings);
        reset.onClick.AddListener(resetSettings);
        
    }

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
       
        Debug.Log("loaded sens" + SLS.sensitivity);
        sensSlider.value = SLS.sensitivity;
        volSlider.value = (SLS.musicVolume);
        Debug.Log("Loaded music volume "+(SLS.musicVolume));
        sfxVolSlider.value = SLS.sfxVolume;
        Debug.Log("Loaded SFX volume "+(SLS.sfxVolume));
        

    }

    private void FixedUpdate()
    {
        sliderValue.text = sensSlider.value.ToString();
         
        mixer.SetFloat("musicVol", Mathf.Log10(volSlider.value) * 20);
        
        volSliderValue.text = Mathf.RoundToInt((volSlider.value)*100).ToString();

        mixer.SetFloat("SFXVol", Mathf.Log10(sfxVolSlider.value) * 20);
        sfxVolSliderValue.text= Mathf.RoundToInt((sfxVolSlider.value) * 100).ToString();

    }






    void saveSettings()
    {
        
        SLS.sensitivity = (int)sensSlider.value;
        SLS.musicVolume = volSlider.value;
        
        SLS.sfxVolume= sfxVolSlider.value;
        
        SLS.save();
        
        


    }

    



}

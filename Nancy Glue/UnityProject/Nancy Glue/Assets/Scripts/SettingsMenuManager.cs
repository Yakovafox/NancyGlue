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
        load();
    }
    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(saveSettings);
        reset.onClick.AddListener(resetSettings);
        
    }

    void load()
    {
        sensSlider.value = SLS.sensitivity;
        volSlider.value = (SLS.musicVolume)/100;
        Debug.Log("Loaded music volume "+(SLS.musicVolume) / 100);
        sfxVolSlider.value = (SLS.sfxVolume)/100;
        Debug.Log("Loaded SFX volume "+(SLS.sfxVolume) / 100);


    }

    private void Update()
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
        SLS.musicVolume = Mathf.RoundToInt((volSlider.value) * 100);
        Debug.Log("Saved target music vol "+Mathf.RoundToInt((volSlider.value) * 100));
        SLS.sfxVolume= Mathf.RoundToInt((sfxVolSlider.value) * 100);
        Debug.Log("Saved target music vol "+Mathf.RoundToInt((sfxVolSlider.value) * 100));
        SLS.save();
    }

    void resetSettings()
    {
        SLS.defaultSettings();
        load();
    }



}

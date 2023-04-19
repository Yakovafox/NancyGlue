using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadSettings : MonoBehaviour
{

    public int sfxVolume=50;
    public int musicVolume=50;
    public Color fontColour;
    public int sensitivity=0;
    public Color backgroundColour;
    public int fontStyle;
    public int isBackgroundEnabled;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void save()
    {
        PlayerPrefs.SetInt("sfxVolume", sfxVolume);
        Debug.Log("sfx vol to save to file "+ sfxVolume);
        PlayerPrefs.SetInt("musicVolume",musicVolume);
        Debug.Log("Music vol to save to file " + musicVolume);
        PlayerPrefs.SetString("fontColour", ColorUtility.ToHtmlStringRGB(fontColour));
        PlayerPrefs.SetInt("fontStyle", fontStyle);
        PlayerPrefs.SetString("backgroundColour", ColorUtility.ToHtmlStringRGB(backgroundColour));
        PlayerPrefs.SetInt("isBackgroundEnabled",isBackgroundEnabled);
        PlayerPrefs.SetInt("sensitivity", sensitivity);
        Debug.Log("sens to file "+sensitivity);
        PlayerPrefs.Save();
        Debug.Log("saved settings");//write path later
    }
    public void load()
    {
        
        if (!ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("fontColour"), out fontColour))
        {
            Default();
            return;
        }
        
        fontStyle = PlayerPrefs.GetInt("fontStyle");
        
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("backgroundColour"),out backgroundColour);
        isBackgroundEnabled = PlayerPrefs.GetInt("isBackgroundEnabled");
        try { 
            sfxVolume = PlayerPrefs.GetInt("sfxVolume");
            musicVolume = PlayerPrefs.GetInt("musicVolume");
            sensitivity = PlayerPrefs.GetInt("sensitivity");
        }
        catch { 
            defaultSettings();
            Debug.Log("failed to load settings from file");
        }

        //sfxVolume = PlayerPrefs.GetInt("sfxVolume");
        //musicVolume = PlayerPrefs.GetInt("musicVolume");
        //sensitivity = PlayerPrefs.GetInt("sensitivity");
    }
    public void Default()
    {
        //revert to default accesibility settings
        backgroundColour = Color.white;
        isBackgroundEnabled = 0;
        fontStyle = 0;
        fontColour = Color.black;


    }
    public void defaultSettings()
    {
        //revert to default settings
        sfxVolume = 50;
        musicVolume = 50;
        sensitivity = 0; 
    }
}

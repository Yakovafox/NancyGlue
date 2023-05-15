using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


//saving and loading of settings and preferences 

public class SaveLoadSettings : MonoBehaviour
{
    public float sfxVolume;
    public float musicVolume;
    public Color fontColour;
    public int sensitivity;
    public Color backgroundColour;
    public int fontStyle;
    public int isBackgroundEnabled;
    
    

    public void save()
    {
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        
        PlayerPrefs.SetFloat("musicVolume",musicVolume);
        
        PlayerPrefs.SetString("fontColour", ColorUtility.ToHtmlStringRGB(fontColour));
        PlayerPrefs.SetInt("fontStyle", fontStyle);
        PlayerPrefs.SetString("backgroundColour", ColorUtility.ToHtmlStringRGB(backgroundColour));
        PlayerPrefs.SetInt("isBackgroundEnabled",isBackgroundEnabled);
        PlayerPrefs.SetInt("sensitivity", sensitivity);
        
        PlayerPrefs.Save();
        
    }
    public void load()
    {
        
        if (!ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("fontColour"), out fontColour))
        {
            
            Default();
            defaultSettings();
            return;
        }
        
        fontStyle = PlayerPrefs.GetInt("fontStyle");
        
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("backgroundColour"),out backgroundColour);
        isBackgroundEnabled = PlayerPrefs.GetInt("isBackgroundEnabled");




        sfxVolume = PlayerPrefs.GetFloat("sfxVolume",0.555f);
        

        musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.555f); 
        

       
        sensitivity = PlayerPrefs.GetInt("sensitivity",0);
        
        
           

        
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
        sfxVolume = 0.555f; 
        musicVolume = 0.555f;
        sensitivity = 0; 
    }

    private void OnApplicationQuit()
    {
        save();
    }
}

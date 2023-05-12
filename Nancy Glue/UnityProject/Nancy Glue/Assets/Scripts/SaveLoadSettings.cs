using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadSettings : MonoBehaviour
{
    public float sfxVolume;
    public float musicVolume;
    public Color fontColour;
    public int sensitivity;
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
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        Debug.Log("sfx vol to save to file "+ sfxVolume);
        PlayerPrefs.SetFloat("musicVolume",musicVolume);
        Debug.Log("Music vol to save to file " + musicVolume);
        PlayerPrefs.SetString("fontColour", ColorUtility.ToHtmlStringRGB(fontColour));
        PlayerPrefs.SetInt("fontStyle", fontStyle);
        PlayerPrefs.SetString("backgroundColour", ColorUtility.ToHtmlStringRGB(backgroundColour));
        PlayerPrefs.SetInt("isBackgroundEnabled",isBackgroundEnabled);
        PlayerPrefs.SetInt("sensitivity", sensitivity);
        //Debug.Log("sens to file "+sensitivity);
        PlayerPrefs.Save();
        Debug.Log("saved settings");//write path later
    }
    public void load()
    {
        
        if (!ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("fontColour"), out fontColour))
        {
            Debug.Log("font colour not found, loading all defaults");
            Default();
            defaultSettings();
            return;
        }
        
        fontStyle = PlayerPrefs.GetInt("fontStyle");
        
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("backgroundColour"),out backgroundColour);
        isBackgroundEnabled = PlayerPrefs.GetInt("isBackgroundEnabled");




        sfxVolume = PlayerPrefs.GetFloat("sfxVolume",0.555f);
        Debug.Log("sfx vol loaded from file " + sfxVolume);

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

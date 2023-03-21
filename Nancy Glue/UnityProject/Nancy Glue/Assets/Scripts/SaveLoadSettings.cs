using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadSettings : MonoBehaviour
{

    public int sfxVolume;
    public int musicVolume;
    public Color fontColour;
    public int sensitivity;
    public Color backgroundColour;
    public string fontStyle;


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
        PlayerPrefs.SetInt("musicVolume",musicVolume);
        PlayerPrefs.SetString("fontColour", ColorUtility.ToHtmlStringRGB(fontColour));
        PlayerPrefs.SetString("fontStyle", fontStyle);
        PlayerPrefs.SetString("backgroundColour", ColorUtility.ToHtmlStringRGB(backgroundColour));
        PlayerPrefs.SetInt("sensitivity", sensitivity);
        PlayerPrefs.Save();
        Debug.Log("saved to");//write path later
    }
    public void load()
    {
        sfxVolume = PlayerPrefs.GetInt("sfxVolume");
        musicVolume = PlayerPrefs.GetInt("musicVolume");
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("fontColour"), out fontColour);
        
        fontStyle = PlayerPrefs.GetString("fontStyle");
        
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("backgroundColour"),out backgroundColour);
        sensitivity = PlayerPrefs.GetInt("sensitivity");
    }
    public void Default()
    { 
        //revert to default settings
    }

}

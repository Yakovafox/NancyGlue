using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccesiMenuManager : MonoBehaviour
{
    public Button back;
    public Button reset;
    public SaveLoadSettings SLS;
    public FontScript FScript;

    //add listeners for buttons
    void Start()
    {
        back.onClick.AddListener(saveSettings);
        reset.onClick.AddListener(resetSettings);
    }
    private void Awake()
    {
        SLS = FindObjectOfType<SaveLoadSettings>();
        FScript = FindObjectOfType<FontScript>();
    }
    //font settings save 
    void saveSettings()
    {
        SLS.save();
    }
    //reset to defaults
    void resetSettings()
    {
        SLS.Default();
        FScript.reset();
    }

}

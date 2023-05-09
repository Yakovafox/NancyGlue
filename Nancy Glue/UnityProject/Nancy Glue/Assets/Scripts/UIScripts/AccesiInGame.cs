using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AccesiInGame : MonoBehaviour
{
    public Button back;
    public OpenCloseUI OCUI;
    public SaveLoadSettings SLS;
    public FontScript fontScript;
    public Button reset;
    void Start()
    {
        back.onClick.AddListener(returnToSettings);
        reset.onClick.AddListener(resetDefaults);
    }

    private void Awake()
    {
        OCUI = FindObjectOfType<OpenCloseUI>();
        SLS = FindObjectOfType<SaveLoadSettings>();
        fontScript = FindObjectOfType<FontScript>();
    }

    public void returnToSettings()
    {
        OCUI.SettingsClicked();
    }

    private void OnDisable()
    {
        SLS.save();
    }

    public void resetDefaults()
    {
        SLS.Default();
        fontScript.reset();
    }


}

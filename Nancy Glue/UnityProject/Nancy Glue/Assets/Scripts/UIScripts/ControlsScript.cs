using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//manages a button in the controls menu

public class ControlsScript : MonoBehaviour
{
    public Button back;
    public OpenCloseUI OCUI;


    
    void Start()
    {
        back.onClick.AddListener(returnToSettings);
    }

    private void Awake()
    {
        OCUI = FindObjectOfType<OpenCloseUI>();
    }

    

    public void returnToSettings()
    {
        OCUI.SettingsClicked();
    }


}

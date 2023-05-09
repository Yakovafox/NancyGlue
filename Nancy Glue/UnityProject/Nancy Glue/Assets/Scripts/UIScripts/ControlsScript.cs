using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControlsScript : MonoBehaviour
{
    public Button back;
    public OpenCloseUI OCUI;


    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(returnToSettings);
    }

    private void Awake()
    {
        OCUI = FindObjectOfType<OpenCloseUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void returnToSettings()
    {
        OCUI.SettingsClicked();
    }


}

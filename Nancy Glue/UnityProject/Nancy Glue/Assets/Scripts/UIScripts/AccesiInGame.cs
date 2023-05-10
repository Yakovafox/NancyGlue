using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class AccesiInGame : MonoBehaviour
{
    public Button back;
    public OpenCloseUI OCUI;
    //public SaveLoadSettings SLS;
    public FontManager fontManager;
    public Button reset;
    [SerializeField] private TMP_Dropdown _dropDown;
    [SerializeField] private TMP_FontAsset[] _fonts = new TMP_FontAsset[2];
    [SerializeField] private bool _dyslexiaTextSelected;
    [SerializeField] private int _activeFontIndex;
    void Start()
    {
        back.onClick.AddListener(returnToSettings);
        reset.onClick.AddListener(resetDefaults);
    }

    private void Awake()
    {
        OCUI = FindObjectOfType<OpenCloseUI>();
        //SLS = FindObjectOfType<SaveLoadSettings>();
        fontManager = FindObjectOfType<FontManager>();
        _dropDown = FindObjectOfType<TMP_Dropdown>();
    }

    public void returnToSettings()
    {
        OCUI.SettingsClicked();
    }

    public void ChangeFonts()
    {

        switch (_dropDown.value)
        {
            case 1:
                _dyslexiaTextSelected = true;
                break;
            case 0:
                _dyslexiaTextSelected = false;
                break;
        }
        //_dyslexiaTextSelected = _dropDown.value ; //flip boolean
        _activeFontIndex = _dyslexiaTextSelected ? 1 : 0; //switch index value
        for (var i = 0; i < fontManager._TMPGameObjectsGUI.Length; i++)
        {
            fontManager._TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().font = _fonts[_dropDown.value]; //specifically for TMP UI text component. 
        }
        //ChangeText();
        if (_dyslexiaTextSelected)
        {
            fontManager.SLS.fontStyle = 1;
        }
        else
        {
            fontManager.SLS.fontStyle = 0;
        }


    }

    public void ChangeTextColour(Color newcolor)
    {
        for (var i = 0; i < fontManager._TMPGameObjectsGUI.Length; i++)
        {
            fontManager._TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().color = newcolor;
        }
        fontManager.SLS.fontColour = newcolor;
    }

    public void ChangeBGColour(Color newcolor)
    {
        for (var i = 0; i < fontManager._BackGroundGameObjectsGUI.Length; i++)
        {
            fontManager._BackGroundGameObjectsGUI[i].SetActive(true);
            fontManager._BackGroundGameObjectsGUI[i].GetComponent<Image>().color = newcolor;
        }
        fontManager.SLS.backgroundColour = newcolor;
        fontManager.SLS.isBackgroundEnabled = 1;
    }


    private void OnDisable()
    {
        fontManager.SLS.save();
        fontManager.load();
    }

    public void resetDefaults()
    {
        Debug.Log("resetting accessi prefs");
        fontManager.SLS.Default();
        _dropDown.value = 0;


        for (int i = 0; i < fontManager._TMPGameObjectsGUI.Length; i++)
        {
            fontManager._TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().font = _fonts[0];
        }
        for (var i = 0; i < fontManager._TMPGameObjectsGUI.Length; i++)
        {
            fontManager._TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().color = fontManager.SLS.fontColour;
        }
        for (var i = 0; i < fontManager._BackGroundGameObjectsGUI.Length; i++)
        {

            fontManager._BackGroundGameObjectsGUI[i].GetComponent<Image>().color = fontManager.SLS.backgroundColour;
            fontManager._BackGroundGameObjectsGUI[i].SetActive(false);
            
        }
        for (var i = 0; i < fontManager._BackGroundGameObjectsGUI.Length; i++)
        {

            fontManager.diaBackground[i].GetComponent<Image>().color = Color.white;
        }
    }


}

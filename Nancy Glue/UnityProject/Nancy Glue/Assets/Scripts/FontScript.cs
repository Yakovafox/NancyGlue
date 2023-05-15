using TMPro;
using UnityEngine;
using UnityEngine.UI;
//this script makes sure that the font settings are loaded and applied to all relevant elements in the menu

public class FontScript : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset[] _fonts = new TMP_FontAsset[2];
    [SerializeField] private int _activeFontIndex;
    [SerializeField] private GameObject[] _TMPGameObjectsGUI;
    [SerializeField] private GameObject[] _BackGroundGameObjectsGUI;
    [SerializeField] private bool _dyslexiaTextSelected;
    [SerializeField] private GameObject _FontEnableButton;
    [SerializeField] private TMP_Dropdown _dropDown;
    public SaveLoadSettings SLS;
    public GameObject[] diaBackground;
    private void Awake()
    {
        _activeFontIndex = 0;
        _TMPGameObjectsGUI = GameObject.FindGameObjectsWithTag("Text");
        _BackGroundGameObjectsGUI = GameObject.FindGameObjectsWithTag("TextBckGrnd");
        _FontEnableButton = GameObject.Find("FontEnableButton");
        _dropDown = FindObjectOfType<TMP_Dropdown>();
        diaBackground = GameObject.FindGameObjectsWithTag("DiaBckGrnd");

        for (var i = 0; i < _BackGroundGameObjectsGUI.Length; i++)
        {
            _BackGroundGameObjectsGUI[i].SetActive(false); 
        }

        SLS = FindObjectOfType<SaveLoadSettings>();
        
    }

    public void ChangeFonts()
    {
        
        switch(_dropDown.value)
        {
            case 1:
                _dyslexiaTextSelected = true;
                break;
            case 0:
                _dyslexiaTextSelected = false;
                break;
        }
        
        _activeFontIndex = _dyslexiaTextSelected ? 1 : 0; //switch index value
        for (var i = 0; i < _TMPGameObjectsGUI.Length; i++)
        {
            _TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().font = _fonts[_dropDown.value]; //specifically for TMP UI text component. 
        }
        
        if (_dyslexiaTextSelected)
        {
            SLS.fontStyle = 1;
        }
        else
        {
            SLS.fontStyle = 0;
        }


    }

    public void ChangeTextColour(Color newcolor)
    {
        for (var i = 0; i < _TMPGameObjectsGUI.Length; i++)
        {
            _TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().color = newcolor;
        }
        SLS.fontColour = newcolor;
    }

    public void ChangeBGColour(Color newcolor)
    {
        for (var i = 0; i < _BackGroundGameObjectsGUI.Length; i++)
        {
            _BackGroundGameObjectsGUI[i].SetActive(true);
            _BackGroundGameObjectsGUI[i].GetComponent<Image>().color = newcolor;
        }
        SLS.backgroundColour = newcolor;
        SLS.isBackgroundEnabled = 1;

        for (int i = 0; i < diaBackground.Length; i++)
        {
            
            diaBackground[i].GetComponent<Image>().color = SLS.backgroundColour;
           

        }


    }

    
    //load and apply accesibility settings 
    public void loadSettings()
    {
        if(SLS.fontStyle == 1)
        {
            for (var i = 0; i < _TMPGameObjectsGUI.Length; i++)
            {
                _TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().font = _fonts[1];
                _dropDown.value = 1;
            }
        }else if(SLS.fontStyle == 0)
        {
            for (var i = 0; i < _TMPGameObjectsGUI.Length; i++)
            {
                _TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().font = _fonts[0];
            }
        }



        for (var i = 0; i < _TMPGameObjectsGUI.Length; i++)
        {
            _TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().color = SLS.fontColour;
        }

        for (int i = 0; i < diaBackground.Length; i++)
        {
            
            if (SLS.isBackgroundEnabled == 1)
            {
                diaBackground[i].GetComponent<Image>().color = SLS.backgroundColour;
            }
            
        }


        // check if background colour is enabled then set it
        if (SLS.isBackgroundEnabled == 1)
        {
            for (var i = 0; i < _BackGroundGameObjectsGUI.Length; i++)
            {
                _BackGroundGameObjectsGUI[i].SetActive(true);
                _BackGroundGameObjectsGUI[i].GetComponent<Image>().color = SLS.backgroundColour;
                
            }
        }
    }

    //reset to defaults
    public void reset()
    {
        _dropDown.value = 0;
        for (var i = 0; i < _TMPGameObjectsGUI.Length; i++)
        {
            _TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().font = _fonts[0];
        }
        for (var i = 0; i < _TMPGameObjectsGUI.Length; i++)
        {
            _TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().color = SLS.fontColour;
        }

        for (var i = 0; i < _BackGroundGameObjectsGUI.Length; i++)
        {
            
            _BackGroundGameObjectsGUI[i].GetComponent<Image>().color = SLS.backgroundColour;
            _BackGroundGameObjectsGUI[i].SetActive(false);
            
        }
        for (var i = 0; i < diaBackground.Length; i++)
        {

            diaBackground[i].GetComponent<Image>().color = Color.white;
        }



    }



}

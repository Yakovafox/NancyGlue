using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private void Awake()
    {
        _activeFontIndex = 0;
        _TMPGameObjectsGUI = GameObject.FindGameObjectsWithTag("Respawn");
        _BackGroundGameObjectsGUI = GameObject.FindGameObjectsWithTag("Finish");
        _FontEnableButton = GameObject.Find("FontEnableButton");
        _dropDown = FindObjectOfType<TMP_Dropdown>();

        for (var i = 0; i < _BackGroundGameObjectsGUI.Length; i++)
        {
            _BackGroundGameObjectsGUI[i].SetActive(false); 
        }

        SLS = FindObjectOfType<SaveLoadSettings>();
        //SLS.isBackgroundEnabled = 0;
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
        //_dyslexiaTextSelected = _dropDown.value ; //flip boolean
        _activeFontIndex = _dyslexiaTextSelected ? 1 : 0; //switch index value
        for (var i = 0; i < _TMPGameObjectsGUI.Length; i++)
        {
            _TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().font = _fonts[_dropDown.value]; //specifically for TMP UI text component. 
        }
        //ChangeText();
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
    }

    /*private void ChangeText()
    {
        var text = _FontEnableButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.text = _dyslexiaTextSelected ? "Enabled" : "Disabled";

    }*/

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

        //background colour is more complex
        if (SLS.isBackgroundEnabled == 1)
        {
            for (var i = 0; i < _BackGroundGameObjectsGUI.Length; i++)
            {
                _BackGroundGameObjectsGUI[i].SetActive(true);
                _BackGroundGameObjectsGUI[i].GetComponent<Image>().color = SLS.backgroundColour;
                Debug.Log("setting background");
            }
        }
    }
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
            Debug.Log("resetting background");
        }
    }



}

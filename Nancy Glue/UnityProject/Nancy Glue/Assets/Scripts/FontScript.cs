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

    private void Awake()
    {
        _activeFontIndex = 0;
        _TMPGameObjectsGUI = GameObject.FindGameObjectsWithTag("Respawn");
        _BackGroundGameObjectsGUI = GameObject.FindGameObjectsWithTag("Finish");
        _FontEnableButton = GameObject.Find("FontEnableButton");

        for (var i = 0; i < _BackGroundGameObjectsGUI.Length; i++)
        {
            _BackGroundGameObjectsGUI[i].SetActive(false); 
        }
    }

    public void ChangeFonts()
    {
        _dyslexiaTextSelected = !_dyslexiaTextSelected; //flip boolean
        _activeFontIndex = _dyslexiaTextSelected ? 1 : 0; //switch index value
        for (var i = 0; i < _TMPGameObjectsGUI.Length; i++)
        {
            _TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().font = _fonts[_activeFontIndex]; //specifically for TMP UI text component. 
        }
        ChangeText();
    }

    public void ChangeTextColour(Color newcolor)
    {
        for (var i = 0; i < _TMPGameObjectsGUI.Length; i++)
        {
            _TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().color = newcolor;
        }
    }

    public void ChangeBGColour(Color newcolor)
    {
        for (var i = 0; i < _BackGroundGameObjectsGUI.Length; i++)
        {
            _BackGroundGameObjectsGUI[i].SetActive(true);
            _BackGroundGameObjectsGUI[i].GetComponent<Image>().color = newcolor;
        }
    }

    private void ChangeText()
    {
        var text = _FontEnableButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.text = _dyslexiaTextSelected ? "Enabled" : "Disabled";
    }
}

using TMPro;
using UnityEngine;

public class FontScript : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset[] _fonts = new TMP_FontAsset[2];
    [SerializeField] private int _activeFontIndex;
    [SerializeField] private GameObject[] _TMPGameObjectsGUI;
    [SerializeField] private bool _dyslexiaTextSelected;
    [SerializeField] private GameObject _FontEnableButton;

    private void Awake()
    {
        _activeFontIndex = 0;
        _TMPGameObjectsGUI = GameObject.FindGameObjectsWithTag("Respawn");
        _FontEnableButton = GameObject.Find("FontEnableButton");
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

    private void ChangeText()
    {
        var text = _FontEnableButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.text = _dyslexiaTextSelected ? "Enabled" : "Disabled";
    }
}

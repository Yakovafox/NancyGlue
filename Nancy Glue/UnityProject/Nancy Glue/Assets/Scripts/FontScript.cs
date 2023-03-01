using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class FontScript : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset[] _fonts = new TMP_FontAsset[2];
    [SerializeField] private int _activeFontIndex;
    [SerializeField] private GameObject[] _TMPGameObjectsGUI;
    [SerializeField] private bool _dyslexiaTextSelected;
    [SerializeField] private bool ChangeFont;

    private void Awake()
    {
        _activeFontIndex = 0;
        _TMPGameObjectsGUI = GameObject.FindGameObjectsWithTag("Respawn");
    }

    private void ChangeFonts()
    {
        _activeFontIndex = _dyslexiaTextSelected ? 1 : 0;
        for (var i = 0; i < _TMPGameObjectsGUI.Length; i++)
        {
            _TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().font = _fonts[_activeFontIndex]; //specifically for TMP UI text component. 
        }
        ChangeFont = false;
    }
}

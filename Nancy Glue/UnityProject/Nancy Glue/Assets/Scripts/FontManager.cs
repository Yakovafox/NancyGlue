using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//this script makes sure that the font settings are loaded and applied to all relevant elements in the game

public class FontManager : MonoBehaviour
{
    
    public SaveLoadSettings SLS;
    [SerializeField] private TMP_FontAsset[] _fonts = new TMP_FontAsset[2];
    [SerializeField] public GameObject[] _TMPGameObjectsGUI;
    [SerializeField] public GameObject[] _BackGroundGameObjectsGUI;
    public GameObject[] diaBackground;
    private void Awake()
    {
        SLS=FindObjectOfType<SaveLoadSettings>();
        _TMPGameObjectsGUI = GameObject.FindGameObjectsWithTag("Text");
        _BackGroundGameObjectsGUI = GameObject.FindGameObjectsWithTag("TextBckGrnd");
        diaBackground = GameObject.FindGameObjectsWithTag("DiaBckGrnd");
    }
    void Start()
    {
        load();
    }

    //load settings
    public void load()
    {
        
        SLS.load();
        //font set
        if (SLS.fontStyle == 1)
        {
            for (var i = 0; i < _TMPGameObjectsGUI.Length; i++)
            {
                _TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().font = _fonts[1]; //specifically for TMP UI text component. 
            }
        }else if(SLS.fontStyle == 0)
        {
            for (var i = 0; i < _TMPGameObjectsGUI.Length; i++)
            {
                _TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().font = _fonts[0]; //specifically for TMP UI text component. 
            }
        }
        //colour set
        for (var i = 0; i < _TMPGameObjectsGUI.Length; i++)
        {
            
            _TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().color = SLS.fontColour;
            
        }
        

        //dia background is always enabled while using the same colour as the optional background
        for (int i = 0; i < diaBackground.Length; i++)
        {
            //diaBackground[i].SetActive(true);
            if (SLS.isBackgroundEnabled == 1)
            {
                diaBackground[i].GetComponent<Image>().color = SLS.backgroundColour;
            }
            
           
        }
        //check if enabled + set
        if (SLS.isBackgroundEnabled == 1)
        {
            for (var i = 0; i < _BackGroundGameObjectsGUI.Length; i++)
            {
                _BackGroundGameObjectsGUI[i].SetActive(true);
                _BackGroundGameObjectsGUI[i].GetComponent<Image>().color = SLS.backgroundColour;
            }
            
        }
        else
        {
            for (var i = 0; i < _BackGroundGameObjectsGUI.Length; i++)
            {
                _BackGroundGameObjectsGUI[i].SetActive(false);
            }
        }



    }

    public void InitList()
    {

        

        
        load();
    }


}

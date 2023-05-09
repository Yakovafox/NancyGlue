using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FontManager : MonoBehaviour
{
    // Start is called before the first frame update
    public SaveLoadSettings SLS;
    [SerializeField] private TMP_FontAsset[] _fonts = new TMP_FontAsset[2];
    [SerializeField] private GameObject[] _TMPGameObjectsGUI;
    [SerializeField] private GameObject[] _BackGroundGameObjectsGUI;
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
    // Update is called once per frame
    void Update()
    {
        
    }

    public void load()
    {
        Debug.Log("loading settings into dialogue");
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
            //Debug.Log("target colour" + SLS.fontColour);
            _TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().color = SLS.fontColour;
            //Debug.Log("setting colour to" + _TMPGameObjectsGUI[i].GetComponent<TextMeshProUGUI>().color);
        }
        //background set

        //dia background is always enabled
        for (int i = 0; i < diaBackground.Length; i++)
        {
            diaBackground[i].SetActive(true);
            if (SLS.isBackgroundEnabled == 1)
            {
                diaBackground[i].GetComponent<Image>().color = SLS.backgroundColour;
            }
        }

        if (SLS.isBackgroundEnabled == 1)
        {
            for (var i = 0; i < _BackGroundGameObjectsGUI.Length; i++)
            {
                _BackGroundGameObjectsGUI[i].SetActive(true);
                _BackGroundGameObjectsGUI[i].GetComponent<Image>().color = SLS.backgroundColour;
            }
        }
        

    }

    public void InitList()
    {

        _TMPGameObjectsGUI = GameObject.FindGameObjectsWithTag("Text");
        _BackGroundGameObjectsGUI = GameObject.FindGameObjectsWithTag("TextBckGrnd");
        //Debug.Log("1");
        load();
    }


}

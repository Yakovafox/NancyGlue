using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FontManager : MonoBehaviour
{
    // Start is called before the first frame update
    public SaveLoadSettings SLS;
    [SerializeField] private TMP_FontAsset[] _fonts = new TMP_FontAsset[2];
    [SerializeField] private GameObject[] _TMPGameObjectsGUI;
    [SerializeField] private GameObject[] _BackGroundGameObjectsGUI;
    private void Awake()
    {
        SLS=FindObjectOfType<SaveLoadSettings>();
    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void load()
    {
        //SLS.load();
    }


}

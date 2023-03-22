using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccesiMenuManager : MonoBehaviour
{
    public Button back;
    public Button reset;
    public SaveLoadSettings SLS;
    public FontScript FScript;
    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(saveSettings);
        reset.onClick.AddListener(resetSettings);
    }
    private void Awake()
    {
        SLS = FindObjectOfType<SaveLoadSettings>();
        FScript = FindObjectOfType<FontScript>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void saveSettings()
    {
        SLS.save();
    }

    void resetSettings()
    {
        SLS.Default();
        FScript.reset();
    }

}

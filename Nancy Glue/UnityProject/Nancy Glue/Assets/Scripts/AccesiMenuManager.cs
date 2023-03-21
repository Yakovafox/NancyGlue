using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccesiMenuManager : MonoBehaviour
{
    public Button back;
    public SaveLoadSettings SLS;
    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(saveSettings);
        SLS=FindObjectOfType<SaveLoadSettings>();
    }
    private void Awake()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void saveSettings()
    {
        SLS.save();
    }



}

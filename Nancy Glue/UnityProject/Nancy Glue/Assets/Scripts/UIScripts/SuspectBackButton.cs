using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspectBackButton : MonoBehaviour
{
    //Attached to the back button for the suspect details page. Just disables the page in the hierarchy and enables the main page again.
    
    [SerializeField] private GameObject _mainPage;
    [SerializeField] private GameObject _detailPage;

    private void Awake()
    {
        _mainPage = transform.parent.parent.GetChild(1).gameObject;
        _detailPage = transform.parent.gameObject;
    }

    public void Clicked()
    {
        _detailPage.SetActive(!_detailPage.activeSelf);
        _mainPage.SetActive(!_mainPage.activeSelf);
    }
}

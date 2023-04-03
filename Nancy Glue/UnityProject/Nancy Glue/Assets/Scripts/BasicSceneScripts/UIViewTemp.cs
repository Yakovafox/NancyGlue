using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIViewTemp : MonoBehaviour
{
    public GameObject HiddenUI;
    public GameObject EvidenceUI;
    public GameObject SuspectUI;
    public GameObject DialogueUI;

    // Start is called before the first frame update
    void Start()
    {
        HiddenUI.SetActive(true);
        EvidenceUI.SetActive(false);
        SuspectUI.SetActive(false);
        DialogueUI.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HiddenUI.SetActive(true);
            EvidenceUI.SetActive(false);
            SuspectUI.SetActive(false);
            DialogueUI.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HiddenUI.SetActive(false);
            EvidenceUI.SetActive(true);
            SuspectUI.SetActive(false);
            DialogueUI.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            HiddenUI.SetActive(false);
            EvidenceUI.SetActive(false);
            SuspectUI.SetActive(true);
            DialogueUI.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            HiddenUI.SetActive(false);
            EvidenceUI.SetActive(false);
            SuspectUI.SetActive(false);
            DialogueUI.SetActive(true);
        }
    }
}

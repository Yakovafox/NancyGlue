using Cinemachine;
using Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class mouseTrack : MonoBehaviour
{
    Vector3 worldPos;
    public Inventory inv;
    public GameObject evid0;
    public GameObject evid1;
    public GameObject evid2;
    public GameObject evid3;
    public GameObject evid4;

    private bool UIOpen;
    public GameObject canvas;
    [Range(0,1000)][SerializeField]private float _range;
    [SerializeField] private GameObject _dialogueBox;
    [SerializeField] private DialogueSystem _dialogueSystemScript;
    //public 
    private void Awake()
    {
        _dialogueBox = GameObject.Find("DialogueBox");
        _dialogueSystemScript = _dialogueBox.GetComponent<DialogueSystem>();
    }
    private void Start()
    {
        UIOpen = false;
        canvas.SetActive(false);
        _dialogueBox.SetActive(false);

        inv = GameObject.Find("Player").GetComponent<Inventory>();
       // evid0 = GameObject.Find("Evidence0");
    }


    void Update()
    {

        

        if (Input.GetMouseButtonDown(0))
        {
            if (UIOpen) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, _range))
            {
                worldPos = hitData.point;


                switch (hitData.transform.tag)
                {
                    case ("Evidence id0"):
                        Debug.Log("Clicked evidence");
                        inv.GiveItem(0);
                        
                        //destroy object 
                        Destroy(evid0);
                        
                        break;
                    case ("Evidence id1"):
                        Debug.Log("Clicked evidence");
                        inv.GiveItem(1);

                        //destroy object 
                        Destroy(evid1);

                        break;
                    case ("Evidence id2"):
                        Debug.Log("Clicked evidence");
                        inv.GiveItem(2);

                        //destroy object 
                        Destroy(evid2);

                        break;

                    case ("Evidence id3"):
                        Debug.Log("Clicked evidence");
                        inv.GiveItem(3);

                        //destroy object 
                        Destroy(evid3);

                        break;
                    case ("Evidence id4"):
                        Debug.Log("Clicked evidence");
                        inv.GiveItem(4);

                        //destroy object 
                        Destroy(evid4);

                        break;





                    case ("NPC"):
                        Debug.Log("Clicked NPC");
                        var npcScript = hitData.transform.GetComponent<npcScript>();
                        _dialogueSystemScript.DialogueContainer = npcScript.DialogueContainers[0];
                        _dialogueBox.gameObject.SetActive(true);
                        break;
                    case ("Interactable"):
                        Debug.Log("Clicked Interactable");
                        break;
                    case ("Untagged"):
                        Debug.Log("Untagged - ignored");
                        break;
                    case ("Finish"):
                        var oldCamName = Camera.main.transform.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Name;
                        var oldCam = GameObject.Find(oldCamName).transform.parent.parent;
                        oldCam.GetComponent<CameraSwitch>().SwitchActiveCam();
                        hitData.transform.GetComponent<CameraSwitch>().SwitchActiveCam();
                        break;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.I))
        {
            Debug.Log("Checking if inv open");
            if (UIOpen)
            {
                Debug.Log("inv open - closing inv");
                canvas.SetActive(false);
                UIOpen = false;
            }
            else
            {
                Debug.Log("inv closed - openeing inv");
                canvas.SetActive(true);
                UIOpen = true;
            }

        }




    }
}
using Cinemachine;
using Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [Range(5,6)][SerializeField]private float _range;
    [SerializeField] private GameObject _dialogueBox;
    [SerializeField] private DialogueSystem _dialogueSystemScript;

    [SerializeField] private Image _cursor;
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private ZoneManager _zoneManager;
    private void Awake()
    {
        _zoneManager = FindObjectOfType<ZoneManager>();
        _dialogueBox = GameObject.Find("DialogueBox");
        _dialogueSystemScript = FindObjectOfType<DialogueSystem>();
        _dialogueBox.SetActive(false);
    }
    private void Start()
    {
        UIOpen = false;
        canvas.SetActive(false);

        inv = GameObject.Find("Player").GetComponent<Inventory>();
       // evid0 = GameObject.Find("Evidence0");
    }


    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        CursorChange(ray);

        if (Input.GetMouseButtonDown(0))
        {
            if (UIOpen) return;
            if (DialogueOpenCheck()) return;

            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, _range))
            {
                worldPos = hitData.point;


                switch (hitData.transform.tag)
                {
                    case ("Evidence"):
                        var gm = FindObjectOfType<GameManager>();
                        var item = hitData.transform.GetComponent<ItemData>().EvidenceItem;
                        //Debug.Log("Clicked " + item.Title + ":"
                         //+ "\n " + item.Description + "\n Item ID: " + item.ItemID);
                        inv.GiveItem(item.ItemID);
                        gm.EvidenceChecker(inv);
                        gm.ReelPickUp(item.Title);
                        break;
                    case ("NPC"):
                        NPCTracker tracker = hitData.transform.GetComponent<NPCTracker>();
                        tracker.ProgressDialogue(NPCTracker.ProgressTriggers.evidence);
                        _dialogueSystemScript.SetContainer(tracker.GetCurrentContainer());
                        tracker.ProgressDialogue(NPCTracker.ProgressTriggers.talking);
                        //var npcScript = hitData.transform.GetComponent<npcScript>();
                        //var ActiveContainer = npcScript.ActiveContainer;
                        //_dialogueSystemScript.SetContainer(npcScript.DialogueContainers[ActiveContainer]);
                        //npcScript.ChangeActiveContainer();
                        var text = FindObjectOfType<FontManager>();
                        text.InitList();
                        //npcScript.EvidenceCheck(inv);
                        var zoneManager = FindObjectOfType<ZoneManager>();
                        zoneManager.SpeakToNPC(hitData.transform.name);
                        break;
                    case ("Interactable"):
                        Debug.Log("Clicked Interactable");
                        break;
                    case ("Untagged"):
                        Debug.Log("Untagged - ignored");
                        break;
                    case ("Finish"):
                        SwitchToBranchCamera(hitData.transform);
                        break;
                }
            }
            if (inv.characterItems.Count == 4)
            {
                SceneManager.LoadScene("MenuScene");
                //reset character items here
                inv.resetInv();
            }
        }

        if (Input.GetKeyUp(KeyCode.I))
        {
            if (DialogueOpenCheck()) return;
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
                var text = FindObjectOfType<FontManager>();
                text.InitList();
            }

        }
        BackToRootCamera();
    }

    private void CursorChange(Ray ray)
    {
        _cursor.transform.position = Input.mousePosition;

        if (!Physics.Raycast(ray, out var hitData, _range))
        {
            _cursor.sprite = _sprites[0];
            return;
        }

        switch (hitData.transform.tag)
        {
            case "Finish":
                _cursor.sprite = _sprites[1];
                break;
            case "NPC":
                _cursor.sprite = _sprites[2];
                break;
        }
    }

    private void BackToRootCamera()
    {
        if(Input.GetMouseButtonDown(1))
        {
            var oldCamName = Camera.main.transform.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Name;
            var oldCamParent = GameObject.Find(oldCamName).transform.parent;
            if (oldCamParent.GetComponent<CameraSwitch>().IsRoot) return;
            oldCamParent.GetComponent<CameraSwitch>().SwitchActiveCam();
            var rootCam = oldCamParent.GetComponent<CameraSwitch>().RootCamera;
            rootCam.GetComponent<CameraSwitch>().SwitchActiveCam();
        }
    }

    private void SwitchToBranchCamera(Transform hitData)
    {
        var oldCamName = Camera.main.transform.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Name;
        var oldCamParent = GameObject.Find(oldCamName).transform.parent;
        if (!oldCamParent.GetComponent<CameraSwitch>().CanSwitch) return;
        oldCamParent.GetComponent<CameraSwitch>().SwitchActiveCam();
        hitData.transform.GetComponent<CameraSwitch>().SwitchActiveCam();
    }

    private bool DialogueOpenCheck()
    {
        return _dialogueBox.activeSelf;
    }
}
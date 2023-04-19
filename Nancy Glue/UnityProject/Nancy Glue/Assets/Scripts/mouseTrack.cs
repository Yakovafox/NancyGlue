using Cinemachine;
using Dialogue;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private OpenCloseUI _uiScript;
    [Range(5,6)][SerializeField]private float _range;
    [SerializeField] private GameObject _dialogueBox;
    [SerializeField] private DialogueSystem _dialogueSystemScript;

    [SerializeField] private Image _cursor;
    [SerializeField] private List<Texture2D> _sprites;
    [SerializeField] private ZoneManager _zoneManager;
    [SerializeField] private Tooltip _toolTip;
    [SerializeField] private SuspectPage _suspects;
    private void Awake()
    {
        _uiScript = FindObjectOfType<OpenCloseUI>();
        _zoneManager = FindObjectOfType<ZoneManager>();
        _dialogueBox = GameObject.Find("DialogueBox");
        _dialogueSystemScript = FindObjectOfType<DialogueSystem>();
        _dialogueBox.SetActive(false);
        _toolTip = FindObjectOfType<Tooltip>();
    }
    private void Start()
    {
        UIOpen = false;
        canvas.SetActive(true);

        inv = GameObject.Find("Player").GetComponent<Inventory>();
       // evid0 = GameObject.Find("Evidence0");
    }


    void Update()
    {
        UIOpen = _uiScript.IsOpen;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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
                        _toolTip.OpenTooltip("Evidence Added:\n" + item.Title);
                        break;
                    case ("NPC"):
                        NPCTracker tracker = hitData.transform.GetComponent<NPCTracker>();
                        //tracker.EvidenceCheck();
                        _dialogueSystemScript.SetContainer(tracker.GetCurrentContainer(), tracker) ;
                        tracker.ProgressDialogue(NPCTracker.ProgressTriggers.talking);
                        if (!tracker.SpokenTo)
                        {
                            _toolTip.OpenTooltip("New Suspect:\n" + tracker.CharName);
                            //add new Entry to the Suspect list
                            var Suspect = Instantiate(_suspects.SuspectPrefab, _suspects.transform.GetChild(0));
                            Suspect.GetComponent<SuspectMugshot>().SetData(tracker.CharName,tracker.CharacterSprite);
                            Suspect.name = Suspect.GetComponent<SuspectMugshot>().Name;
                            Suspect.GetComponent<SuspectMugshot>()._npcTracker = tracker;
                            tracker.SpokenTo = true;
                        }
                        var text = FindObjectOfType<FontManager>();
                        text.InitList();
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

        /*
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
        */
        BackToRootCamera();
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        CursorChange(ray);
    }

    private void CursorChange(Ray ray)
    {
        _cursor.transform.position = Input.mousePosition;
        if (!Physics.Raycast(ray, out var hitData, _range))
        {
            Cursor.SetCursor(_sprites[0],Vector2.zero,CursorMode.ForceSoftware);
            return;
        }
        switch (hitData.transform.tag)
        {
            case "Finish" when !UIOpen || !DialogueOpenCheck():
                Cursor.SetCursor(_sprites[1], Vector2.zero, CursorMode.ForceSoftware);
                break;
            case "NPC" when !UIOpen || !DialogueOpenCheck():
                Cursor.SetCursor(_sprites[2], Vector2.zero, CursorMode.ForceSoftware);
                break;
            case "Evidence" when !UIOpen || !DialogueOpenCheck():
                Cursor.SetCursor(_sprites[3], Vector2.zero, CursorMode.ForceSoftware);
                break;
            default:
                Cursor.SetCursor(_sprites[0],Vector2.zero, CursorMode.ForceSoftware);
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

    private void SwitchToBranchCamera(Transform newCam)
    {
        var currentCameraName = Camera.main.transform.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Name;
        Debug.Log(currentCameraName);
        var currentCameraParent = GameObject.Find(currentCameraName).transform.parent;
        Debug.Log(currentCameraParent.transform);
        if (!currentCameraParent.GetComponent<CameraSwitch>().CanSwitch) return;
        foreach (var branch in currentCameraParent.GetComponent<CameraSwitch>().SwitchableCameras)
        {
            if (newCam != branch) continue;
            currentCameraParent.GetComponent<CameraSwitch>().SwitchActiveCam();
            newCam.transform.GetComponent<CameraSwitch>().SwitchActiveCam();
        }
    }

    private bool DialogueOpenCheck()
    {
        return _dialogueBox.activeSelf;
    }
}
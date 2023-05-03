using Cinemachine;
using Dialogue;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    [SerializeField] private bool _isOverUI;
    [SerializeField] public bool OverMovement;
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
        _isOverUI = EventSystem.current.IsPointerOverGameObject();
        UIOpen = _uiScript.IsOpen;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (UIOpen) return;
            if (DialogueOpenCheck()) return;

            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, _range) && !_isOverUI)
            {
                worldPos = hitData.point;


                switch (hitData.transform.tag)
                {
                    case ("briefcase"):
                        hitData.transform.GetComponent<IClickable>().Clickable();
                        break;
                    case ("Evidence"):
                        var invEmptyText = GameObject.Find("BlankTextInventory");
                        if (invEmptyText != null && invEmptyText.activeSelf)
                            invEmptyText.SetActive(false);
                        var gm = FindObjectOfType<GameManager1>();
                        var item = hitData.transform.GetComponent<ItemData>().EvidenceItem;
                        //Debug.Log("Clicked " + item.Title + ":"
                         //+ "\n " + item.Description + "\n Item ID: " + item.ItemID);
                        //inv.GiveItem(item.ItemID);
                        gm.UpdateScene(item.ItemID);
                        _toolTip.OpenTooltip("Evidence Added:\n" + item.Title);

                        AudioSource audioSourceSound = GameObject.Find("Player").GetComponents<AudioSource>()[1];
                        audioSourceSound.clip = Resources.Load<AudioClip>("Sfx/SoundEffects/Paper/Paper_Shuffle_001");
                        audioSourceSound.pitch = 8.5f;
                        audioSourceSound.Play();

                        break;
                    case ("NPC"):
                        var susEmptyText = GameObject.Find("BlankTextSuspect");
                        if (susEmptyText != null && susEmptyText.activeSelf)
                            susEmptyText.SetActive(false);
                        NPCTracker tracker = hitData.transform.GetComponent<NPCTracker>();
                        tracker.ProgressDialogue(NPCTracker.ProgressTriggers.evidence);
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
                            _suspects.suspectList.Add(tracker.gameObject.name);
                        }
                        var text = FindObjectOfType<FontManager>();
                        text.InitList();
                        var zoneManager = FindObjectOfType<ZoneManager>();
                        zoneManager.SpeakToNPC(hitData.transform.name);
                        break;
                    case ("Phoney"):
                        NPCTracker phoney = hitData.transform.GetComponent<NPCTracker>();
                        _dialogueSystemScript.SetContainer(phoney.GetCurrentContainer(), phoney);
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
                    case ("InspectionCamera"):
                        SwitchToBranchCamera(hitData.transform);
                        break;
                    case ("RandomDialogue"):
                        var hit = hitData.transform.GetComponent<RandomDialogueScript>();
                        hit.SelectRandomDialogue();
                        break;
                }
            }
        }
        BackToRootCamera();
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        CursorChange(ray);
    }

    private void CursorChange(Ray ray)
    {
        if(_isOverUI)
        {
            Cursor.SetCursor(_sprites[0], new Vector2(10, 10), CursorMode.Auto);
            return;
        }
        //_cursor.transform.position = Input.mousePosition;
        if (!Physics.Raycast(ray, out var hitData, _range) || UIOpen || _dialogueBox.activeSelf)
        {
            Cursor.SetCursor(_sprites[0], new Vector2(10, 10), CursorMode.Auto);
            return;
        }
        switch (hitData.transform.tag)
        {
            case "Finish" when !UIOpen || !DialogueOpenCheck():
                if (Camera.main.transform.GetComponent<CinemachineBrain>().ActiveVirtualCamera == null) return;
                var cam = hitData.transform;
                var mainName = Camera.main.transform.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Name;
                var mainCam = GameObject.Find(mainName).transform.parent.GetComponent<CameraSwitch>();
                foreach(var switchableCam in mainCam.SwitchableCameras)
                {
                    if (cam == switchableCam)
                    {
                        Cursor.SetCursor(_sprites[1], new Vector2(32, 6), CursorMode.Auto);
                        OverMovement = true;
                    }
                    else
                        OverMovement = false;
                }
                break;
            case "InspectionCamera" when !UIOpen || !DialogueOpenCheck():
                if (Camera.main.transform.GetComponent<CinemachineBrain>().ActiveVirtualCamera == null) return;
                cam = hitData.transform;
                mainName = Camera.main.transform.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Name;
                mainCam = GameObject.Find(mainName).transform.parent.GetComponent<CameraSwitch>();
                foreach (var switchableCam in mainCam.SwitchableCameras)
                {
                    if (cam == switchableCam)
                    {
                        Cursor.SetCursor(_sprites[4], new Vector2(32, 6), CursorMode.Auto);
                        OverMovement = true;
                    }
                    else
                        OverMovement = false;
                }
                //Cursor.SetCursor(_sprites[1], new Vector2(32, 6), CursorMode.Auto);
                break;
            case "NPC" when !UIOpen || !DialogueOpenCheck():
                Cursor.SetCursor(_sprites[2], new Vector2(30,10), CursorMode.Auto);
                break;
            case "Phoney" when !UIOpen || !DialogueOpenCheck():
                Cursor.SetCursor(_sprites[2], new Vector2(30, 10), CursorMode.Auto);
                break;
            case "Evidence" when !UIOpen || !DialogueOpenCheck():
                Cursor.SetCursor(_sprites[3], new Vector2(32,32), CursorMode.Auto);
                break;
            case "RandomDialogue" when !UIOpen || !DialogueOpenCheck():
                Cursor.SetCursor(_sprites[2], new Vector2(30, 10), CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(_sprites[0],new Vector2(10,10), CursorMode.Auto);
                break;
        }
    }

    private void BackToRootCamera()
    {
        if (UIOpen || DialogueOpenCheck()) return;
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
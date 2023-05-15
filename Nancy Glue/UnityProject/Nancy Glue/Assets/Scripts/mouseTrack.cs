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
    private float _interactTimer;

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
        _interactTimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0)) //if Left click is pressed, check if the UI is open before running Raycast checks.
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (UIOpen) return;
            

            if (DialogueOpenCheck()) //disable clicks while the dialogue box is open
            {
                _interactTimer = 0;
                return;
            }
            else if (_interactTimer < 0.7f) //the interaction time is blocked for up to 0.7 seconds. 
            {
                return;
            }

            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, _range) && !_isOverUI) //block raycasts on UI elements
            {
                worldPos = hitData.point;


                switch (hitData.transform.tag) //Get the tag for the hit item
                {
                    case ("briefcase"):
                        hitData.transform.GetComponent<IClickable>().Clickable(); //if briefcase, run the Clickable function for the object
                        break;
                    case ("Evidence"):

                        /*
                         * if the hit object is an evidence item. Store the item Id to be passed into the Game manager
                         * disable the empty inventory text if enabled
                         * play the audio track, enable the tooltip
                         * pass the item into the game manager for deletion from the game world.
                         */
                        var invEmptyText = GameObject.Find("BlankTextInventory");
                        if (invEmptyText != null && invEmptyText.activeSelf)
                            invEmptyText.SetActive(false);
                        var gm = FindObjectOfType<GameManager1>();
                        var item = hitData.transform.GetComponent<ItemData>().EvidenceItem;
                        gm.UpdateScene(item.ItemID);
                        _toolTip.EnqueueTooltip("Evidence Added:\n" + item.Title);
                        AudioSource audioSourceSound = GameObject.Find("Player").GetComponents<AudioSource>()[1];
                        audioSourceSound.clip = Resources.Load<AudioClip>("Sfx/SoundEffects/Paper/Paper_Shuffle_001");
                        audioSourceSound.pitch = 8.5f;
                        audioSourceSound.Play();
                        break;
                    case ("NPC"):
                        /*
                         * if the hit object has the NPC tag get reference to the characters tracker script.
                         * disable the empty suspect page text if enabled
                         * progress through the characters dialogue graphs and set the dialogue systems current container.
                         * if this is the first time speaking to a character, log the character in the suspect page.
                         */
                        var susEmptyText = GameObject.Find("BlankTextSuspect");
                        if (susEmptyText != null && susEmptyText.activeSelf)
                            susEmptyText.SetActive(false);
                        NPCTracker tracker = hitData.transform.GetComponent<NPCTracker>();
                        tracker.ProgressDialogue(NPCTracker.ProgressTriggers.evidence);
                        _dialogueSystemScript.SetContainer(tracker.GetCurrentContainer(), tracker) ;
                        tracker.ProgressDialogue(NPCTracker.ProgressTriggers.talking);
                        if (!tracker.SpokenTo)
                        {
                            _toolTip.EnqueueTooltip("New Suspect:\n" + tracker.CharName);
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
                        //if the hit data object has the Phoney tag, play one of Phoney's dialogue graphs
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
                        //begin switch to a new camera
                        SwitchToBranchCamera(hitData.transform);
                        break;
                    case ("InspectionCamera"):
                        //begin switch to a new camera
                        SwitchToBranchCamera(hitData.transform);
                        break;
                    case ("RandomDialogue"):
                        //get reference to the random dialogue script and play it.
                        var hit = hitData.transform.GetComponent<RandomDialogueScript>();
                        hit.SelectRandomDialogue();
                        break;
                }
            }
        }
        BackToRootCamera(); //check if a switch is required to a root camera.
    }

    private void FixedUpdate()
    {
        //Send out a ray from the mouse position on screen
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        CursorChange(ray); //change cursor
    }

    private void CursorChange(Ray ray)
    {
        if(_isOverUI) //block cursor changes when hovering over UI elements
        {
            Cursor.SetCursor(_sprites[0], new Vector2(10, 10), CursorMode.Auto);
            return;
        }
        if (!Physics.Raycast(ray, out var hitData, _range) || UIOpen || _dialogueBox.activeSelf) //if there is no hit from the physics raycast return out and keep the default cursor
        {
            Cursor.SetCursor(_sprites[0], new Vector2(10, 10), CursorMode.Auto);
            return;
        }
        switch (hitData.transform.tag)
        {
            case "Finish" when !UIOpen || !DialogueOpenCheck():
                /*
                 * if hovering over a new camera position, check that the new camera is one of the branching cameras.
                 * if this is true, then display the move camera cursor icon.
                 */
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
                /*
                 * if hovering over a new camera position, check that the new camera is one of the branching cameras.
                 * if this is true, then display the move camera cursor icon.
                 */
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
                //Display the dialogue cursor when over NPCs 
                Cursor.SetCursor(_sprites[2], new Vector2(30,10), CursorMode.Auto);
                break;
            case "Phoney" when !UIOpen || !DialogueOpenCheck():
                //Display the dialogue cursor when over Phoney 
                Cursor.SetCursor(_sprites[2], new Vector2(30, 10), CursorMode.Auto);
                break;
            case "Evidence" when !UIOpen || !DialogueOpenCheck():
                //Display the pickup evidence cursor when over evidence objects.
                Cursor.SetCursor(_sprites[3], new Vector2(32,32), CursorMode.Auto);
                break;
            case "RandomDialogue" when !UIOpen || !DialogueOpenCheck():
                //Display the dialogue cursor when over NPCs with random dialogue 
                Cursor.SetCursor(_sprites[2], new Vector2(30, 10), CursorMode.Auto);
                break;
            default:
                //Display the Default cursor
                Cursor.SetCursor(_sprites[0],new Vector2(10,10), CursorMode.Auto);
                break;
        }
    }

    private void BackToRootCamera()
    {
        /*
         * Switch back to the current camera's "Root" parent when Right click is pressed.
         * Enable new camera, disable old camera. 
         */
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
        /*
         * get the current camera's CameraSwitch script.
         * Check that the camera can switch.
         * if the clicked location matches one of the current camera's branches
         * switch off the old camera and enable the new one. 
         */
        var currentCameraName = Camera.main.transform.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Name;
        var currentCameraParent = GameObject.Find(currentCameraName).transform.parent;
        if (!currentCameraParent.GetComponent<CameraSwitch>().CanSwitch) return;
        foreach (var branch in currentCameraParent.GetComponent<CameraSwitch>().SwitchableCameras)
        {
            if (newCam != branch) continue;
            currentCameraParent.GetComponent<CameraSwitch>().SwitchActiveCam();
            newCam.transform.GetComponent<CameraSwitch>().SwitchActiveCam();
        }
    }

    private bool DialogueOpenCheck() //check the dialogue box is active/inactive.
    {
        return _dialogueBox.activeSelf;
    }
}
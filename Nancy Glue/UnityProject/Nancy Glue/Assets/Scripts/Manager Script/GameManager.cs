using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Dialogue;
using Dialogue.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Introduction,
        DriveInInvestigation,
        GrizzlyInterrogation,
        SecondDriveInVisit,
        FilmReelFound,
    }

    [SerializeField] public GameState _gameState;
    [SerializeField] private NPCTracker[] _npcScripts;
    [SerializeField] private NPCTracker _AnatolyTracker;
    [SerializeField] private DialogueSystem _dialogueSystem;

    [SerializeField] private bool _introStarted;

    [SerializeField] private List<GameObject> _evidenceGameObjects;
    [SerializeField] private Animator _animator;
    [SerializeField] private Coroutine _zoneTransitionCoroutine;
    [SerializeField] private ZoneManager _zoneManager;

    public bool ReelFound;
    public bool InterogationFinished;

    //public int currentGS;
    
    void Awake()
    {

#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
#else
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
#endif
        _npcScripts = FindObjectsOfType<NPCTracker>();
        _AnatolyTracker = GameObject.Find("AnatolyDialogue").GetComponent<NPCTracker>();
        _dialogueSystem = FindObjectOfType<DialogueSystem>();

        
        _evidenceGameObjects = GameObject.FindGameObjectsWithTag("Evidence").ToList();
        _zoneManager = GetComponent<ZoneManager>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        if (NewOrLoad.isLoad)
        {
            //set in save load
        }
        else
        {
            _gameState = GameState.Introduction;
            _zoneTransitionCoroutine = StartCoroutine(FadeTransition());
        }
        
    }
    

    public void UpdateScene(int target)
    {
        for(var i = 0; i < _evidenceGameObjects.Count; i++)
        {
            var eviId = _evidenceGameObjects[i].GetComponent<ItemData>().EvidenceItem.ItemID;
            if(target == eviId)
            {
                Destroy(_evidenceGameObjects[i]);
                Debug.Log("destroyed item" + _evidenceGameObjects[i].GetComponent<ItemData>().EvidenceItem.ItemID);
                _evidenceGameObjects.RemoveAt(i);
                break;
            }
        }
    }

    void FixedUpdate()
    {
        switch (_gameState)
        {
            case GameState.Introduction:
                IntroductionUpdate();
                break;
            case GameState.DriveInInvestigation:
                //DriveInUpdate1();
                break;
            case GameState.GrizzlyInterrogation:
                GrizzlyInterrogation();
                break;
            case GameState.SecondDriveInVisit:
                DriveInUpdate2();
                break;
            case GameState.FilmReelFound:
                FoundReelUpdate();
                break;
        }
    }

#region IntroductionSegment
    private void IntroductionUpdate()
    {
        if (!_introStarted) return;
        if (_dialogueSystem.gameObject.activeSelf) return;
        StateSwitch(GameState.DriveInInvestigation, _zoneManager.OfficeCam, _zoneManager.DriveInCam);
    }
#endregion
    private void DriveInUpdate1()
    {
        foreach(var npc in _npcScripts)
        {
            if(npc.gameObject.name == "TedGrizzly")
            {
                if(npc.canBeQuestioned)
                {
                    _zoneManager.ConfrontedTed = true;
                    var oldCam = Camera.main.gameObject.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Name;
                    Debug.Log(oldCam);
                    var oldCamObj = GameObject.Find(oldCam);
                    var camSwitch = oldCamObj.transform.parent.transform.GetComponent<CameraSwitch>();
                    StateSwitch(GameState.GrizzlyInterrogation, camSwitch, _zoneManager.OfficeCam);
                }
                break;
            }
        }
    }

    private void GrizzlyInterrogation()
    {
        if (_dialogueSystem.gameObject.activeSelf) return;
        if (InterogationFinished)
            StateSwitch(GameState.SecondDriveInVisit, _zoneManager.OfficeCam, _zoneManager.DriveInCam);
    }

    private void DriveInUpdate2()
    {
        if (!ReelFound) return;
        var oldCam = Camera.main.gameObject.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Name;
        Debug.Log(oldCam);
        var oldCamObj = GameObject.Find(oldCam);
        var camSwitch = oldCamObj.transform.parent.transform.GetComponent<CameraSwitch>();
        StateSwitch(GameState.FilmReelFound, camSwitch, _zoneManager.OfficeCam);
    }

    private void FoundReelUpdate()
    {
        if (_dialogueSystem.gameObject.activeSelf) return;
        if(InterogationFinished)
            _zoneTransitionCoroutine = StartCoroutine(GameEnd());
    }

    private void NewStateSetup()
    {
        switch(_gameState)
        {
            case GameState.Introduction:
                SetIntroDialogue();
                break;
            case GameState.DriveInInvestigation:
                SetupDriveIn1();
                break;
            case GameState.GrizzlyInterrogation:
                SetupGrizzlyInt();
                break;
            case GameState.SecondDriveInVisit:
                SetupDriveIn2();
                break;
            case GameState.FilmReelFound:
                SetupReelFound();
                break;
        }
    }

#region gameState Setups
    private void SetIntroDialogue()
    {

        if (_introStarted) return;
        _dialogueSystem.SetContainer(_AnatolyTracker.GetCurrentContainer(), _AnatolyTracker);
        /*
         for (var i = 0; i < _npcScripts.Length; i++)
        {
            if (_npcScripts[i].name.Contains("Anatoly"))
            {
                _dialogueSystem.SetContainer(_npcScripts[i].DialogueContainers[0]);
                break;
            }
        }
        */
        _introStarted = true;
    }

    private void SetupDriveIn1()
    {
        var driveInRootCam = _zoneManager.DriveInCam;
        var projRoomcam = driveInRootCam.SwitchableCameras[1];
        projRoomcam.gameObject.SetActive(false);
        projRoomcam = driveInRootCam.SwitchableCameras[2];
        projRoomcam.gameObject.SetActive(false);
    }
    private void SetupGrizzlyInt()
    {
        var Teddy = GameObject.Find("TedGrizzly").transform;
        var InterrogationSeat = GameObject.Find("SeatLocation").transform;
        Teddy.transform.position = InterrogationSeat.position;
        Teddy.transform.eulerAngles = new Vector3(Teddy.eulerAngles.x, InterrogationSeat.eulerAngles.y, Teddy.eulerAngles.z);
        //_zoneTransitionCoroutine = StartCoroutine(DialogueStartup(Teddy.GetComponent<NPCTracker>().GetCurrentInterContainer()));
    }

    private void SetupDriveIn2()
    {
        var driveInRootCam = _zoneManager.DriveInCam;
        var underCouchCam = driveInRootCam.SwitchableCameras[2];
        underCouchCam.gameObject.SetActive(true);
        InterogationFinished = false;
    }
    private void SetupReelFound()
    {
        _AnatolyTracker.dialogueIterator++;
        _zoneTransitionCoroutine = StartCoroutine(DialogueStartup(_AnatolyTracker.GetCurrentContainer(), _AnatolyTracker));
    }

    private void StateSwitch(GameState gameState, CameraSwitch oldCam, CameraSwitch newCam)
    {
        _gameState = gameState;
        _zoneTransitionCoroutine = StartCoroutine(ZoneTransition(oldCam, newCam));
        NewStateSetup();
    }

    public void EvidenceChecker(Inventory inv)
    {
        foreach(var npc in _npcScripts)
        {
            Debug.Log(npc.name);
            //npc.EvidenceCheck(inv);
        }
    }

    public void ReelPickUp(string title)
    {
        if (title == "Film Reel")
            ReelFound = true;
    }

    IEnumerator DialogueStartup(DialogueContainerSO dialogueContainer, NPCTracker tracker)
    {
        yield return new WaitForSeconds(3f);
        _dialogueSystem.SetContainer(dialogueContainer, tracker);
        InterogationFinished = true;
    }

    IEnumerator ZoneTransition(CameraSwitch oldCam, CameraSwitch newCam)
    {
        _animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1);
        oldCam.SwitchActiveCam();
        newCam.SwitchActiveCam();
        while (Camera.main.GetComponent<CinemachineBrain>().IsBlending)
        {
            yield return null;

        }
        _animator.SetTrigger("FadeOut");
    }

    IEnumerator FadeTransition()
    {
        yield return new WaitForSeconds(2);
        SetIntroDialogue();
    }

    IEnumerator GameEnd()
    {
        _animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MenuScene");
    }
#endregion
}

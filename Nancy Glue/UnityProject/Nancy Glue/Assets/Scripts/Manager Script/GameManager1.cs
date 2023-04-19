using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Dialogue;
using Dialogue.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager1 : MonoBehaviour
{
    public enum GameState
    {
        Intro,                          // Anatoly indroduces case. Doesn't need tracking but initialises the scene.
        DriveIn,                        // Need to talk to Ted. Used to track where you were for loading.
        UnlockProjector,                // Unlock after tealking to ted the first time. Tracks the projector room cameras.
        GrizzlyInterrogation,           // Interrogate Ted. Track if this has been done to activate under couch cam.
        UnlockHiddenLiar,               // Unlock after crown has be picked up. Track to activate cameras in finger alley.
        FingerMonsterInterrogation,     // Interrogate after finding the crown and talking to them. Track to activate camera near briefcase.
        FingerMonsterInterrogation2,    // Interrogate after finding the briefcase. Track wheter crown has been picked up and if the player has handed it to one of them.
        OpenBriefcase,                  // Tracks if the chapter is complete.
        Idle
    }

    [SerializeField] public GameState _gameState;
    [SerializeField] private NPCTracker[] _npcTrackers;
    [SerializeField] private NPCTracker _AnatolyTracker;
    [SerializeField] private NPCTracker _NancyTracker;
    [SerializeField] private ItemScriptableObject CrownReference;
    [SerializeField] private DialogueSystem _dialogueSystem;

    [SerializeField] private bool _introStarted;

    [SerializeField] private List<GameObject> _evidenceGameObjects;
    [SerializeField] private Animator _animator;
    [SerializeField] private Coroutine _zoneTransitionCoroutine;
    [SerializeField] private ZoneManager _zoneManager;
    [SerializeField] private BriefcaseScript briefcase;

    private int[] stateTracker = new int[8] { 0,0,0,0,0,0,0,0 };

    public bool ReelFound;
    public bool InterogationFinished;

    //public int currentGS;
    
    void Awake()
    {

#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
#else
        Cursor.lockState = CursorLockMode.Confined;
#endif
        _npcTrackers = FindObjectsOfType<NPCTracker>();
        _AnatolyTracker = GameObject.Find("AnatolyDialogue").GetComponent<NPCTracker>();
        _NancyTracker = GameObject.Find("NancyDialogue").GetComponent<NPCTracker>();
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
            // Instatiate Level
            LoadProgress();
        }
        else
        {
            _gameState = GameState.Intro;
            IntroInit();
            _dialogueSystem.SetContainer(_AnatolyTracker.GetCurrentContainer(), _AnatolyTracker);
            _zoneTransitionCoroutine = StartCoroutine(FadeTransition());
        }
        
    }

    #region Load Progress
    private void LoadProgress()
    {
        // Init Scene
        DeactivateSceneCams();

        if (stateTracker[0] == 1)
        {
            IntroInit();
        }
        if (stateTracker[1] == 1)
        {
            DriveInInit();
        }
        if (stateTracker[2] == 1)
        {
            UnlockProjectorInit();
        }
        if (stateTracker[3] == 1)
        {
            GrizzlyInterrogateInit();
        }
        if (stateTracker[4] == 1)
        {
            UnlockHiddenLairInit();
        }
        if (stateTracker[5] == 1)
        {
            FingerInterrogate1Init();
        }
        if (stateTracker[6] == 1)
        {
            FingerInterrogate2Init();
        }
        if (stateTracker[7] == 1)
        {
            OpenBriefcaseInit();
        }
    }

    private void DeactivateSceneCams()
    {
        foreach(Transform camera in _zoneManager.OfficeCam.SwitchableCameras)
        {
            camera.gameObject.SetActive(false);
        }
        foreach(Transform camera in _zoneManager.AlleyCam.SwitchableCameras)
        {
            camera.gameObject.SetActive(false);
        }
        foreach(Transform camera in _zoneManager.DriveInCam.SwitchableCameras)
        {
            camera.gameObject.SetActive(false);
        }
    }

    private void IntroInit()
    {
        _zoneManager.OfficeCam.gameObject.SetActive(true);
        stateTracker[0] = 1;
    }

    private void DriveInInit()
    {
        _zoneManager.DriveInCam.gameObject.SetActive(true);
        _zoneManager.DriveInCam.SwitchableCameras[0].gameObject.SetActive(true);
        stateTracker[1] = 1;
    }

    private void UnlockProjectorInit()
    {
        _zoneManager.DriveInCam.SwitchableCameras[1].gameObject.SetActive(true);
        _zoneManager.DriveInCam.SwitchableCameras[1].GetComponent<CameraSwitch>().SwitchableCameras[0].gameObject.SetActive(true);
        stateTracker[2] = 1;
    }

    private void GrizzlyInterrogateInit()
    {
        _zoneManager.DriveInCam.SwitchableCameras[2].gameObject.SetActive(true);
        _zoneManager.DriveInCam.SwitchableCameras[2].GetComponent<CameraSwitch>().SwitchableCameras[0].gameObject.SetActive(true);
        stateTracker[3] = 1;
    }

    private void UnlockHiddenLairInit()
    {
        _zoneManager.AlleyCam.gameObject.SetActive(true);
        stateTracker[4] = 1;
    }

    private void FingerInterrogate1Init()
    {
        Debug.LogError("Activating camera");
        _zoneManager.AlleyCam.SwitchableCameras[0].gameObject.SetActive(true);
        stateTracker[5] = 1;
    }

    private void FingerInterrogate2Init()
    {
        // Add dialogue and code for briefcase
        briefcase.gameObject.SetActive(true);
        stateTracker[6] = 1;
    }

    private void OpenBriefcaseInit()
    {
        // Track that we have completed the tutorial case?
        SceneManager.LoadScene("MenuScene");
        stateTracker[7] = 1;
    }
    #endregion

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
            case GameState.Idle:
                UpdateStates();
                break;

            case GameState.Intro:
                IntroInit();
                _gameState = GameState.Idle;
                break;

            case GameState.DriveIn:
                DriveInInit();
                _gameState = GameState.Idle;
                break;

            case GameState.UnlockProjector:
                UnlockProjectorInit();
                _gameState = GameState.Idle;
                break;

            case GameState.GrizzlyInterrogation:
                GrizzlyInterrogateInit();
                _gameState = GameState.Idle;
                break;

            case GameState.UnlockHiddenLiar:
                UnlockHiddenLairInit();
                _dialogueSystem.SetContainer(_NancyTracker.GetCurrentContainer(), _NancyTracker);
                _gameState = GameState.Idle;
                break;

            case GameState.FingerMonsterInterrogation:
                FingerInterrogate1Init();
                _gameState = GameState.Idle;
                break;

            case GameState.FingerMonsterInterrogation2:
                FingerInterrogate2Init();
                _gameState = GameState.Idle;
                break;

            case GameState.OpenBriefcase:
                OpenBriefcaseInit();
                _gameState = GameState.Idle;
                break;
        }
    }

    private void UpdateStates()
    {
        // Intro
        if (stateTracker[0] == 0)
        {
            // Happens on launch of a new game
            _gameState = GameState.Intro;
        }
        // Drive In
        else if (stateTracker[1] == 0)
        {
            if (stateTracker[0] == 1)
            {
                // Happens on launch of a new game after the initial dialogue
                _gameState = GameState.DriveIn;
            }
        }
        // Unlock Projector
        else if (stateTracker[2] == 0)
        {
            foreach (var npc in _npcTrackers)
            {
                if (npc.gameObject.name == "TedGrizzly")
                {
                    if (npc.dialogueIterator > 0)
                    {
                        // Have you talked to Ted
                        _gameState = GameState.UnlockProjector;
                    }
                }
            }
        }
        // Ted Interrogation
        else if (stateTracker[3] == 0)
        {
            foreach (var npc in _npcTrackers)
            {
                if (npc.gameObject.name == "TedGrizzly")
                {
                    if (npc.dialogueIterator > 1)
                    {
                        // Have you interrogated Ted
                        _gameState = GameState.GrizzlyInterrogation;
                    }
                }
            }
        }
        // Unlock Hidden Lair
        else if (stateTracker[4] == 0)
        {
            ItemData[] itemsArray = (ItemData[])FindSceneObjectsOfType(typeof(ItemData));

            bool itemCollected = true;
            foreach (ItemData item in itemsArray)
            {
                var itemId = item.EvidenceItem.ItemID;
                Debug.Log("Searching items");
                if (itemId == CrownReference.ItemID) itemCollected = false;
            }

            if (itemCollected)
            {
                _gameState = GameState.UnlockHiddenLiar;
            }
        }
        // Finger Interrogation
        else if (stateTracker[5] == 0)
        {
            foreach (var npc in _npcTrackers)
            {
                if (npc.gameObject.name == "FingerMonsterA")
                {
                    if (npc.interrogationIterator > 0)
                    {
                        // Have you interrogated Ted
                        _gameState = GameState.FingerMonsterInterrogation;
                    }
                }
            }
        }
        // Finger Interrogation 2
        else if (stateTracker[6] == 0)
        {
            foreach (var npc in _npcTrackers)
            {
                if (npc.gameObject.name == "FingerMonsterA")
                {
                    if (npc.interrogationIterator > 1)
                    {
                        // Have you interrogated FingerMonsters
                        _gameState = GameState.FingerMonsterInterrogation2;
                    }
                }
            }
        }
        else if (stateTracker[7] == 0)
        {
            Debug.Log(briefcase.Clicked);
            // Not sure what the trigger is here
            if(briefcase.Clicked)
            {
                _gameState = GameState.OpenBriefcase;
            }
        }
    }

    #region IntroductionSegment
    /*
    private void IntroductionUpdate()
    {
        if (!_introStarted) return;
        if (_dialogueSystem.gameObject.activeSelf) return;
        StateSwitch(GameState.DriveInInvestigation, _zoneManager.OfficeCam, _zoneManager.DriveInCam);
    }
    */
    #endregion
    /*
    private void DriveInUpdate1()
    {
        foreach(var npc in _npcTrackers)
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
    */
    #region gameState Setups
    /*
    private void SetIntroDialogue()
    {

        if (_introStarted) return;
        _dialogueSystem.SetContainer(_AnatolyTracker.GetCurrentContainer());
        /*
         for (var i = 0; i < _npcTrackers.Length; i++)
        {
            if (_npcTrackers[i].name.Contains("Anatoly"))
            {
                _dialogueSystem.SetContainer(_npcTrackers[i].DialogueContainers[0]);
                break;
            }
        }
        *//*
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
        _zoneTransitionCoroutine = StartCoroutine(DialogueStartup(Teddy.GetComponent<NPCTracker>().GetCurrentInterContainer()));
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
        _zoneTransitionCoroutine = StartCoroutine(DialogueStartup(_AnatolyTracker.GetCurrentContainer()));
    }

    private void StateSwitch(GameState gameState, CameraSwitch oldCam, CameraSwitch newCam)
    {
        _gameState = gameState;
        _zoneTransitionCoroutine = StartCoroutine(ZoneTransition(oldCam, newCam));
        NewStateSetup();
    }

    public void EvidenceChecker(Inventory inv)
    {
        foreach(var npc in _npcTrackers)
        {
            Debug.Log(npc.name);
            //npc.EvidenceCheck(inv);
        }
    }

    public void ReelPickUp(string title)
    {
        if (title == "Film Reel")
            ReelFound = true;
    }*/
    /*
    IEnumerator DialogueStartup(DialogueContainerSO dialogueContainer)
    {
        yield return new WaitForSeconds(3f);
        _dialogueSystem.SetContainer(dialogueContainer, );
        InterogationFinished = true;
    }
    */
    /*
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
    */

    IEnumerator FadeTransition()
    {
        yield return new WaitForSeconds(2);
        //SetIntroDialogue();
    }

    IEnumerator GameEnd()
    {
        _animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MenuScene");
    }
#endregion
}

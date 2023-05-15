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
        CollectedStuffing,              // Used during gameplay to trigger dialogue
        GrizzlyInterrogation,           // Interrogate Ted. Track if this has been done to activate under couch cam.
        UnlockHiddenLiar,               // Unlock after crown has be picked up. Track to activate cameras in finger alley.
        FingerMonsterInterrogation,     // Interrogate after finding the crown and talking to them. Track to activate camera near briefcase.
        CollectedBriefcase,             // Used during gameplay to trigger dialogue
        FingerMonsterInterrogation2,    // Interrogate after finding the briefcase. Track wheter crown has been picked up and if the player has handed it to one of them.
        OpenBriefcase,                  // Tracks if the chapter is complete.
        End,
        Idle
    }

    [SerializeField] public GameState _gameState;
    [SerializeField] private NPCTracker[] _npcTrackers;
    [SerializeField] private NPCTracker _AnatolyTracker;
    [SerializeField] private NPCTracker _PhoneyTracker;
    [SerializeField] private ItemScriptableObject StuffingReference;
    [SerializeField] private ItemScriptableObject CrownReference;
    [SerializeField] private ItemScriptableObject BriefcaseReference;
    [SerializeField] private DialogueSystem _dialogueSystem;

    [SerializeField] private bool _introStarted;
    [SerializeField] private Inventory _inv;
    [SerializeField] private List<GameObject> _evidenceGameObjects;
    [SerializeField] private Animator _animator;
    [SerializeField] private Coroutine _zoneTransitionCoroutine;
    [SerializeField] private ZoneManager _zoneManager;
    [SerializeField] private BriefcaseScript briefcase;
    public SaveLoadGameState SLGS;
    public int[] stateTracker = new int[11] { 0,0,0,0,0,0,0,0,0,0,0 };

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
        _inv = FindObjectOfType<Inventory>();
        _npcTrackers = FindObjectsOfType<NPCTracker>();
        _AnatolyTracker = GameObject.Find("AnatolyDialogue").GetComponent<NPCTracker>();
        _dialogueSystem = FindObjectOfType<DialogueSystem>();

        
        _evidenceGameObjects = GameObject.FindGameObjectsWithTag("Evidence").ToList();
        _zoneManager = GetComponent<ZoneManager>();
        SLGS = FindObjectOfType<SaveLoadGameState>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (NewOrLoad.isLoad)
        {
            SLGS.Load();
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
    public void LoadProgress()
    {
        // Init Scene
        //DeactivateSceneCams();

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
        if (stateTracker[4] == 1)
        {
            GrizzlyInterrogateInit();
        }
        if (stateTracker[5] == 1)
        {
            UnlockHiddenLairInit();
        }
        if (stateTracker[6] == 1)
        {
            FingerInterrogate1Init();
        }
        if (stateTracker[8] == 1)
        {
            FingerInterrogate2Init();
        }
        if (stateTracker[9] == 1)
        {
            OpenBriefcaseInit();
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
        stateTracker[4] = 1;
    }

    private void UnlockHiddenLairInit()
    {
        _zoneManager.AlleyCam.gameObject.SetActive(true);
        stateTracker[5] = 1;
    }

    private void FingerInterrogate1Init()
    {
        _zoneManager.AlleyCam.SwitchableCameras[0].gameObject.SetActive(true);
        stateTracker[6] = 1;
    }

    private void FingerInterrogate2Init()
    {
        // Add dialogue and code for briefcase
        briefcase.gameObject.SetActive(true);
        stateTracker[8] = 1;
    }

    private void OpenBriefcaseInit()
    {
        // Track that we have completed the tutorial case?
        _dialogueSystem.SetContainer("BriefCaseEnd");
        stateTracker[9] = 1;
    }
    #endregion

    public void UpdateScene(int target) //add the given item into the players inventory. Find the item with the corresponding ID in world and delete it. remove the item from the game managers list.
    {
        for(var i = 0; i < _evidenceGameObjects.Count; i++)
        {
            var eviId = _evidenceGameObjects[i].GetComponent<ItemData>().EvidenceItem.ItemID;
            if(target == eviId)
            {
                _inv.GiveItem(eviId);
                Destroy(_evidenceGameObjects[i]);
                //Debug.Log("destroyed item" + _evidenceGameObjects[i].GetComponent<ItemData>().EvidenceItem.ItemID);
                _evidenceGameObjects.RemoveAt(i);
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
                _PhoneyTracker.ProgressDialogue(NPCTracker.ProgressTriggers.phoney);
                _gameState = GameState.Idle;
                break;

            case GameState.CollectedStuffing:

                _dialogueSystem.SetContainer("StuffingInternalDialogue");//this is a problem on load - can this be skipped safely?
                _PhoneyTracker.ProgressDialogue(NPCTracker.ProgressTriggers.phoney);
                stateTracker[3] = 1;
                _gameState = GameState.Idle;
                break;

            case GameState.GrizzlyInterrogation:
                GrizzlyInterrogateInit();
                _PhoneyTracker.ProgressDialogue(NPCTracker.ProgressTriggers.phoney);
                _gameState = GameState.Idle;
                break;

            case GameState.UnlockHiddenLiar:
                UnlockHiddenLairInit();
                _PhoneyTracker.ProgressDialogue(NPCTracker.ProgressTriggers.phoney);
                _dialogueSystem.SetContainer("NancyInternalDialogue");//this is a problem on load - can this be skipped safely?
                _gameState = GameState.Idle;
                break;

            case GameState.FingerMonsterInterrogation:
                FingerInterrogate1Init();
                _PhoneyTracker.ProgressDialogue(NPCTracker.ProgressTriggers.phoney);
                _gameState = GameState.Idle;
                break;

            case GameState.CollectedBriefcase:
                _dialogueSystem.SetContainer("BriefcaseInternalDialogue");//this is a problem on load - can this be skipped safely?
                _PhoneyTracker.ProgressDialogue(NPCTracker.ProgressTriggers.phoney);
                stateTracker[7] = 1;
                _gameState = GameState.Idle;
                break;

            case GameState.FingerMonsterInterrogation2:
                FingerInterrogate2Init();
                _PhoneyTracker.ProgressDialogue(NPCTracker.ProgressTriggers.phoney);
                _gameState = GameState.Idle;
                break;

            case GameState.OpenBriefcase:
                OpenBriefcaseInit();
                _gameState = GameState.Idle;
                break;

            case GameState.End:
                SceneManager.LoadScene("CreditsScene");
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
        // Collect Stuffing
        else if (stateTracker[3] == 0)
        {
            ItemData[] itemsArray = (ItemData[])FindSceneObjectsOfType(typeof(ItemData));

            bool itemCollected = true;
            foreach (ItemData item in itemsArray)
            {
                var itemId = item.EvidenceItem.ItemID;
                //Debug.Log("Searching items");
                if (itemId == StuffingReference.ItemID) itemCollected = false;
            }

            if (itemCollected)
            {
                _gameState = GameState.CollectedStuffing;
            }
        }
        // Ted Interrogation
        else if (stateTracker[4] == 0)
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
        else if (stateTracker[5] == 0)
        {
            ItemData[] itemsArray = (ItemData[])FindSceneObjectsOfType(typeof(ItemData));

            bool itemCollected = true;
            foreach (ItemData item in itemsArray)
            {
                var itemId = item.EvidenceItem.ItemID;
                //Debug.Log("Searching items");
                if (itemId == CrownReference.ItemID) itemCollected = false;
            }

            if (itemCollected)
            {
                _gameState = GameState.UnlockHiddenLiar;
            }
        }
        // Finger Interrogation
        else if (stateTracker[6] == 0)
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
        // Collect Briefcase
        else if (stateTracker[7] == 0)
        {
            ItemData[] itemsArray = (ItemData[])FindSceneObjectsOfType(typeof(ItemData));

            bool itemCollected = true;
            foreach (ItemData item in itemsArray)
            {
                var itemId = item.EvidenceItem.ItemID;
                //Debug.Log("Searching items");
                if (itemId == BriefcaseReference.ItemID) itemCollected = false;
            }

            if (itemCollected)
            {
                _gameState = GameState.CollectedBriefcase;
            }
        }
        // Finger Interrogation 2
        else if (stateTracker[8] == 0)
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
        else if (stateTracker[9] == 0)
        {
            if(briefcase.Clicked)
            {
                _gameState = GameState.OpenBriefcase;
            }
        }
        else if (stateTracker[10] == 0)
        {
            if(!_dialogueSystem.inDialogue)
            {
                _gameState = GameState.End;
            }
        }
    }

    IEnumerator FadeTransition()
    {
        yield return new WaitForSeconds(2);
    }

    IEnumerator GameEnd()
    {
        _animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MenuScene");
    }
}

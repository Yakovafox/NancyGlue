using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dialogue;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        Introduction,
        DriveInInvestigation,
        GrizzlyInterrogation,
        SecondDriveInVisit,
        FilmReelFound,
    }

    [SerializeField] private GameState _gameState;
    [SerializeField] private npcScript[] _npcScripts;
    [SerializeField] private DialogueSystem _dialogueSystem;

    [SerializeField] private bool _introStarted;

    [SerializeField] private List<GameObject> _evidenceGameObjects;

    void Awake()
    {

#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
#else
        Cursor.LockState = CursorLockMode.Confined;
#endif
        Cursor.visible = false;
        _npcScripts = FindObjectsOfType<npcScript>();
        _dialogueSystem = FindObjectOfType<DialogueSystem>();

        /*
        var evidence = GameObject.FindGameObjectsWithTag("Evidence").ToList();
        foreach(var e in evidence)
        {
           var id = e.GetComponent<ItemData>().EvidenceItem.ItemID;
            _evidenceGameObjectIDs.Add(id);
        }
        */

    }

    // Start is called before the first frame update
    void Start()
    {
        _gameState = GameState.Introduction;
        SetIntroDialogue();
    }

    void FixedUpdate()
    {
        switch (_gameState)
        {
            case GameState.Introduction:
                IntroductionUpdate();
                break;
            case GameState.DriveInInvestigation:
                DriveInUpdate1();
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
        if (_dialogueSystem.gameObject.activeSelf) return;
        SetIntroDialogue();
    }
#endregion
    private void DriveInUpdate1()
    {

    }

    private void GrizzlyInterrogation()
    {

    }

    private void DriveInUpdate2()
    {

    }

    private void FoundReelUpdate()
    {

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
        for (var i = 0; i < _npcScripts.Length; i++)
        {
            if (_npcScripts[i].name.Contains("Anatoly"))
            {
                _dialogueSystem.SetContainer(_npcScripts[i].DialogueContainers[0]);
                break;
            }
        }
        _introStarted = true;
    }

    private void SetupDriveIn1()
    {

    }
    private void SetupGrizzlyInt()
    {

    }

    private void SetupDriveIn2()
    {

    }
    private void SetupReelFound()
    {

    }

#endregion
}

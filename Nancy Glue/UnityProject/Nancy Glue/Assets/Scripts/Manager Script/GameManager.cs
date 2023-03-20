using System.Collections;
using System.Collections.Generic;
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

    void Awake()
    {
        _npcScripts = FindObjectsOfType<npcScript>();
        _dialogueSystem = FindObjectOfType<DialogueSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameState = GameState.Introduction;
        SetIntroDialogue();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        switch (_gameState)
        {
            case GameState.Introduction:
                break;
            case GameState.DriveInInvestigation:
                break;
            case GameState.GrizzlyInterrogation:
                break;
            case GameState.SecondDriveInVisit:
                break;
            case GameState.FilmReelFound:
                break;
        }
    }

    #region IntroductionSegment
    private void IntroductionUpdate()
    {
        //SetIntroDialogue();
    }

    private void SetIntroDialogue()
    {
        if (_introStarted) return;
        for (var i = 0; i < _npcScripts.Length; i++)
        {
            if (_npcScripts[i].name.Contains("Anatoly"))
                _dialogueSystem.SetContainer(_npcScripts[i].DialogueContainers[0]);
            break;
        }
        
        _introStarted = true;
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
}

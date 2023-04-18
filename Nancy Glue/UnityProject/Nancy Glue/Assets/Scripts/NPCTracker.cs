using Dialogue.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTracker : MonoBehaviour
{

    
    public enum ProgressTriggers
    {
        talking,
        evidence,
        sceneChange,
        END
    }

    [Serializable]
    public struct DialogueInformation
    {
        public DialogueContainerSO Container;
        public ProgressTriggers Trigger;
        public GameObject EvidenceRequired;
    }

    // Tracking
    [SerializeField] private List<DialogueInformation> Dialogues=new List<DialogueInformation>();
    [SerializeField] private int CanQuestionAt;
    [SerializeField] private npcScript[] npcScripts;
    public bool canBeQuestioned;

    [field: Header("Data For UI")]
    [field: SerializeField] public string CharName { get; private set; }
    [field: SerializeField] public bool SpokenTo { get; set; }
    [field: SerializeField] public Sprite CharacterSprite { get; private set; }

    // Saved
    public int dialogueIterator = 0;
    public string attachedNPC = "";
    [SerializeField] private List<string> notes = new List<string>();
    public List<string> Notes { get => notes; }

    private void Awake()
    {
        if (dialogueIterator >= CanQuestionAt)
        {
            canBeQuestioned = true;
        }

    }

    public void Reset()
    {
        dialogueIterator = 0;
        attachedNPC = transform.name;
    }

    public void ProgressDialogue(ProgressTriggers trigger)
    {
        if (Dialogues[dialogueIterator].Trigger == trigger)
        {
            if (trigger == ProgressTriggers.evidence)
            {
                ItemData[] itemsArray = (ItemData[]) FindSceneObjectsOfType(typeof(ItemData));

                bool itemCollected = true;
                foreach (ItemData item in itemsArray)
                {
                    Debug.Log("Searching items");
                    if (item.transform.gameObject == Dialogues[dialogueIterator].EvidenceRequired) itemCollected = false;
                }

                if (itemCollected) dialogueIterator++;
            }
            else
            {
                if (Dialogues[dialogueIterator].Trigger == trigger)
                {
                    dialogueIterator++;
                }
            }
        }

        if (dialogueIterator == CanQuestionAt)
        {
            canBeQuestioned = true;
        }
    }

    public void AddNote(string note)
    {
        if (!notes.Contains(note))
        {
            notes.Add(note);
        }
    }

    public void onLoadGame(string name, int iterator)
    {
        if (name == attachedNPC)
        {
            dialogueIterator = iterator;
        }
        else
        {
            Debug.Log("NPC not found in dataSet");


        }

    }

    public DialogueContainerSO GetCurrentContainer()
    {
        return Dialogues[dialogueIterator].Container;
    }


    public void OnSaveGame()
    {
        //NPCsaveData.diaData.Add(attachedNPC, dialogueIterator);


    }



}

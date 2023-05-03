using Dialogue.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCTracker : MonoBehaviour
{
    public enum ProgressTriggers
    {
        talking,
        END,
        evidence
    }

    [Serializable]
    public struct DialogueInformation
    {
        public DialogueContainerSO Container;
        public ItemScriptableObject EvidenceRequired;
        public ProgressTriggers Trigger;
    }

    [Serializable]
    public struct InterrogationInformation
    {
        public DialogueContainerSO Container;
        public ItemScriptableObject[] EvidenceRequired;
        public int ProgressTo;
    }

    // Tracking
    [SerializeField] private List<DialogueInformation> Dialogues = new List<DialogueInformation>();
    [SerializeField] private List<InterrogationInformation> Interrogations = new List<InterrogationInformation>();
    public bool canBeQuestioned;

    [field: Header("Data For UI")]
    [field: SerializeField] public string CharName { get; private set; }
    [field: SerializeField] public bool SpokenTo { get; set; }
    [field: SerializeField] public Sprite CharacterSprite { get; private set; }

    public Vector3 originalPosition;

    // Saved
    public int dialogueIterator = 0;
    public int interrogationIterator = 0;
    public string attachedNPC = "";
    
    [SerializeField] private List<string> notes = new List<string>();
    public List<string> Notes { get => notes; }

    private void Awake()
    {
        originalPosition = transform.position;
        attachedNPC = transform.name;
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
                ItemData[] itemsArray = (ItemData[])FindSceneObjectsOfType(typeof(ItemData));

                bool itemCollected = true;
                foreach (ItemData item in itemsArray)
                {
                    var itemId = item.EvidenceItem.ItemID;
                    if (itemId == Dialogues[dialogueIterator].EvidenceRequired.ItemID) return;
                }
            }

            dialogueIterator++;
        }
    }

    public void EvidenceCheck()
    {
        if (interrogationIterator >= Interrogations.Count) return;

        ItemData[] itemsArray = (ItemData[])FindSceneObjectsOfType(typeof(ItemData));

        bool itemCollected = true;
        for (int i = 0; i < Interrogations[interrogationIterator].EvidenceRequired.Length; i++)
        { 
            foreach (ItemData item in itemsArray)
            {
                var itemId = item.EvidenceItem.ItemID;
                Debug.Log("Searching items");
                if (itemId == Interrogations[interrogationIterator].EvidenceRequired[i].ItemID) itemCollected = false;
            }
        }

        if (itemCollected) canBeQuestioned = true;
    }

    public void AddNote(string note)
    {
        if (!notes.Contains(note) && notes.Count != 3)
        {
            notes.Add(note);
        }
    }

    public void onLoadGame(string name, int diaIterator, int intIterator, string[] saveNotes)
    {
        if (name == attachedNPC)
        {
            dialogueIterator = diaIterator;
            interrogationIterator = intIterator;
            notes = saveNotes.ToList();
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

    public DialogueContainerSO GetCurrentInterContainer()
    {
        int currentItterator = interrogationIterator;
        interrogationIterator++;
        dialogueIterator = Interrogations[currentItterator].ProgressTo;
        canBeQuestioned = false;
        return Interrogations[currentItterator].Container;
    }
}

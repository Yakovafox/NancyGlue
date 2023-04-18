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
        END
    }

    [Serializable]
    public struct DialogueInformation
    {
        public DialogueContainerSO Container;
        public ProgressTriggers Trigger;
    }

    [Serializable]
    public struct InterrigationInformation
    {
        public DialogueContainerSO Container;
        public ItemScriptableObject[] EvidenceRequired;
        public int ProgressTo;
    }

    // Tracking
    [SerializeField] private List<DialogueInformation> Dialogues = new List<DialogueInformation>();
    [SerializeField] private List<InterrigationInformation> Interrigations = new List<InterrigationInformation>();
    [SerializeField] private npcScript[] npcScripts;
    public bool canBeQuestioned;

    [field: Header("Data For UI")]
    [field: SerializeField] public string CharName { get; private set; }
    [field: SerializeField] public bool SpokenTo { get; set; }
    [field: SerializeField] public Sprite CharacterSprite { get; private set; }

    // Saved
    public int dialogueIterator = 0;
    public int interregationIterator = 0;
    public string attachedNPC = "";
    [SerializeField] private List<string> notes { get; } = new List<string>();

    public void Reset()
    {
        dialogueIterator = 0;
        attachedNPC = transform.name;
    }

    public void ProgressDialogue(ProgressTriggers trigger)
    {
        if (Dialogues[dialogueIterator].Trigger == trigger)
        {
            dialogueIterator++;
        }
    }

    public void EvidenceCheck()
    {
        ItemData[] itemsArray = (ItemData[])FindSceneObjectsOfType(typeof(ItemData));

        bool itemCollected = true;
        for (int i = 0; i < Interrigations[interregationIterator].EvidenceRequired.Length; i++)
        { 
            foreach (ItemData item in itemsArray)
            {
                var itemId = item.EvidenceItem.ItemID;
                Debug.Log("Searching items");
                if (itemId == Interrigations[interregationIterator].EvidenceRequired[i].ItemID) itemCollected = false;
            }
        }

        if (itemCollected) canBeQuestioned = true;
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

    public DialogueContainerSO GetCurrentInterContainer()
    {
        return Interrigations[interregationIterator].Container;
    }
}

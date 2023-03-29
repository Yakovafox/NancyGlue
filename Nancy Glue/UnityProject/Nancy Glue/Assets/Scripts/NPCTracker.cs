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
    [SerializeField] private List<DialogueInformation> Dialogues;
    [SerializeField] private int CanQuestionAt;
    public bool canBeQuestioned;

    // Saved
    public int dialogueIterator = 0;
    public string attachedNPC = "";

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

    public DialogueContainerSO GetCurrentContainer()
    {
        return Dialogues[dialogueIterator].Container;
    }


    public void OnSaveGame()
    {
        //NPCsaveData.diaData.Add(attachedNPC, dialogueIterator);


    }

    public void OnLoadGame()
    {

    }




}

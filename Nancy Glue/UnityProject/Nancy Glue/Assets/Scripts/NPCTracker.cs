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
        sceneChange
    }

    // Tracking
    [SerializeField] private Dictionary<DialogueContainerSO, ProgressTriggers> Dialogues;
    [SerializeField] private int CanQuestionAt;
    public bool canBeQuestioned;

    // Saved
    public int dialogueIterator = 0;
    private string attachedNPC = "";

    public void Reset()
    {
        dialogueIterator = 0;
        attachedNPC = transform.name;
    }

    public void ProgressDialogue(DialogueContainerSO dialogue, ProgressTriggers trigger)
    {
        foreach (KeyValuePair<DialogueContainerSO, ProgressTriggers> container in Dialogues)
        {
            // Search until we find the container we want
            if (dialogue.fileName != container.Key.fileName) continue;

            if (container.Value == trigger) dialogueIterator++;

            if (dialogueIterator == CanQuestionAt) canBeQuestioned = true;
            
            // Return as we have found the container
            return;
        }

        // Debug if we get here as it means the NPC doesn't have the script we are refering to
        Debug.LogWarning("NPC does not have container at filepath " + dialogue.fileName);
    }

    // FUNCTION FOR ADAM xoxo
    private void Save()
    {
        // Save dialogueIterator

        attachedNPC = transform.name;
        // Save attachedNPC
    }
}

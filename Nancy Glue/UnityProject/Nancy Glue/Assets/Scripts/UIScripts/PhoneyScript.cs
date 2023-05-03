using Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue.ScriptableObjects;

public class PhoneyScript : MonoBehaviour
{
    public int helpTracker;
    [SerializeField] private DialogueContainerSO[] _helpContainers;
    [SerializeField] private DialogueSystem _dialogueSystem;

    public void GetHelpDialogue()
    {
        _dialogueSystem.SetContainer(_helpContainers[helpTracker]);
    }
}

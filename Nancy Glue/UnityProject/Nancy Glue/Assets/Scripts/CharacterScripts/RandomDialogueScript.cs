using Dialogue;
using Dialogue.ScriptableObjects;
using UnityEngine;

public class RandomDialogueScript : MonoBehaviour
{
    [SerializeField] private DialogueContainerSO[] _dialogues;

    public void SelectRandomDialogue() //Pick a random dialogue from the array.
    {
        var random = Random.Range(0, _dialogues.Length);
        var dSystem = FindObjectOfType<DialogueSystem>(true);
        dSystem.SetContainer(_dialogues[random], null);
    }
}

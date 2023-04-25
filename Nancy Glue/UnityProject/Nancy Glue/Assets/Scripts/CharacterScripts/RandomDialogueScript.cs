using Dialogue;
using Dialogue.ScriptableObjects;
using UnityEngine;

public class RandomDialogueScript : MonoBehaviour
{
    [SerializeField] private DialogueContainerSO[] _dialogues;

    public void SelectRandomDialogue()
    {
        var random = Random.Range(0, _dialogues.Length + 1);
        var dSystem = FindObjectOfType<DialogueSystem>();
        dSystem.SetContainer(_dialogues[random], null);
    }
}

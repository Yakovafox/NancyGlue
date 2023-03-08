using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.ScriptableObjects
{
    using Data;
    using Enumerators;

    public class DialogueSO : ScriptableObject
    {
        [field: SerializeField] public string dialogueName { get; set; }
        [field: SerializeField] public string characterName { get; set; }
        [field: SerializeField] public DialogueType dialogueType { get; set; }
        [field: SerializeField] public List<DialogueChoiceData> dialogueChoices { get; set; }
        [field: SerializeField] [field: TextArea()] public string dialogueText { get; set; }
        [field: SerializeField] public string dialogueSpriteAssetPath { get; set; }
        [field: SerializeField] public string dialogueAudioAssetPath { get; set; }
        [field: SerializeField] public bool isStartingDialogue { get; set; }

        public void Init(string name, string text, List<DialogueChoiceData> choices, DialogueType type, bool starting, string character, string spriteAssetPath, string audioAssetPath)
        {
            dialogueName = name;
            dialogueText = text;
            dialogueChoices = choices;
            dialogueType = type;
            isStartingDialogue = starting;
            characterName = character;
            dialogueSpriteAssetPath = spriteAssetPath;
            dialogueAudioAssetPath = audioAssetPath;
        }
    }
}
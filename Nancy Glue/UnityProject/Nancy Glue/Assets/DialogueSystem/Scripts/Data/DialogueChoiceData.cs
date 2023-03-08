using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Dialogue.Data
{
    using ScriptableObjects;

    [Serializable]
    public class DialogueChoiceData
    {
        [field: SerializeField] public string text { get; set; }
        [field: SerializeField] public DialogueSO NextDialogue { get; set; }
    }
}
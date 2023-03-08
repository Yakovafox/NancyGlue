using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Data.Save
{
    using Enumerators;

    [Serializable]
    public class NodeSaveData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public string dialogueName { get; set; }
        [field: SerializeField] public string characterName { get; set; }
        [field: SerializeField] public string text { get; set; }
        [field: SerializeField] public List<ChoiceSaveData> options { get; set; }
        [field: SerializeField] public string groupID { get; set; }
        [field: SerializeField] public DialogueType type { get; set; }
        [field: SerializeField] public Vector2 position { get; set; }
        [field: SerializeField] public string spritePath { get; set; }
        [field: SerializeField] public string audioPath { get; set; }

    }
}
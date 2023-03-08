using System;
using UnityEngine;

namespace Dialogue.Data.Save
{
    [Serializable]
    public class GroupSaveData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public string name { get; set; }
        [field: SerializeField] public Vector2 position { get; set; }
    }
}
using System;
using UnityEngine;

namespace Dialogue.Data.Save
{
    [Serializable]
    public class ChoiceSaveData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string NodeID { get; set; }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.ScriptableObjects
{
    public class DialogueGroupSO : ScriptableObject
    {
        [field: SerializeField] public string groupName { get; set; }

        public void Init(string name)
        {
            groupName = name;
        }
    }
}
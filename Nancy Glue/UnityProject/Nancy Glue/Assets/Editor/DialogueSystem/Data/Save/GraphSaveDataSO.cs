using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Data.Save
{
    public class GraphSaveDataSO : ScriptableObject
    {
        [field: SerializeField] public string fileName { get; set; }
        [field: SerializeField] public List<GroupSaveData> groups { get; set; }
        [field: SerializeField] public List<NodeSaveData> nodes { get; set; }
        [field: SerializeField] public List<string> oldGroupNames { get; set; }
        [field: SerializeField] public List<string> oldUngroupedNames { get; set; }
        [field: SerializeField] public SerializableDictionary<string, List<string>> oldGroupedNames { get; set; }
    
        public void Init(string name)
        {
            fileName = name;

            groups = new List<GroupSaveData>();
            nodes = new List<NodeSaveData>();
        }
    }
}
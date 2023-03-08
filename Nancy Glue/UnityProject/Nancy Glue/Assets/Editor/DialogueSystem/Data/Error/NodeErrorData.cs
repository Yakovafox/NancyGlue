using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Data.Error
{
    using Elements;

    public class NodeErrorData
    {
        public ErrorData ErrorData { get; set; }
        public List<DialogueNode> Nodes { get; set; }
        public NodeErrorData()
        {
            ErrorData = new ErrorData();
            Nodes = new List<DialogueNode>();
        }
    }
}
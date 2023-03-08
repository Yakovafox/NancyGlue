using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Dialogue.Data.Error
{
    using Dialogue.Elements;
    public class GroupErrorData
    {
        public ErrorData errorData { get; set; }
        public List<DialogueGroup> groups { get; set; }

        public GroupErrorData()
        {
            errorData = new ErrorData();
            groups = new List<DialogueGroup>();
        }
    }
}
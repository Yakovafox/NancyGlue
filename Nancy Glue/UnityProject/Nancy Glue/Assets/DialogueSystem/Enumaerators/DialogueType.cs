using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Enumerators
{
    // Enumerator values for the types of dialogue
    public enum DialogueType
    {
        SingleChoice,       // Single for if there's a progression to single box
        MultiChoice,        // Multi for if there's a dialogue tree
        Evidence            // Evidence for if it should be recorded
    }
}

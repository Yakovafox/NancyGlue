using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.ScriptableObjects
{
    [Serializable]
    public class DialogueContainerSO : ScriptableObject
    {
        [field: SerializeField] public string fileName { get; set; }
        [field: SerializeField] public SerializableDictionary<DialogueGroupSO, List<DialogueSO>> dialogueGroups { get; set; }
        [field: SerializeField] public List<DialogueSO> ungroupedDialogues { get; set; }

        public void Init(string name)
        {
            fileName = name;

            dialogueGroups = new SerializableDictionary<DialogueGroupSO, List<DialogueSO>>();
            ungroupedDialogues = new List<DialogueSO>();
        }

        public List<string> GetDialogueGroupNames()
        {
            List<string> dialogueGroupNames = new List<string>();

            foreach (DialogueGroupSO dialogueGroup in dialogueGroups.Keys)
            {
                dialogueGroupNames.Add(dialogueGroup.groupName);
            }

            return dialogueGroupNames;
        }

        public List<string> GetGroupedDialogueNames(DialogueGroupSO dialogueGroup, bool startingDialoguesOnly)
        {
            List<DialogueSO> groupedDialogues = dialogueGroups[dialogueGroup];

            List<string> groupedDialogueNames = new List<string>();

            foreach (DialogueSO groupedDialogue in groupedDialogues)
            {
                if (startingDialoguesOnly && !groupedDialogue.isStartingDialogue) 
                {
                    continue;
                }

                groupedDialogueNames.Add(groupedDialogue.dialogueName);
            }

            return groupedDialogueNames;
        }

        public List<string> GetUngroupedDialogueNames(bool startingDialoguesOnly)
        {
            List<string> ungroupedDialogueNames = new List<string>();

            foreach (DialogueSO ungroupedDialogue in ungroupedDialogues)
            {
                if (startingDialoguesOnly && !ungroupedDialogue.isStartingDialogue)
                {
                    continue;
                }

                ungroupedDialogueNames.Add(ungroupedDialogue.dialogueName);
            }

            return ungroupedDialogueNames;
        }
    }
}
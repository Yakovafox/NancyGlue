using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dialogue.Inspectors
{
    using ScriptableObjects;
    using Utilities;

    [CustomEditor(typeof(DialogueSystem))]
    public class DialogueInspector : Editor
    {
        // Scriptable Objects
        private SerializedProperty dialogueContainerProperty;
        private SerializedProperty dialogueGroupProperty;
        private SerializedProperty dialogueProperty;

        //Filters
        private SerializedProperty groupedDialoguesProperty;
        private SerializedProperty startingDialoguesOnlyProperty;

        // Indexes
        private SerializedProperty selectedDialogueGroupIndexProperty;
        private SerializedProperty selectedDialogueIndexProperty;

        // Interactables
        private SerializedProperty nancyPortraitUIProperty;
        private SerializedProperty portraitUIProperty;
        private SerializedProperty nameUIProperty;
        private SerializedProperty bodyTextUIProperty;
        private SerializedProperty gridUIProperty;

        private void OnEnable()
        {
            dialogueContainerProperty = serializedObject.FindProperty("dialogueContainer");
            dialogueGroupProperty = serializedObject.FindProperty("dialogueGroup");
            dialogueProperty = serializedObject.FindProperty("dialogue");

            groupedDialoguesProperty = serializedObject.FindProperty("groupedDialogues");
            startingDialoguesOnlyProperty = serializedObject.FindProperty("startingDialogueOnly");

            selectedDialogueGroupIndexProperty = serializedObject.FindProperty("selectedDialogueGroupIndex");
            selectedDialogueIndexProperty = serializedObject.FindProperty("selectedDialogueIndex");

            portraitUIProperty = serializedObject.FindProperty("characterPortrait");
            nancyPortraitUIProperty = serializedObject.FindProperty("nancyPortrait");
            nameUIProperty = serializedObject.FindProperty("characterNameUI");
            bodyTextUIProperty = serializedObject.FindProperty("bodyTextUI");
            gridUIProperty = serializedObject.FindProperty("optionGridUI");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDialogueContainerArea();

            DialogueContainerSO dialogueContainer = (DialogueContainerSO) dialogueContainerProperty.objectReferenceValue;

            if (dialogueContainer == null)
            {
                StopDrawing("Select a dialogue container to see the rest of the inspector");

                return;
            }

            DrawFiltersArea();

            bool currentStartingDialogueOnlyFilter = startingDialoguesOnlyProperty.boolValue;

            List<string> dialogueNames;

            string dialogueFolderPath = $"Assets/Resources/Dialogues/{dialogueContainer.fileName}";

            string dialogueInfoMessage;

            if (groupedDialoguesProperty.boolValue)
            {
                List<string> dialogueGroupNames = dialogueContainer.GetDialogueGroupNames();

                if (dialogueGroupNames.Count == 0)
                {
                    StopDrawing("There are no dialogue groups in this container.");

                    return;
                }

                DrawDialogueGroupArea(dialogueContainer, dialogueGroupNames);

                DialogueGroupSO dialogueGroup = (DialogueGroupSO) dialogueGroupProperty.objectReferenceValue;

                dialogueNames = dialogueContainer.GetGroupedDialogueNames(dialogueGroup, currentStartingDialogueOnlyFilter);

                dialogueFolderPath += $"/Groups/{dialogueGroup.groupName}/Dialogues";

                dialogueInfoMessage = "There are no" + (currentStartingDialogueOnlyFilter ? " starting" : "") + "dialogues in this group.";
            }
            else
            {
                dialogueNames = dialogueContainer.GetUngroupedDialogueNames(currentStartingDialogueOnlyFilter);

                dialogueFolderPath += "/Global/Dialogues";

                dialogueInfoMessage = "There are no" + (currentStartingDialogueOnlyFilter ? " starting" : "") + " ungrouped dialogues in this container.";
            }

            if (dialogueNames.Count == 0)
            {
                StopDrawing(dialogueInfoMessage);

                return;
            }

            DrawDialogueArea(dialogueNames, dialogueFolderPath);

            DrawUIElements();

            serializedObject.ApplyModifiedProperties(); 
        }

        #region Draw
        private void DrawDialogueContainerArea()
        {
            InspectorUtility.DrawHeader("Dialogue Container");

            dialogueContainerProperty.DrawPropertyField();

            InspectorUtility.DrawSpace(4);
        }

        private void DrawFiltersArea()
        {
            InspectorUtility.DrawHeader("Filters");

            groupedDialoguesProperty.DrawPropertyField();
            startingDialoguesOnlyProperty.DrawPropertyField();

            InspectorUtility.DrawSpace(4);
        }

        private void DrawDialogueGroupArea(DialogueContainerSO dialogueContainer, List<string> dialogueGroupNames)
        {
            InspectorUtility.DrawHeader("Dialogue Group");

            int oldSelectedDialogueGroupIndex = selectedDialogueGroupIndexProperty.intValue;

            DialogueGroupSO oldDialogueGroup = (DialogueGroupSO)dialogueGroupProperty.objectReferenceValue;

            bool isOldDialogueGroupNull = oldDialogueGroup == null;

            string oldDialogueGroupName = isOldDialogueGroupNull ? "" : oldDialogueGroup.groupName;

            UpdateIndexOnNameListUpdate(dialogueGroupNames, selectedDialogueGroupIndexProperty, oldSelectedDialogueGroupIndex, oldDialogueGroupName, isOldDialogueGroupNull);

            selectedDialogueGroupIndexProperty.intValue = InspectorUtility.DrawPopup("Dialogue Group", selectedDialogueGroupIndexProperty, dialogueGroupNames.ToArray());

            string selectedDialogueGroupName = dialogueGroupNames[selectedDialogueGroupIndexProperty.intValue];

            DialogueGroupSO selectedGroup = IOUtility.LoadAsset<DialogueGroupSO>($"Assets/Resources/Dialogues/{dialogueContainer.fileName}/Groups/{selectedDialogueGroupName}", selectedDialogueGroupName);

            dialogueGroupProperty.objectReferenceValue = selectedGroup;

            dialogueGroupProperty.DrawPropertyField();

            InspectorUtility.DrawDisabledFields(() => dialogueGroupProperty.DrawPropertyField());

            InspectorUtility.DrawSpace(4);
        }

        private void DrawDialogueArea(List<string> dialogueNames, string dialogueFolderPath)
        {
            InspectorUtility.DrawHeader("Dialogue");

            int oldSelectedDialogueIndex = selectedDialogueIndexProperty.intValue;

            DialogueSO oldDialogue = (DialogueSO)dialogueProperty.objectReferenceValue;

            bool isOldDialogueNull = oldDialogue == null;

            string oldDialogueName = isOldDialogueNull ? "" : oldDialogue.name;

            UpdateIndexOnNameListUpdate(dialogueNames, selectedDialogueIndexProperty, oldSelectedDialogueIndex, oldDialogueName, isOldDialogueNull);

            selectedDialogueIndexProperty.intValue = InspectorUtility.DrawPopup("Dialogue", selectedDialogueIndexProperty, dialogueNames.ToArray());

            string selectedDialogueName = dialogueNames[selectedDialogueIndexProperty.intValue];

            DialogueSO selectedDialogue = IOUtility.LoadAsset<DialogueSO>(dialogueFolderPath, selectedDialogueName);

            dialogueProperty.objectReferenceValue = selectedDialogue;

            InspectorUtility.DrawDisabledFields(() => dialogueProperty.DrawPropertyField());

            dialogueProperty.DrawPropertyField();

            InspectorUtility.DrawSpace(4);
        }

        private void DrawUIElements()
        {
            InspectorUtility.DrawHeader("UI Elements");

            portraitUIProperty.DrawPropertyField();
            nancyPortraitUIProperty.DrawPropertyField();
            nameUIProperty.DrawPropertyField();
            bodyTextUIProperty.DrawPropertyField();
            gridUIProperty.DrawPropertyField();

        }

        private void StopDrawing(string message, MessageType messageType = MessageType.Info)
        {
            InspectorUtility.DrawHelpBox(message, messageType);

            InspectorUtility.DrawSpace();

            InspectorUtility.DrawHelpBox("You need to select a dialogue for this component to work properly at runtime!", MessageType.Warning);

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Index Methods
        private void UpdateIndexOnNameListUpdate(List<string> optionNames, SerializedProperty indexProperty, int oldSelectedDialogueGroupIndex, string oldPropertyName, bool isOldPropertyNull)
        {
            if (isOldPropertyNull)
            {
                indexProperty.intValue = 0;
            }
            else
            {
                if (oldSelectedDialogueGroupIndex > optionNames.Count - 1 || oldPropertyName != optionNames[oldSelectedDialogueGroupIndex])
                {
                    if (optionNames.Contains(oldPropertyName))
                    {
                        indexProperty.intValue = optionNames.IndexOf(oldPropertyName);
                    }
                    else
                    {
                        indexProperty.intValue = 0;
                    }
                }
            }
        }
        #endregion
    }
}
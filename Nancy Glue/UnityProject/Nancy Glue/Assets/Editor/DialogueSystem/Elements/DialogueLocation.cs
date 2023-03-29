using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UIElements;

namespace Dialogue.Elements
{
    using Enumerators;
    using Utilities;
    using Data.Save;
    using Windows;

    public class DialogueLocation : DialogueNode
    {
        public override void Init(string nodeName, DialogueGraphView dialogueGraphView, Vector2 pos)
        {
            base.Init(nodeName, dialogueGraphView, pos);

            type = DialogueType.Location;

            ChoiceSaveData optionData = new ChoiceSaveData()
            {
                Text = "Next Dialogue..."
            };

            extensionContainer.style.backgroundColor = new Color(237f / 255f, 197f / 255f, 38f / 255f);

            Options.Add(optionData);
        }

        public override void Draw()
        {
            // Title Container
            TextField dialogueNameTextField = DialogueElementUtility.CreateTextField(DialogueName, 0, null, callback =>
            {
                TextField target = (TextField)callback.target;

                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(target.value))
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        graphView.RepeatedNames++;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        graphView.RepeatedNames--;
                    }
                }

                if (group == null)
                {
                    graphView.RemoveUngroupedNode(this);

                    DialogueName = target.value;

                    graphView.AddUngroupedNode(this);

                    return;
                }

                DialogueGroup currentGroup = group;

                graphView.RemoveGroupedNode(this, group);

                DialogueName = target.value;

                graphView.AddGroupedNode(this, currentGroup);
            });

            dialogueNameTextField.AddToClassList("dialogue-node__textfield_hidden");
            dialogueNameTextField.AddToClassList("dialogue-node__textfield_hidden:textfield");
            dialogueNameTextField.AddToClassList("dialogue-node__textfield_hidden:hidden");

            titleContainer.Insert(0, dialogueNameTextField);

            VisualElement characterNameContainer = new VisualElement();

            characterNameContainer.AddToClassList("dialogue-node__custom-data-container");

            // Character Name Field
            TextField characterNameTextField = DialogueElementUtility.CreateTextField(CharacterName,0, null, callback =>
            {
                CharacterName = callback.newValue;
            });

            characterNameTextField.AddToClassList("dialogue-node__textfield");

            characterNameContainer.Insert(0, characterNameTextField);

            extensionContainer.Add(characterNameTextField);

            // Input Container
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));

            inputPort.portName = "Input Pin";

            inputContainer.Add(inputPort);

            // Extension Container
            VisualElement textDataContainer = new VisualElement();

            textDataContainer.AddToClassList("dialogue-node__custom-data-container");

            Foldout textFoldout = DialogueElementUtility.CreateFoldout("Location Text");

            TextField textInputField = DialogueElementUtility.CreateTextArea(Text, 0, null, callback =>
            {
                Text = callback.newValue;
            });

            textInputField.AddToClassList("dialogue-node__textfield");
            textInputField.AddToClassList("dialogue-node__quote-textfield");

            textFoldout.Add(textInputField);

            textDataContainer.Add(textFoldout);

            extensionContainer.Add(textDataContainer);



            // Output container
            foreach (ChoiceSaveData option in Options)
            {
                Port optionPort = CreateOption(option.Text);

                optionPort.userData = option;

                outputContainer.Add(optionPort);
            }

            RefreshExpandedState();
        }

        private Port CreateOption(string choice)
        {
            Port optionPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));

            optionPort.portName = "";

            Button deleteOption = DialogueElementUtility.CreateButton("X");

            deleteOption.AddToClassList("dialogue-node__button");

            TextField optionText = DialogueElementUtility.CreateTextField(choice);

            optionText.AddToClassList("dialogue-node__textfield_hidden");
            optionText.AddToClassList("dialogue-node__textfield_hidden:textfield");
            optionText.AddToClassList("dialogue-node__textfield_hidden:hidden");

            VisualElement textContainer = new VisualElement();

            textContainer.Add(optionText);
            optionPort.Add(textContainer);
            optionPort.Add(deleteOption);
            return optionPort;
        }
    }
}
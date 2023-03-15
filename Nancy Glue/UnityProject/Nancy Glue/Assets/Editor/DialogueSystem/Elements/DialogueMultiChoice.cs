using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogue.Elements
{
    using Enumerators;
    using Utilities;
    using Windows;
    using Data.Save;

    public class DialogueMultiChoice : DialogueNode
    {
        public override void Init(string nodeName, DialogueGraphView dialogueGraphView, Vector2 pos)
        {
            base.Init(nodeName, dialogueGraphView, pos);

            type = DialogueType.MultiChoice;

            ChoiceSaveData optionData = new ChoiceSaveData()
            {
                Text = "New Option"
            };

            //extensionContainer.style.backgroundColor = new Color(79f / 255f, 29f / 255f, 80f / 255f);

            Options.Add(optionData);
        }

        public override void Draw()
        {
            base.Draw();

            Button addOption = DialogueElementUtility.CreateButton("Add Option", () =>
            {
                ChoiceSaveData optionData = new ChoiceSaveData()
                {
                    Text = "New Option"
                };

                Options.Add(optionData);

                Port optionPort = CreateOption(optionData);

                outputContainer.Add(optionPort);
            }
            );

            addOption.AddToClassList("dialogue-node__button");

            mainContainer.Insert(1, addOption);

            // Output container
            foreach (ChoiceSaveData option in Options)
            {
                Port optionPort = CreateOption(option);

                outputContainer.Add(optionPort);
            }

            RefreshExpandedState();
        }

        private Port CreateOption(object userData)
        {
            Port optionPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));

            optionPort.portName = "";

            optionPort.userData = userData;

            ChoiceSaveData choiceData = (ChoiceSaveData) userData;

            Button deleteOption = DialogueElementUtility.CreateButton("X", () =>
            {
                if (Options.Count == 1)
                {
                    return;
                }

                if (optionPort.connected)
                {
                    graphView.DeleteElements(optionPort.connections);
                }

                Options.Remove(choiceData);
                graphView.RemoveElement(optionPort);
            });

            deleteOption.AddToClassList("dialogue-node__button");

            TextField optionText = DialogueElementUtility.CreateTextField(choiceData.Text, null, callback => 
            {
                choiceData.Text = callback.newValue;
            });

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

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue.Elements
{
    using Enumerators;
    using Utilities;
    using Windows;
    using Data.Save;
    using UnityEngine.UIElements;

    public class DialogueSingleChoice : DialogueNode
    {
        public override void Init(string nodeName, DialogueGraphView dialogueGraphView, Vector2 pos)
        {
            base.Init(nodeName, dialogueGraphView, pos);

            type = DialogueType.SingleChoice;

            ChoiceSaveData optionData = new ChoiceSaveData()
            {
                Text = "Next Dialogue..."
            };

            //extensionContainer.style.backgroundColor = new Color(29f / 255f, 79f / 255f, 30f / 255f);

            Options.Add(optionData);
        }

        public override void Draw()
        {
            base.Draw();

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

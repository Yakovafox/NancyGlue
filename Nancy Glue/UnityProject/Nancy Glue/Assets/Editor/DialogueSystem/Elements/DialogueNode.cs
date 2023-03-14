using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Dialogue.Elements
{
    using Enumerators;
    using Utilities;
    using Windows;
    using Data.Save;

    public class DialogueNode : Node
    {
        // Variables contained in a node
        public string ID { get; set; }
        public string DialogueName { get; set; }
        public string CharacterName { get; set; }
        public Sprite CharacterPortrait { get; set; }
        public List<ChoiceSaveData> Options { get; set; }
        public string SpritePath { get; set; }
        public string AudioPath { get; set; }
        public string Text { get; set; }
        public DialogueType type { get; set; }
        public DialogueGroup group { get; set; }

        protected DialogueGraphView graphView;

        protected Color defaultBackgroundColor;

        // Initialisation
        public virtual void Init(string name, DialogueGraphView dialogueGraphView, Vector2 pos)
        {
            ID = Guid.NewGuid().ToString();
            DialogueName = name;
            CharacterName = "Character Name";
            SpritePath = "UI/Sprites/missing_texture";
            AudioPath = "Sfx/Dialogue/speaking.mp3";
            CharacterPortrait = Resources.Load<Sprite>(SpritePath);
            Options = new List<ChoiceSaveData>();
            Text = "Dialogue Text.";

            graphView = dialogueGraphView;
            defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            SetPosition(new Rect(pos, Vector2.zero));

            mainContainer.AddToClassList("dialogue-node__main-container");
            extensionContainer.AddToClassList("dialogue-node__extension-container");
        }

        #region contextual menu
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectPorts(inputContainer));
            evt.menu.AppendAction("Disconnect Ouput Ports", actionEvent => DisconnectPorts(outputContainer));

            base.BuildContextualMenu(evt);
        }
        #endregion

        public virtual void Draw()
        {
            // Title Container
            TextField dialogueNameTextField = DialogueElementUtility.CreateTextField(DialogueName, null, callback =>
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
            TextField characterNameTextField = DialogueElementUtility.CreateTextField(CharacterName, null, callback => 
            {
                CharacterName = callback.newValue;
            });

            characterNameTextField.AddToClassList("dialogue-node__textfield");

            characterNameContainer.Insert(0, characterNameTextField);

            extensionContainer.Add(characterNameTextField);

            // Character Container
            VisualElement characterDataContainer = new VisualElement();

            characterDataContainer.AddToClassList("dialogue-node__custom-data-container");

            // Character Image Field
            Image characterPortrait = new Image
            {
                image = Resources.Load<Texture2D>(SpritePath)
                //image = (Texture2D)EditorGUIUtility.Load(SpritePath + ".png")
            };

            // Sprite Asset Path Field
            TextField spriteAssetPathTextField = DialogueElementUtility.CreateTextField(SpritePath, null, callback =>
            {
                SpritePath = callback.newValue;
                characterPortrait.image = Resources.Load<Texture2D>(callback.newValue);
                //characterPortrait.image = (Texture2D)EditorGUIUtility.Load(SpritePath + ".png");
                RefreshExpandedState();
            });
            
            // Audio Asset Path Field
            TextField audioAssetPathTextField = DialogueElementUtility.CreateTextField(AudioPath, null, callback =>
            {
                AudioPath = callback.newValue;
            });

            spriteAssetPathTextField.AddToClassList("dialogue-node__textfield");

            audioAssetPathTextField.AddToClassList("dialogue-node__textfield");

            characterPortrait.AddToClassList("dialogue-node__image");

            characterDataContainer.Insert(0, spriteAssetPathTextField);

            characterDataContainer.Insert(1, characterPortrait);

            characterDataContainer.Insert(2, audioAssetPathTextField);

            extensionContainer.Add(characterDataContainer);

            // Input Container
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));

            inputPort.portName = "Start Flag";

            inputContainer.Add(inputPort);

            // Extension Container
            VisualElement textDataContainer = new VisualElement();

            textDataContainer.AddToClassList("dialogue-node__custom-data-container");

            Foldout textFoldout = DialogueElementUtility.CreateFoldout("Dialogue Text");

            TextField textInputField = DialogueElementUtility.CreateTextArea(Text, null, callback => 
            {
                Text = callback.newValue;
            });

            textInputField.AddToClassList("dialogue-node__textfield");
            textInputField.AddToClassList("dialogue-node__quote-textfield");

            textFoldout.Add(textInputField);

            textDataContainer.Add(textFoldout);

            extensionContainer.Add(textDataContainer);
        }

        #region Utility Methods
        public void DisconnectAllPorts()
        {
            DisconnectPorts(inputContainer);
            DisconnectPorts(outputContainer);
        }

        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (!port.connected)
                {
                    continue;
                }

                graphView.DeleteElements(port.connections);
            }
        }

        public bool IsStartingNode()
        {
            Port inputPort = (Port) inputContainer.Children().First();

            return !inputPort.connected;
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defaultBackgroundColor;
        }
        #endregion
    }
}

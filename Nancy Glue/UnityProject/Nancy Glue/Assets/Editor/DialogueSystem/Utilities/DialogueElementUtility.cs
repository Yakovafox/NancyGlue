using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogue.Utilities
{
    public static class DialogueElementUtility
    {
        public static Button CreateButton(string text, Action onClick = null)
        {
            Button button = new Button(onClick)
            {
                text = text
            };

            return button;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout foldout = new Foldout()
            {
                text = title,
                value = !collapsed
            };

            return foldout;
        }

        public static TextField CreateTextField(string value = null, int limit = 0, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = new TextField(limit, true, false, '\x0000')
            {
                value = value,
                label = label
            };

            if (onValueChanged != null)
            {
                textField.RegisterValueChangedCallback(onValueChanged);
            }

            return textField;
        }

        public static TextField CreateTextArea(string value = null, int limit = 0, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textArea = CreateTextField(value, limit, label, onValueChanged);

            textArea.multiline = true;

            return textArea; 
        }
    }
}

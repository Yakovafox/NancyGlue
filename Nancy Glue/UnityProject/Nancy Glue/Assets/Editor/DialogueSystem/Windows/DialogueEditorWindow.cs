using System;
using UnityEditor;
using System.IO;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Dialogue.Windows
{
    using Utilities;

    public class DialogueEditorWindow : EditorWindow
    {
        private DialogueGraphView graphView;
        private readonly string defaultFileName = "DialogueFileName";
        private static TextField fileName;
        private Button saveButton;

        // Add to the menu ribon
        [MenuItem("Window/Dialogue/Dialogue Graph")]

        // Title the window
        public static void ShowExample()
        {
            GetWindow<DialogueEditorWindow>("Dialogue Graph Interface");
        }

        // Once opened
        private void OnEnable()
        {
            // Add the graph
            AddGraphView();
            AddToolbar();

            // Load the style sheet
            AddStyles();
        }

        #region toolbar
        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            fileName = DialogueElementUtility.CreateTextField(defaultFileName, "File Name:", callback => 
            {
                fileName.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            });

            saveButton = DialogueElementUtility.CreateButton("Save", () => Save());

            Button newGraphButton = DialogueElementUtility.CreateButton("New", () => NewGraph());
            Button loadButton = DialogueElementUtility.CreateButton("Load", () => Load());
            Button clearButton = DialogueElementUtility.CreateButton("Clear", () => Clear());
            Button resetButton = DialogueElementUtility.CreateButton("Reset", () => ResetGraph());

            toolbar.Add(fileName);
            toolbar.Add(saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);
            toolbar.Add(resetButton);
            toolbar.Add(newGraphButton);

            rootVisualElement.Add(toolbar);
        }

        private void NewGraph()
        {
            Clear();
            fileName.value = defaultFileName;
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(fileName.value))
            {
                EditorUtility.DisplayDialog(
                    "Invalid file name",
                    "Please ensure you have entered a valid file name.",
                    "OK"
                );

                return;
            }

            IOUtility.Init(graphView, fileName.value);
            IOUtility.Save();
        }

        private void Load()
        {
            string filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/Editor/DialogueSystem/Graphs", "asset");

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            Clear();

            IOUtility.Init(graphView, Path.GetFileNameWithoutExtension(filePath));
            IOUtility.Load();
        }

        private void Clear()
        {
            graphView.ClearGraph();
        }

        private void ResetGraph()
        {
            Clear();
            UpdateFileName(defaultFileName);
        }
        #endregion

        #region Utilitites
        // Load the graph appearance
        private void AddGraphView()
        {
            graphView = new DialogueGraphView(this);

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }

        // Load the stlye sheet variables
        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/DialogueVariables.uss");

            rootVisualElement.styleSheets.Add(styleSheet);
        }

        public void EnableSave()
        {
            saveButton.SetEnabled(true);
        }

        public void DisableSave()
        {
            saveButton.SetEnabled(false);
        }

        public static void UpdateFileName(string newName)
        {
            fileName.value = newName;
        }
        #endregion
    }
}

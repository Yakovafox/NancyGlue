using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Dialogue.Utilities
{
    using ScriptableObjects;
    using Data.Save;
    using Elements;
    using Windows;
    using Dialogue.Data;

    public static class IOUtility
    {
        private static DialogueGraphView graphView;

        private static string graphFileName;
        private static string containerFolderPath;

        private static List<DialogueGroup> groups;
        private static List<DialogueNode> nodes;

        private static Dictionary<string, DialogueGroupSO> createdDialogueGroups;
        private static Dictionary<string, DialogueSO> createdDialogues;

        private static Dictionary<string, DialogueGroup> loadedGroups;
        private static Dictionary<string, DialogueNode> loadedNodes;

        public static void Init(DialogueGraphView dialogueGraphView, string graphName)
        {
            graphView = dialogueGraphView;

            graphFileName = graphName;
            containerFolderPath = $"Assets/Resources/Dialogues/{graphFileName}";

            groups = new List<DialogueGroup>();
            nodes = new List<DialogueNode>();

            createdDialogueGroups = new Dictionary<string, DialogueGroupSO>();
            createdDialogues = new Dictionary<string, DialogueSO>();

            loadedGroups = new Dictionary<string, DialogueGroup>();
            loadedNodes = new Dictionary<string, DialogueNode>();
        }

        #region Save
        public static void Save()
        {
            CreateStaticFolders();

            GetElementsFromGraphView();

            GraphSaveDataSO graphData = CreateAsset<GraphSaveDataSO>("Assets/Editor/DialogueSystem/Graphs", $"{graphFileName}Graph");

            graphData.Init(graphFileName);

            DialogueContainerSO dialogueContainer = CreateAsset<DialogueContainerSO>(containerFolderPath, graphFileName);

            dialogueContainer.Init(graphFileName);

            SaveGroups(graphData, dialogueContainer);
            SaveNodes(graphData, dialogueContainer);

            SaveAsset(graphData);
            SaveAsset(dialogueContainer);
        }

        private static void SaveNodes(GraphSaveDataSO graphData, DialogueContainerSO dialogueContainer)
        {
            SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
            List<string> ungroupedNodeNames = new List<string>();

            foreach (DialogueNode node in nodes)
            {
                SaveNodeToGraph(node, graphData);
                SaveNodeToScriptableObject(node, dialogueContainer);

                if (node.group != null)
                {
                    groupedNodeNames.AddItem(node.group.title, node.DialogueName);

                    continue;
                }

                ungroupedNodeNames.Add(node.DialogueName);
            }

            UpdateDialogueChoiceConections();

            UpdateOldGroupedNodes(groupedNodeNames, graphData);
            UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
        }

        private static void SaveNodeToScriptableObject(DialogueNode node, DialogueContainerSO dialogueContainer)
        {
            DialogueSO dialogue;

            if (node.group != null)
            {
                dialogue = CreateAsset<DialogueSO>($"{containerFolderPath}/Groups/{node.group.title}/Dialogues", node.DialogueName);

                dialogueContainer.dialogueGroups.AddItem(createdDialogueGroups[node.group.ID], dialogue);
            }
            else
            {
                dialogue = CreateAsset<DialogueSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);

                dialogueContainer.ungroupedDialogues.Add(dialogue);
            }

            dialogue.Init(node.DialogueName, node.Text, ConvertNodeChoiceToDialogueChoices(node.Options), node.type, node.IsStartingNode(), node.CharacterName, node.SpritePath, node.AudioPath);

            createdDialogues.Add(node.ID, dialogue);

            SaveAsset(dialogue);
        }

        private static List<DialogueChoiceData> ConvertNodeChoiceToDialogueChoices(List<ChoiceSaveData> nodeChoices)
        {
            List<DialogueChoiceData> dialogueChoices = new List<DialogueChoiceData>();

            foreach (ChoiceSaveData nodeChoice in nodeChoices)
            {
                DialogueChoiceData choiceData = new DialogueChoiceData()
                {
                    text = nodeChoice.Text
                };

                dialogueChoices.Add(choiceData);
            }

            return dialogueChoices;
        }

        private static void SaveNodeToGraph(DialogueNode node, GraphSaveDataSO graphData)
        {
            List<ChoiceSaveData> choices = CloneNodeOptions(node.Options);

            NodeSaveData nodeData = new NodeSaveData()
            {
                ID = node.ID,
                dialogueName = node.DialogueName,
                characterName = node.CharacterName,
                options = choices,
                text = node.Text,
                groupID = node.group?.ID,
                type = node.type,
                position = node.GetPosition().position,
                spritePath = node.SpritePath,
                audioPath = node.AudioPath
            };

            graphData.nodes.Add(nodeData);
        }

        private static void SaveGroups(GraphSaveDataSO graphData, DialogueContainerSO dialogueContainer)
        {
            List<string> groupNames = new List<string>();

            foreach (DialogueGroup group in groups)
            {
                SaveGroupToGraph(group, graphData);
                SaveGroupToScriptableObject(group, dialogueContainer);

                groupNames.Add(group.title);
            }

            UpdateOldGroups(groupNames, graphData);
        }

        private static void SaveGroupToScriptableObject(DialogueGroup group, DialogueContainerSO dialogueContainer)
        {
            string groupName = group.title;

            CreateFolder($"{containerFolderPath}/Groups", groupName);
            CreateFolder($"{containerFolderPath}/Groups/{groupName}", "Dialogues");

            DialogueGroupSO dialogueGroup = CreateAsset<DialogueGroupSO>($"{containerFolderPath}/Groups/{groupName}", groupName);

            dialogueGroup.Init(groupName);

            createdDialogueGroups.Add(group.ID, dialogueGroup);

            dialogueContainer.dialogueGroups.Add(dialogueGroup, new List<DialogueSO>());

            SaveAsset(dialogueGroup);
        }

        private static void SaveGroupToGraph(DialogueGroup group, GraphSaveDataSO graphData)
        {
            GroupSaveData groupData = new GroupSaveData()
            {
                ID = group.ID,
                name = group.title,
                position = group.GetPosition().position
            };

            graphData.groups.Add(groupData);
        }

        public static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        #endregion

        #region Load
        public static void Load()
        {
            GraphSaveDataSO graphData = LoadAsset<GraphSaveDataSO>("Assets/Editor/DialogueSystem/Graphs", graphFileName);

            if (graphData == null)
            {
                EditorUtility.DisplayDialog(
                    "Couldn't load the file!",
                    "Could not find file at path:\n\n" + $"Assets/Editor/DialogueSystem/Graphs/{graphFileName}\n\n" + "Make sure you have chosen the correct file",
                    "OK"
                );

                return;
            }

            DialogueEditorWindow.UpdateFileName(graphData.fileName);

            LoadGroups(graphData.groups);
            LoadNodes(graphData.nodes);
            LoadConnections();
        }

        private static void LoadNodes(List<NodeSaveData> nodes)
        {
            foreach (NodeSaveData nodeData in nodes)
            {
                List<ChoiceSaveData> options = CloneNodeOptions(nodeData.options);

                DialogueNode node = graphView.CreateNode(nodeData.dialogueName, nodeData.type, nodeData.position, false, false);

                node.ID = nodeData.ID;
                node.type = nodeData.type;
                node.Options = options;
                node.CharacterName = nodeData.characterName;
                node.Text = nodeData.text;
                node.SpritePath = nodeData.spritePath;
                node.AudioPath = nodeData.audioPath;

                node.Draw();

                graphView.AddElement(node);

                loadedNodes.Add(node.ID, node);

                if (string.IsNullOrEmpty(nodeData.groupID))
                {
                    continue;
                }

                DialogueGroup group = loadedGroups[nodeData.groupID];

                node.group = group;

                group.AddElement(node);
            }
        }

        private static void LoadConnections()
        {
            foreach(KeyValuePair<string, DialogueNode> loadedNode in loadedNodes)
            {
                foreach (Port optionPort in loadedNode.Value.outputContainer.Children())
                {
                    ChoiceSaveData optionData = (ChoiceSaveData)optionPort.userData;

                    if (string.IsNullOrEmpty(optionData.NodeID))
                    {
                        continue;
                    }

                    DialogueNode nextNode = loadedNodes[optionData.NodeID];

                    Port nextNodeInputPort = (Port) nextNode.inputContainer.Children().First();

                    Edge edge = optionPort.ConnectTo(nextNodeInputPort);

                    graphView.AddElement(edge);

                    loadedNode.Value.RefreshPorts();
                }
            }
        }

        private static void LoadGroups(List<GroupSaveData> groups)
        {
            foreach (GroupSaveData groupData in groups)
            {
                DialogueGroup group = graphView.CreateGroup(groupData.name, groupData.position);

                group.ID = groupData.ID;

                loadedGroups.Add(group.ID, group);
            }
        }
        #endregion

        #region Create
        private static void CreateStaticFolders()
        {
            CreateFolder("Assets/Editor/DialogueSystem", "Graphs");

            CreateFolder("Assets", "DialogueSystem");
            CreateFolder("Assets/Resources", "Dialogues");

            CreateFolder("Assets/Resources/Dialogues", graphFileName);
            CreateFolder(containerFolderPath, "Global");
            CreateFolder(containerFolderPath, "Groups");
            CreateFolder($"{containerFolderPath}/Global", "Dialogues");
        }

        public static void CreateFolder(string path, string folderName)
        {
            if (AssetDatabase.IsValidFolder($"{path}/{folderName}"))
            {
                return;
            }

            AssetDatabase.CreateFolder(path, folderName);
        }

        public static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            T asset = LoadAsset<T>(path, assetName);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();

                AssetDatabase.CreateAsset(asset, fullPath);

            }

            return asset;
        }

        public static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }
        #endregion

        #region Fetch Methods
        private static void GetElementsFromGraphView()
        {
            Type groupType = typeof(DialogueGroup);

            graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is DialogueNode node)
                {
                    nodes.Add(node);

                    return;
                }

                if (graphElement.GetType() == groupType)
                {
                    DialogueGroup group = (DialogueGroup) graphElement;

                    groups.Add(group);

                    return;
                }
            });
        }
        #endregion

        #region Utilities
        public static void UpdateDialogueChoiceConections()
        {
            foreach (DialogueNode node in nodes)
            {
                DialogueSO dialogue = createdDialogues[node.ID];

                for (int index = 0; index < node.Options.Count; index++)
                {
                    ChoiceSaveData nodeChoice = node.Options[index];

                    if (string.IsNullOrEmpty(nodeChoice.NodeID))
                    {
                        continue;
                    }

                    dialogue.dialogueChoices[index].NextDialogue = createdDialogues[nodeChoice.NodeID];

                    SaveAsset(dialogue);
                }
            }
        }

        public static void UpdateOldGroups(List<string> currentGroupNames, GraphSaveDataSO graphData)
        {
            if (graphData.oldGroupNames != null && graphData.oldGroupNames.Count != 0)
            {
                List<string> groupsToRemove = graphData.oldGroupNames.Except(currentGroupNames).ToList();

                foreach (string groupToRemove in groupsToRemove)
                {
                    RemoveFolder($"{containerFolderPath}/Groups/{groupToRemove}");
                }
            }

            graphData.oldGroupNames = new List<string>(currentGroupNames);
        }

        public static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, GraphSaveDataSO graphData)
        {
            if (graphData.oldGroupedNames != null && graphData.oldGroupedNames.Count != 0)
            {
                foreach (KeyValuePair<string, List<string>> oldGroupedNode in graphData.oldGroupedNames)
                {
                    List<string> nodesToRemove = new List<string>();

                    if (currentGroupedNodeNames.ContainsKey(oldGroupedNode.Key))
                    {
                        nodesToRemove = oldGroupedNode.Value.Except(currentGroupedNodeNames[oldGroupedNode.Key]).ToList();
                    }

                    foreach (string nodeToRemove in nodesToRemove)
                    {
                        RemoveAsset($"{containerFolderPath}/Group/{oldGroupedNode.Key}/Dialogues", nodeToRemove);
                    }
                }
            }

            graphData.oldGroupedNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
        }

        public static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, GraphSaveDataSO graphData)
        {
            if (graphData.oldUngroupedNames != null && graphData.oldUngroupedNames.Count != 0)
            {
                List<string> nodesToRemove = graphData.oldUngroupedNames.Except(currentUngroupedNodeNames).ToList();

                foreach (string nodeToRmove in nodesToRemove)
                {
                    RemoveAsset($"{containerFolderPath}/Global/Dialogues", nodeToRmove);
                }
            }

            graphData.oldUngroupedNames = new List<string>(currentUngroupedNodeNames);
        }

        private static List<ChoiceSaveData> CloneNodeOptions(List<ChoiceSaveData> nodeOptions)
        {
            List<ChoiceSaveData> choices = new List<ChoiceSaveData>();

            foreach (ChoiceSaveData choice in nodeOptions)
            {
                ChoiceSaveData choiceData = new ChoiceSaveData()
                {
                    Text = choice.Text,
                    NodeID = choice.NodeID
                };

                choices.Add(choiceData);
            }

            return choices;
        }
        #endregion

        #region Removal
        public static void RemoveFolder(string fullPath)
        {
            FileUtil.DeleteFileOrDirectory($"{fullPath}.meta");
            FileUtil.DeleteFileOrDirectory($"{fullPath}/");
        }

        public static void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
        }

        #endregion
    }
}
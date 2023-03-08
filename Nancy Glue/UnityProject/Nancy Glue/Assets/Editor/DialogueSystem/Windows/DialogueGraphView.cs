using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using System;

// View for inside the dialogue graph window
namespace Dialogue.Windows
{
    using Data.Error;
    using Elements;
    using Enumerators;
    using Data.Save;

    public class DialogueGraphView : GraphView
    {
        private DialogueEditorWindow editorWindow;

        private SerializableDictionary<string, NodeErrorData> ungroupedNodes;
        private SerializableDictionary<string, GroupErrorData> groups;
        private SerializableDictionary<Group, SerializableDictionary<string, NodeErrorData>> groupedNodes;

        private int repeatedNames = 0;

        public int RepeatedNames {
            get 
            {
                return repeatedNames; 
            } 

            set 
            {
                repeatedNames = value; 

                if (repeatedNames == 0)
                {
                    editorWindow.EnableSave();
                }

                if (repeatedNames == 1)
                {
                    editorWindow.DisableSave();
                }
            } 
        }

        // Constructor
        public DialogueGraphView(DialogueEditorWindow dialogueEditorWindow)
        {
            editorWindow = dialogueEditorWindow;

            ungroupedNodes = new SerializableDictionary<string, NodeErrorData>();
            groups = new SerializableDictionary<string, GroupErrorData>();
            groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, NodeErrorData>>();

            // Create a background that can be panned and zoomed
            AddManipulators();
            AddBackground();

            OnElementsDeleted();
            OnGroupElementsAdd();
            OnGroupElementRemoved();
            onGroupRenamed();
            OnGraphViewChanged();

            // Add the styling for the grid background
            AddStyles();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port)
                {
                    return;
                }

                if (startPort.node == port.node)
                {
                    return;
                }

                if (startPort.direction == port.direction)
                {
                    return;
                }

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        // Add interactability
        private void AddManipulators()
        {
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DialogueType.MultiChoice));

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateGroupContextualMenu());
        }

        // Add the grid background
        private void AddBackground()
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }

        // Create elements
        #region create elements
        public DialogueGroup CreateGroup(string title, Vector2 localMousePosition)
        {
            Vector2 viewPos = new Vector2(viewTransform.position.x, viewTransform.position.y);
            DialogueGroup group = new DialogueGroup(title, (localMousePosition - viewPos) / viewTransform.scale);

            AddGroup(group);

            AddElement(group);

            foreach (GraphElement selectedElement in selection)
            {
                if (!(selectedElement is DialogueNode))
                {
                    continue;
                }

                DialogueNode node = (DialogueNode)selectedElement;

                group.AddElement(node);
            }

            return group;
        }

        private void AddGroup(DialogueGroup group)
        {
            string groupName = group.title.ToLower();

            if (!groups.ContainsKey(groupName))
            {
                GroupErrorData groupErrorData = new GroupErrorData();

                groupErrorData.groups.Add(group);

                groups.Add(groupName, groupErrorData);

                return;
            }

            List<DialogueGroup> groupsList = groups[groupName].groups;

            groupsList.Add(group);

            Color errorColor = groups[groupName].errorData.color;

            group.SetErrorStyle(errorColor);

            if (groupsList.Count == 2)
            {
                groupsList[0].SetErrorStyle(errorColor);
            }
        }

        // Create a new node
        public DialogueNode CreateNode(string nodeName, DialogueType type, Vector2 pos, bool shouldDraw = true)
        {
            Type nodeType = Type.GetType($"Dialogue.Elements.Dialogue{type}");
            Type defaultNode = typeof(DialogueNode);

            DialogueNode node = (DialogueNode) Activator.CreateInstance(nodeType);

            Vector2 viewPos = new Vector2(viewTransform.position.x, viewTransform.position.y);
            node.Init(nodeName, this, (pos - viewPos)/viewTransform.scale);

            if (shouldDraw)
            {
                node.Draw();
            }

            AddElement(node);

            AddUngroupedNode(node);

            return node;
        }
        #endregion

        // Callback events
        #region callback events
        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, AskUser) =>
            {
                Type groupType = typeof(DialogueGroup);
                Type edgeType = typeof(Edge);

                List<DialogueGroup> groupsToDelete = new List<DialogueGroup>();
                List<Edge> edgesToDelete = new List<Edge>();
                List<DialogueNode> nodesToDelete = new List<DialogueNode>();

                foreach (GraphElement element in selection)
                {
                    if (element is DialogueNode node)
                    {
                        nodesToDelete.Add(node);

                        continue;
                    }

                    if (element.GetType() == edgeType){
                        Edge edge = (Edge)element;

                        edgesToDelete.Add(edge);
                    }

                    if (element.GetType() != groupType)
                    {
                        continue;
                    }

                    DialogueGroup group = (DialogueGroup) element;

                    RemoveGroup(group);

                    groupsToDelete.Add(group);
                }

                DeleteElements(edgesToDelete);

                foreach (DialogueGroup group in groupsToDelete)
                {
                    List<DialogueNode> groupNodes = new List<DialogueNode>();

                    foreach (GraphElement groupElement in group.containedElements)
                    {
                        if (!(groupElement is DialogueNode))
                        {
                            continue;
                        }

                        DialogueNode groupNode = (DialogueNode) groupElement;

                        groupNodes.Add(groupNode);
                    }

                    group.RemoveElements(groupNodes);

                    RemoveElement(group);
                }

                foreach (DialogueNode node in nodesToDelete)
                {
                    if (node.group != null)
                    {
                        node.group.RemoveElement(node);
                    }

                    node.DisconnectAllPorts();

                    RemoveUngroupedNode(node);

                    RemoveElement(node);
                }
            };
        }

        private void OnGroupElementsAdd()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is DialogueNode))
                    {
                        continue;
                    }

                    DialogueGroup nodeGroup = (DialogueGroup)group;
                    DialogueNode node = (DialogueNode)element;

                    RemoveUngroupedNode(node);
                    AddGroupedNode(node, nodeGroup);
                }
            };
        }

        private void OnGroupElementRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is DialogueNode))
                    {
                        continue;
                    }

                    DialogueGroup nodeGroup = (DialogueGroup)group;
                    DialogueNode node = (DialogueNode)element;

                    RemoveGroupedNode(node, nodeGroup);
                    AddUngroupedNode(node);
                }
            };
        }

        private void onGroupRenamed()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                DialogueGroup dialogueGroup = (DialogueGroup)group;

                dialogueGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(dialogueGroup.title))
                {
                    if (!string.IsNullOrEmpty(dialogueGroup.oldTitle))
                    {
                        RepeatedNames++;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(dialogueGroup.oldTitle))
                    {
                        RepeatedNames--;
                    }
                }

                RemoveGroup(dialogueGroup);

                dialogueGroup.oldTitle = dialogueGroup.title;

                AddGroup(dialogueGroup);
            };
        }

        private void OnGraphViewChanged()
        {
            graphViewChanged = (changes) => 
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        DialogueNode nextNode = (DialogueNode) edge.input.node;

                        ChoiceSaveData choiceData = (ChoiceSaveData) edge.output.userData;

                        choiceData.NodeID = nextNode.ID;
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    Type edgeType = typeof(Edge);

                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        if (element.GetType() != edgeType) 
                        {
                            continue;
                        }

                        Edge edge = (Edge) element;

                        ChoiceSaveData choiceData = (ChoiceSaveData) edge.output.userData;

                        choiceData.NodeID = "";
                    }
                }

                return changes;
            };
        }
        #endregion

        // node management
        #region node management
        private void RemoveGroup(DialogueGroup group)
        {
            string oldGroupName = group.oldTitle.ToLower();

            List<DialogueGroup> groupsList = groups[oldGroupName].groups;

            groupsList.Remove(group);

            group.ResetStyle();

            if (groupsList.Count == 1)
            {
                --RepeatedNames;

                groupsList[0].ResetStyle();

                return;
            }

            if (groupsList.Count == 0)
            {
                groups.Remove(oldGroupName);
            }
        }

        public void AddGroupedNode(DialogueNode node, DialogueGroup group)
        {
            string nodeName = node.DialogueName.ToLower();

            node.group = group;

            if (!groupedNodes.ContainsKey(group))
            {
                groupedNodes.Add(group, new SerializableDictionary<string, NodeErrorData>());
            }

            if (!groupedNodes[group].ContainsKey(nodeName))
            {
                NodeErrorData nodeErrorData = new NodeErrorData();

                nodeErrorData.Nodes.Add(node);
                groupedNodes[group].Add(nodeName, nodeErrorData);

                return;
            }

            List<DialogueNode> groupedNodesList = groupedNodes[group][nodeName].Nodes;

            groupedNodesList.Add(node);

            Color errorColor = groupedNodes[group][nodeName].ErrorData.color;

            node.SetErrorStyle(errorColor);

            if (groupedNodesList.Count == 2)
            {
                ++RepeatedNames;

                groupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveGroupedNode(DialogueNode node, DialogueGroup group)
        {
            string nodeName = node.DialogueName.ToLower();

            node.group = null;

            List<DialogueNode> groupedNodeList = groupedNodes[group][nodeName].Nodes;

            groupedNodeList.Remove(node);

            node.ResetStyle();

            if (groupedNodeList.Count == 1)
            {
                --RepeatedNames;

                groupedNodeList[0].ResetStyle();

                return;
            }

            if (groupedNodeList.Count == 0)
            {
                groupedNodes[group].Remove(nodeName);

                if (groupedNodes[group].Count == 0)
                {
                    groupedNodes.Remove(group);
                }
            }
        }

        public void AddUngroupedNode(DialogueNode node)
        {
            string nodeName = node.DialogueName.ToLower();

            if (!ungroupedNodes.ContainsKey(nodeName))
            {
                NodeErrorData nodeErrorData = new NodeErrorData();

                nodeErrorData.Nodes.Add(node);

                ungroupedNodes.Add(nodeName, nodeErrorData);

                return;
            }

            List<DialogueNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Add(node);

            Color errorColor = ungroupedNodes[nodeName].ErrorData.color;

            node.SetErrorStyle(errorColor);

            if (ungroupedNodes[nodeName].Nodes.Count == 2)
            {
                ++RepeatedNames;
                ungroupedNodes[nodeName].Nodes[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveUngroupedNode(DialogueNode node)
        {
            string nodeName = node.DialogueName.ToLower();

            List<DialogueNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Remove(node);

            node.ResetStyle();

            if (ungroupedNodes[nodeName].Nodes.Count == 1)
            {
                --RepeatedNames;

                ungroupedNodes[nodeName].Nodes[0].ResetStyle();

                return;
            }

            else if(ungroupedNodes[nodeName].Nodes.Count == 0)
            {
                ungroupedNodes.Remove(nodeName);
            }
        }
        #endregion

        #region Utilities
        // Load the style sheet
        private void AddStyles()
        {
            StyleSheet graphStyleSheet = (StyleSheet) EditorGUIUtility.Load("DialogueSystem/DialogueGraphViewStyle.uss");
            StyleSheet nodeStyleSheet = (StyleSheet) EditorGUIUtility.Load("DialogueSystem/NodeStyle.uss");

            styleSheets.Add(graphStyleSheet);
            styleSheets.Add(nodeStyleSheet);
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent =>CreateGroup("DialogueGroup", actionEvent.eventInfo.localMousePosition)));

            return contextualMenuManipulator;
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, DialogueType type)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode("DialogueName", type, actionEvent.eventInfo.localMousePosition))));

            return contextualMenuManipulator;
        }

        public void ClearGraph()
        {
            graphElements.ForEach(graphElements => RemoveElement(graphElements));

            groups.Clear();
            groupedNodes.Clear();
            ungroupedNodes.Clear();

            repeatedNames = 0;
        }
        #endregion
    }
}
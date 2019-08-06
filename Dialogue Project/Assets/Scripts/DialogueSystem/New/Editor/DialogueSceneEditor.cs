using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#region Вариант с наследованием
//public class DialogueSceneEditor : EditorWindow
//{
//    #region Поля

//    public DialogueSceneKit sceneKit;

//    private DialogueNode leftNodeBufer, drugNodeBufer;
//    private Rect buferRect;
//    private NodeType nodeType;
//    private Vector2 scrollPosition = Vector2.zero;
//    private Vector2 clickPoint;

//    private bool dragNode;
//    private bool mouseScroll;
//    private bool debug;
//    private int exitBufer;

//    private const float mainInfoYSize = 50;

//    #endregion

//    #region Основные методы

//    [MenuItem("Window/IgoGoTools/DialogueEditor %>")]
//    public static void Init()
//    {
//        DialogueSceneEditor sceneEditor = GetWindow<DialogueSceneEditor>();
//        sceneEditor.Show();
//    }
//    public static DialogueSceneEditor GetEditor()
//    {
//        return GetWindow<DialogueSceneEditor>();
//    }
//    private void OnGUI()
//    {
//        DrawMainInfo();
//        if (sceneKit != null)
//        {
//            DrawOptions();
//            DrawNodes();
//            DragNode();
//            DebugWindow();
//        }
//        MouseScroll();
//    }
//    #endregion
//    #region Методы отрисовки
//    private void DrawMainInfo()
//    {
//        buferRect = new Rect(0, 0, position.width, 20);
//        GUILayout.BeginVertical();
//        GUI.Box(buferRect, GUIContent.none);
//        GUILayout.BeginHorizontal();
//        sceneKit = (DialogueSceneKit)EditorGUILayout.ObjectField(sceneKit, typeof(DialogueSceneKit), false, GUILayout.MaxWidth(200));
//        if (sceneKit != null)
//        {
//            GUILayout.Label("Сцена: " + sceneKit.sceneName);
//            GUILayout.Label("Узлов: " + sceneKit.nodes.Count);
//        }
//        GUILayout.EndHorizontal();
//        GUILayout.EndVertical();
//    }
//    private void DrawOptions()
//    {
//        buferRect = new Rect(0, 20, position.width, 25);
//        GUILayout.BeginVertical();
//        GUI.Box(buferRect, GUIContent.none);
//        GUILayout.BeginHorizontal();
//        nodeType = (NodeType)EditorGUILayout.EnumPopup(nodeType, GUILayout.MaxWidth(100), GUILayout.MinWidth(100));
//        if (GUILayout.Button("Создать узел", GUILayout.MaxWidth(100), GUILayout.MinWidth(100)))
//        {
//            CreateNode();
//        }
//        debug = GUILayout.Toggle(debug, "Debug");
//        GUILayout.EndHorizontal();
//        GUILayout.Space(10);
//        GUILayout.EndVertical();
//    }
//    private void DrawNodes()
//    {
//        scrollPosition = GUI.BeginScrollView(new Rect(0, mainInfoYSize, position.width, position.height - mainInfoYSize), scrollPosition,
//            GetScrollViewZone(), false, false);
//        DrawRelations();

//        for (int i = 0; i < sceneKit.nodes.Count; i++)
//        {
//            DrawNode(sceneKit.nodes[i]);
//        }

//        //DrawNode(sceneKit.firstNode);
//        //for (int i = 0; i < sceneKit.replicList.Count; i++)
//        //{
//        //    DrawNode(sceneKit.replicList[i]);
//        //}
//        //for (int i = 0; i < sceneKit.choicesList.Count; i++)
//        //{
//        //    DrawNode(sceneKit.choicesList[i]);
//        //}
//        //for (int i = 0; i < sceneKit.conditionList.Count; i++)
//        //{
//        //    DrawNode(sceneKit.conditionList[i]);
//        //}
//        //for (int i = 0; i < sceneKit.eventList.Count; i++)
//        //{
//        //    DrawNode(sceneKit.eventList[i]);
//        //}
//        GUI.EndScrollView();
//    }
//    #endregion
//    #region Служебные методы
//    private void DrawRelations()
//    {
//        for (int i = 0; i < sceneKit.nodes.Count; i++)
//        {
//            if (sceneKit.nodes[i] is Replica)
//            {
//                Replica replica = sceneKit.nodes[i] as Replica;
//                if (replica.NextNode != null)
//                {
//                    DrawCurve(new Vector2(replica.TransformRect.x + replica.ExitPointOffset.x, replica.TransformRect.y + replica.ExitPointOffset.y),
//                        new Vector2(replica.NextNode.TransformRect.x + replica.NextNode.EnterPointOffset.x,
//                        replica.NextNode.TransformRect.y + replica.NextNode.EnterPointOffset.y), Color.white);
//                }
//            }
//            else if (sceneKit.nodes[i] is Choice)
//            {
//                Choice choice = sceneKit.nodes[i] as Choice;
//                for (int j = 0; j < choice.AnswerChoice.Count; j++)
//                {
//                    if (choice.AnswerChoice[j].NextNode != null)
//                    {
//                        DrawCurve(new Vector2(choice.TransformRect.x + choice.ExitPointsOffset[j].x, choice.TransformRect.y + choice.ExitPointsOffset[j].y),
//                            new Vector2(choice.AnswerChoice[j].NextNode.TransformRect.x + choice.AnswerChoice[j].NextNode.EnterPointOffset.x,
//                            choice.AnswerChoice[j].NextNode.TransformRect.y + choice.AnswerChoice[j].NextNode.EnterPointOffset.y), Color.white);
//                    }
//                }
//            }
//            else if (sceneKit.nodes[i] is ConditionContainer)
//            {
//                ConditionContainer conditionContainer = sceneKit.nodes[i] as ConditionContainer;
//                if (conditionContainer.PositiveNextNode != null)
//                {
//                    DrawCurve(new Vector2(conditionContainer.TransformRect.x + conditionContainer.PositiveExitPointOffset.x,
//                        conditionContainer.TransformRect.y + conditionContainer.PositiveExitPointOffset.y),
//                        new Vector2(conditionContainer.PositiveNextNode.TransformRect.x + conditionContainer.PositiveNextNode.EnterPointOffset.x,
//                        conditionContainer.PositiveNextNode.TransformRect.y + conditionContainer.PositiveNextNode.EnterPointOffset.y), Color.green);
//                }
//                if (conditionContainer.NegativeNextNode != null)
//                {
//                    DrawCurve(new Vector2(conditionContainer.TransformRect.x + conditionContainer.NegativeExitPointOffset.x,
//                        conditionContainer.TransformRect.y + conditionContainer.NegativeExitPointOffset.y),
//                        new Vector2(conditionContainer.NegativeNextNode.TransformRect.x + conditionContainer.NegativeNextNode.EnterPointOffset.x,
//                        conditionContainer.NegativeNextNode.TransformRect.y + conditionContainer.NegativeNextNode.EnterPointOffset.y), Color.red);
//                }
//            }
//            else if (sceneKit.nodes[i] is DialogueEventContainer)
//            {
//                DialogueEventContainer dialogueEvent = sceneKit.nodes[i] as DialogueEventContainer;
//                if (dialogueEvent.NextNode != null)
//                {
//                    DrawCurve(new Vector2(dialogueEvent.TransformRect.x + dialogueEvent.ExitPointOffset.x, dialogueEvent.TransformRect.y + dialogueEvent.ExitPointOffset.y),
//                        new Vector2(dialogueEvent.NextNode.TransformRect.x + dialogueEvent.NextNode.EnterPointOffset.x,
//                        dialogueEvent.NextNode.TransformRect.y + dialogueEvent.NextNode.EnterPointOffset.y), Color.yellow);
//                }
//            }
//        }
//    }
//    private void DrawCurve(Vector2 start, Vector2 end, Color color)
//    {
//        Vector3 bufer1, bufer2;
//        bufer1 = new Vector3(start.x + (end.x - start.x) / 2, start.y + 10 + mainInfoYSize, 0);
//        bufer2 = new Vector3(end.x - (end.x - start.x) / 2, end.y + 10 + mainInfoYSize, 0);
//        Handles.DrawBezier(new Vector3(start.x + 5, start.y + 10 + mainInfoYSize, 0), new Vector3(end.x, end.y + 10 + mainInfoYSize, 0), bufer1, bufer2, color, null, 3);
//    }
//    private void DrawNode(DialogueNode node)
//    {
//        if (node.TransformRect.x < 0)
//        {
//            node.TransformRect = new Rect(0, node.TransformRect.y, node.TransformRect.width, node.TransformRect.height);
//        }
//        if (node.TransformRect.y < 0)
//        {
//            node.TransformRect = new Rect(node.TransformRect.x, 0, node.TransformRect.width, node.TransformRect.height);
//        }

//        Rect nodeTransform = new Rect(node.TransformRect.x, node.TransformRect.y + mainInfoYSize, node.TransformRect.width, node.TransformRect.height);
//        EditorGUI.DrawRect(nodeTransform, node.ColorInEditor);
//        Rect bufer = new Rect(nodeTransform.x + nodeTransform.width - 21, nodeTransform.y + 1, 20, 20);
//        if (GUI.Button(bufer, "X"))
//        {
//            sceneKit.Remove(node);
//        }
//        if (!(node is ConditionContainer))
//        {
//            bufer.x -= 21;
//            if (GUI.Button(bufer, "="))
//            {
//                DialogueNodeEditorWindow nodeEditorWindow = DialogueNodeEditorWindow.GetNodeEditor();
//                nodeEditorWindow.kit = sceneKit;
//                nodeEditorWindow.node = node;
//                if (node is Replica)
//                {
//                    nodeEditorWindow.minSize = nodeEditorWindow.maxSize = new Vector2(400, 150);
//                }
//                else if (node is Choice)
//                {
//                    nodeEditorWindow.minSize = nodeEditorWindow.maxSize = new Vector2(400, 150);
//                }
//                else if (node is DialogueEventContainer)
//                {
//                    nodeEditorWindow.minSize = nodeEditorWindow.maxSize = new Vector2(400, 250);
//                }
//                nodeEditorWindow.Show();
//            }
//        }
//        bufer = new Rect(nodeTransform.x + node.EnterPointOffset.x, nodeTransform.y + node.EnterPointOffset.y, 20, 20);
//        if (GUI.Button(bufer, "O"))
//        {
//            if (leftNodeBufer != null)
//            {
//                AddRelation(node);
//            }
//        }
//        if (node is Replica)
//        {
//            Replica replica = node as Replica;
//            if (replica.Character != null)
//            {
//                bufer = new Rect(nodeTransform.x + 1, nodeTransform.y + 1, 20, 20);
//                EditorGUI.DrawRect(bufer, replica.Character.color);
//            }
//            bufer = new Rect(nodeTransform.x + nodeTransform.width - 63, nodeTransform.y + 1, 21, 21);
//            if (GUI.Button(bufer, replica.EndNode ? "-|" : "->"))
//            {
//                node.EndNode = !node.EndNode;
//                if (node.EndNode)
//                {
//                    node.ClearNextRelations();
//                }
//            }
//            if (!replica.EndNode)
//            {
//                bufer = new Rect(nodeTransform.x + replica.ExitPointOffset.x, nodeTransform.y + replica.ExitPointOffset.y, 20, 20);
//                if (GUI.Button(bufer, ">"))
//                {
//                    leftNodeBufer = node;
//                    exitBufer = 0;
//                }
//            }
//            bufer = new Rect(nodeTransform.x + 21, nodeTransform.y + 22, nodeTransform.width - 39, 20);
//            replica.ReplicText = EditorGUI.TextField(bufer, replica.ReplicText);
//        }
//        else if (node is Choice)
//        {
//            Choice choice = node as Choice;

//            if (choice.Character != null)
//            {
//                bufer = new Rect(nodeTransform.x + 1, nodeTransform.y + 1, 20, 20);
//                EditorGUI.DrawRect(bufer, choice.Character.color);
//            }
//            for (int i = 0; i < choice.AnswerChoice.Count; i++)
//            {
//                bufer = new Rect(nodeTransform.x + 21, nodeTransform.y + 21 * (i + 1), 20, 20);
//                if (GUI.Button(bufer, "x"))
//                {
//                    choice.RemoveAnsver(i);
//                    if (choice.AnswerChoice.Count < 2)
//                    {
//                        choice.TransformRect = new Rect(choice.TransformRect.x, choice.TransformRect.y, choice.TransformRect.width, choice.TransformRect.height - 22);
//                    }
//                    break;
//                }
//                bufer = new Rect(bufer.x + 21, bufer.y, nodeTransform.width - 63, 20);
//                choice.AnswerChoice[i].ReplicText = EditorGUI.TextField(bufer, choice.AnswerChoice[i].ReplicText);
//                bufer = new Rect(nodeTransform.x + choice.ExitPointsOffset[i].x, nodeTransform.y + choice.ExitPointsOffset[i].y, 20, 20);
//                if (GUI.Button(bufer, ">"))
//                {
//                    leftNodeBufer = node;
//                    exitBufer = i;
//                }
//            }
//            bufer = new Rect(nodeTransform.x + 21, nodeTransform.y + 21 * (choice.AnswerChoice.Count + 1), nodeTransform.width - 42, 20);
//            if (choice.AnswerChoice.Count < 3)
//            {
//                if (GUI.Button(bufer, "+"))
//                {
//                    var rep = new Replica { EndNode = false };
//                    choice.AnswerChoice.Add(rep);
//                    if (choice.AnswerChoice.Count < 3)
//                    {
//                        choice.TransformRect = new Rect(choice.TransformRect.x, choice.TransformRect.y, choice.TransformRect.width, choice.TransformRect.height + 22);
//                    }
//                    choice.CheckExitOffset();
//                }
//            }
//        }
//        else if (node is ConditionContainer)
//        {
//            ConditionContainer condition = node as ConditionContainer;
//            bufer = new Rect(nodeTransform.x + condition.PositiveExitPointOffset.x, nodeTransform.y + condition.PositiveExitPointOffset.y, 30, 20);
//            if (GUI.Button(bufer, "+>"))
//            {
//                leftNodeBufer = node;
//                exitBufer = 0;
//            }
//            bufer = new Rect(nodeTransform.x + condition.NegativeExitPointOffset.x, nodeTransform.y + condition.NegativeExitPointOffset.y, 30, 20);
//            if (GUI.Button(bufer, "->"))
//            {
//                leftNodeBufer = node;
//                exitBufer = 1;
//            }
//            bufer = new Rect(nodeTransform.x + 21, nodeTransform.y + 21, 130, 20);
//            condition.Condition = (ConditionPack)EditorGUI.ObjectField(bufer, condition.Condition, typeof(ConditionPack), allowSceneObjects: true);

//            if (condition.Condition != null)
//            {
//                bufer = new Rect(nodeTransform.x + 1, nodeTransform.y + 42, 82, 20);
//                condition.ConditionNumber = EditorGUI.Popup(bufer, condition.ConditionNumber, condition.Condition.GetCharacteristic());
//                bufer = new Rect(bufer.x + 81, bufer.y, 35, 20);
//                if (condition.Condition.conditions[condition.ConditionNumber].type == ConditionType.Bool)
//                {
//                    condition.CheckType = (CheckType)EditorGUI.Popup(bufer, (int)condition.CheckType, new string[2] { "==", "!=" });
//                    bufer = new Rect(bufer.x + 36, bufer.y, 20, 20);
//                    condition.CheckBoolValue = EditorGUI.Toggle(bufer, condition.CheckBoolValue);
//                }
//                else
//                {
//                    condition.CheckType = (CheckType)EditorGUI.Popup(bufer, (int)condition.CheckType, new string[4] { "==", "!=", ">", "<" });
//                    bufer = new Rect(bufer.x + 36, bufer.y, 30, 15);
//                    condition.CheckIntValue = EditorGUI.IntField(bufer, condition.CheckIntValue);
//                }
//            }
//        }
//        else if (node is DialogueEventContainer)
//        {
//            DialogueEventContainer dialogueEvent = node as DialogueEventContainer;
//            bufer = new Rect(nodeTransform.x + nodeTransform.width - 63, nodeTransform.y + 1, 21, 21);
//            if (GUI.Button(bufer, dialogueEvent.EndNode ? "-|" : "->"))
//            {
//                dialogueEvent.EndNode = !dialogueEvent.EndNode;
//                if (dialogueEvent.EndNode)
//                {
//                    node.ClearNextRelations();
//                }
//            }
//            if (!dialogueEvent.EndNode)
//            {
//                bufer = new Rect(nodeTransform.x + dialogueEvent.ExitPointOffset.x, nodeTransform.y + dialogueEvent.ExitPointOffset.y, 20, 20);
//                if (GUI.Button(bufer, ">"))
//                {
//                    leftNodeBufer = node;
//                    exitBufer = 0;
//                }
//            }
//            bufer = new Rect(nodeTransform.x + 21, nodeTransform.y + 21, 110, 20);
//            dialogueEvent.ConditionCharacteristic = (ConditionPack)EditorGUI.ObjectField(bufer, dialogueEvent.ConditionCharacteristic,
//                typeof(ConditionPack), allowSceneObjects: true);
//            if (dialogueEvent.ConditionCharacteristic != null)
//            {
//                if (dialogueEvent.ChangeCondition)
//                {
//                    bufer = new Rect(nodeTransform.x + 21, bufer.y + 21, 80, 20);
//                    dialogueEvent.ChangeConditionNumber = EditorGUI.Popup(bufer, dialogueEvent.ChangeConditionNumber,
//                        dialogueEvent.ConditionCharacteristic.GetCharacteristic());

//                    if (dialogueEvent.ConditionCharacteristic.conditions[dialogueEvent.ChangeConditionNumber].type == ConditionType.Bool)
//                    {
//                        bufer = new Rect(bufer.x + 81, bufer.y, 15, 20);
//                        EditorGUI.LabelField(bufer, "=");
//                        bufer = new Rect(bufer.x + 16, bufer.y, 20, 20);
//                        dialogueEvent.ChangeBoolValue = EditorGUI.Toggle(bufer, dialogueEvent.ChangeBoolValue);
//                    }
//                    else
//                    {
//                        bufer = new Rect(bufer.x + 81, bufer.y, 15, 20);
//                        EditorGUI.LabelField(bufer, "+");
//                        bufer = new Rect(bufer.x + 16, bufer.y, 20, 20);
//                        dialogueEvent.AddIntValue = EditorGUI.IntField(bufer, dialogueEvent.AddIntValue);
//                    }
//                }
//                if (dialogueEvent.IsMessage)
//                {
//                    bufer = new Rect(nodeTransform.x + 21, bufer.y + 21, 100, 20);
//                    dialogueEvent.MessageText = EditorGUI.TextArea(bufer, dialogueEvent.MessageText);
//                }
//                if (dialogueEvent.InSceneInvoke)
//                {
//                    bufer = new Rect(nodeTransform.x + nodeTransform.width - 21, nodeTransform.y + 63, 20, 20);
//                    EditorGUI.LabelField(bufer, "#");
//                }
//            }
//        }
//    }
//    private void CreateNode()
//    {
//        switch (nodeType)
//        {
//            case NodeType.Replica:
//                Replica rep = new Replica(new Vector2(position.width / 2 + scrollPosition.x, position.height / 2 + scrollPosition.y));
//                sceneKit.nodes.Add(rep);
//                break;
//            case NodeType.Choice:
//                sceneKit.nodes.Add(new Choice(new Vector2(position.width / 2 + scrollPosition.x, position.height / 2 + scrollPosition.y)));
//                break;
//            case NodeType.Condition:
//                sceneKit.nodes.Add(new ConditionContainer(new Vector2(position.width / 2 + scrollPosition.x, position.height / 2 + scrollPosition.y)));
//                break;
//            case NodeType.Event:
//                sceneKit.nodes.Add(new DialogueEventContainer(new Vector2(position.width / 2 + scrollPosition.x, position.height / 2 + scrollPosition.y)));
//                break;
//        }
//    }
//    private void AddRelation(DialogueNode node)
//    {
//        if (leftNodeBufer is Replica)
//        {
//            Replica replica = leftNodeBufer as Replica;
//            replica.RemoveFromNext(replica.NextNode);
//            replica.NextNode = node;
//            replica.NextNode.AddInPreviousRelations(replica);
//        }
//        else if (leftNodeBufer is Choice)
//        {
//            Choice choice = leftNodeBufer as Choice;
//            choice.RemoveFromNext(choice.AnswerChoice[exitBufer].NextNode, exitBufer);
//            choice.AnswerChoice[exitBufer].NextNode = node;
//            choice.AnswerChoice[exitBufer].NextNode.AddInPreviousRelations(choice.AnswerChoice[exitBufer]);
//        }
//        else if (leftNodeBufer is ConditionContainer)
//        {
//            ConditionContainer conditionContainer = leftNodeBufer as ConditionContainer;
//            if (exitBufer == 0)
//            {
//                conditionContainer.RemoveFromNext(conditionContainer.PositiveNextNode);
//                conditionContainer.PositiveNextNode = node;
//                conditionContainer.PositiveNextNode.AddInPreviousRelations(conditionContainer);
//            }
//            else
//            {
//                conditionContainer.RemoveFromNext(conditionContainer.NegativeNextNode);
//                conditionContainer.NegativeNextNode = node;
//                conditionContainer.NegativeNextNode.AddInPreviousRelations(conditionContainer);
//            }
//        }
//        else if (leftNodeBufer is DialogueEventContainer)
//        {
//            DialogueEventContainer dialogueEvent = leftNodeBufer as DialogueEventContainer;
//            dialogueEvent.RemoveFromNext(dialogueEvent.NextNode);
//            dialogueEvent.NextNode = node;
//            dialogueEvent.NextNode.AddInPreviousRelations(dialogueEvent);
//        }
//        leftNodeBufer = null;
//        exitBufer = 0;
//    }
//    private void DragNode()
//    {
//        if (debug)
//        {
//            EditorGUI.DrawRect(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 30, 30), Color.cyan);
//        }
//        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
//        {
//            if (ClickInNode(Event.current.mousePosition, out drugNodeBufer))
//            {
//                dragNode = true;
//            }
//        }
//        if (dragNode)
//        {
//            if (Event.current.type == EventType.MouseDrag && Event.current.button == 0 && drugNodeBufer != null)
//            {
//                Vector2 offset = Event.current.mousePosition - clickPoint;
//                clickPoint += offset;
//                buferRect = new Rect(drugNodeBufer.TransformRect.x + offset.x, drugNodeBufer.TransformRect.y + offset.y, drugNodeBufer.TransformRect.width,
//                    drugNodeBufer.TransformRect.height);
//                drugNodeBufer.TransformRect = buferRect;
//            }
//            if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && drugNodeBufer != null)
//            {
//                dragNode = false;
//            }
//        }
//    }
//    private void DebugWindow()
//    {
//        if (debug)
//        {
//            foreach (var item in sceneKit.nodes)
//            {
//                EditorGUI.DrawRect(new Rect(item.TransformRect.x - scrollPosition.x, item.TransformRect.y + mainInfoYSize - scrollPosition.y,
//                    item.TransformRect.width, item.TransformRect.height), Color.blue);
//            }
//        }
//    }
//    private bool ClickInNode(Vector2 mousePos, out DialogueNode node)
//    {
//        node = null;
//        for (int i = sceneKit.nodes.Count - 1; i >= 0; i--)
//        {
//            if (mousePos.x > sceneKit.nodes[i].TransformRect.x - scrollPosition.x && mousePos.x < sceneKit.nodes[i].TransformRect.x - scrollPosition.x
//                + sceneKit.nodes[i].TransformRect.width &&
//                mousePos.y > sceneKit.nodes[i].TransformRect.y - scrollPosition.y + mainInfoYSize && mousePos.y < sceneKit.nodes[i].TransformRect.y - scrollPosition.y
//                + mainInfoYSize + sceneKit.nodes[i].TransformRect.height)
//            {
//                node = sceneKit.nodes[i];
//                clickPoint = mousePos;
//                return true;
//            }
//        }
//        return false;
//    }
//    private bool ClickInNode(Vector2 mousePos)
//    {
//        for (int i = sceneKit.nodes.Count - 1; i >= 0; i--)
//        {
//            if (mousePos.x > sceneKit.nodes[i].TransformRect.x - scrollPosition.x && mousePos.x < sceneKit.nodes[i].TransformRect.x - scrollPosition.x
//                + sceneKit.nodes[i].TransformRect.width &&
//                mousePos.y > sceneKit.nodes[i].TransformRect.y - scrollPosition.y + mainInfoYSize && mousePos.y < sceneKit.nodes[i].TransformRect.y - scrollPosition.y
//                + mainInfoYSize + sceneKit.nodes[i].TransformRect.height)
//            {
//                clickPoint = mousePos;
//                return true;
//            }
//        }
//        return false;
//    }
//    private Rect GetScrollViewZone()
//    {
//        Rect rezult = new Rect(0, mainInfoYSize, position.width, position.height - mainInfoYSize);
//        float maxX, maxY;
//        maxX = maxY = 0;

//        foreach (var item in sceneKit.nodes)
//        {
//            if (maxX < item.TransformRect.x + item.TransformRect.width)
//            {
//                maxX = item.TransformRect.x + item.TransformRect.width;
//            }
//            if (maxY < item.TransformRect.y + mainInfoYSize + item.TransformRect.height)
//            {
//                maxY = item.TransformRect.y + mainInfoYSize + item.TransformRect.height;
//            }
//        }

//        if (maxX > rezult.x + rezult.width)
//        {
//            rezult = new Rect(rezult.x, rezult.y, rezult.width + 200, rezult.height);
//        }
//        if (maxY > rezult.y + rezult.height)
//        {
//            rezult = new Rect(rezult.x, rezult.y, rezult.width, rezult.height + 200);
//        }
//        return rezult;
//    }
//    private void MouseScroll()
//    {
//        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
//        {
//            if (!ClickInNode(Event.current.mousePosition))
//            {
//                mouseScroll = true;
//                clickPoint = Event.current.mousePosition;
//            }
//        }
//        if (mouseScroll)
//        {
//            Vector2 offset = Event.current.mousePosition - clickPoint;
//            clickPoint = Event.current.mousePosition;
//            scrollPosition += offset;
//        }
//        if (Event.current.type == EventType.MouseUp && Event.current.button == 1)
//        {
//            mouseScroll = false;
//        }
//    }

//    #endregion

//}
#endregion


public class DialogueSceneEditor : EditorWindow
{
    #region Поля

    public DialogueSceneKit sceneKit;

    private DialogueNode beginRelationNodeBufer, drugNodeBufer;
    private Rect buferRect, scrollViewRect;
    private NodeType nodeType;
    private Vector2 scrollPosition = Vector2.zero;
    private Vector2 clickPoint;

    private bool dragNode;
    private bool mouseScroll;
    private bool debug;
    private int exitBufer;

    private const float mainInfoYSize = 50;

    #endregion

    #region Основные методы

    [MenuItem("Window/IgoGoTools/DialogueEditor %>")]
    public static void Init()
    {
        DialogueSceneEditor sceneEditor = GetWindow<DialogueSceneEditor>();
        sceneEditor.Show();
    }
    public static DialogueSceneEditor GetEditor()
    {
        return GetWindow<DialogueSceneEditor>();
    }

    private void OnEnable()
    {
        scrollViewRect = new Rect(0, mainInfoYSize, position.width, position.height - mainInfoYSize);
    }

    private void OnGUI()
    {
        DrawMainInfo();
        if (sceneKit != null)
        {
            DrawOptions();
            DrawNodes();
            DragNode();
            DebugWindow();
        }
        MouseScroll();
    }
    #endregion
    #region Методы отрисовки
    private void DrawMainInfo()
    {
        buferRect = new Rect(0, 0, position.width, 20);
        GUILayout.BeginVertical();
        GUI.Box(buferRect, GUIContent.none);
        GUILayout.BeginHorizontal();
        sceneKit = (DialogueSceneKit)EditorGUILayout.ObjectField(sceneKit, typeof(DialogueSceneKit), false, GUILayout.MaxWidth(200));
        if (sceneKit != null)
        {
            GUILayout.Label("Сцена: " + sceneKit.sceneName);
            GUILayout.Label("Узлов: " + sceneKit.nodes.Count);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
    private void DrawOptions()
    {
        buferRect = new Rect(0, 20, position.width, 25);
        GUILayout.BeginVertical();
        GUI.Box(buferRect, GUIContent.none);
        GUILayout.BeginHorizontal();
        nodeType = (NodeType)EditorGUILayout.EnumPopup(nodeType, GUILayout.MaxWidth(100), GUILayout.MinWidth(100));
        if (GUILayout.Button("Создать узел", GUILayout.MaxWidth(100), GUILayout.MinWidth(100)))
        {
            CreateNode();
        }
        debug = GUILayout.Toggle(debug, "Debug");
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.EndVertical();
    }
    private void DrawNodes()
    {
        scrollViewRect = GetScrollViewZone();
        scrollPosition = GUI.BeginScrollView(new Rect(0, mainInfoYSize, position.width, position.height - mainInfoYSize), scrollPosition,
            scrollViewRect, false, false);
        DrawRelations();

        for (int i = 0; i < sceneKit.nodes.Count; i++)
        {
            if(!sceneKit.nodes[i].Hide)
            {
                DrawNode(sceneKit.nodes[i]);
            }
        }
        GUI.EndScrollView();
    }
    #endregion
    #region Служебные методы
    private void DrawRelations()
    {
        for (int i = 0; i < sceneKit.nodes.Count; i++)
        {
            if(!sceneKit.nodes[i].Hide)
            {
                int startMultiplicator, endMultiplicator;
                startMultiplicator = endMultiplicator = 1;
                switch (sceneKit.nodes[i].Type)
                {
                    case NodeType.Replica:
                        if (sceneKit.nodes[i].NextNodeNumber != -1)
                        {
                            DialogueNode node = sceneKit.nodes[sceneKit.nodes[i].NextNodeNumber];

                            Vector2 startPoint = new Vector2(sceneKit.nodes[i].TransformRect.x + sceneKit.nodes[i].RightPointOffset.x, sceneKit.nodes[i].TransformRect.y +
                                sceneKit.nodes[i].RightPointOffset.y);
                            if (!sceneKit.nodes[i].LeftToRight)
                            {
                                startPoint = new Vector2(sceneKit.nodes[i].TransformRect.x + sceneKit.nodes[i].LeftPointOffset.x, sceneKit.nodes[i].TransformRect.y +
                                sceneKit.nodes[i].LeftPointOffset.y);
                                startMultiplicator = -1;
                            }

                            Vector2 endPoint = new Vector2(node.TransformRect.x + node.LeftPointOffset.x, node.TransformRect.y + node.LeftPointOffset.y);
                            if (!node.LeftToRight)
                            {
                                endPoint = new Vector2(node.TransformRect.x + node.RightPointOffset.x, node.TransformRect.y + node.RightPointOffset.y);
                                endMultiplicator = -1;
                            }

                            DrawCurve(startPoint, endPoint, startMultiplicator, endMultiplicator, Color.white);
                        }
                        break;
                    case NodeType.Choice:
                        for (int j = 0; j < sceneKit.nodes[i].AnswerChoice.Count; j++)
                        {
                            if (sceneKit.nodes[sceneKit.nodes[i].AnswerChoice[j]].NextNodeNumber != -1)
                            {
                                DialogueNode node = sceneKit.nodes[sceneKit.nodes[sceneKit.nodes[i].AnswerChoice[j]].NextNodeNumber];
                                

                                Vector2 startPoint = new Vector2(sceneKit.nodes[i].TransformRect.x + sceneKit.nodes[i].ExitPointsOffset[j].x, sceneKit.nodes[i].TransformRect.y +
                                    sceneKit.nodes[i].ExitPointsOffset[j].y);

                                if (!sceneKit.nodes[i].LeftToRight)
                                {
                                    startPoint = new Vector2(sceneKit.nodes[i].TransformRect.x + 1, sceneKit.nodes[i].TransformRect.y +
                                    sceneKit.nodes[i].ExitPointsOffset[j].y);
                                    startMultiplicator = -1;
                                }

                                Vector2 endPoint = new Vector2(node.TransformRect.x + node.LeftPointOffset.x, node.TransformRect.y + node.LeftPointOffset.y);
                                if (!node.LeftToRight)
                                {
                                    endPoint = new Vector2(node.TransformRect.x + node.RightPointOffset.x, node.TransformRect.y + node.RightPointOffset.y);
                                    endMultiplicator = -1;
                                }

                                DrawCurve(startPoint, endPoint, startMultiplicator, endMultiplicator, Color.white);
                            }
                        }
                        break;
                    case NodeType.Event:
                        if (sceneKit.nodes[i].NextNodeNumber != -1)
                        {
                            DialogueNode node = sceneKit.nodes[sceneKit.nodes[i].NextNodeNumber];

                            Vector2 startPoint = new Vector2(sceneKit.nodes[i].TransformRect.x + sceneKit.nodes[i].RightPointOffset.x, sceneKit.nodes[i].TransformRect.y +
                                sceneKit.nodes[i].RightPointOffset.y);
                            if (!sceneKit.nodes[i].LeftToRight)
                            {
                                startPoint = new Vector2(sceneKit.nodes[i].TransformRect.x + sceneKit.nodes[i].LeftPointOffset.x, sceneKit.nodes[i].TransformRect.y +
                                sceneKit.nodes[i].LeftPointOffset.y);
                                startMultiplicator = -1;
                            }

                            Vector2 endPoint = new Vector2(node.TransformRect.x + node.LeftPointOffset.x, node.TransformRect.y + node.LeftPointOffset.y);
                            if (!node.LeftToRight)
                            {
                                endPoint = new Vector2(node.TransformRect.x + node.RightPointOffset.x, node.TransformRect.y + node.RightPointOffset.y);
                                endMultiplicator = -1;
                            }

                            DrawCurve(startPoint, endPoint, startMultiplicator, endMultiplicator, Color.yellow);
                        }
                        break;
                    case NodeType.Condition:
                        if (sceneKit.nodes[i].PositiveNextNumber != -1)
                        {
                            DialogueNode node = sceneKit.nodes[sceneKit.nodes[i].PositiveNextNumber];

                            Vector2 startPoint = new Vector2(sceneKit.nodes[i].TransformRect.x + sceneKit.nodes[i].PositiveExitPointOffset.x,
                                sceneKit.nodes[i].TransformRect.y + sceneKit.nodes[i].PositiveExitPointOffset.y);
                            if (!sceneKit.nodes[i].LeftToRight)
                            {
                                startPoint = new Vector2(sceneKit.nodes[i].TransformRect.x -1, sceneKit.nodes[i].TransformRect.y +
                                    sceneKit.nodes[i].PositiveExitPointOffset.y);
                                startMultiplicator = -1;
                            }

                            Vector2 endPoint = new Vector2(node.TransformRect.x + node.LeftPointOffset.x, node.TransformRect.y + node.LeftPointOffset.y);
                            if (!node.LeftToRight)
                            {
                                endPoint = new Vector2(node.TransformRect.x + node.RightPointOffset.x, node.TransformRect.y + node.RightPointOffset.y);
                                endMultiplicator = -1;
                            }

                            DrawCurve(startPoint, endPoint, startMultiplicator, endMultiplicator, Color.green);
                        }
                        if (sceneKit.nodes[i].NegativeNextNumber != -1)
                        {
                            DialogueNode node = sceneKit.nodes[sceneKit.nodes[i].NegativeNextNumber];

                            Vector2 startPoint = new Vector2(sceneKit.nodes[i].TransformRect.x + sceneKit.nodes[i].NegativeExitPointOffset.x,
                                sceneKit.nodes[i].TransformRect.y + sceneKit.nodes[i].NegativeExitPointOffset.y);
                            if (!sceneKit.nodes[i].LeftToRight)
                            {
                                startPoint = new Vector2(sceneKit.nodes[i].TransformRect.x -1, sceneKit.nodes[i].TransformRect.y +
                                    sceneKit.nodes[i].NegativeExitPointOffset.y);
                                startMultiplicator = -1;
                            }

                            Vector2 endPoint = new Vector2(node.TransformRect.x + node.LeftPointOffset.x, node.TransformRect.y + node.LeftPointOffset.y);
                            if (!node.LeftToRight)
                            {
                                endPoint = new Vector2(node.TransformRect.x + node.RightPointOffset.x, node.TransformRect.y + node.RightPointOffset.y);
                                endMultiplicator = -1;
                            }

                            DrawCurve(startPoint, endPoint, startMultiplicator, endMultiplicator, Color.red);
                        }
                        break;
                }
            }
        }
    }
    private void DrawCurve(Vector2 start, Vector2 end, int startM, int endM, Color color)
    {
        Vector3 bufer1, bufer2;
        Vector2 startMultiplicator, endMultiplicator;
        startMultiplicator = endMultiplicator = Vector2.one;

        if (start.x * startM > end.x * endM)
        {
            if (start.y > end.y)
            {
                startMultiplicator.y = endMultiplicator.y = - 100;
            }
            else if (start.y == end.y)
            {
                startMultiplicator.y = 100;
                endMultiplicator.y = -100;
            }
            else
            {
                startMultiplicator.y = endMultiplicator.y =  100;
            }
        }

        startMultiplicator.x *= startM;
        endMultiplicator.x *= endM;

        bufer1 = new Vector3(start.x + startMultiplicator.x * Mathf.Abs(end.x - start.x) / 2, start.y + startMultiplicator.y + mainInfoYSize, 0);
        bufer2 = new Vector3(end.x - endMultiplicator.x * Mathf.Abs(end.x - start.x) / 2, end.y - endMultiplicator.y + mainInfoYSize, 0);

        Handles.DrawBezier(new Vector3(start.x + 5, start.y + 10 + mainInfoYSize, 0), new Vector3(end.x, end.y + 10 + mainInfoYSize, 0),
            bufer1, bufer2, color, null, 3);
    }
    private void DrawNode(DialogueNode node)
    {
        if (node.TransformRect.x < 0)
        {
            node.TransformRect = new Rect(0, node.TransformRect.y, node.TransformRect.width, node.TransformRect.height);
        }
        if (node.TransformRect.y < 0)
        {
            node.TransformRect = new Rect(node.TransformRect.x, 0, node.TransformRect.width, node.TransformRect.height);
        }

        Rect nodeTransform = new Rect(node.TransformRect.x, node.TransformRect.y + mainInfoYSize, node.TransformRect.width, node.TransformRect.height);
        EditorGUI.DrawRect(nodeTransform, node.ColorInEditor);
        Rect bufer = new Rect(nodeTransform.x + nodeTransform.width - 21, nodeTransform.y + 1, 20, 20);
        if (GUI.Button(bufer, "X"))
        {
            sceneKit.Remove(node);
            return;
        }
        if (node.Type != NodeType.Condition)
        {
            bufer.x -= 21;
            if (GUI.Button(bufer, "="))
            {
                DialogueNodeEditorWindow nodeEditorWindow = DialogueNodeEditorWindow.GetNodeEditor();
                nodeEditorWindow.kit = sceneKit;
                nodeEditorWindow.node = node;

                switch (node.Type)
                {
                    case NodeType.Replica:
                        nodeEditorWindow.minSize = nodeEditorWindow.maxSize = new Vector2(400, 150);
                        break;
                    case NodeType.Choice:
                        nodeEditorWindow.minSize = nodeEditorWindow.maxSize = new Vector2(400, 150);
                        break;
                    case NodeType.Event:
                        nodeEditorWindow.minSize = nodeEditorWindow.maxSize = new Vector2(400, 250);
                        break;
                }
                nodeEditorWindow.Show();
            }
        }

        bufer = new Rect(nodeTransform.x + 22, nodeTransform.y + 1, 30, 20);
        if (node.LeftToRight)
        {
            if (GUI.Button(bufer, ">>"))
            {
                node.LeftToRight = !node.LeftToRight;
            }
            bufer = new Rect(nodeTransform.x + node.LeftPointOffset.x, nodeTransform.y + node.LeftPointOffset.y, 20, 20);
        }
        else
        {
            if (GUI.Button(bufer, "<<"))
            {
                node.LeftToRight = !node.LeftToRight;
            }
            bufer = new Rect(nodeTransform.x + node.RightPointOffset.x, nodeTransform.y + node.RightPointOffset.y, 20, 20);
        }
        if (GUI.Button(bufer, "O"))
        {
            if (beginRelationNodeBufer != null)
            {
                AddRelation(node);
            }
        }

        switch (node.Type)  
        {
            case NodeType.Replica:

                if (node.Character != null)
                {
                    bufer = new Rect(nodeTransform.x + 1, nodeTransform.y + 1, 20, 20);
                    EditorGUI.DrawRect(bufer, node.Character.color);
                }

                if (node.LeftToRight)
                {
                    bufer = new Rect(nodeTransform.x + nodeTransform.width - 63, nodeTransform.y + 1, 21, 21);
                    if (GUI.Button(bufer, node.EndNode ? "-|" : "->"))
                    {
                        node.EndNode = !node.EndNode;
                        if (node.EndNode)
                        {
                            sceneKit.ClearNextRelations(node);
                        }
                    }
                    if (!node.EndNode)
                    {
                        bufer = new Rect(nodeTransform.x + node.RightPointOffset.x, nodeTransform.y + node.RightPointOffset.y, 20, 20);
                        if (GUI.Button(bufer, ">"))
                        {
                            beginRelationNodeBufer = node;
                            exitBufer = 0;
                        }
                    }
                    bufer = new Rect(nodeTransform.x + 21, nodeTransform.y + 22, nodeTransform.width - 39, 20);
                    node.ReplicText = EditorGUI.TextField(bufer, node.ReplicText);
                }
                else
                {
                    bufer = new Rect(nodeTransform.x + 53, nodeTransform.y + 1, 21, 21);
                    if (GUI.Button(bufer, node.EndNode ? "|-" : "<-"))
                    {
                        node.EndNode = !node.EndNode;
                        if (node.EndNode)
                        {
                            sceneKit.ClearNextRelations(node);
                        }
                    }
                    if (!node.EndNode)
                    {
                        bufer = new Rect(nodeTransform.x + node.LeftPointOffset.x, nodeTransform.y + node.LeftPointOffset.y, 20, 20);
                        if (GUI.Button(bufer, "<"))
                        {
                            beginRelationNodeBufer = node;
                            exitBufer = 0;
                        }
                    }
                    bufer = new Rect(nodeTransform.x + 21, nodeTransform.y + 22, nodeTransform.width - 39, 20);
                    node.ReplicText = EditorGUI.TextField(bufer, node.ReplicText);
                }
                break;
            case NodeType.Choice:
                if (node.Character != null)
                {
                    bufer = new Rect(nodeTransform.x + 1, nodeTransform.y + 1, 20, 20);
                    EditorGUI.DrawRect(bufer, node.Character.color);
                }
                for (int i = 0; i < node.AnswerChoice.Count; i++)
                {
                    if(node.LeftToRight)
                    {
                        bufer = new Rect(nodeTransform.x + 21, nodeTransform.y + 21 * (i + 1), 20, 20);
                        if (GUI.Button(bufer, "x"))
                        {
                            sceneKit.Remove(sceneKit.nodes[node.AnswerChoice[i]]);
                            node.RemoveAnsver(i);
                            if (node.AnswerChoice.Count < 2)
                            {
                                node.TransformRect = new Rect(node.TransformRect.x, node.TransformRect.y, node.TransformRect.width, node.TransformRect.height - 22);
                            }
                            break;
                        }
                        bufer = new Rect(bufer.x + 21, bufer.y, nodeTransform.width - 63, 20);
                        sceneKit.nodes[node.AnswerChoice[i]].ReplicText = EditorGUI.TextField(bufer, sceneKit.nodes[node.AnswerChoice[i]].ReplicText);
                        bufer = new Rect(nodeTransform.x + node.ExitPointsOffset[i].x, nodeTransform.y + node.ExitPointsOffset[i].y, 20, 20);
                        if (GUI.Button(bufer, ">"))
                        {
                            beginRelationNodeBufer = node;
                            exitBufer = i;
                        }
                    }
                    else
                    {
                        bufer = new Rect(nodeTransform.x + 1, nodeTransform.y + 21 * (i + 1), 20, 20);
                        if (GUI.Button(bufer, "<"))
                        {
                            beginRelationNodeBufer = node;
                            exitBufer = i;
                        }
                        bufer = new Rect(bufer.x + 21, bufer.y, nodeTransform.width - 63, 20);
                        sceneKit.nodes[node.AnswerChoice[i]].ReplicText = EditorGUI.TextField(bufer, sceneKit.nodes[node.AnswerChoice[i]].ReplicText);
                        bufer = new Rect(nodeTransform.x + node.ExitPointsOffset[i].x - 21, nodeTransform.y + node.ExitPointsOffset[i].y, 20, 20);
                        if (GUI.Button(bufer, "x"))
                        {
                            sceneKit.Remove(sceneKit.nodes[node.AnswerChoice[i]]);
                            node.RemoveAnsver(i);
                            if (node.AnswerChoice.Count < 2)
                            {
                                node.TransformRect = new Rect(node.TransformRect.x, node.TransformRect.y, node.TransformRect.width, node.TransformRect.height - 22);
                            }
                            break;
                        }
                    }
                }
                bufer = new Rect(nodeTransform.x + 21, nodeTransform.y + 21 * (node.AnswerChoice.Count + 1), nodeTransform.width - 42, 20);
                if (node.AnswerChoice.Count < 3)
                {
                    if (GUI.Button(bufer, "+"))
                    {
                        var rep = new DialogueNode(NodeType.Replica, sceneKit.nodes.Count, true) { EndNode = false };
                        sceneKit.nodes.Add(rep);
                        node.AnswerChoice.Add(rep.Index);
                        if (node.AnswerChoice.Count < 3)
                        {
                            node.TransformRect = new Rect(node.TransformRect.x, node.TransformRect.y, node.TransformRect.width, node.TransformRect.height + 22);
                        }
                        node.CheckExitOffset();
                    }
                }
                break;
            case NodeType.Event:
                bufer = new Rect(nodeTransform.x + nodeTransform.width - 63, nodeTransform.y + 1, 21, 21);
                if (GUI.Button(bufer, node.EndNode ? "-|" : "->"))
                {
                    node.EndNode = !node.EndNode;
                    if (node.EndNode)
                    {
                        sceneKit.ClearNextRelations(node);
                    }
                }
                if (!node.EndNode)
                {
                    if(node.LeftToRight)
                    {
                        bufer = new Rect(nodeTransform.x + node.RightPointOffset.x, nodeTransform.y + node.RightPointOffset.y, 20, 20);
                        if (GUI.Button(bufer, ">"))
                        {
                            beginRelationNodeBufer = node;
                            exitBufer = 0;
                        }
                    }
                    else
                    {
                        bufer = new Rect(nodeTransform.x + node.LeftPointOffset.x, nodeTransform.y + node.LeftPointOffset.y, 20, 20);
                        if (GUI.Button(bufer, "<"))
                        {
                            beginRelationNodeBufer = node;
                            exitBufer = 0;
                        }
                    }
                    
                }
                bufer = new Rect(nodeTransform.x + 21, nodeTransform.y + 21, 110, 20);
                node.Condition = (ConditionPack)EditorGUI.ObjectField(bufer, node.Condition, typeof(ConditionPack), allowSceneObjects: true);
                if (node.Condition != null)
                {
                    if (node.ChangeCondition)
                    {
                        bufer = new Rect(nodeTransform.x + 21, bufer.y + 21, 80, 20);
                        node.ConditionNumber = EditorGUI.Popup(bufer, node.ConditionNumber,
                            node.Condition.GetCharacteristic());

                        if (node.Condition.conditions[node.ConditionNumber].type == ConditionType.Bool)
                        {
                            bufer = new Rect(bufer.x + 81, bufer.y, 15, 20);
                            EditorGUI.LabelField(bufer, "=");
                            bufer = new Rect(bufer.x + 16, bufer.y, 20, 20);
                            node.ChangeBoolValue = EditorGUI.Toggle(bufer, node.ChangeBoolValue);
                        }
                        else
                        {
                            bufer = new Rect(bufer.x + 81, bufer.y, 15, 20);
                            EditorGUI.LabelField(bufer, "+");
                            bufer = new Rect(bufer.x + 16, bufer.y, 20, 20);
                            node.AddIntValue = EditorGUI.IntField(bufer, node.AddIntValue);
                        }
                    }
                }
                if (node.IsMessage)
                {
                    bufer = new Rect(nodeTransform.x + 21, bufer.y + 21, 100, 20);
                    node.MessageText = EditorGUI.TextArea(bufer, node.MessageText);
                }
                if (node.InSceneInvoke)
                {
                    bufer = new Rect(nodeTransform.x + nodeTransform.width - 21, nodeTransform.y + 63, 20, 20);
                    EditorGUI.LabelField(bufer, "#");
                }
                break;
            case NodeType.Condition:
                if(node.LeftToRight)
                {
                    bufer = new Rect(nodeTransform.x + node.PositiveExitPointOffset.x, nodeTransform.y + node.PositiveExitPointOffset.y, 30, 20);
                    if (GUI.Button(bufer, "+>"))
                    {
                        beginRelationNodeBufer = node;
                        exitBufer = 0;
                    }
                    bufer = new Rect(nodeTransform.x + node.NegativeExitPointOffset.x, nodeTransform.y + node.NegativeExitPointOffset.y, 30, 20);
                    if (GUI.Button(bufer, "->"))
                    {
                        beginRelationNodeBufer = node;
                        exitBufer = 1;
                    }
                    bufer = new Rect(nodeTransform.x + 21, nodeTransform.y + 21, 130, 20);
                    node.Condition = (ConditionPack)EditorGUI.ObjectField(bufer, node.Condition, typeof(ConditionPack), allowSceneObjects: true);
                    if (node.Condition != null)
                    {
                        bufer = new Rect(nodeTransform.x + 1, nodeTransform.y + 42, 82, 20);
                        node.ConditionNumber = EditorGUI.Popup(bufer, node.ConditionNumber, node.Condition.GetCharacteristic());
                        bufer = new Rect(bufer.x + 81, bufer.y, 35, 20);
                        if (node.Condition.conditions[node.ConditionNumber].type == ConditionType.Bool)
                        {
                            node.CheckType = (CheckType)EditorGUI.Popup(bufer, (int)node.CheckType, new string[2] { "==", "!=" });
                            bufer = new Rect(bufer.x + 36, bufer.y, 20, 20);
                            node.CheckBoolValue = EditorGUI.Toggle(bufer, node.CheckBoolValue);
                        }
                        else
                        {
                            node.CheckType = (CheckType)EditorGUI.Popup(bufer, (int)node.CheckType, new string[4] { "==", "!=", ">", "<" });
                            bufer = new Rect(bufer.x + 36, bufer.y, 30, 15);
                            node.CheckIntValue = EditorGUI.IntField(bufer, node.CheckIntValue);
                        }
                    }
                }
                else
                {
                    bufer = new Rect(nodeTransform.x + 1, nodeTransform.y + node.PositiveExitPointOffset.y, 30, 20);
                    if (GUI.Button(bufer, "<+"))
                    {
                        beginRelationNodeBufer = node;
                        exitBufer = 0;
                    }
                    bufer = new Rect(nodeTransform.x + 1, nodeTransform.y + node.NegativeExitPointOffset.y, 30, 20);
                    if (GUI.Button(bufer, "<-"))
                    {
                        beginRelationNodeBufer = node;
                        exitBufer = 1;
                    }
                    bufer = new Rect(nodeTransform.x + 31, nodeTransform.y + 21, 130, 20);
                    node.Condition = (ConditionPack)EditorGUI.ObjectField(bufer, node.Condition, typeof(ConditionPack), allowSceneObjects: true);
                    if (node.Condition != null)
                    {
                        bufer = new Rect(nodeTransform.x + 31, nodeTransform.y + 42, 82, 20);
                        node.ConditionNumber = EditorGUI.Popup(bufer, node.ConditionNumber, node.Condition.GetCharacteristic());
                        bufer = new Rect(bufer.x + 81, bufer.y, 35, 20);
                        if (node.Condition.conditions[node.ConditionNumber].type == ConditionType.Bool)
                        {
                            node.CheckType = (CheckType)EditorGUI.Popup(bufer, (int)node.CheckType, new string[2] { "==", "!=" });
                            bufer = new Rect(bufer.x + 36, bufer.y, 20, 20);
                            node.CheckBoolValue = EditorGUI.Toggle(bufer, node.CheckBoolValue);
                        }
                        else
                        {
                            node.CheckType = (CheckType)EditorGUI.Popup(bufer, (int)node.CheckType, new string[4] { "==", "!=", ">", "<" });
                            bufer = new Rect(bufer.x + 36, bufer.y, 30, 15);
                            node.CheckIntValue = EditorGUI.IntField(bufer, node.CheckIntValue);
                        }
                    }
                }
                break;
        }
    }
    private void CreateNode()
    {
        sceneKit.nodes.Add(new DialogueNode(new Vector2(position.width / 2 + scrollPosition.x, position.height / 2 + scrollPosition.y), nodeType, sceneKit.nodes.Count));
    }
    private void AddRelation(DialogueNode node)
    {
        switch (beginRelationNodeBufer.Type)
        {
            case NodeType.Replica:
                if(beginRelationNodeBufer.NextNodeNumber != -1)
                {
                    sceneKit.RemoveFromNext(beginRelationNodeBufer, sceneKit.nodes[beginRelationNodeBufer.NextNodeNumber]);
                }
                beginRelationNodeBufer.NextNodeNumber = node.Index;
                sceneKit.AddInPreviousRelations(sceneKit.nodes[beginRelationNodeBufer.NextNodeNumber],beginRelationNodeBufer);
                break;
            case NodeType.Choice:
                if (sceneKit.nodes[beginRelationNodeBufer.AnswerChoice[exitBufer]].NextNodeNumber != -1)
                {
                    sceneKit.RemoveFromNext(beginRelationNodeBufer, sceneKit.nodes[beginRelationNodeBufer.AnswerChoice[exitBufer]]);
                }
                sceneKit.nodes[beginRelationNodeBufer.AnswerChoice[exitBufer]].NextNodeNumber = node.Index;
                sceneKit.AddInPreviousRelations(sceneKit.nodes[node.Index], sceneKit.nodes[beginRelationNodeBufer.AnswerChoice[exitBufer]]);
                break;
            case NodeType.Event:
                if (beginRelationNodeBufer.NextNodeNumber != -1)
                {
                    sceneKit.RemoveFromNext(beginRelationNodeBufer, sceneKit.nodes[beginRelationNodeBufer.NextNodeNumber]);
                }
                beginRelationNodeBufer.NextNodeNumber = node.Index;
                sceneKit.AddInPreviousRelations(sceneKit.nodes[beginRelationNodeBufer.NextNodeNumber], beginRelationNodeBufer);
                break;
            case NodeType.Condition:
                if (exitBufer == 0)
                {
                    if (beginRelationNodeBufer.PositiveNextNumber != -1)
                    {
                        sceneKit.ConditionRemoveFromNext(beginRelationNodeBufer, sceneKit.nodes[beginRelationNodeBufer.PositiveNextNumber], 0);
                    }
                    beginRelationNodeBufer.PositiveNextNumber = node.Index;
                    sceneKit.AddInPreviousRelations(sceneKit.nodes[beginRelationNodeBufer.PositiveNextNumber], beginRelationNodeBufer);
                }
                else
                {
                    if (beginRelationNodeBufer.NegativeNextNumber != -1)
                    {
                        sceneKit.ConditionRemoveFromNext(beginRelationNodeBufer, sceneKit.nodes[beginRelationNodeBufer.NegativeNextNumber], 1);
                    }
                    beginRelationNodeBufer.NegativeNextNumber = node.Index;
                    sceneKit.AddInPreviousRelations(sceneKit.nodes[beginRelationNodeBufer.NegativeNextNumber], beginRelationNodeBufer);
                }
                break;
        }
        beginRelationNodeBufer = null;
        exitBufer = 0;
    }
    private void DragNode()
    {
        if (debug)
        {
            EditorGUI.DrawRect(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 30, 30), Color.cyan);
        }
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            if (ClickInNode(Event.current.mousePosition, out drugNodeBufer))
            {
                dragNode = true;
            }
        }
        if (dragNode)
        {
            if (Event.current.type == EventType.MouseDrag && Event.current.button == 0 && drugNodeBufer != null)
            {
                Vector2 offset = Event.current.mousePosition - clickPoint;
                clickPoint += offset;
                buferRect = new Rect(drugNodeBufer.TransformRect.x + offset.x, drugNodeBufer.TransformRect.y + offset.y, drugNodeBufer.TransformRect.width,
                    drugNodeBufer.TransformRect.height);
                drugNodeBufer.TransformRect = buferRect;
            }
            if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && drugNodeBufer != null)
            {
                dragNode = false;
            }
        }
    }
    private void DebugWindow()
    {
        if (debug)
        {
            foreach (var item in sceneKit.nodes)
            {
                if(!item.Hide)
                {
                    EditorGUI.DrawRect(new Rect(item.TransformRect.x - scrollPosition.x, item.TransformRect.y + mainInfoYSize - scrollPosition.y,
                    item.TransformRect.width, item.TransformRect.height), Color.blue);
                }
            }
        }
    }
    private bool ClickInNode(Vector2 mousePos, out DialogueNode node)
    {
        node = null;
        for (int i = sceneKit.nodes.Count - 1; i >= 0; i--)
        {
            if(!sceneKit.nodes[i].Hide)
            {
                if (mousePos.x > sceneKit.nodes[i].TransformRect.x - scrollPosition.x && mousePos.x < sceneKit.nodes[i].TransformRect.x - scrollPosition.x
                + sceneKit.nodes[i].TransformRect.width &&
                mousePos.y > sceneKit.nodes[i].TransformRect.y - scrollPosition.y + mainInfoYSize && mousePos.y < sceneKit.nodes[i].TransformRect.y - scrollPosition.y
                + mainInfoYSize + sceneKit.nodes[i].TransformRect.height)
                {
                    node = sceneKit.nodes[i];
                    clickPoint = mousePos;
                    return true;
                }
            }
        }
        return false;
    }
    private bool ClickInNode(Vector2 mousePos)
    {
        for (int i = sceneKit.nodes.Count - 1; i >= 0; i--)
        {
            if(!sceneKit.nodes[i].Hide)
            {
                if (mousePos.x > sceneKit.nodes[i].TransformRect.x - scrollPosition.x && mousePos.x < sceneKit.nodes[i].TransformRect.x - scrollPosition.x
               + sceneKit.nodes[i].TransformRect.width &&
               mousePos.y > sceneKit.nodes[i].TransformRect.y - scrollPosition.y + mainInfoYSize && mousePos.y < sceneKit.nodes[i].TransformRect.y - scrollPosition.y
               + mainInfoYSize + sceneKit.nodes[i].TransformRect.height)
                {
                    clickPoint = mousePos;
                    return true;
                }
            }
        }
        return false;
    }
    private Rect GetScrollViewZone()
    {
        Rect rezult = new Rect(scrollViewRect.x, scrollViewRect.y, scrollViewRect.width, scrollViewRect.height);
        float maxX, maxY;
        maxX = maxY = 0;

        foreach (var item in sceneKit.nodes)
        {
            if (maxX < item.TransformRect.x + item.TransformRect.width)
            {
                maxX = item.TransformRect.x + item.TransformRect.width;
            }
            if (maxY < item.TransformRect.y + mainInfoYSize + item.TransformRect.height)
            {
                maxY = item.TransformRect.y + mainInfoYSize + item.TransformRect.height;
            }
        }

        if (maxX > rezult.x + rezult.width)
        {
            rezult = new Rect(rezult.x, rezult.y, rezult.width + 200, rezult.height);
        }
        else if(maxX < rezult.x + rezult.width - 200)
        {
            rezult = new Rect(rezult.x, rezult.y, rezult.width - 200, rezult.height);
        }
        if (maxY > rezult.y + rezult.height)
        {
            rezult = new Rect(rezult.x, rezult.y, rezult.width, rezult.height + 200);
        }
        else if (maxY < rezult.y + rezult.height - 200)
        {
            rezult = new Rect(rezult.x, rezult.y, rezult.width, rezult.height - 200);
        }
        return rezult;
    }
    private void MouseScroll()
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            if (!ClickInNode(Event.current.mousePosition))
            {
                mouseScroll = true;
                clickPoint = Event.current.mousePosition;
            }
        }
        if (mouseScroll)
        {
            Vector2 offset = Event.current.mousePosition - clickPoint;
            clickPoint = Event.current.mousePosition;
            scrollPosition += offset;
        }
        if (Event.current.type == EventType.MouseUp && Event.current.button == 1)
        {
            mouseScroll = false;
        }
    }

    #endregion

}
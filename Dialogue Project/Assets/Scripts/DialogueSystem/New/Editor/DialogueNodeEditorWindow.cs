using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#region Вариант с наследованием

//public class DialogueNodeEditorWindow : EditorWindow
//{
//    public DialogueSceneKit kit;
//    public DialogueNode node;
//    private Rect bufer;
//    private Vector2 horizontalScrollPosition;
//    private Vector2 verticalScrollPosition;
//    private int selectedChekType;
//    private DialogueNodeEditorWindow nodeEditorWindow;

//    public static DialogueNodeEditorWindow GetNodeEditor()
//    {
//        return GetWindow<DialogueNodeEditorWindow>();
//    }
//    private void OnGUI()
//    {
//        if (node is Replica)
//        {
//            DrawReplica(node as Replica);
//        }
//        else if (node is Choice)
//        {
//            DrawChoice(node as Choice);
//        }
//        else
//        {
//            DrawDialogueEvent(node as DialogueEventContainer);
//        }
//    }

//    private void DrawReplica(Replica replica)
//    {
//        EditorGUILayout.BeginVertical();
//        verticalScrollPosition = EditorGUILayout.BeginScrollView(verticalScrollPosition);
//        DialogueCharacter characterBufer = replica.Character;
//        replica.Character = (DialogueCharacter)EditorGUILayout.ObjectField(replica.Character, typeof(DialogueCharacter), allowSceneObjects: true);
//        if (characterBufer != replica.Character)
//        {
//            EditorUtility.SetDirty(kit);
//        }

//        string text = replica.ReplicText;
//        replica.ReplicText = EditorGUILayout.TextArea(replica.ReplicText, GUILayout.MinHeight(50));
//        if (replica.ReplicText != null && !replica.ReplicText.Equals(text))
//        {
//            EditorUtility.SetDirty(kit);
//        }

//        AudioClip clip = replica.Clip;
//        replica.Clip = (AudioClip)EditorGUILayout.ObjectField(replica.Clip, typeof(AudioClip), allowSceneObjects: true);
//        if (clip != replica.Clip)
//        {
//            EditorUtility.SetDirty(kit);
//        }

//        DialogueAnimType type = replica.AnimType;
//        replica.AnimType = (DialogueAnimType)EditorGUILayout.EnumPopup(replica.AnimType, GUILayout.MinWidth(80), GUILayout.MinHeight(20));
//        if (type != replica.AnimType)
//        {
//            EditorUtility.SetDirty(kit);
//        }

//        EditorGUILayout.BeginHorizontal();
//        EditorGUILayout.LabelField("Ракурс № ");
//        int number = replica.CamPositionNumber;
//        replica.CamPositionNumber = EditorGUILayout.IntField(replica.CamPositionNumber, GUILayout.MinWidth(40));
//        if (number != replica.CamPositionNumber)
//        {
//            EditorUtility.SetDirty(this);
//        }
//        EditorGUILayout.EndHorizontal();
//        EditorGUILayout.EndScrollView();
//        EditorGUILayout.EndVertical();
//    }
//    private void DrawChoice(Choice choice)
//    {
//        EditorGUILayout.BeginVertical();
//        verticalScrollPosition = EditorGUILayout.BeginScrollView(verticalScrollPosition);
//        DialogueCharacter characterBufer = choice.Character;
//        choice.Character = (DialogueCharacter)EditorGUILayout.ObjectField(choice.Character, typeof(DialogueCharacter), allowSceneObjects: true);
//        if (characterBufer != choice.Character)
//        {
//            EditorUtility.SetDirty(kit);
//        }
//        EditorGUILayout.BeginHorizontal();
//        int number = choice.CamPositionNumber;
//        EditorGUILayout.LabelField("Ракурс № ");
//        choice.CamPositionNumber = EditorGUILayout.IntField(choice.CamPositionNumber, GUILayout.MinWidth(40));
//        if (number != choice.CamPositionNumber)
//        {
//            EditorUtility.SetDirty(this);
//        }
//        EditorGUILayout.EndHorizontal();
//        for (int i = 0; i < choice.AnswerChoice.Count; i++)
//        {
//            EditorGUILayout.BeginHorizontal();
//            if (GUILayout.Button("x"))
//            {
//                choice.RemoveAnsver(i);
//                EditorUtility.SetDirty(kit);
//                break;
//            }
//            choice.AnswerChoice[i].ReplicText = EditorGUILayout.TextField(choice.AnswerChoice[i].ReplicText);
//            if (GUILayout.Button("="))
//            {
//                nodeEditorWindow = CreateInstance<DialogueNodeEditorWindow>();
//                nodeEditorWindow.kit = kit;
//                nodeEditorWindow.Show();
//            }
//            EditorGUILayout.EndHorizontal();
//        }
//        EditorGUILayout.EndScrollView();
//        EditorGUILayout.EndVertical();
//    }
//    private void DrawDialogueEvent(DialogueEventContainer eventContainer)
//    {
//        EditorGUILayout.BeginVertical();
//        verticalScrollPosition = EditorGUILayout.BeginScrollView(verticalScrollPosition);
//        ConditionPack pack = eventContainer.ConditionCharacteristic;
//        eventContainer.ConditionCharacteristic = (ConditionPack)EditorGUILayout.ObjectField(eventContainer.ConditionCharacteristic,
//            typeof(ConditionPack), allowSceneObjects: true);
//        if (pack != eventContainer.ConditionCharacteristic)
//        {
//            EditorUtility.SetDirty(kit);
//        }

//        if (eventContainer.ConditionCharacteristic != null)
//        {
//            EditorGUILayout.BeginHorizontal();
//            EditorGUILayout.LabelField("Менять параметр");
//            bool toggleBufer = eventContainer.ChangeCondition;
//            eventContainer.ChangeCondition = EditorGUILayout.Toggle(eventContainer.ChangeCondition);
//            if (toggleBufer != eventContainer.ChangeCondition)
//            {
//                EditorUtility.SetDirty(kit);
//            }
//            EditorGUILayout.EndHorizontal();
//            if (eventContainer.ChangeCondition)
//            {
//                EditorGUILayout.BeginHorizontal();
//                int number = eventContainer.ChangeConditionNumber;
//                eventContainer.ChangeConditionNumber = EditorGUILayout.Popup(eventContainer.ChangeConditionNumber, eventContainer.ConditionCharacteristic.GetCharacteristic());
//                if (number != eventContainer.ChangeConditionNumber)
//                {
//                    EditorUtility.SetDirty(kit);
//                }

//                if (eventContainer.ConditionCharacteristic.conditions[eventContainer.ChangeConditionNumber].type == ConditionType.Bool)
//                {
//                    EditorGUILayout.LabelField("Значение после события");
//                    toggleBufer = eventContainer.ChangeBoolValue;
//                    eventContainer.ChangeBoolValue = EditorGUILayout.Toggle(eventContainer.ChangeBoolValue);
//                    if (toggleBufer != eventContainer.ChangeBoolValue)
//                    {
//                        EditorUtility.SetDirty(kit);
//                    }
//                }
//                else
//                {
//                    EditorGUILayout.LabelField("Сместить на");
//                    number = eventContainer.AddIntValue;
//                    eventContainer.AddIntValue = EditorGUILayout.IntField(eventContainer.AddIntValue);
//                    if (number != eventContainer.AddIntValue)
//                    {
//                        EditorUtility.SetDirty(kit);
//                    }
//                }
//                EditorGUILayout.EndHorizontal();

//            }
//        }


//        EditorGUILayout.BeginHorizontal();
//        EditorGUILayout.LabelField("Событие в игровой сцене");
//        bool toggle = eventContainer.InSceneInvoke;
//        eventContainer.InSceneInvoke = EditorGUILayout.Toggle(eventContainer.InSceneInvoke);
//        if (toggle != eventContainer.InSceneInvoke)
//        {
//            EditorUtility.SetDirty(kit);
//        }
//        EditorGUILayout.EndHorizontal();
//        if (eventContainer.InSceneInvoke)
//        {
//            horizontalScrollPosition = EditorGUILayout.BeginScrollView(horizontalScrollPosition, GUILayout.MaxHeight(100));
//            EditorGUILayout.BeginHorizontal();
//            for (int i = 0; i < eventContainer.ReactorsNumbers.Count; i++)
//            {
//                EditorGUILayout.BeginVertical();
//                int numberBufer = eventContainer.ReactorsNumbers[i];
//                eventContainer.ReactorsNumbers[i] = EditorGUILayout.IntField(eventContainer.ReactorsNumbers[i], GUILayout.MaxWidth(80));
//                if (numberBufer != eventContainer.ReactorsNumbers[i])
//                {
//                    EditorUtility.SetDirty(kit);
//                }
//                GUILayout.Space(3);
//                if (GUILayout.Button("x", GUILayout.MaxWidth(20)))
//                {
//                    eventContainer.ReactorsNumbers.Remove(eventContainer.ReactorsNumbers[i]);
//                    EditorUtility.SetDirty(kit);
//                    break;
//                }
//                EditorGUILayout.EndVertical();
//            }
//            GUILayout.Space(5);
//            if (GUILayout.Button("+", GUILayout.MaxWidth(20), GUILayout.MinWidth(20)))
//            {
//                eventContainer.ReactorsNumbers.Add(0);
//                EditorUtility.SetDirty(kit);
//            }
//            EditorGUILayout.EndHorizontal();
//            EditorGUILayout.EndScrollView();
//            EditorGUILayout.BeginHorizontal();
//            EditorGUILayout.LabelField("Ракурс № во время события");
//            int number = eventContainer.EventCamPositionNumber;
//            eventContainer.EventCamPositionNumber = EditorGUILayout.IntField(eventContainer.EventCamPositionNumber, GUILayout.MinWidth(40));
//            if (number != eventContainer.EventCamPositionNumber)
//            {
//                EditorUtility.SetDirty(kit);
//            }
//            EditorGUILayout.EndHorizontal();
//            EditorGUILayout.BeginHorizontal();
//            EditorGUILayout.LabelField("Время фокусировки на событии");
//            float time = eventContainer.EventTime;
//            eventContainer.EventTime = EditorGUILayout.Slider(eventContainer.EventTime, 0, 120);
//            if (time != eventContainer.EventTime)
//            {
//                EditorUtility.SetDirty(kit);
//            }
//            EditorGUILayout.EndHorizontal();
//        }

//        EditorGUILayout.BeginHorizontal();
//        EditorGUILayout.LabelField("Сообщение при событии");
//        toggle = eventContainer.IsMessage;
//        eventContainer.IsMessage = EditorGUILayout.Toggle(eventContainer.IsMessage);
//        if (toggle != eventContainer.IsMessage)
//        {
//            EditorUtility.SetDirty(kit);
//        }
//        EditorGUILayout.EndHorizontal();
//        if (eventContainer.IsMessage)
//        {
//            string text = eventContainer.MessageText;
//            eventContainer.MessageText = EditorGUILayout.TextArea(eventContainer.MessageText);
//            if (text != null && !text.Equals(eventContainer.MessageText))
//            {
//                EditorUtility.SetDirty(kit);
//            }
//        }
//        EditorGUILayout.EndScrollView();
//        EditorGUILayout.EndVertical();
//    }
//}


#endregion
public class DialogueNodeEditorWindow : EditorWindow
{
    public DialogueSceneKit kit;
    public DialogueNode node;
    private Rect bufer;
    private Vector2 horizontalScrollPosition;
    private Vector2 verticalScrollPosition;
    private int selectedChekType;
    private DialogueNodeEditorWindow nodeEditorWindow;

    public static DialogueNodeEditorWindow GetNodeEditor()
    {
        return GetWindow<DialogueNodeEditorWindow>();
    }
    private void OnGUI()
    {
        switch(node.Type)
        {
            case NodeType.Replica:
                DrawReplica(node);
                break;
            case NodeType.Choice:
                DrawChoice(node);
                break;
            case NodeType.Event:
                DrawDialogueEvent(node);
                break;
        }
    }

    private void DrawReplica(DialogueNode replica)
    {
        EditorGUILayout.BeginVertical();
        verticalScrollPosition = EditorGUILayout.BeginScrollView(verticalScrollPosition);
        DialogueCharacter characterBufer = replica.Character;
        replica.Character = (DialogueCharacter)EditorGUILayout.ObjectField(replica.Character, typeof(DialogueCharacter), allowSceneObjects: true);
        if (characterBufer != replica.Character)
        {
            EditorUtility.SetDirty(kit);
        }

        string text = replica.ReplicText;
        replica.ReplicText = EditorGUILayout.TextArea(replica.ReplicText, GUILayout.MinHeight(50), GUILayout.MaxWidth(position.width - 10));
        if (replica.ReplicText != null && !replica.ReplicText.Equals(text))
        {
            EditorUtility.SetDirty(kit);
        }

        AudioClip clip = replica.Clip;
        replica.Clip = (AudioClip)EditorGUILayout.ObjectField(replica.Clip, typeof(AudioClip), allowSceneObjects: true);
        if (clip != replica.Clip)
        {
            EditorUtility.SetDirty(kit);
        }

        DialogueAnimType type = replica.AnimType;
        replica.AnimType = (DialogueAnimType)EditorGUILayout.EnumPopup(replica.AnimType, GUILayout.MinWidth(80), GUILayout.MinHeight(20));
        if (type != replica.AnimType)
        {
            EditorUtility.SetDirty(kit);
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Ракурс № ");
        int number = replica.CamPositionNumber;
        replica.CamPositionNumber = EditorGUILayout.IntField(replica.CamPositionNumber, GUILayout.MinWidth(40));
        if (number != replica.CamPositionNumber)
        {
            EditorUtility.SetDirty(this);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
    private void DrawChoice(DialogueNode choice)
    {
        EditorGUILayout.BeginVertical();
        verticalScrollPosition = EditorGUILayout.BeginScrollView(verticalScrollPosition);
        DialogueCharacter characterBufer = choice.Character;
        choice.Character = (DialogueCharacter)EditorGUILayout.ObjectField(choice.Character, typeof(DialogueCharacter), allowSceneObjects: true);
        if (characterBufer != choice.Character)
        {
            EditorUtility.SetDirty(kit);
        }
        EditorGUILayout.BeginHorizontal();
        int number = choice.CamPositionNumber;
        EditorGUILayout.LabelField("Ракурс № ");
        choice.CamPositionNumber = EditorGUILayout.IntField(choice.CamPositionNumber, GUILayout.MinWidth(40));
        if (number != choice.CamPositionNumber)
        {
            EditorUtility.SetDirty(this);
        }
        EditorGUILayout.EndHorizontal();
        for (int i = 0; i < choice.AnswerChoice.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("x"))
            {
                kit.Remove(kit.nodes[choice.AnswerChoice[i]]);
                choice.RemoveAnsver(i);
                EditorUtility.SetDirty(kit);
                break;
            }
            kit.nodes[choice.AnswerChoice[i]].ReplicText = EditorGUILayout.TextField(kit.nodes[choice.AnswerChoice[i]].ReplicText);
            if (GUILayout.Button("="))
            {
                nodeEditorWindow = CreateInstance<DialogueNodeEditorWindow>();
                nodeEditorWindow.kit = kit;
                nodeEditorWindow.node = kit.nodes[choice.AnswerChoice[i]];
                nodeEditorWindow.Show();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
    private void DrawDialogueEvent(DialogueNode eventContainer)
    {
        EditorGUILayout.BeginVertical();
        verticalScrollPosition = EditorGUILayout.BeginScrollView(verticalScrollPosition);
        ConditionPack pack = eventContainer.Condition;
        eventContainer.Condition = (ConditionPack)EditorGUILayout.ObjectField(eventContainer.Condition,
            typeof(ConditionPack), allowSceneObjects: true);
        if (pack != eventContainer.Condition)
        {
            EditorUtility.SetDirty(kit);
        }

        if (eventContainer.Condition != null)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Менять параметр");
            bool toggleBufer = eventContainer.ChangeCondition;
            eventContainer.ChangeCondition = EditorGUILayout.Toggle(eventContainer.ChangeCondition);
            if (toggleBufer != eventContainer.ChangeCondition)
            {
                EditorUtility.SetDirty(kit);
            }
            EditorGUILayout.EndHorizontal();
            if (eventContainer.ChangeCondition)
            {
                EditorGUILayout.BeginHorizontal();
                int number = eventContainer.ConditionNumber;
                eventContainer.ConditionNumber = EditorGUILayout.Popup(eventContainer.ConditionNumber, eventContainer.Condition.GetCharacteristic());
                if (number != eventContainer.ConditionNumber)
                {
                    EditorUtility.SetDirty(kit);
                }

                if (eventContainer.Condition.conditions[eventContainer.ConditionNumber].type == ConditionType.Bool)
                {
                    EditorGUILayout.LabelField("Значение после события");
                    toggleBufer = eventContainer.ChangeBoolValue;
                    eventContainer.ChangeBoolValue = EditorGUILayout.Toggle(eventContainer.ChangeBoolValue);
                    if (toggleBufer != eventContainer.ChangeBoolValue)
                    {
                        EditorUtility.SetDirty(kit);
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("Сместить на");
                    number = eventContainer.AddIntValue;
                    eventContainer.AddIntValue = EditorGUILayout.IntField(eventContainer.AddIntValue);
                    if (number != eventContainer.AddIntValue)
                    {
                        EditorUtility.SetDirty(kit);
                    }
                }
                EditorGUILayout.EndHorizontal();

            }
        }


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Событие в игровой сцене");
        bool toggle = eventContainer.InSceneInvoke;
        eventContainer.InSceneInvoke = EditorGUILayout.Toggle(eventContainer.InSceneInvoke);
        if (toggle != eventContainer.InSceneInvoke)
        {
            EditorUtility.SetDirty(kit);
        }
        EditorGUILayout.EndHorizontal();
        if (eventContainer.InSceneInvoke)
        {
            horizontalScrollPosition = EditorGUILayout.BeginScrollView(horizontalScrollPosition, GUILayout.MaxHeight(100));
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < eventContainer.ReactorsNumbers.Count; i++)
            {
                EditorGUILayout.BeginVertical();
                int numberBufer = eventContainer.ReactorsNumbers[i];
                eventContainer.ReactorsNumbers[i] = EditorGUILayout.IntField(eventContainer.ReactorsNumbers[i], GUILayout.MaxWidth(80));
                if (numberBufer != eventContainer.ReactorsNumbers[i])
                {
                    EditorUtility.SetDirty(kit);
                }
                GUILayout.Space(3);
                if (GUILayout.Button("x", GUILayout.MaxWidth(20)))
                {
                    eventContainer.ReactorsNumbers.Remove(eventContainer.ReactorsNumbers[i]);
                    EditorUtility.SetDirty(kit);
                    break;
                }
                EditorGUILayout.EndVertical();
            }
            GUILayout.Space(5);
            if (GUILayout.Button("+", GUILayout.MaxWidth(20), GUILayout.MinWidth(20)))
            {
                eventContainer.ReactorsNumbers.Add(0);
                EditorUtility.SetDirty(kit);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Ракурс № во время события");
            int number = eventContainer.EventCamPositionNumber;
            eventContainer.EventCamPositionNumber = EditorGUILayout.IntField(eventContainer.EventCamPositionNumber, GUILayout.MinWidth(40));
            if (number != eventContainer.EventCamPositionNumber)
            {
                EditorUtility.SetDirty(kit);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Время фокусировки на событии");
            float time = eventContainer.EventTime;
            eventContainer.EventTime = EditorGUILayout.Slider(eventContainer.EventTime, 0, 120);
            if (time != eventContainer.EventTime)
            {
                EditorUtility.SetDirty(kit);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Сообщение при событии");
        toggle = eventContainer.IsMessage;
        eventContainer.IsMessage = EditorGUILayout.Toggle(eventContainer.IsMessage);
        if (toggle != eventContainer.IsMessage)
        {
            EditorUtility.SetDirty(kit);
        }
        EditorGUILayout.EndHorizontal();
        if (eventContainer.IsMessage)
        {
            string text = eventContainer.MessageText;
            eventContainer.MessageText = EditorGUILayout.TextArea(eventContainer.MessageText, GUILayout.MaxWidth(position.width-10));
            if (text != null && !text.Equals(eventContainer.MessageText))
            {
                EditorUtility.SetDirty(kit);
            }
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}

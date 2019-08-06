using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestUndo : EditorWindow
{
    public DialogueSceneKit kit;


    [MenuItem("Window/IgoGoTools/UndoTest")]
    public static void GetWindow()
    {
        GetWindow<TestUndo>();
    }

    public static TestUndo GetEditor()
    {
        return GetWindow<TestUndo>();
    }
    
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        kit = (DialogueSceneKit)EditorGUILayout.ObjectField(kit, typeof(DialogueSceneKit), false);
        if(kit != null)
        {
            Undo.RecordObject(kit, "Добавить параметр");
            if (GUILayout.Button("Добавить"))
            {
                //kit.nodes.Add(new DialogueNode(NodeType.Replica));
            }
            foreach (var item in kit.nodes)
            {
                item.ReplicText = GUILayout.TextField(item.ReplicText);
            }
        }
        EditorGUILayout.EndVertical();
    }
}

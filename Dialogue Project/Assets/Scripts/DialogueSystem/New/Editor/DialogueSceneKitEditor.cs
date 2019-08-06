using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueSceneKit))] 
public class DialogueSceneKitEditor : Editor 
{
    private DialogueSceneKit sceneKit;

    private void OnEnable()
    {
        sceneKit = (DialogueSceneKit)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical();
        sceneKit.sceneName=  GUILayout.TextField(sceneKit.sceneName);
        GUILayout.Label("Количество узлов: " + sceneKit.nodes.Count);
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Редактировать", GUILayout.MinWidth(80)))
        {
            DialogueSceneEditor sceneEditor = DialogueSceneEditor.GetEditor();
            sceneEditor.sceneKit = sceneKit;
            sceneEditor.minSize = new Vector2(400, 300);
            sceneEditor.Show();
        }
        if (GUILayout.Button("Сохранить", GUILayout.MinWidth(80)))
        {
            EditorUtility.SetDirty(sceneKit);
        }
        GUILayout.Space(80);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
}


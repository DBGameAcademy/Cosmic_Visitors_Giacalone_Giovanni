using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelScriptable))]
public class LevelScriptableEditor : Editor
{
    private LevelScriptable level;
    GUIContent guiMessage = new GUIContent("Create the level, each row represents a level row. \r\nUse - to create a blank space and # to place an alien");

    private void OnEnable()
    {
        level = (LevelScriptable)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.HelpBox(guiMessage);
        
        level.Row1 = EditorGUILayout.TextField("ROW 1", level.Row1);
        level.Row2 = EditorGUILayout.TextField("ROW 2", level.Row2);
        level.Row3 = EditorGUILayout.TextField("ROW 3", level.Row3);
        level.Row4 = EditorGUILayout.TextField("ROW 4", level.Row4);
        level.Row5 = EditorGUILayout.TextField("ROW 5", level.Row5);

        DrawLabelField("ROW1 Characters 13/ ", level.Row1);
        DrawLabelField("ROW2 Characters 13/ ", level.Row2);
        DrawLabelField("ROW3 Characters 13/ ", level.Row3);
        DrawLabelField("ROW4 Characters 13/ ", level.Row4);
        DrawLabelField("ROW5 Characters 13/ ", level.Row5);

        level.moveDelay = EditorGUILayout.Slider("Move Delay", level.moveDelay, 3, .8f);

        EditorUtility.SetDirty(level);
    }

    private void DrawLabelField(string label, string rowData)
    {
        if (string.IsNullOrEmpty(rowData))
            return;

        EditorGUILayout.LabelField(label, rowData.Length.ToString());
    }

    private void DisplayErrorMessage(int _nChar)
    {
        GUIStyle errorGUIStyle = new GUIStyle();
        errorGUIStyle.normal.textColor = Color.red;

        if (_nChar != 13)
        {
            EditorGUILayout.LabelField("PUT EXACTLY 13 CHARACTERS!!!", errorGUIStyle);
        }
    }
}

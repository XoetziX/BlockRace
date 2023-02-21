using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerDataSO))]
public class PlayerDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerDataSO playerDataSO = (PlayerDataSO)target;

        GUILayout.Label("Easy Level Passed: ");

        if (playerDataSO.EasyLevelPassed == null || playerDataSO.EasyLevelPassed.Count == 0)
        {
            EditorGUILayout.LabelField("No Easy Level Passed", GUILayout.Width(150));
        }
        foreach (LevelPassed levelPassed in playerDataSO.EasyLevelPassed)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Main-Level: ", GUILayout.Width(80));
            EditorGUILayout.TextField(levelPassed.MainLevel);
            EditorGUILayout.EndHorizontal();

            if (levelPassed.SubLevel == null || levelPassed.SubLevel.Count == 0)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("No Sub Level Passed");
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.BeginVertical();
                for (int i = 0; i < levelPassed.SubLevel.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Sub-Level: ", GUILayout.Width(100));
                    EditorGUILayout.TextField(levelPassed.SubLevel[i]);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
        }

    }
}

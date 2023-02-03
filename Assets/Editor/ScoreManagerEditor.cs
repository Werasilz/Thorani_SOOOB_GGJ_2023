using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScoreManager))]
public class ScoreManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ScoreManager scoreManager = (ScoreManager)target;

        if (GUILayout.Button("Add Score To P1"))
        {
            scoreManager.AddPlayerScore(0, 1);
        }
        if (GUILayout.Button("Add Score To P2"))
        {
            scoreManager.AddPlayerScore(1, 1);
        }

        base.OnInspectorGUI();
    }
}
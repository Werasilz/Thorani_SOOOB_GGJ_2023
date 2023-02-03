using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbilityController))]
public class AbilityControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AbilityController abilityController = (AbilityController)target;


        if (GUILayout.Button("Enable All Skill"))
        {
            abilityController.abilityUIManager.abilityUIs[0].EnableSkill();
            abilityController.abilityUIManager.abilityUIs[1].EnableSkill();
            abilityController.abilityUIManager.abilityUIs[2].EnableSkill();
        }

        if (GUILayout.Button("Use Skill A"))
        {
            abilityController.CastSkill(0);
        }
        if (GUILayout.Button("Use Skill B"))
        {
            abilityController.CastSkill(1);
        }
        if (GUILayout.Button("Use Skill C"))
        {
            abilityController.CastSkill(2);
        }

        base.OnInspectorGUI();
    }
}

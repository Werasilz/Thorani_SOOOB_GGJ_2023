using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityUIManager : MonoBehaviour
{
    public AbilityUI[] abilityUIs;
}

[System.Serializable]
public class AbilityUI
{
    public Image skill_black;
    public Image skill_fill;
    public Image skill_lock;
    public TextMeshProUGUI cooldownText;

    public void ShowCooldown(float _maxCooldown, float _currentCooldown)
    {
        if (!skill_black.enabled)
        {
            skill_black.enabled = true;
            cooldownText.enabled = true;
            skill_fill.enabled = true;
        }

        cooldownText.text = _currentCooldown.ToString("F0");
        skill_fill.fillAmount = _currentCooldown / _maxCooldown;
    }

    public void EnableSkill()
    {
        skill_lock.enabled = false;
        skill_black.enabled = false;
        skill_fill.enabled = false;
        cooldownText.enabled = false;
    }
}

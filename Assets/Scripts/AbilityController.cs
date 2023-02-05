using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    PlayerController playerController => GetComponent<PlayerController>();

    public float maxMP = 100;
    public float currentMP;
    public float regenMPPerSecond = 2f;

    public AbilityUIManager abilityUIManager;
    public AbilitySetting[] abilitys;

    void Start()
    {
        currentMP = maxMP;

        foreach (AbilitySetting ability in abilitys)
        {
            ability.countCooldown = ability.cooldown;
        }
    }

    public void CastSkill(int _skillIndex)
    {
        if (PlayerManager.instance.gameState != GameState.GameplayState)
            return;

        if (!abilitys[_skillIndex].isUnlock)
            return;

        if (currentMP > abilitys[_skillIndex].MPCost)
        {
            if (abilitys[_skillIndex].isCooldown == false)
            {
                currentMP -= abilitys[_skillIndex].MPCost;

                StartCoroutine(abilitys[_skillIndex].CooldownCounting(abilityUIManager.abilityUIs[_skillIndex]));

                // Cast Skill Here
                switch (_skillIndex)
                {
                    case 0:
                        playerController.SkillA();
                        break;
                    case 1:
                        playerController.SkillX();
                        break;
                    case 2:
                        playerController.SkillY();
                        break;
                }
            }
        }
    }
}


[System.Serializable]
public class AbilitySetting
{
    public bool isUnlock = false;
    public float cooldown = 10f;
    public float countCooldown;
    public bool isCooldown;
    public float MPCost = 0;

    public IEnumerator CooldownCounting(AbilityUI _abilityUI)
    {
        countCooldown = cooldown;
        isCooldown = true;

        while (countCooldown > 0)
        {
            countCooldown -= Time.deltaTime;

            _abilityUI.ShowCooldown(cooldown, countCooldown);

            if (countCooldown <= 0)
            {
                isCooldown = false;
                _abilityUI.EnableSkillIcon();
            }

            yield return null;
        }
    }
}

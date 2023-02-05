using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrapArea : MonoBehaviour
{
    [Header("Owner")]
    public PlayerInput playerInput;

    public GameObject trapPrefab;

    public List<Transform> trapSpawnPoints;

    public float duration;
    public int spawnTrapLimit;
    int countSpawnTrap;

    public List<TrapSkillY> trapSkillYs;

    public float spawnRange;

    // Start is called before the first frame update
    public void StartSpawnTrap()
    {
        StartCoroutine(CreateTrap());
        StartCoroutine(DestroyArea());
    }

    IEnumerator CreateTrap()
    {
        countSpawnTrap = 0;

        // Set color
        MeshRenderer[] rootRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (var renderer in rootRenderers)
        {
            renderer.material = PlayerManager.instance.rootLineColorMaterials[PlayerManager.instance.playerSelectIndex[playerInput.playerIndex]];
        }

        while (countSpawnTrap < spawnTrapLimit)
        {
            int spawnIndex = Random.Range(0, trapSpawnPoints.Count);

            Vector3 randomPoint = trapSpawnPoints[spawnIndex].position;
            trapSpawnPoints.RemoveAt(spawnIndex);

            GameObject trapClone = Instantiate(trapPrefab);
            trapClone.transform.position = randomPoint;

            TrapSkillY trapSkillY = trapClone.GetComponent<TrapSkillY>();
            trapSkillY.playerInput = playerInput;
            trapSkillYs.Add(trapSkillY);
            countSpawnTrap++;

            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator DestroyArea()
    {
        yield return new WaitForSeconds(duration);

        for (int i = 0; i < trapSkillYs.Count; i++)
        {
            if (trapSkillYs[i] == null) continue;

            if (trapSkillYs[i].target == null)
            {
                trapSkillYs[i].finalModel.GetComponentInChildren<Animator>().SetTrigger("goDown");
                trapSkillYs[i].GetComponent<Collider>().enabled = false;
                Destroy(trapSkillYs[i], 1f);
            }
        }

        GetComponent<Animator>().SetTrigger("Deactivate");

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }
}

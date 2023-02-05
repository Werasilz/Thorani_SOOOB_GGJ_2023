using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;


    public float spawnRange;
    public List<GameObject> enemies;

    public int maxEnemyInMap;

    public Vector2Int[] minMaxSPawnPerWave;

    public float waveRange;

    public bool spawning;

    private void Start()
    {
        StartCoroutine(StartSpawnEnemy());
    }
    IEnumerator StartSpawnEnemy()
    {
        int wave = 0;

        while (true)
        {
            yield return new WaitForSeconds(waveRange);

            if (spawning)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i] == null)
                    {
                        enemies.RemoveAt(i);
                        i--;
                    }
                }

                if (enemies.Count < maxEnemyInMap)
                {
                    if (wave >= minMaxSPawnPerWave.Length)
                        wave = minMaxSPawnPerWave.Length - 1;

                    int countToSpawn = Random.Range(minMaxSPawnPerWave[wave].x, minMaxSPawnPerWave[wave].y);

                    for (int i = 0; i < countToSpawn; i++)
                    {
                        if (enemies.Count >= maxEnemyInMap) break;

                        CreateEnemy();
                    }

                    wave++;
                }
            }
        }
    }

    void CreateEnemy()
    {
        Quaternion q = Quaternion.identity;
        q.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);

        Vector3 randomSpawnPoint = new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));

        GameObject enemyClone = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], transform.position + randomSpawnPoint, q);
        enemyClone.transform.parent = PlayerManager.instance.enemyGroup.transform;

        EnemyController enemy = enemyClone.GetComponent<EnemyController>();
        enemy.EnableAgent(false);
        enemies.Add(enemyClone);
    }
}

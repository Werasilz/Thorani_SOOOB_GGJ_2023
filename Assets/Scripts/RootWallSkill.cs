using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootWallSkill : MonoBehaviour
{
    [Header("Spawn Settings")]
    private List<Vector3> spawnPos;
    [SerializeField] private Vector2 randomPosX;
    [SerializeField] private int spawnAmount;
    [SerializeField] private float offsetLength;
    [SerializeField] private float spawnDelay;
    [SerializeField] private float reverseDelay;

    [Header("Prefabs")]
    [SerializeField] private GameObject rootLinePrefab;

    [Header("Animations")]
    public List<Animation> animations;
    [SerializeField] private AnimationClip normalRootLine;
    [SerializeField] private AnimationClip reverseRootLine;

    [Header("Debug")]
    [SerializeField] private bool showGizmos;

    public void StartSpawnRootWall()
    {
        StartCoroutine(SpawnRoot());
    }

    IEnumerator SpawnRoot()
    {
        spawnPos = new List<Vector3>();
        animations = new List<Animation>();

        // Random position
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 newPos = new Vector3(transform.localPosition.x + (Random.Range(randomPosX.x, randomPosX.y)), transform.localPosition.y, (transform.localPosition.z + i) * offsetLength);
            spawnPos.Add(newPos);
        }

        for (int i = 0; i < spawnPos.Count; i++)
        {
            // Spawn root
            GameObject newRoot = Instantiate(rootLinePrefab, transform);

            // Enable and set position
            newRoot.SetActive(true);
            newRoot.transform.localPosition = spawnPos[i];

            // Store to array
            animations.Add(newRoot.GetComponentInChildren<Animation>());

            yield return new WaitForSeconds(spawnDelay);
        }

        yield return new WaitForSeconds(reverseDelay);

        StartCoroutine(ReverseRoot());
    }

    IEnumerator ReverseRoot()
    {
        for (int i = animations.Count - 1; i >= 0; i--)
        {
            if (animations[i] == null)
                continue;

            // Set reverse animation
            animations[i].clip = reverseRootLine;
            animations[i].Play();

            yield return new WaitForSeconds(spawnDelay);
        }

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        if (!showGizmos)
            return;

        Gizmos.color = Color.green;

        for (int i = 0; i < spawnPos.Count; i++)
        {
            Gizmos.DrawWireSphere(spawnPos[i], 0.25f);
        }
    }
}

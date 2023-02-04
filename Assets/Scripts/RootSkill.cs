using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RootSkill : MonoBehaviour
{
    [Header("Owner")]
    public PlayerInput playerInput;

    [Header("Spawn Settings")]
    private List<Vector3> spawnPos;
    [SerializeField] private Vector2 randomPosX;
    [SerializeField] private int spawnAmount;
    [SerializeField] private float offsetLength;
    [SerializeField] private float spawnDelay;
    [SerializeField] private float reverseDelay;

    [Header("Collider")]
    public Collider rootSpinCollider;

    [Header("Prefabs")]
    [SerializeField] private GameObject rootLinePrefab;
    [SerializeField] private GameObject rootSpinPrefab;

    [Header("Animations")]
    public List<Animation> animations;
    [SerializeField] private AnimationClip normalRootLine;
    [SerializeField] private AnimationClip reverseRootLine;
    [SerializeField] private AnimationClip normalRootSpin;
    [SerializeField] private AnimationClip reverseRootSpin;

    [Header("Debug")]
    [SerializeField] private bool showGizmos;

    public void StartSpawnRoot()
    {
        StartCoroutine(SpawnRoot());
    }

    public void StartReverseRoot()
    {
        StartCoroutine(ReverseRoot());
    }

    public void SetActiveCollider(bool value)
    {
        rootSpinCollider.enabled = value;
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
            GameObject newRoot;

            // Spawn root line
            if (i < spawnPos.Count - 1)
            {
                newRoot = Instantiate(rootLinePrefab, transform);
            }
            // Spawn root spin at last index
            else
            {
                newRoot = Instantiate(rootSpinPrefab, transform);
                rootSpinCollider = newRoot.GetComponent<Collider>();
            }

            // Enable and set position
            newRoot.SetActive(true);
            newRoot.transform.localPosition = spawnPos[i];

            // Store to array
            animations.Add(newRoot.GetComponentInChildren<Animation>());

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator ReverseRoot()
    {
        SetActiveCollider(false);

        gameObject.transform.parent = null;

        for (int i = animations.Count - 1; i >= 0; i--)
        {
            if (animations[i] == null)
                continue;

            // Set reverse animation at last index
            if (i == spawnPos.Count - 1)
            {
                animations[i].clip = reverseRootSpin;
            }
            // Set reverse animation
            else
            {
                animations[i].clip = reverseRootLine;
            }

            animations[i].Play();

            yield return new WaitForSeconds(spawnDelay);
        }

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }

    public void ReverseSingleRootLine(Animation animation)
    {
        animation.clip = reverseRootLine;
        animation.Play();

        Destroy(animation.transform.parent.gameObject, 1);
    }

    // Pull enemy down
    public void ReverseSingleRootSpin(Animation animation)
    {
        animation.clip = reverseRootSpin;
        animation.Play();

        PlayerManager.instance.AddPlayerScore(playerInput, 1);

        Destroy(animation.transform.parent.gameObject, 1);
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

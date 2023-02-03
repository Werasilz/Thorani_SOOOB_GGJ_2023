using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float maxHP;
    public float currentHP;
    public float radius;
    public bool playerInArea;

    public float randomWanderTime = 1f;
    public float reWanderTime = 2f;
    NavMeshAgent agent;

    private bool wandering = true;
    private Coroutine ReWanderIE;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(Wander());
    }

    private void Update()
    {
        FindPlayer();
    }

    IEnumerator Wander()
    {
        while (true)
        {
            if (wandering)
            {
                agent.SetDestination(transform.position + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius)));
            }
            yield return new WaitForSeconds(randomWanderTime);
        }
    }

    IEnumerator ReWander()
    {
        yield return new WaitForSeconds(reWanderTime);
        wandering = true;
    }

    public void FindPlayer()
    {
        Collider[] playerObjects = Physics.OverlapSphere(transform.position, radius);

        int playerCount = 0;
        Vector3 pointToRunAway = Vector3.zero;

        foreach (var playerObject in playerObjects)
        {
            if (playerObject.gameObject.CompareTag("Player"))
            {
                playerCount++;
                pointToRunAway += new Vector3(playerObject.transform.position.x, 0, playerObject.transform.position.z);
            }
        }

        if (playerCount > 0)
        {
            playerInArea = true;

            pointToRunAway = ((pointToRunAway / playerCount) - transform.position).normalized;

            agent.SetDestination(transform.position - pointToRunAway);

            wandering = false;
        }
        else
        {
            if (playerInArea)
            {
                if (ReWanderIE != null)
                {
                    StopCoroutine(ReWanderIE);
                }
                ReWanderIE = StartCoroutine(ReWander());
            }

            playerInArea = false;
        }
    }
}
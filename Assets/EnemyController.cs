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

    private float countReWanderTime = 0f;

    NavMeshAgent agent;

    private bool wandering = true;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(Wander());
    }

    private void FixedUpdate()
    {
        FindPlayer();
    }

    IEnumerator Wander()
    {
        while (true)
        {
            if (wandering)
            {
                int ranX = Random.Range(0, 2);
                ranX = ranX == 1 ? 1 : -1;
                int ranZ = Random.Range(0, 2);
                ranZ = ranZ == 1 ? 1 : -1;

                agent.SetDestination(transform.position + new Vector3(radius * ranX, 0, radius * ranZ));
            }
            yield return new WaitForSeconds(randomWanderTime);
        }
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

            if (countReWanderTime < reWanderTime)
            {
                countReWanderTime = reWanderTime;
            }
        }
        else
        {
            playerInArea = false;

            if (countReWanderTime > 0)
            {
                countReWanderTime -= Time.deltaTime;

                if (countReWanderTime <= 0)
                {
                    countReWanderTime = 0;
                    wandering = true;
                }
            }
        }
    }
}

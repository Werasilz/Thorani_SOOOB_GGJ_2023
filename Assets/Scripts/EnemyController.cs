using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public EnemySkinController enemySkinController;

    public float maxHP;
    public float currentHP;
    public float radius;
    public bool playerInArea;

    public float randomWanderTime = 1f;
    public float reWanderTime = 2f;

    private float countReWanderTime = 0f;

    [HideInInspector] public NavMeshAgent agent;

    private bool wandering = true;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(Wander());
    }

    private void Start()
    {
        enemySkinController.RandomSkinAndEquipment();
    }

    private void FixedUpdate()
    {
        FindPlayer();
    }

    public void EnableAgent(bool _enable)
    {
        agent.enabled = _enable;
        agent.GetComponent<Rigidbody>().isKinematic = _enable;
    }

    public void GetCatch()
    {
        agent.enabled = false;
    }

    IEnumerator Wander()
    {
        while (true)
        {
            yield return new WaitForSeconds(randomWanderTime);
            SetWander();
        }
    }

    public void SetWander()
    {
        if (wandering && agent.enabled)
        {
            int ranX = Random.Range(0, 2);
            ranX = ranX == 1 ? 1 : -1;
            int ranZ = Random.Range(0, 2);
            ranZ = ranZ == 1 ? 1 : -1;

            agent.SetDestination(transform.position + new Vector3(radius * ranX, 0, radius * ranZ));
        }
    }

    public void FindPlayer()
    {
        if (!agent.enabled) return;

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

    bool onGround;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (!onGround)
            {
                onGround = true;

                Debug.Log("Trigger");
                EnableAgent(true);

                SetWander();
            }
        }
    }
}

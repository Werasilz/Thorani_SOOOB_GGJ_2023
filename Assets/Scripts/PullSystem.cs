using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PullSystem
{
    [HideInInspector] public PlayerController playerController;

    [Header("Status")]
    public bool isPulling;
    public bool attached;
    public float pullDistance;

    [Header("Pull Transform")]
    public float defaultDistance;
    public Transform attachTransform;

    [Header("Settings")]
    [SerializeField] private float pullSpeed;
    [SerializeField] private float requireDistance;
    public float decreaseAmount;
    public float pullBackDelay;
    private bool pulled;
    private bool startPullBack;

    public void ScoreDecreasing()
    {
        // Pulling state now but not have any pull action from player
        if (isPulling && !pulled)
        {
            // Decrease pull score
            pullDistance += decreaseAmount;

            // Reset
            if (pullDistance >= defaultDistance + 3)
            {
                Detach();
                ResetPull();
            }
        }
    }

    public void Detach()
    {
        // Free that Enemy
        if (attached)
        {
            attachTransform.GetComponentInChildren<EnemyController>().EnableAgent(true);
            attachTransform.transform.GetChild(0).parent = null;
        }
    }

    public void CheckingComplete()
    {
        // Reach require score
        if (isPulling && pullDistance <= requireDistance)
        {
            ResetPull();

            // Destroy enemy
            Object.Destroy(attachTransform.transform.GetChild(0).gameObject);
        }
    }

    public void ResetPull()
    {
        // Reset to zero
        pullDistance = 0;

        // Reset pulling action
        isPulling = false;

        // Reset attach enemy
        attached = false;

        // Reset pull back
        startPullBack = false;
        pulled = true;

        // Change to move state
        playerController.StateUpdate(PlayerState.MoveState);

        // Reset transform
        attachTransform.transform.localPosition = new Vector3(0, 0, defaultDistance);
        attachTransform.transform.localRotation = Quaternion.identity;
    }

    public IEnumerator TimeCounting()
    {
        // Waiting
        yield return new WaitForSeconds(pullBackDelay);

        // Reset pull action, it will use for decrease pull score
        startPullBack = true;
    }

    public void PullInput()
    {
        if (isPulling)
        {
            // Pull action true
            pulled = true;

            // Add pull score
            pullDistance -= pullSpeed;
        }
    }

    public void PullBack()
    {
        if (startPullBack)
        {
            pulled = false;
        }
    }

    public void PullingTarget()
    {
        if (isPulling)
        {
            attachTransform.position = playerController.transform.position + ((playerController.transform.forward).normalized * pullDistance);
        }
    }
}

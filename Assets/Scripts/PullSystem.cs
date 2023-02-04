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
    [SerializeField] private float pullScore;

    [Header("Pull Transform")]
    public float defaultDistance;
    public Transform attachTransform;

    [Header("Settings")]
    [SerializeField] private float pullSpeed;
    [SerializeField] private int requireScore;
    [SerializeField] private float decreaseAmount;
    [SerializeField] private float waitTimer;
    private bool pulled;

    public void ScoreDecreasing()
    {
        // Pulling state now but not have any pull action from player
        if (isPulling && !pulled)
        {
            // Decrease pull score
            pullScore -= decreaseAmount;

            // Reset
            if (pullScore <= 0)
            {
                pullScore = 0;
            }
        }
    }

    public void CheckingComplete()
    {
        // Reach require score
        if (pullScore >= requireScore)
        {
            // Reset to zero
            pullScore = 0;

            // Reset pulling action
            isPulling = false;

            // Reset attach enemy
            attached = false;

            // Change to move state
            playerController.StateUpdate(PlayerState.MoveState);
        }
    }

    public IEnumerator TimeCounting()
    {
        // Waiting
        yield return new WaitForSeconds(waitTimer);

        // Reset pull action, it will use for decrease pull score
        pulled = false;
    }

    public void PullInput()
    {
        if (isPulling)
        {
            // Pull action true
            pulled = true;

            // Add pull score
            pullScore += 1;
        }
    }

    public void PullingTarget()
    {
        if (isPulling)
        {
            var scaledMoveSpeed = pullSpeed * Time.deltaTime;
            var move = Quaternion.Euler(0, playerController.centerDirection.eulerAngles.y, 0) * new Vector3(0, 0, 0.25f);

            if (pullScore > 0)
            {
                attachTransform.localPosition -= new Vector3(0, 0, move.z * scaledMoveSpeed);
            }
            else
            {
                attachTransform.localPosition += new Vector3(0, 0, move.z * scaledMoveSpeed);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PullSystem
{
    PlayerController playerController;

    [Header("Status")]
    public bool isPulling;
    [SerializeField] private float pullScore;

    [Header("Settings")]
    [SerializeField] private int requireScore;
    [SerializeField] private float decreaseAmount;
    [SerializeField] private float waitTimer;
    private bool pulled;

    public PullSystem(PlayerController playerController)
    {
        this.playerController = playerController;
        requireScore = 20;
        decreaseAmount = 0.05f;
        waitTimer = 1.5f;
    }

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

    public void Pull()
    {
        if (isPulling)
        {
            // Pull action true
            pulled = true;

            // Add pull score
            pullScore += 1;
        }
    }
}

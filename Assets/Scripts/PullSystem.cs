using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
            playerController.inputManager.ActiveGamepadMotor();

            // Reset
            if (pullDistance >= defaultDistance + 1.5f)
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
            playerController.rootSkill.rootSpinCollider.GetComponentInChildren<EnemyController>().EnableAgent(true);
            playerController.rootSkill.rootSpinCollider.transform.GetChild(1).parent = null;
            playerController.rootSkill.StartReverseRoot();
        }
    }

    public void CheckingComplete()
    {
        // Reach require score
        if (isPulling && pullDistance <= requireDistance)
        {
            ResetPull();

            playerController.rootSkill.animations.Clear();

            // Destroy enemy
            Animation rootSpinAnim = playerController.rootSkill.rootSpinCollider.GetComponentInChildren<Animation>();
            playerController.rootSkill.ReverseSingleRootSpin(rootSpinAnim);
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
            playerController.rootSkill.rootSpinCollider.transform.position = playerController.transform.position + ((playerController.transform.forward).normalized * pullDistance);
        }
    }
}

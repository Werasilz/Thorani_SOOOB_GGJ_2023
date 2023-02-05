using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pullable : MonoBehaviour
{
    public float decreaseAmount;
    public float pullBackDelay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RootSpin")
        {
            EnemyController enemyController = transform.GetComponentInParent<EnemyController>();
            if (!enemyController.agent.enabled) return;

            // Stop enemy running
            enemyController.GetCatch();

            // Change to pull state
            PlayerController playerController = other.GetComponentInParent<PlayerController>();
            playerController.StateUpdate(PlayerState.PullState);

            // Disable collider
            playerController.rootSkill.SetActiveCollider(false);

            // Attach this enemy to root
            transform.parent = playerController.rootSkill.rootSpinCollider.transform;

            // Send pullDistance to player
            playerController.pullSystem.pullDistance = playerController.pullSystem.defaultDistance;

            // Send decreaseAmount to player
            playerController.pullSystem.decreaseAmount = decreaseAmount;

            // Start Counting decreaseAmount Start Time
            playerController.pullSystem.pullBackDelay = pullBackDelay;

            // Start delay pull back
            playerController.StartDelayPullBack();

            // Set attached
            playerController.pullSystem.attached = true;

            // Look at opposite to player
            enemyController.transform.localPosition = Vector3.zero;
            enemyController.transform.LookAt(playerController.transform.position);
            enemyController.transform.localEulerAngles = new Vector3(0, 180, 0);

            enemyController.touch.transform.SetParent(null);
            enemyController.touch.GetComponent<Collider>().enabled = true;
            enemyController.touch.GetComponent<Rigidbody>().isKinematic = false;
            enemyController.holdingTouch = false;
            Destroy(enemyController.touch, 5f);
        }

        if (other.gameObject.tag == "RootLine")
        {
            // Check this enemy is attach to player
            if (!GetComponentInParent<PlayerController>())
                return;

            Animation animation = other.GetComponentInChildren<Animation>();
            RootSkill rootSkill = other.GetComponentInParent<RootSkill>();

            // Reverse root animation
            if (rootSkill && animation)
            {
                animation.GetComponentInParent<SphereCollider>().enabled = false;
                rootSkill.ReverseSingleRootLine(animation);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "RootLine")
        {
            // Check this enemy is attach to player
            if (!GetComponentInParent<PlayerController>())
                return;

            Animation animation = other.GetComponentInChildren<Animation>();
            RootSkill rootSkill = other.GetComponentInParent<RootSkill>();

            // Reverse root animation
            if (rootSkill && animation)
            {
                animation.GetComponentInParent<SphereCollider>().enabled = false;
                rootSkill.ReverseSingleRootLine(animation);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pullable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Root")
        {
            EnemyController enemyController = transform.GetComponentInParent<EnemyController>();
            if (!enemyController.agent.enabled) return;

            // Stop enemy running
            enemyController.GetCatch();

            // Change to pull state
            PlayerController playerController = other.GetComponentInParent<PlayerController>();
            playerController.StateUpdate(PlayerState.PullState);

            // Disable collider
            playerController.SetActiveCollider(false);

            // Attach this enemy to root
            transform.parent = playerController.pullSystem.attachTransform.transform;

            // Set attached
            playerController.pullSystem.attached = true;

            // Look at opposite to player
            enemyController.transform.localPosition = Vector3.zero;
            enemyController.transform.LookAt(playerController.transform.position);
            enemyController.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
    }
}

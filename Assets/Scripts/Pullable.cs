using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pullable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Root")
        {
            // Change to pull state
            PlayerController playerController = other.GetComponentInParent<PlayerController>();
            playerController.StateUpdate(PlayerState.PullState);

            // Attach this enemy to root
            transform.parent = playerController.pullSystem.attachTransform.transform;

            // Set attached
            playerController.pullSystem.attached = true;

            // Stop enemy running
            EnemyController enemyController = transform.GetComponentInParent<EnemyController>();
            enemyController.GetCatch();
            enemyController.transform.localPosition = Vector3.zero;

            Vector3 lookDirection = (enemyController.transform.position - playerController.transform.forward).normalized;
            enemyController.transform.LookAt(enemyController.transform.position + lookDirection);
        }
    }
}

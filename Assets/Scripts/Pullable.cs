using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pullable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Root")
        {
            PlayerController playerController = other.GetComponentInParent<PlayerController>();
            playerController.StateUpdate(PlayerState.PullState);

            Puller puller = other.GetComponentInParent<Puller>();
            transform.root.parent = puller.attachTransform.transform;

            EnemyController enemyController = transform.GetComponentInParent<EnemyController>();
            enemyController.GetCatch();
            enemyController.transform.localPosition = Vector3.zero;
        }
    }
}

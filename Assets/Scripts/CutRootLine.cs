using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutRootLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RootLine"))
        {
            if (other.gameObject.GetComponentInParent<PlayerController>() != null)
            {
                PlayerController playerController = other.gameObject.GetComponentInParent<PlayerController>();

                if (playerController.playerState == PlayerState.PullState)
                {
                    playerController.pullSystem.Detach();
                    playerController.pullSystem.ResetPull();
                    playerController.StateUpdate(PlayerState.MoveState);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public Transform centerDirection;
    public List<PlayerInput> players = new List<PlayerInput>();
    public Transform[] spawnPoints;

    void OnPlayerJoined(PlayerInput playerInput)
    {
        players.Add(playerInput);
        playerInput.gameObject.name = "Player " + playerInput.playerIndex;
        playerInput.GetComponent<PlayerController>().centerDirection = centerDirection;
        playerInput.transform.position = spawnPoints[playerInput.playerIndex].position;
        Debug.Log(string.Format("PlayerInput {0} Joined", playerInput.playerIndex));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public Transform centerDirection;
    public List<PlayerInput> players = new List<PlayerInput>();

    void OnPlayerJoined(PlayerInput playerInput)
    {
        players.Add(playerInput);
        playerInput.gameObject.name = "Player " + players.Count;
        playerInput.GetComponent<PlayerController>().centerDirection = centerDirection;
        Debug.Log("PlayerInput Joined");
    }
}

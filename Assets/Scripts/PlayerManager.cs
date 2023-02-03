using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public Transform centerDirection;
    public List<PlayerInput> players = new List<PlayerInput>();
    public Transform[] spawnPoints;
    public AbilityUIManager[] abilityUIManagers;

    void OnPlayerJoined(PlayerInput playerInput)
    {
        // Add new player
        players.Add(playerInput);

        // Change object name
        playerInput.gameObject.name = "Player " + playerInput.playerIndex;

        // Set transform center direction for moving correctly
        playerInput.GetComponent<PlayerController>().centerDirection = centerDirection;

        // Set ability manager
        playerInput.GetComponent<AbilityController>().abilityUIManager = abilityUIManagers[playerInput.playerIndex];
        playerInput.GetComponent<AbilityController>().abilityUIManager.abilityUIs[0].EnableSkill();
        playerInput.GetComponent<AbilityController>().abilityUIManager.abilityUIs[1].EnableSkill();
        playerInput.GetComponent<AbilityController>().abilityUIManager.abilityUIs[2].EnableSkill();

        // Set spawn position
        playerInput.transform.position = spawnPoints[playerInput.playerIndex].position;

        Debug.Log(string.Format("PlayerInput {0} Joined", playerInput.playerIndex));
    }
}

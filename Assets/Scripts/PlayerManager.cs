using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public GameState gameState;

    public Transform centerDirection;
    public List<PlayerInput> players = new List<PlayerInput>();
    public Transform[] spawnPoints;
    public AbilityUIManager[] abilityUIManagers;

    [Header("UserInterface")]
    public TextMeshProUGUI pressStartTextP1;
    public TextMeshProUGUI pressStartTextP2;


    [Header("Choose Color")]
    public int[] playerSelectIndex;
    public GameObject chooseColorMenu;
    public Image[] p1ColorImage;
    public Image[] p2ColorImage;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameState = GameState.WaitingState;
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        // Add new player
        players.Add(playerInput);

        if (players.Count == 2)
        {
            gameState = GameState.ChooseColorState;

            chooseColorMenu.SetActive(true);
        }

        // Change object name
        playerInput.gameObject.name = "Player " + playerInput.playerIndex;

        // Hide text
        if (playerInput.playerIndex == 0)
        {
            pressStartTextP1.gameObject.SetActive(false);
            p1ColorImage[playerSelectIndex[playerInput.playerIndex]].enabled = true;
        }
        else if (playerInput.playerIndex == 1)
        {
            pressStartTextP2.gameObject.SetActive(false);
            p2ColorImage[playerSelectIndex[playerInput.playerIndex]].enabled = true;
        }

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

    public void DisableLastSelect(PlayerInput playerInput)
    {
        if (playerInput.playerIndex == 0)
        {
            p1ColorImage[playerSelectIndex[playerInput.playerIndex]].enabled = false;
        }
        else if (playerInput.playerIndex == 1)
        {
            p2ColorImage[playerSelectIndex[playerInput.playerIndex]].enabled = false;
        }
    }

    public void EnableNextSelect(PlayerInput playerInput)
    {
        if (playerInput.playerIndex == 0)
        {
            p1ColorImage[playerSelectIndex[playerInput.playerIndex]].enabled = true;
        }
        else if (playerInput.playerIndex == 1)
        {
            p2ColorImage[playerSelectIndex[playerInput.playerIndex]].enabled = true;
        }
    }
}
public enum GameState { WaitingState, ChooseColorState, GameplayState }
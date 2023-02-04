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
    public List<PlayerInput> playersInput = new List<PlayerInput>();
    public List<PlayerController> playerControllers = new List<PlayerController>();

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

    [Header("Player Status")]
    public List<PlayerScore> playerScores;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameState = GameState.WaitingState;
    }

    public void AddPlayerScore(PlayerInput playerInput, int _score)
    {
        playerScores[playerInput.playerIndex].AddScore(_score);
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        // Add new player
        playersInput.Add(playerInput);

        // Set transform center direction for moving correctly
        playerInput.GetComponent<PlayerController>().centerDirection = centerDirection;

        // Set ability manager
        playerInput.GetComponent<AbilityController>().abilityUIManager = abilityUIManagers[playerInput.playerIndex];

        // Set spawn position
        playerInput.transform.position = spawnPoints[playerInput.playerIndex].position;

        // Get player controller
        playerControllers.Add(playerInput.GetComponent<PlayerController>());

        // New score and level
        PlayerScore playerScore = new PlayerScore();
        playerScore.Setup(playerInput);
        playerScores.Add(playerScore);

        if (playersInput.Count == 2)
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

[System.Serializable]
public class PlayerScore
{
    public int score;
    public int playerLevel = 1;
    public PlayerInput playerInput;

    public void Setup(PlayerInput playerInput)
    {
        score = 0;
        playerLevel = 1;
        this.playerInput = playerInput;

        UpdateLevel();
    }

    public void AddScore(int _score)
    {
        score += _score;

        playerLevel = (int)Mathf.Sqrt((float)score);

        if (playerLevel > 5)
            playerLevel = 5;

        UpdateLevel();
    }

    public void UpdateLevel()
    {
        switch (playerLevel)
        {
            case 1:
                PlayerManager.instance.playerControllers[playerInput.playerIndex].abilityController.abilityUIManager.abilityUIs[0].EnableSkillIcon();
                PlayerManager.instance.playerControllers[playerInput.playerIndex].abilityController.abilitys[0].isUnlock = true;
                PlayerManager.instance.playerControllers[playerInput.playerIndex].abilityController.abilitys[0].cooldown = 3;
                break;
            case 2:
                PlayerManager.instance.playerControllers[playerInput.playerIndex].abilityController.abilityUIManager.abilityUIs[1].EnableSkillIcon();
                PlayerManager.instance.playerControllers[playerInput.playerIndex].abilityController.abilitys[1].isUnlock = true;
                PlayerManager.instance.playerControllers[playerInput.playerIndex].abilityController.abilitys[1].cooldown = 15;
                break;
            case 3:
                PlayerManager.instance.playerControllers[playerInput.playerIndex].abilityController.abilityUIManager.abilityUIs[2].EnableSkillIcon();
                PlayerManager.instance.playerControllers[playerInput.playerIndex].abilityController.abilitys[2].isUnlock = true;
                PlayerManager.instance.playerControllers[playerInput.playerIndex].abilityController.abilitys[2].cooldown = 20;
                break;
            case 4:
                PlayerManager.instance.playerControllers[playerInput.playerIndex].abilityController.abilitys[0].cooldown = 2;
                PlayerManager.instance.playerControllers[playerInput.playerIndex].abilityController.abilitys[1].cooldown = 10;
                PlayerManager.instance.playerControllers[playerInput.playerIndex].abilityController.abilitys[2].cooldown = 15;
                break;
            case 5:
                PlayerManager.instance.playerControllers[playerInput.playerIndex].abilityController.abilitys[0].cooldown = 1.5f;
                PlayerManager.instance.playerControllers[playerInput.playerIndex].abilityController.abilitys[1].cooldown = 5;
                PlayerManager.instance.playerControllers[playerInput.playerIndex].abilityController.abilitys[2].cooldown = 10;
                break;
        }
    }

}
public enum GameState { WaitingState, ChooseColorState, GameplayState }
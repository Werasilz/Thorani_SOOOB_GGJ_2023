using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScoreManager : MonoBehaviour
{
    public PlayerScore[] playerScores;

    private void Start()
    {
        foreach (PlayerScore playerScore in playerScores)
        {
            playerScore.Setup();
        }
    }

    public void AddPlayerScore(int _playerIndex, int _score)
    {
        playerScores[_playerIndex].AddScore(_score);
    }
}

[System.Serializable]
public class PlayerScore
{
    public int score;
    public int playerState = 1;

    public void Setup()
    {
        score = 0;
        playerState = 1;
    }

    public void AddScore(int _score)
    {
        score += _score;

        

        if (score >= 25)
            playerState = 5;
        else if (score >= 16)
            playerState = 4;
        else if (score >= 9)
            playerState = 3;
        else if (score >= 4)
            playerState = 2;
        else
            playerState = 1;
    }

}

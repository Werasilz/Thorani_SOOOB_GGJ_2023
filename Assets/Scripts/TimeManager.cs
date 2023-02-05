using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public float time = 180;
    public float counting;
    public bool isCounting;
    public TextMeshProUGUI countingText;
    public GameObject leaderBoard;
    public Image rootWinnerImage;
    public bool isEndGame;

    public SpawnManager spawnManager;

    public void StartCounting()
    {
        isCounting = true;

        counting = time;

        spawnManager.spawning = true;

        GetComponent<Animator>().SetTrigger("isActivate");
    }

    private void FixedUpdate()
    {
        if (isCounting)
        {
            counting -= Time.deltaTime;

            countingText.text = counting.ToString("F0");

            if (counting <= 0)
            {
                isCounting = false;
                isEndGame = true;

                counting = 0;
                countingText.text = counting.ToString("F0");

                // Show Winner
                PlayerManager.instance.gameState = GameState.WaitingState;
                leaderBoard.SetActive(true);

                // Hard code check score
                if (PlayerManager.instance.playerScores[0].score > PlayerManager.instance.playerScores[1].score)
                {
                    rootWinnerImage.sprite = PlayerManager.instance.scoreColorSprites[PlayerManager.instance.playerSelectIndex[0]];
                }
                else
                {
                    rootWinnerImage.sprite = PlayerManager.instance.scoreColorSprites[PlayerManager.instance.playerSelectIndex[1]];
                }
            }
        }
    }
}



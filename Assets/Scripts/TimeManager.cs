using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public float time = 180;
    public float counting;
    public bool isCounting;
    public TextMeshProUGUI countingText;


    public void StartCounting()
    {
        isCounting = true;

        counting = time;

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

                counting = 0;
                countingText.text = counting.ToString("F0");


                // Stop

                // Show Winner
            }
        }
    }
}



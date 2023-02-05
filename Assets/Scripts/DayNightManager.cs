using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DayNightManager : MonoBehaviour
{
    public Animator ground;
    public Volume profile;
    private bool isNight;
    private bool isDay;
    private ColorAdjustments colorAdjustments;
    private WhiteBalance whiteBalance;

    private void Start()
    {
        profile.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        profile.profile.TryGet<WhiteBalance>(out whiteBalance);
    }

    [ContextMenu("Start Night")]
    public void StartNight()
    {
        ground.CrossFade("DayToNight", 0.01f);
        isNight = true;
        isDay = false;
    }

    [ContextMenu("Start Day")]
    public void StartDay()
    {
        ground.CrossFade("NightToDay", 0.01f);
        isNight = false;
        isDay = true;
    }

    void FixedUpdate()
    {
        if (profile != null)
        {
            if (isNight)
            {
                if (colorAdjustments.postExposure.value > 0)
                {
                    colorAdjustments.postExposure.value -= 0.5f * Time.deltaTime;
                }

                if (whiteBalance.temperature.value > -20)
                {
                    whiteBalance.temperature.value -= Time.deltaTime;
                }
            }
            else if (isDay)
            {
                if (colorAdjustments.postExposure.value < 1)
                {
                    colorAdjustments.postExposure.value += 0.5f * Time.deltaTime;
                }

                if (whiteBalance.temperature.value < 0)
                {
                    whiteBalance.temperature.value += Time.deltaTime;
                }
            }
        }
    }
}

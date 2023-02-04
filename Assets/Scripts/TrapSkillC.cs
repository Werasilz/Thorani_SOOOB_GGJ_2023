using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSkillC : MonoBehaviour
{
    public GameObject startModel;
    public GameObject finalModel;

    public float delayChange = 1f;

    void Start()
    {
        startModel.SetActive(true);
        finalModel.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkinHandler : MonoBehaviour
{
    public static EnemySkinHandler Instance;

    public Material[] skinMaterials;
    private void Awake()
    {
        EnemySkinHandler.Instance = this;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPopup : MonoBehaviour
{
    public GameObject analogRight;
    public GameObject shoot;
    private Camera camera;
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (analogRight.activeInHierarchy)
        {
            Vector3 lookPoint = (camera.transform.forward - camera.transform.position);
            analogRight.transform.LookAt(transform.position + lookPoint.normalized + Vector3.up * 2);
        }
        else if (shoot.activeInHierarchy)
        {
            Vector3 lookPoint = (camera.transform.forward - camera.transform.position);
            shoot.transform.LookAt(transform.position + lookPoint.normalized + Vector3.up * 2);
        }
    }
}

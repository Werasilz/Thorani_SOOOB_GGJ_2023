using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerController playerController => GetComponent<PlayerController>();

    [Header("Input")]
    public Vector2 direction;
    // [SerializeField] private bool A;
    // [SerializeField] private bool B;
    // [SerializeField] private bool X;
    // [SerializeField] private bool Y;
    // [SerializeField] private bool RT;
    // [SerializeField] private bool RB;
    // [SerializeField] private bool LT;
    // [SerializeField] private bool LB;

    public void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {

    }

    public void OnA(InputValue value)
    {
        //A = value.isPressed;

        playerController.SkillA();
    }

    public void OnB(InputValue value)
    {
        //B = value.isPressed;
    }

    public void OnX(InputValue value)
    {
        //X = value.isPressed;
    }

    public void OnY(InputValue value)
    {
        //Y = value.isPressed;
    }

    public void OnLT(InputValue value)
    {
        //LT = value.isPressed;
    }

    public void OnLB(InputValue value)
    {
        //LT = value.isPressed;
    }

    public void OnRT(InputValue value)
    {
        //RT = value.isPressed;
    }

    public void OnRB(InputValue value)
    {
        //RB = value.isPressed;
    }
}

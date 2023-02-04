using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInput playerInput => GetComponent<PlayerInput>();
    PlayerController playerController => GetComponent<PlayerController>();
    [HideInInspector] public Vector2 direction;

    public void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {

    }

    public void OnA(InputValue value)
    {
        // Change move state to shoot state
        if (playerController.playerState == PlayerState.MoveState)
        {
            if (!playerController.abilityController.abilitys[0].isCooldown)
            {
                playerController.StateUpdate(PlayerState.ShootState);
            }
        }
        // Start active root
        else if (playerController.playerState == PlayerState.ShootState)
        {
            playerController.abilityController.CastSkill(0);
        }
    }

    public void OnB(InputValue value)
    {
        if (playerController.playerState == PlayerState.PullState)
        {
            playerController.pullSystem.Detach();
            playerController.pullSystem.ResetPull();
        }

        playerController.StateUpdate(PlayerState.MoveState);
    }

    public void OnX(InputValue value)
    {
        if (playerController.playerState == PlayerState.MoveState)
        {
            playerController.abilityController.CastSkill(1);
        }
    }

    public void OnY(InputValue value)
    {
        if (playerController.playerState == PlayerState.MoveState)
        {
            playerController.abilityController.CastSkill(2);
        }
    }

    public void OnLT(InputValue value)
    {
    }

    public void OnLB(InputValue value)
    {
    }

    public void OnRT(InputValue value)
    {
    }

    public void OnRB(InputValue value)
    {
    }

    public void OnStickUp(InputValue value)
    {
        playerController.Pull();
    }

    public void OnStickDown(InputValue value)
    {
        playerController.Pull();
    }

    public void OnDPadLeft(InputValue value)
    {
        if (PlayerManager.instance.gameState == GameState.ChooseColorState)
        {
            if (value.isPressed)
            {
                PlayerManager.instance.DisableLastSelect(playerInput);
                PlayerManager.instance.playerSelectIndex[playerInput.playerIndex] -= 1;

                if (PlayerManager.instance.playerSelectIndex[playerInput.playerIndex] < 0)
                {
                    PlayerManager.instance.playerSelectIndex[playerInput.playerIndex] = 4;
                }

                PlayerManager.instance.EnableNextSelect(playerInput);
            }
        }
    }

    public void OnDPadRight(InputValue value)
    {
        if (PlayerManager.instance.gameState == GameState.ChooseColorState)
        {
            if (value.isPressed)
            {
                PlayerManager.instance.DisableLastSelect(playerInput);
                PlayerManager.instance.playerSelectIndex[playerInput.playerIndex] += 1;

                if (PlayerManager.instance.playerSelectIndex[playerInput.playerIndex] > 4)
                {
                    PlayerManager.instance.playerSelectIndex[playerInput.playerIndex] = 0;
                }

                PlayerManager.instance.EnableNextSelect(playerInput);
            }
        }
    }

    public bool isStartMotor;
    public float motorTimer;

    public void ActiveGamepadMotor()
    {
        Gamepad.current.SetMotorSpeeds(0.1f, 0.2f);
        isStartMotor = true;
        motorTimer = 1.5f;
    }

    public void CooldownGamepadMotor()
    {
        if (isStartMotor)
        {
            motorTimer -= Time.deltaTime;

            if (motorTimer <= 0)
            {
                motorTimer = 0;
                isStartMotor = false;
                Gamepad.current.SetMotorSpeeds(0.0f, 0.0f);
            }
        }

        if (!playerController.pullSystem.isPulling)
        {
            motorTimer = 0;
            isStartMotor = false;
            Gamepad.current.SetMotorSpeeds(0.0f, 0.0f);
        }
    }
}

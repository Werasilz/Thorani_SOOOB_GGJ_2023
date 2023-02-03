using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput => GetComponent<PlayerInput>();
    InputManager inputManager => GetComponent<InputManager>();
    [HideInInspector] public Transform centerDirection;

    [Header("System")]
    public PullSystem pullSystem;
    public Coroutine pullTimeCountingCoroutine;

    [Header("State")]
    public PlayerState playerState;

    [Header("Model")]
    public GameObject character;
    public GameObject rootCharacter;
    public GameObject shootingArea;

    [Header("Animator")]
    public Animator root;

    [Header("Status")]
    public float moveSpeed;
    public bool movable;
    public bool rotatable;

    private void Start()
    {
        // Create Object System
        pullSystem = new PullSystem(this);

        // Set first state to move 
        StateUpdate(PlayerState.MoveState);
    }

    public void Update()
    {
        // Movement
        Move(inputManager.direction);
        Rotate();

        // Decreasing score when not pulling
        pullSystem.ScoreDecreasing();

        // Checking when player reach the require score
        pullSystem.CheckingComplete();
    }

    public void StateUpdate(PlayerState newPlayerState)
    {
        // Apply new state
        playerState = newPlayerState;

        // Always can rotate in any state
        rotatable = true;

        if (playerState == PlayerState.MoveState)
        {
            // Set move 
            movable = true;

            // Show character and hide other model
            character.SetActive(true);
            rootCharacter.SetActive(false);
            shootingArea.SetActive(false);
        }
        else if (playerState == PlayerState.ShootState)
        {
            // Set move 
            movable = false;

            // Hide character and show root shooting area
            character.SetActive(false);
            rootCharacter.SetActive(true);
            shootingArea.SetActive(true);
        }
        else if (playerState == PlayerState.PullState)
        {
            // Set move 
            movable = false;

            // Start pulling
            pullSystem.isPulling = true;
        }
    }

    private void Move(Vector2 direction)
    {
        // Return when not have value
        if (direction.sqrMagnitude < 0.01)
            return;

        // Return when is not move state
        if (playerState != PlayerState.MoveState)
            return;

        // Move code
        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        var move = Quaternion.Euler(0, centerDirection.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        transform.position += move * scaledMoveSpeed;
    }

    private void Rotate()
    {
        // Can't rotate
        if (!rotatable)
            return;

        // Rotate code
        var move = Quaternion.Euler(0, centerDirection.eulerAngles.y, 0) * new Vector3(inputManager.direction.x, 0, inputManager.direction.y);
        transform.LookAt(transform.position + move);
    }

    public void ActivePull()
    {
        // Set to pull state
        StateUpdate(PlayerState.PullState);
    }

    public void Pull()
    {
        // Add pull score
        pullSystem.Pull();

        // Check coroutine
        if (pullTimeCountingCoroutine != null)
        {
            StopCoroutine(pullTimeCountingCoroutine);
        }

        // Start time counting
        pullTimeCountingCoroutine = StartCoroutine(pullSystem.TimeCounting());
    }

    public void SkillA()
    {
        // Change move state to shoot state
        if (playerState == PlayerState.MoveState)
        {
            StateUpdate(PlayerState.ShootState);
        }
        // Start active root
        else if (playerState == PlayerState.ShootState)
        {
            rotatable = false;
            root.SetTrigger("ActiveRoot");

            //StartCoroutine(ActiveRoot());
        }

        // IEnumerator ActiveRoot()
        // {
        //     // Play animation
        //     root.SetTrigger("ActiveRoot");

        //     // Waiting
        //     yield return new WaitForSeconds(1f);

        //     // Reset to move state
        //     StateUpdate(PlayerState.MoveState);
        // }
    }

    public void SkillB()
    {

    }

    public void SkillX()
    {

    }

    public void SkillY()
    {

    }
}
public enum PlayerState { MoveState, ShootState, PullState };

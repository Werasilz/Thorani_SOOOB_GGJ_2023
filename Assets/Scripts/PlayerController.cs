using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput => GetComponent<PlayerInput>();
    InputManager inputManager => GetComponent<InputManager>();

    [HideInInspector] public Transform centerDirection;

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
        StateUpdate(PlayerState.MoveState);
        SetRotatable(true);
    }

    public void Update()
    {
        Move(inputManager.direction);
        Rotate();
    }

    private void StateUpdate(PlayerState newPlayerState)
    {
        playerState = newPlayerState;
        SetRotatable(true);

        if (playerState == PlayerState.MoveState)
        {
            movable = true;

            character.SetActive(true);
            rootCharacter.SetActive(false);
            shootingArea.SetActive(false);
        }
        else if (playerState == PlayerState.ShootState)
        {
            movable = false;

            character.SetActive(false);
            rootCharacter.SetActive(true);
            shootingArea.SetActive(true);
        }
    }

    private void SetRotatable(bool value)
    {
        rotatable = value;
    }

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;

        if (playerState != PlayerState.MoveState)
            return;

        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        var move = Quaternion.Euler(0, centerDirection.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        transform.position += move * scaledMoveSpeed;
    }

    private void Rotate()
    {
        if (!rotatable)
            return;

        var move = Quaternion.Euler(0, centerDirection.eulerAngles.y, 0) * new Vector3(inputManager.direction.x, 0, inputManager.direction.y);
        transform.LookAt(transform.position + move);
    }

    public void SkillA()
    {
        if (playerState == PlayerState.MoveState)
        {
            StateUpdate(PlayerState.ShootState);
        }
        else if (playerState == PlayerState.ShootState)
        {
            if (playerState == PlayerState.ShootState)
            {
                SetRotatable(false);
                StartCoroutine(ActiveRoot());
            }
        }

        IEnumerator ActiveRoot()
        {
            root.SetTrigger("ActiveRoot");
            yield return new WaitForSeconds(1f);
            StateUpdate(PlayerState.MoveState);
        }
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
public enum PlayerState { MoveState, ShootState };

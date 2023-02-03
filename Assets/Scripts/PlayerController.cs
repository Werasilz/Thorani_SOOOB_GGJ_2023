using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput => GetComponent<PlayerInput>();
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

    [Header("Input")]
    [SerializeField] private Vector2 direction;
    // [SerializeField] private bool A;
    // [SerializeField] private bool B;
    // [SerializeField] private bool X;
    // [SerializeField] private bool Y;
    // [SerializeField] private bool RT;
    // [SerializeField] private bool RB;
    // [SerializeField] private bool LT;
    // [SerializeField] private bool LB;

    private void Start()
    {
        StateUpdate(PlayerState.MoveState);
        SetRotatable(true);
    }

    public void Update()
    {
        Move(direction);
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

        var move = Quaternion.Euler(0, centerDirection.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        transform.LookAt(transform.position + move);
    }

    private void ActiveShootingArea()
    {
        if (playerState == PlayerState.MoveState)
        {
            StateUpdate(PlayerState.ShootState);
        }
        else if (playerState == PlayerState.ShootState)
        {
            StateUpdate(PlayerState.MoveState);
        }
    }

    private void Shoot()
    {
        if (playerState == PlayerState.ShootState)
        {
            SetRotatable(false);
            StartCoroutine(ActiveRoot());
        }

        IEnumerator ActiveRoot()
        {
            root.SetTrigger("ActiveRoot");
            yield return new WaitForSeconds(1f);
            StateUpdate(PlayerState.MoveState);
        }
    }

    #region PlayerActions
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

        ActiveShootingArea();
    }

    public void OnB(InputValue value)
    {
        //B = value.isPressed;
    }

    public void OnX(InputValue value)
    {
        //X = value.isPressed;

        Shoot();
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
    #endregion
}
public enum PlayerState { MoveState, ShootState };

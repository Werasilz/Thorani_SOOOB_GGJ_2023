using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput => GetComponent<PlayerInput>();
    InputManager inputManager => GetComponent<InputManager>();
    [HideInInspector] public AbilityController abilityController => GetComponent<AbilityController>();
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

    [Header("Root Skill")]
    public Transform rootSpawnPoint;
    public GameObject rootSkillPrefab;
    [HideInInspector] public RootSkill rootSkill;

    [Header("Root Wall Skill")]
    public GameObject rootWallSkillPrefab;

    [Header("Status")]
    public float moveSpeed;
    public bool movable;
    public bool rotatable;

    private void Start()
    {
        pullSystem.playerController = this;

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

        // Pull target
        pullSystem.PullingTarget();

        // Enemy pull back
        pullSystem.PullBack();

        if (rootSkill != null && rootSkill.rootSpinCollider != null)
        {
            rootSkill.rootSpinCollider.transform.LookAt(transform.position);
        }
    }

    public void StateUpdate(PlayerState newPlayerState)
    {
        // Apply new state
        playerState = newPlayerState;

        if (playerState == PlayerState.MoveState)
        {
            // Set move 
            movable = true;
            rotatable = true;

            // Show character and hide other model
            character.SetActive(true);
            rootCharacter.SetActive(false);
            shootingArea.SetActive(false);
        }
        else if (playerState == PlayerState.ShootState)
        {
            // Set move 
            movable = false;
            rotatable = true;

            // Hide character and show root shooting area
            character.SetActive(false);
            rootCharacter.SetActive(true);
            shootingArea.SetActive(true);
        }
        else if (playerState == PlayerState.PullState)
        {
            // Set move 
            movable = false;
            rotatable = false;

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
        pullSystem.PullInput();
    }

    public void StartDelayPullBack()
    {
        if (pullTimeCountingCoroutine != null)
        {
            StopCoroutine(pullTimeCountingCoroutine);
        }
        pullTimeCountingCoroutine = StartCoroutine(pullSystem.TimeCounting());
    }

    public void SkillA()
    {
        rotatable = false;
        shootingArea.SetActive(false);
        StartCoroutine(ActiveRoot());

        IEnumerator ActiveRoot()
        {
            GameObject newRootSkill = Instantiate(rootSkillPrefab, rootSpawnPoint);
            newRootSkill.SetActive(true);
            rootSkill = newRootSkill.GetComponent<RootSkill>();

            // Spawn root
            rootSkill.StartSpawnRoot();

            // Waiting
            yield return new WaitForSeconds(1f);

            // Fail to pull enemy, Reset to move state
            if (!pullSystem.attached)
            {
                // Disable collider
                rootSkill.StartReverseRoot();
                rootSkill = null;

                StateUpdate(PlayerState.MoveState);
            }
        }
    }

    public void SkillX()
    {
        GameObject newRootWallSkill = Instantiate(rootWallSkillPrefab, rootSpawnPoint);
        newRootWallSkill.SetActive(true);

        // Spawn root
        RootWallSkill rootWallSkill = newRootWallSkill.GetComponent<RootWallSkill>();
        rootWallSkill.StartSpawnRootWall();

        newRootWallSkill.transform.parent = null;
    }

    public void SkillY()
    {

    }
}
public enum PlayerState { MoveState, ShootState, PullState };

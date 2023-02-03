using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    public Transform centerDirection;
    public float moveSpeed;
    [SerializeField] private Vector2 m_Move;

    public void OnMove(InputValue value)
    {
        m_Move = value.Get<Vector2>();
    }

    public void Update()
    {
        Move(m_Move);
    }

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;

        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        var move = Quaternion.Euler(0, centerDirection.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        transform.position += move * scaledMoveSpeed;
        transform.LookAt(transform.position + move);
    }
}

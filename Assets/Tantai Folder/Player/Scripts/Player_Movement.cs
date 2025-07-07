using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private float move_Speed = 5f;
    private Rigidbody2D rb;
    private Vector2 move_Input;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = move_Input.normalized * move_Speed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        move_Input = context.ReadValue<Vector2>();
    }
}

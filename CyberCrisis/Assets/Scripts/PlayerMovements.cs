using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private int speed = 5;
    public VariableJoystick variableJoystick;

    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start() {
    }

    private void FixedUpdate()
    {
        Vector2 direction = new Vector2(-variableJoystick.Horizontal, -variableJoystick.Vertical);
        rb.AddForce(direction.normalized * speed * Time.fixedDeltaTime, ForceMode2D.Impulse);

        if (direction.x != 0 || direction.y != 0)
        {
            animator.SetFloat("X", direction.x);
            animator.SetFloat("Y", direction.y);

            animator.SetBool("IsRunning", true);

        } else
        {
            animator.SetBool("IsRunning", false);
        }
    }
}

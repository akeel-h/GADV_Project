using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float sprintSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);

        if (moveInput != Vector2.zero)
        {
            animator.SetBool("isWalking", true);

            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void FixedUpdate()
    {
        float currentSpeed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
            animator.speed = 1f * (sprintSpeed / moveSpeed);
        }
        else
        {
            animator.speed = 1f;
        }

        rb.velocity = moveInput * currentSpeed;
    }

    public bool IsMoving()
    {
        return moveInput != Vector2.zero;
    }
}

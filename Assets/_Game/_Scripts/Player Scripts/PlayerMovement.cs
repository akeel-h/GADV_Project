using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;    // Normal walking speed
    public float sprintSpeed = 3f;  // Sprinting speed

    private Rigidbody2D rb;         // Reference to Rigidbody2D component
    private Vector2 moveInput;      // Stores current input direction
    private Animator animator;      // Reference to Animator component

    private void Start()
    {
        InitializeComponents();
    }

    private void Update()
    {
        HandleMovementInput();
        UpdateAnimatorMovement();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    // ---------------- Initialization ----------------

    // Grab references to required components
    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // ---------------- Input Handling ----------------

    // Read movement input and normalize
    private void HandleMovementInput()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
    }

    // ---------------- Animator ----------------

    // Update animator parameters based on movement input
    private void UpdateAnimatorMovement()
    {
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

    // ---------------- Movement ----------------

    // Move the character based on input and speed
    private void MoveCharacter()
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

    // ---------------- Public Methods ----------------

    // Returns true if the player is currently moving
    public bool IsMoving()
    {
        return moveInput != Vector2.zero;
    }
}

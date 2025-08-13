using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float detectionRange = 5f;
    public float patrolSpeed = 0.4f;
    public float chaseSpeed = 0.8f;
    public Transform[] patrolPoints; // Assign in Inspector
    public float waitTime = 1.5f;

    private int currentPointIndex = 0;
    private int patrolDirection = 1;
    private float waitCounter = 0f;
    private bool isWaiting = false;

    private bool playerDetected;
    private Transform player;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (patrolPoints.Length > 0)
            transform.position = patrolPoints[0].position; // Start at first point
    }

    void Update()
    {
        bool playerIsHiding = PlayerHideState.IsHiding;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange && !playerIsHiding)
        {
            if (!playerDetected) Debug.Log("Enemy: Player detected!");
            playerDetected = true;
        }
        else if (distance > detectionRange + 1f || playerIsHiding)
        {
            if (playerDetected) Debug.Log("Enemy: Lost player or player is hiding");
            playerDetected = false;
        }
    }

    void FixedUpdate()
    {
        if (playerDetected)
        {
            animator.speed = 1f * (chaseSpeed / patrolSpeed);
            MoveTowards(player.position, chaseSpeed);
        }
        else
        {
            animator.speed = 1f;
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (isWaiting)
        {
            SetAnimation(Vector2.zero);
            waitCounter -= Time.fixedDeltaTime;
            if (waitCounter <= 0)
            {
                isWaiting = false;
                currentPointIndex += patrolDirection;

                if (currentPointIndex >= patrolPoints.Length)
                {
                    currentPointIndex = patrolPoints.Length - 2;
                    patrolDirection = -1;
                }
                else if (currentPointIndex < 0)
                {
                    currentPointIndex = 1;
                    patrolDirection = 1;
                }
            }
            return;
        }

        Vector2 targetPos = patrolPoints[currentPointIndex].position;
        MoveTowards(targetPos, patrolSpeed);

        if (Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            isWaiting = true;
            waitCounter = waitTime;
        }
    }

    private void MoveTowards(Vector2 target, float moveSpeed)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        SetAnimation(direction);
    }

    private Vector2 lastMoveDir = Vector2.down; // default facing down

    void SetAnimation(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0.001f) // moving
        {
            lastMoveDir = direction;
            animator.SetFloat("InputX", direction.x);
            animator.SetFloat("InputY", direction.y);
        }
        else // idle, keep last direction
        {
            animator.SetFloat("InputX", lastMoveDir.x);
            animator.SetFloat("InputY", lastMoveDir.y);
        }

        animator.SetFloat("Speed", direction.magnitude);
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.PlayerDied(collision.gameObject);
        }
    }                
}

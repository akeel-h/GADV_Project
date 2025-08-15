using UnityEngine;

public class AnurodelaPatrol : MonoBehaviour
{
    [Header("Detection & Speeds")]
    public float detectionRange = 5f;
    public float patrolSpeed = 0.4f;
    public float chaseSpeed = 0.8f;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints; // Patrol waypoints
    public float waitTime = 1.5f;

    [Header("References")]
    [SerializeField] private Transform colliderChild; // For adjusting collider rotation

    // Patrol state variables
    private int currentPointIndex = 0;
    private int patrolDirection = 1;
    private float waitCounter = 0f;
    private bool isWaiting = false;
    private bool returningToPatrol = false;

    // Player detection
    private bool playerDetected = false;
    private Transform player;

    // Components
    private Animator animator;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    // Animation tracking
    private Vector2 lastMoveDir = Vector2.down; // Default facing down

    void Start()
    {
        InitializeComponents();
        PositionAtFirstPatrolPoint();
    }

    void Update()
    {
        HandlePlayerDetection();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void LateUpdate()
    {
        AdjustColliderRotation();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandlePlayerCollision(collision);
    }

    // ---------------- Initialization ----------------

    // Cache components and find player
    private void InitializeComponents()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        boxCollider2D = GetComponentInChildren<BoxCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Set starting position to the first patrol point
    private void PositionAtFirstPatrolPoint()
    {
        if (patrolPoints.Length > 0)
            transform.position = patrolPoints[0].position;
    }

    // ---------------- Detection ----------------

    // Check if player is in detection range and update chase state
    private void HandlePlayerDetection()
    {
        bool playerIsHiding = PlayerHideState.IsHiding;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange && !playerIsHiding)
        {
            if (!playerDetected)
                Debug.Log("Enemy: Player detected!");

            playerDetected = true;

            // Increase player's sanity while chasing
            SanitySystem playerSanity = player.GetComponent<SanitySystem>();
            if (playerSanity != null)
            {
                playerSanity.IncreaseSanity(playerSanity.chaseSanityGainRate * Time.deltaTime);
            }
        }
        else if (distance > detectionRange + 1f || playerIsHiding)
        {
            if (playerDetected)
            {
                Debug.Log("Enemy: Lost player or player is hiding");
                playerDetected = false;
                SetNearestPatrolPointAsTarget();
            }
        }
    }

    // Find nearest patrol point and set it as target
    private void SetNearestPatrolPointAsTarget()
    {
        if (patrolPoints.Length == 0) return;

        float minDist = float.MaxValue;
        int nearestIndex = 0;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            float d = Vector2.Distance(transform.position, patrolPoints[i].position);
            if (d < minDist)
            {
                minDist = d;
                nearestIndex = i;
            }
        }

        currentPointIndex = nearestIndex;
        returningToPatrol = true;
    }

    // ---------------- Movement ----------------

    // Decide whether to patrol or chase
    private void HandleMovement()
    {
        if (playerDetected)
        {
            animator.speed = chaseSpeed / patrolSpeed;
            MoveTowards(player.position, chaseSpeed);
        }
        else
        {
            animator.speed = 1f;
            Patrol();
        }
    }

    // Patrol between points with wait times
    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (isWaiting)
        {
            SetAnimation(Vector2.zero);
            waitCounter -= Time.fixedDeltaTime;

            if (waitCounter <= 0)
            {
                isWaiting = false;

                if (!returningToPatrol)
                {
                    // Move to next patrol point
                    currentPointIndex += patrolDirection;

                    // Reverse direction at ends
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
                else
                {
                    returningToPatrol = false;
                }
            }
            return;
        }

        Vector2 targetPos = patrolPoints[currentPointIndex].position;
        MoveTowards(targetPos, patrolSpeed);

        // Start waiting if reached patrol point
        if (Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            isWaiting = true;
            waitCounter = waitTime;
        }
    }

    // Move enemy towards a target position
    private void MoveTowards(Vector2 target, float moveSpeed)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        SetAnimation(direction);
    }

    // Rotate collider depending on movement direction
    private void AdjustColliderRotation()
    {
        float x = animator.GetFloat("InputX");
        float y = animator.GetFloat("InputY");

        bool isVertical = Mathf.Abs(y) > Mathf.Abs(x);
        colliderChild.localRotation = Quaternion.Euler(0, 0, isVertical ? 90f : 0f);
    }

    // ---------------- Animation ----------------

    // Update animator with movement direction and speed
    private void SetAnimation(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0.001f)
        {
            lastMoveDir = direction;
            animator.SetFloat("InputX", direction.x);
            animator.SetFloat("InputY", direction.y);
        }
        else
        {
            animator.SetFloat("InputX", lastMoveDir.x);
            animator.SetFloat("InputY", lastMoveDir.y);
        }

        animator.SetFloat("Speed", direction.magnitude);
    }

    // ---------------- Public Checks ----------------

    public bool IsChasing()
    {
        return playerDetected;
    }

    public bool IsMoving()
    {
        return animator.GetFloat("Speed") > 0.01f;
    }

    public bool IsNearCabinet(Transform cabinet, float range)
    {
        return Vector2.Distance(transform.position, cabinet.position) <= range;
    }

    // ---------------- Collisions ----------------

    // Trigger jumpscare when colliding with player
    private void HandlePlayerCollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AnurodelaJumpscare jumpscare = FindObjectOfType<AnurodelaJumpscare>();
            if (jumpscare != null)
            {
                jumpscare.PlayJumpscare();
            }
        }
    }
}

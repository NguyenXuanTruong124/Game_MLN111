using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    private bool isWaitingAtPoint = false;
    private float waitTime = 1.5f;
    private float waitTimer = 0f;

    [Header("Dive Attack")]
    public float diveSpeed = 8f;
    public float diveCooldown = 5f;
    public float diveWarningTime = 1f;
    private float diveTimer = 0f;
    private bool isDiving = false;
    private bool isDiveCharging = false;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootInterval = 2f;
    private float shootTimer = 0f;

    [Header("References")]
    public Transform player;
    private Animator animator;
    private Rigidbody2D rb;

    [Header("State")]
    private bool isHurt = false;
    private float hurtCooldown = 0.5f;
    private float hurtTimer = 0f;

    private Vector2 diveTargetPosition; // Thêm biến này để lưu vị trí mục tiêu dive

    [Header("Phase 2 Settings")]
    public Transform phase2Waypoint;
    public float phase2MoveSpeed = 4f;
    public float phase2DiveSpeed = 14f;
    public float phase2ShootInterval = 0.7f;
    public float phase2ChargeTime = 3f; // Thời gian tích tụ năng lượng (giây)
    private bool isChargingPhase2 = false;
    private float phase2ChargeTimer = 0f;

    private bool isPhase2 = false;
    private bool isMovingToPhase2Waypoint = false;
    private bool hasBuffedPhase2 = false;

    [Header("Summon Settings")]
    private int maxSummonedEnemies = 4;
    public GameObject enemyPrefab; // Prefab quái triệu hồi
    public Transform[] spawnPoints; // Các điểm spawn
    public float summonInterval = 5f; // Thời gian giữa các lần triệu hồi
    private float summonTimer = 0f;
    private bool canSummon = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        shootTimer = shootInterval;
        diveTimer = diveCooldown;

        BossHealth health = GetComponent<BossHealth>();
        if (health != null)
        {
            health.OnPhase2 += EnterPhase2;
        }
    }

    void EnterPhase2()
    {
        Debug.Log("PHASE 2 TỚI! Boss đã vào giai đoạn 2!");
        isPhase2 = true;
        isMovingToPhase2Waypoint = true;
        canSummon = true; // Bắt đầu cho phép triệu hồi quái
        summonTimer = summonInterval; // Reset timer
    }

    void Update()
    {
        if (isChargingPhase2)
        {
            rb.linearVelocity = Vector2.zero;
            animator.Play("Eagle-Animation"); // hoặc animation tích tụ năng lượng
            phase2ChargeTimer -= Time.deltaTime;
            if (phase2ChargeTimer <= 0f)
            {
                isChargingPhase2 = false;
                BuffPhase2Stats();
            }
            return;
        }
        if (isMovingToPhase2Waypoint && patrolPoints.Length > 0)
        {
            MoveToPhase2Waypoint();
            return;
        }

        if (isHurt)
        {
            hurtTimer -= Time.deltaTime;
            if (hurtTimer <= 0f)
            {
                isHurt = false;
            }
            return;
        }

        if (isDiveCharging)
        {
            rb.linearVelocity = Vector2.zero;
            return; // đứng im trong thời gian báo hiệu dive
}

        if (isDiving)
        {
            HandleDiveMovement();
            return;
        }

        HandlePatrol();
        HandleShoot();
        HandleDiveCheck();
        // Triệu hồi quái phase 2
        if (canSummon)
        {
            summonTimer -= Time.deltaTime;
            if (summonTimer <= 0f)
            {
                SummonEnemies();
                summonTimer = summonInterval;
            }
        }
    }

    void HandlePatrol()
    {
        if (patrolPoints.Length == 0 || isWaitingAtPoint) return;

        Vector2 target = patrolPoints[currentPatrolIndex].position;
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        HandleFlip(direction);
        if (Vector2.Distance(transform.position, target) < 0.2f)
        {
            rb.linearVelocity = Vector2.zero;
            isWaitingAtPoint = true;
            waitTimer = waitTime;
            Invoke(nameof(ContinuePatrol), waitTime);
        }

        animator.Play("Eagle-Animation");
    }

    void ContinuePatrol()
    {
        isWaitingAtPoint = false;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void HandleShoot()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootInterval;
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null && player != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                Vector2 dir = (player.position - firePoint.position).normalized;
                bulletRb.linearVelocity = dir * 8f;
            }
        }
    }

    void HandleDiveCheck()
    {
        if (player == null) return;

        diveTimer -= Time.deltaTime;
        if (Vector2.Distance(transform.position, player.position) < 5f && diveTimer <= 0f)
        {
            StartCoroutine(ChargeDive());
            diveTimer = diveCooldown;
        }
    }

    System.Collections.IEnumerator ChargeDive()
    {
        isDiveCharging = true;
        rb.linearVelocity = Vector2.zero;
        animator.Play("Eagle-dive-attack-Animation"); // hoặc tạo animation riêng cho báo hiệu
        yield return new WaitForSeconds(diveWarningTime);

        isDiveCharging = false;
        isDiving = true;
        if (player != null)
            diveTargetPosition = player.position; // Lưu vị trí player tại thời điểm bắt đầu dive
    }

    void HandleDiveMovement()
    {
        Vector2 direction = (diveTargetPosition - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * diveSpeed;
        HandleFlip(direction);
        // Nếu gần điểm target thì dừng dive
        if (Vector2.Distance(transform.position, diveTargetPosition) < 0.5f)
        {
            StopDive();
        }
    }

    void StopDive()
    {
        isDiving = false;
        rb.linearVelocity = Vector2.zero;
        animator.Play("Eagle-Animation");
    }
void MoveToPhase2Waypoint()
    {
        if (patrolPoints.Length == 0) return;
        Vector2 target = patrolPoints[currentPatrolIndex].position;
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * phase2MoveSpeed;
        HandleFlip(direction);
        animator.Play("Eagle-Animation");

        if (Vector2.Distance(transform.position, target) < 0.2f)
        {
            rb.linearVelocity = Vector2.zero;
            isMovingToPhase2Waypoint = false;
            isChargingPhase2 = true;
            phase2ChargeTimer = phase2ChargeTime;
            Debug.Log("Boss bắt đầu tích tụ năng lượng phase 2!");
            // Có thể bật animation hoặc hiệu ứng ở đây
        }
    }

    void BuffPhase2Stats()
    {
        if (hasBuffedPhase2) return;
        moveSpeed = phase2MoveSpeed;
        diveSpeed = phase2DiveSpeed;
        shootInterval = phase2ShootInterval;
        hasBuffedPhase2 = true;
        Debug.Log("Boss đã tích tụ năng lượng xong! Tăng sức mạnh phase 2!");
        // Có thể thêm hiệu ứng hoặc animation ở đây
    }

    public void Hurt()
    {
        animator.Play("Eagle-hurt-Animation");
        isHurt = true;
        hurtTimer = hurtCooldown;
        rb.linearVelocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Hurt();
            if (isDiving) StopDive();
            // Gọi BossHealth nếu có
            BossHealth health = GetComponent<BossHealth>();
            if (health != null)
            {
                health.TakeDamage(10); 
            }
        }
        if (isDiving && other.CompareTag("Player"))
        {
            StopDive();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDiving && collision.gameObject.CompareTag("Ground"))
        {
            StopDive();
        }
        if (isDiving && collision.gameObject.CompareTag("Player"))
        {
            StopDive();
        }
    }
    void HandleFlip(Vector2 moveDirection)
{
    if (moveDirection.x > 0.1f)
        transform.localScale = new Vector3(-3, 3, 1); // sang phải
    else if (moveDirection.x < -0.1f)
        transform.localScale = new Vector3(3, 3, 1);  // sang trái
}

    void SummonEnemies()
    {
        if (enemyPrefab == null || spawnPoints == null || spawnPoints.Length == 0) return;
        int currentEnemies = GameObject.FindGameObjectsWithTag(enemyPrefab.tag).Length;
        if (currentEnemies >= maxSummonedEnemies) return;
        foreach (Transform spawn in spawnPoints)
        {
            // Kiểm tra lại sau mỗi lần spawn
            currentEnemies = GameObject.FindGameObjectsWithTag(enemyPrefab.tag).Length;
            if (currentEnemies >= maxSummonedEnemies) break;
            Instantiate(enemyPrefab, spawn.position, Quaternion.identity);
        }
        Debug.Log("Boss đã triệu hồi quái!");
    }
}
using UnityEngine;

public class BatChase : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 5f;
    public float speed = 3f;

    private Vector3 originalPosition;
    private bool isChasing = false;
    private Animator anim;

    void Start()
    {
        originalPosition = transform.position;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            isChasing = true;
        }
        else if (isChasing && distanceToPlayer > chaseRange + 1f)
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            ReturnToOriginal();
        }

        // Animation bay
        if (anim != null)
        {
            bool isFlying = isChasing || Vector2.Distance(transform.position, originalPosition) > 0.1f;
            anim.SetBool("isFlying", isFlying);
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        FlipSprite(direction.x);
    }

    void ReturnToOriginal()
    {
        Vector2 direction = (originalPosition - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);

        FlipSprite(direction.x);
    }

    void FlipSprite(float xDirection)
    {
        if (xDirection > 0.1f)
            transform.localScale = new Vector3(1, 1, 1); // Quay mặt phải
        else if (xDirection < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1); // Quay mặt trái
    }
}

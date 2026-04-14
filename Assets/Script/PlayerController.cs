using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float hurtDuration = 1f;

    private Animator animator;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool isGrounded;
    private bool isHurting = false;
    private float hurtTimer = 0f;
    
    private float highJumpForce; // Lực nhảy cao hơn
    private bool canDoubleJump = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        highJumpForce = jumpForce * 1.5f;
    }

    private void Update()
    {
        if (isHurting)
        {
            hurtTimer -= Time.deltaTime;
            if (hurtTimer <= 0f)
            {
                isHurting = false;
                animator.SetBool("isHurting", false);
            }
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // ✔️ Xử lý nhảy + double jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                canDoubleJump = true;
                PlayJumpSound();
            }
            else if (canDoubleJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, highJumpForce);
                canDoubleJump = false;
                PlayJumpSound();
            }
        }

        HandleMovement();
        UpdateAnimation();

        float moveX = Input.GetAxis("Horizontal");
        bool isRunning = Mathf.Abs(moveX) > 0.1f && isGrounded;

        if (isRunning)
        {
            if ((!audioSource.isPlaying) || audioSource.clip != AudioManager.Instance.runSFX.clip)
            {
                audioSource.clip = AudioManager.Instance.runSFX.clip;
                audioSource.loop = true;

                if (AudioManager.Instance.IsSFXOn())
                    audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying && audioSource.clip == AudioManager.Instance.runSFX.clip)
            {
                audioSource.Stop();
                audioSource.clip = null;
            }
        }
    }

    private void HandleMovement()
    {
        if (isHurting) return;

        float moveX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        if (moveX > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveX < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f && isGrounded;
        bool isJumping = !isGrounded && rb.linearVelocity.y > 0f;
        bool isFalling = !isGrounded && rb.linearVelocity.y < 0f;

        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", isFalling);
    }

    public void HurtPlayer()
    {
        if (isHurting) return;

        isHurting = true;
        hurtTimer = hurtDuration;
        animator.SetBool("isHurting", true);

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        AudioManager.Instance.PlayHurt();
    }

    private void PlayJumpSound()
    {
        if (audioSource.isPlaying && audioSource.clip == AudioManager.Instance.runSFX.clip)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
        AudioManager.Instance.PlayJump();
    }
}

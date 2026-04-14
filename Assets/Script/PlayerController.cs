using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    private Animator animator;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool isGrounded;
    
    private float highJumpForce;
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        Debug.Log($"isGrounded: {isGrounded}, Velocity X: {rb.linearVelocity.x}");

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
        Debug.Log($"moveX: {moveX}, isRunning: {isRunning}");

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
        float moveX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        if (moveX > 0) 
        {
            Vector3 scale = transform.localScale;
            transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
        }
        else if (moveX < 0) 
        {
            Vector3 scale = transform.localScale;
            transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);
        }
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f && isGrounded;
        Debug.Log($"UpdateAnimation - isRunning: {isRunning}");

        animator.SetBool("isRunning", isRunning);
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

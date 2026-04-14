using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    private bool isFacingRight = true; // hướng mặc định là bên phải
    public AudioClip shootSound;
    private AudioSource audioSource;
    private int bulletsShotThisBeat = 0;
    private float beatStartTime = 0f;
    private float beatDuration = 1f;
    private int maxBulletsPerBeat = 2;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        beatStartTime = Time.time;
    }
    void Update()
    {
        // Reset nhịp nếu đã qua 2s
        if (Time.time - beatStartTime >= beatDuration)
        {
            bulletsShotThisBeat = 0;
            beatStartTime = Time.time;
        }
        // Di chuyển để xác định hướng
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput > 0)
            isFacingRight = true;
        else if (moveInput < 0)
            isFacingRight = false;

        // Bắn
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (bulletsShotThisBeat < maxBulletsPerBeat)
            {
                Shoot();
                bulletsShotThisBeat++;
            }
            // Nếu đã đủ số viên, không bắn nữa cho đến nhịp mới
        }
    }

    void Shoot()
    {
        if (shootSound != null)
            audioSource.PlayOneShot(shootSound);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            float direction = isFacingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(direction * bulletSpeed, 0f);
        }
    }
}

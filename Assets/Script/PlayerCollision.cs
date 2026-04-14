using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerHealth playerHealth;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        playerHealth = GetComponent<PlayerHealth>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gem"))
        {
            AudioManager.Instance.PlayCoin();
            Destroy(collision.gameObject);
            gameManager.AddScore(1);
        }
        else if (collision.CompareTag("Enemy"))
        {
            if (!playerHealth.IsInvulnerable())
            {
                Debug.Log("Player collided with an enemy!");
                GetComponent<PlayerController>().HurtPlayer();
                playerHealth.TakeDamage(10);
                
            }
        }
        else if (collision.CompareTag("Key"))
        {

            Debug.Log("Player collected the key — WIN!");
            
            Destroy(collision.gameObject);
            FindAnyObjectByType<GameUIManager>().ShowGameWin();
            AudioManager.Instance.StopAllEffects();
            

        }
        // Thêm xử lý cho EnemyBullet và BossBullet
        else if (collision.CompareTag("EnemyBullet"))
        {
            if (!playerHealth.IsInvulnerable())
            {
                Debug.Log("Player collided with bbullet!");
                GetComponent<PlayerController>().HurtPlayer();
                playerHealth.TakeDamage(10); // hoặc lấy damage từ script đạn nếu muốn
                // Nếu muốn đạn tự hủy khi trúng player:
                Destroy(collision.gameObject);
                
            }
            else
            {
                Destroy(collision.gameObject); // vẫn hủy đạn nếu bất tử
            }
        }
        // Thêm xử lý cho Boss (nếu muốn boss chạm vào là mất máu)
        else if (collision.CompareTag("Boss"))
        {
            if (!playerHealth.IsInvulnerable())
            {
                Debug.Log("Player collided with Boss!");
                GetComponent<PlayerController>().HurtPlayer();
                playerHealth.TakeDamage(10); // hoặc damage khác nếu muốn
                
            }
        }
        else if (collision.CompareTag("Heal"))
        {
            playerHealth.Heal(20); // hoặc giá trị bạn muốn
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Invisible"))
        {
            playerHealth.GrantVulnerability();
            Destroy(collision.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Debug.Log("Player collided with trap!");
            GetComponent<PlayerController>().HurtPlayer();
            playerHealth.TakeDamage(5);
            
        }
        else if (collision.gameObject.CompareTag("DieTrap"))
        {
            Debug.Log("Player collided with trap!");
            GetComponent<PlayerController>().HurtPlayer();
            playerHealth.TakeDamage(100);
            
        }
    }
}
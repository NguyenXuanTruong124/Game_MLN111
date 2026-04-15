using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            AudioManager.Instance.PlayCoin();
            Destroy(collision.gameObject);
            gameManager.AddScore(1);
        }

        else if (collision.CompareTag("Cloth"))
        {
            AudioManager.Instance.PlayCoin();
            Destroy(collision.gameObject);
            gameManager.AddClothScore(1);
        }

        else if (collision.CompareTag("Coin"))
        {
            AudioManager.Instance.PlayCoin();
            Destroy(collision.gameObject);
            gameManager.AddScore(1);
        }

        else if (collision.CompareTag("Key"))
        {
            // Kiểm tra điều kiện win
            if (gameManager.CanWin())
            {
                Debug.Log("Player collected the key — WIN!");
                Destroy(collision.gameObject);
                FindAnyObjectByType<GameUIManager>().ShowGameWin();
                AudioManager.Instance.StopAllEffects();
            }
            else
            {
                Debug.Log("Bạn chưa đủ item để nhặt key!");
                // Có thể thêm hiệu ứng/âm thanh báo lỗi
                AudioManager.Instance.PlayCoin(); // Hoặc sound khác
            }
        }
    }
}
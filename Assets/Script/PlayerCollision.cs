using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private RequirementNotificationUI requirementUI;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        requirementUI = FindAnyObjectByType<RequirementNotificationUI>();
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
            gameManager.AddCoinScore(1);
        }

        else if (collision.CompareTag("Book"))
        {
            AudioManager.Instance.PlayCoin();
            Destroy(collision.gameObject);
            gameManager.AddBookScore(1);
        }

        else if (collision.CompareTag("Key"))
        {
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
                // Hiển thị thông báo yêu cầu
                if (requirementUI != null)
                {
                    requirementUI.ShowRequirement();
                }
            }
        }
    }
}
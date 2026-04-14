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

        else if (collision.CompareTag("Key"))
        {

            Debug.Log("Player collected the key — WIN!");

            Destroy(collision.gameObject);
            FindAnyObjectByType<GameUIManager>().ShowGameWin();
            AudioManager.Instance.StopAllEffects();


        }
    }
}
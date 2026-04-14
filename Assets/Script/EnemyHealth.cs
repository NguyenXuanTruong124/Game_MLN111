using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    private Animator animator;
    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public GameObject healthItemPrefab;
    public GameObject vulnerabilityItemPrefab;

    void Die()
    {
        Debug.Log("Enemy died!");
        float rand = Random.value; // giá trị từ 0 đến 1
        if (rand < 0.15f) // 15% invisible
        {
            if (vulnerabilityItemPrefab != null)
                Instantiate(vulnerabilityItemPrefab, transform.position, Quaternion.identity);
        }
        else if (rand < 0.45f) // 30% heal (0.15 -> 0.45)
        {
            if (healthItemPrefab != null)
                Instantiate(healthItemPrefab, transform.position, Quaternion.identity);
        }
        // 55% còn lại không rơi gì
        Destroy(gameObject);
    }
}
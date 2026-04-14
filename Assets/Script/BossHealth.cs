
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 200;
    private int currentHealth;
    private bool hasTriggeredPhase2 = false;

    // Thêm biến miễn nhiễm sát thương
    private bool isInvulnerable = false;
    public float invulnerableDuration = 0.5f;
    private float invulnerableTimer = 0f;

    public delegate void Phase2Triggered();
    public event Phase2Triggered OnPhase2;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isInvulnerable)
        {
            invulnerableTimer -= Time.deltaTime;
            if (invulnerableTimer <= 0f)
            {
                isInvulnerable = false;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable) return; // Nếu đang miễn nhiễm, bỏ qua sát thương

        currentHealth -= amount;
        isInvulnerable = true;
        invulnerableTimer = invulnerableDuration;

        Debug.Log($"Boss current health: {currentHealth}");

        // Gọi phase 2 nếu máu <= 100 và chưa gọi lần nào
        if (!hasTriggeredPhase2 && currentHealth <= 100)
        {
            hasTriggeredPhase2 = true;
            OnPhase2?.Invoke();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private Animator animator;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameUIManager uiManager;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    private bool isInvulnerable = false;
    private float invulnerableTimer = 0f;
    public float invulnerableDuration = 10f; // thời gian miễn nhiễm khi nhặt item (đổi thành 5s)

    public void Heal(int amount)
    {
        if (currentHealth >= maxHealth) return;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        if (healthSlider != null)
            healthSlider.value = currentHealth;
        Debug.Log("Player healed! Current HP: " + currentHealth);
    }

    public void GrantVulnerability()
    {
        isInvulnerable = true;
        invulnerableTimer = invulnerableDuration;
        Debug.Log("Player is now invulnerable!");
    }

    void Update()
    {
        if (isInvulnerable)
        {
            invulnerableTimer -= Time.deltaTime;
            if (invulnerableTimer <= 0f)
            {
                isInvulnerable = false;
                Debug.Log("Player is no longer invulnerable!");
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable) return; // miễn nhiễm sát thương
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        Debug.Log("Player took damage! Current HP: " + currentHealth);

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");

        // Gọi UI Game Over
        if (uiManager != null)
        {
            uiManager.ShowGameOver();
        }
        AudioManager.Instance.StopAllEffects();
        // Tắt di chuyển của player
        var moveScript = GetComponent<PlayerController>(); 
        if (moveScript != null)
        {
            moveScript.enabled = false;
        }
    }
    public int get => currentHealth;

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }
}
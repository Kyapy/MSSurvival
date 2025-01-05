    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController Instance;

    private void Awake()
    {
        Instance = this;

    }

    public float currnetHealth, maxHealth;
    public bool isInvulerable = false;
    public Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerController.instance.playerStats != null)
        {
            maxHealth = PlayerController.instance.playerStats.health;
            currnetHealth = maxHealth;

            healthSlider.maxValue = maxHealth;
            healthSlider.value = currnetHealth;
        }
        else
        {
            Debug.LogWarning("PlayerStatController is not initialized");
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currnetHealth;
    }


    public void TakeDamage(float damageToTake, Transform enemyTransform, float knockBackForce = 1f, float knockBackDuration = 0.5f)
    {
        if (PlayerController.instance.isInvulerable) return;

        // Reduce health
        currnetHealth -= damageToTake;

        PlayerController.instance.isInvulerable = true;

        // Calculate knockback direction
        Vector2 knockbackDirection = (transform.position - enemyTransform.position).normalized;

        // If collide enemy at the exact same position, randomise the knockback direction
        if (knockbackDirection == Vector2.zero)
        {
            knockbackDirection = (new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f))).normalized;
        }

        // Apply knockback
        PlayerController.instance.ApplyKnockback(knockbackDirection, knockBackForce, knockBackDuration);




        // Update health slider
        healthSlider.value = currnetHealth;

        if (currnetHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}

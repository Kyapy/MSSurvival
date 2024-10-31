using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController Instance;
    PlayerStatController playerStats;

    private void Awake()
    {
        Instance = this;

    }

    public float currnetHealth, maxHealth;

    public Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponent<PlayerStatController>();

        if (PlayerStatController.instance != null)
        {
            maxHealth = playerStats.health;
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


    public void TakeDamage(float damangeToTake)
    {
        currnetHealth -= damangeToTake;

        if (currnetHealth <= 0)
        {
            gameObject.SetActive(false);
        }

        healthSlider.value = currnetHealth; 
    }
}

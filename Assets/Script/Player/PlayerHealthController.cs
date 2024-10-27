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

    public Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = PlayerStatController.instance.health;
        currnetHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currnetHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
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

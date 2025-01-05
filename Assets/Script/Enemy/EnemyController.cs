using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Internal Components")]
    public Rigidbody2D theRB;
    public SpriteRenderer spriteRender;
    public Animator anim;
    public float moveSpeed;
    public Transform target;

    [Header("Stats")]
    public float damage;
    public bool canMove;
    public bool isInvulnerable = false;
    public float hitWaitTime = 1f;
    public float hitConuter;

    public float health = 5;

    public float knockBackTime = 0.5f;
    public float knockBackCounter;

    public int expToGive = 1;

    public int coinValue = 1;
    public float coinDropRate = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        target = FindObjectOfType<PlayerController>().transform;
        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();    
    }

    public void ChangeSpriteDirection()
    {
        if (theRB.velocity.x != 0f)
        {
            spriteRender.flipX = theRB.velocity.x > 0f;
        }

        if (hitConuter > 0f)
        {
            hitConuter -= Time.deltaTime;
        }
    }
    public void MoveTowardPlayer()
    {
        theRB.velocity = (target.position - transform.position).normalized * moveSpeed;
    }

    public void OnCollisionStay2D(Collision2D collision)
    {

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Hiting & Damaging player logic
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealthController.Instance.TakeDamage(damage, transform, 20f);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Hiting & Damaging player logic
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealthController.Instance.TakeDamage(damage, transform, 20f);
        }
    }
    
    public virtual void Death()
    {
            // Kill enemy
            Destroy(gameObject);


            // Drop exp
            ExperienceLevelController.Instance.SpawnExp(transform.position, expToGive);

            if (Random.value <= coinDropRate)
            {
                CoinController.instance.DropCoin(transform.position, coinValue);
            }
        
    }

    public virtual void TakeDamage(float damageToTake)
    {
        // Enemey take damage
        health -= damageToTake;

        DamageNumberController.instance.SpawnDamage(damageToTake, transform.position);

        if (health <= 0f)
        {
            Death();
        }

    }

    public virtual void TakeDamage(float damageToTake, bool shouldKnockBack)
    {
        TakeDamage(damageToTake);

        // check if enemy is knockable, boss monsters should not be knockable
        if (shouldKnockBack)
        {
            knockBackCounter = knockBackTime;
        }
    }
}

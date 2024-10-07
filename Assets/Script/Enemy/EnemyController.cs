using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D theRB;
    private SpriteRenderer spriteRender;
    public float moveSpeed;
    private Transform target;

    public float damage;

    public float hitWaitTime = 1f;
    private float hitConuter;

    public float health = 5;

    public float knockBackTime = 0.5f;
    public float knockBackCounter;

    public int expToGive = 1;
    // Start is called before the first frame update
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        target = FindObjectOfType<PlayerController>().transform;
        theRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Knockback logic
        if (knockBackCounter > 0)
        {
            knockBackCounter -= Time.deltaTime;

            if (moveSpeed > 0)
            {
                moveSpeed = -moveSpeed * 2;
            }

            if (knockBackCounter <= 0)
            {
                moveSpeed = Mathf.Abs(moveSpeed * 0.5f);
            }
        }

        // Swap sprite direction
        theRB.velocity = (target.position - transform.position).normalized * moveSpeed;

        if (theRB.velocity.x != 0f)
        {
            spriteRender.flipX = theRB.velocity.x > 0f;
        }    

        if (hitConuter > 0f)
        {
            hitConuter -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Hiting & Damaging player logic
        if (collision.gameObject.tag == "Player" && hitConuter <= 0f)
        {
            PlayerHealthController.Instance.TakeDamage(damage);

            hitConuter = hitWaitTime;
        }
    }

    public void TakeDamage(float damageToTake)
    {
        // Enemey take damage
        health -= damageToTake;
        if (health <= 0f)
        {
            // Kill enemy
            Destroy(gameObject);

            // Drop exp
            ExperienceLevelController.Instance.SpawnExp(transform.position, expToGive);   
        }

        DamageNumberController.instance.SpawnDamage(damageToTake, transform.position);
     
    }

    public void TakeDamage(float damageToTake, bool shouldKnockBack)
    {
        TakeDamage(damageToTake);

        // check if enemy is knockable, boss monsters should not be knockable
        if (shouldKnockBack)
        {
            knockBackCounter = knockBackTime;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    // Damage
    public float damageAmount;

    // Starting Spawn animation
    public float lifeTime, growSpeed = 5f;
    public Vector3 targetSize;

    // Does it knock back enemies?
    public bool shouldKnockBack;

    public bool destoryParent;

    // Scale animation?
    public bool ScaleAnimation;

    // Properties for AOE weapon
    public bool damageOverTime;
    public float timeBetweenDamage;
    private float damageCounter;
    private List<EnemyController> enemiesInRange = new List<EnemyController>();

    // Properties for projectile
    public bool destoryOnHit;



    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject, lifeTime);

        targetSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, growSpeed * Time.deltaTime);

        if (damageOverTime == true)
        {
            damageCounter -= Time.deltaTime;

            if (damageCounter <= 0)
            {
                damageCounter = timeBetweenDamage;

                for (int i = 0; i < enemiesInRange.Count; i++)
                {
                    if (enemiesInRange[i] != null)
                    {
                        enemiesInRange[i].TakeDamage(damageAmount, shouldKnockBack);
                    }
                    else
                    {
                        enemiesInRange.RemoveAt(i);
                        i--; 
                    }
                }
            }
        }
        else
        {
            lifeTime -= Time.deltaTime;

            if (lifeTime <= 0)
            {
                targetSize = Vector3.zero;

                if (transform.localScale.x == 0f)
                {
                    Destroy(gameObject);

                    if (destoryParent)
                    {
                        Destroy(transform.parent.gameObject);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( damageOverTime == false)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<EnemyController>().TakeDamage(damageAmount, shouldKnockBack);

                if (destoryOnHit == true)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (collision.tag == "Enemy")
            {
                enemiesInRange.Add(collision.GetComponent<EnemyController>());
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (damageOverTime == true)
        {
            if (collision.tag == "Enemy")
            {
                enemiesInRange.Remove(collision.GetComponent<EnemyController>());
            }
        }
    }
}

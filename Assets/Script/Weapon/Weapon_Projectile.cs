using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Projectile : Weapon
{
    public EnemyDamager damager;
    public Projectile projectile;

    private float hitCounter;

    public float weaponRange;
    public LayerMask whatIsEnemy;

    // Start is called before the first frame update
    void Start()
    {
        SetStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (statsUpdated == true)
        {
            statsUpdated = false;

            SetStats();
        }

        hitCounter -= Time.deltaTime;
        if (hitCounter <= 0)
        {
            hitCounter = stats[weaponLevel].timeBetweenAttacks;

            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange * stats[weaponLevel].range, whatIsEnemy);
            if (enemies.Length > 0)
            {
                for (int i = 0; i < stats[weaponLevel].amount; i++)
                {
                    Vector3 targetPosition = enemies[Random.Range(0, enemies.Length)].transform.position;

                    Vector3 direction = targetPosition - transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    angle -= 90;
                    projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    Instantiate(projectile, projectile.transform.position, projectile.transform.rotation).gameObject.SetActive(true);
                }
            }
        }
    }

    void SetStats()
    {
        damager.damageAmount = stats[weaponLevel].damage;

        damager.lifeTime = stats[weaponLevel].duration;

        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;

        hitCounter = 0f;

        projectile.moveSpeed = stats[weaponLevel].speed;
    }
}

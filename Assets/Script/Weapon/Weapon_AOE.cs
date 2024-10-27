using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_AOE : Weapon
{
    public EnemyDamager damager;

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
    }

    void SetStats()
    {
        damager.damageAmount = stats[weaponLevel].damage;

        damager.timeBetweenDamage = stats[weaponLevel].speed;

        damager.targetSize = Vector3.one * (stats[weaponLevel].range * 3);
    }
}

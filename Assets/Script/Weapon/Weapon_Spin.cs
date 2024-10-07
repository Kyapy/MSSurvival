using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Spin : Weapon
{
    public float rotateSpeed;

    public Transform holder, leekToSpawn;

    public float timeBtweenSpawn;
    private float spawnCounter;

    public EnemyDamager damager;
    // Start is called before the first frame update
    void Start()
    {
        SetStats();

        //UIController.Instance.LevelUpButtons[0].UpdateButtonDisplay(this);
    }

    // Update is called once per frame
    void Update()
    {
        //holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));
        holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime * stats[weaponLevel].speed));

        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0)
        {
            spawnCounter = timeBtweenSpawn;

            Instantiate(leekToSpawn, leekToSpawn.position, leekToSpawn.rotation, holder).gameObject.SetActive(true);

        }

        if (statsUpdated)
        {
            statsUpdated = false;
            
            SetStats();
        }
    }

    public void SetStats()
    {
        damager.damageAmount = stats[weaponLevel].damage;

        transform.localScale = Vector3.one * stats[weaponLevel].range;

        timeBtweenSpawn = stats[weaponLevel].timeBetweenAttacks;

        damager.lifeTime = stats[weaponLevel].duration;

        spawnCounter = 0f;
    }
}



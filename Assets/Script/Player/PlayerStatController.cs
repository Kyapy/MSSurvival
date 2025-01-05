using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatController : MonoBehaviour
{
    public static PlayerStatController instance;

    private void Awake()
    {
        instance = this;
        InitializeStats();
    }

    // Set stats from base stats
    public BaseStats baseStats;

    [Header("Offensive Stats")]
    public float damage;
    public float damageMultiplier;
    public float criticalRate;
    public float criticalDamageMultiplier;
    public float weaponRange;
    public float attackSpeed;
    public float knockbackTime;
    public float knockbackForce;

    [Header("Defensive Stats")]
    public int health;
    public float damageReduction;
    public float blockRate;
    public float invulerableDuration;

    [Header("Movement Stats")]
    public float moveSpeed;
    public float pickUpRange;

    private void Start()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        if (baseStats != null)
        {
            damage = baseStats.damage;
            damageMultiplier = baseStats.damageMultifier;
            criticalRate = baseStats.criticalRate;
            criticalDamageMultiplier = baseStats.criticalDamageMultifier;
            weaponRange = baseStats.weaponRange;
            attackSpeed = baseStats.attackSpeed;

            health = baseStats.Health;
            damageReduction = baseStats.damageReduction;
            blockRate = baseStats.blockRate;
            invulerableDuration = baseStats.invulerableDuration;

            moveSpeed = baseStats.moveSpeed;
            pickUpRange = baseStats.pickupRange;
        }
    }

}

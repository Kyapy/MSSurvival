using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBaseStats", menuName = "Stats/Base Stats")]
public class BaseStats : ScriptableObject 
{
    [Header("Offensive Stats")]
    public float damage;
    public float damageMultifier = 1.0f;
    public float criticalRate;
    public float criticalDamageMultifier = 150f;
    public float weaponRange;
    public float attackSpeed;
    public float knockbackTime;
    public float knockbackForce;

    [Header("Defencive Stats")]
    public int Health;
    public float damageReduction;
    public float blockRate;

    [Header("Movement Stats")]
    public float moveSpeed = 1f;
    public float pickupRange = 1f;
}


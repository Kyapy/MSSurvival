using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VisionOS;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class EnemyController_Boss_Slime : EnemyController
{
    private enum BossSlimeState { Idle, Run, Attack, Jump, InAir, Landing, Summon, Death }

    [Header("Internal Status")]
    private BossSlimeState state;
    public bool stateComplete = true;
    public bool isDead = false;

    private float counter, movingCounter;
    public GameObject slimeSummon;

    [Header("Global Cooldowns")]
    public float globalCooldown;
    private float globalCooldownCounter;

    [Header("Move Lists")]
    public MoveStats summon;
    public MoveStats jump;
    private readonly List<MoveStats> moveList = new List<MoveStats>();
    private readonly List<MoveStats> onCooldown = new List<MoveStats>();
    public GameObject shadow;

    private void Awake()
    {
        moveList.Add(summon);
        moveList.Add(jump);
        globalCooldownCounter = globalCooldown;

        // Turn off shadow if on
        shadow.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isDead) return;

        ChangeSpriteDirection();
        counter -= Time.deltaTime;
        globalCooldownCounter = Mathf.Max(0, globalCooldownCounter - Time.deltaTime);

        checkCooldowns();

        if (stateComplete)
        {
            selectState();
        }

        UpdateState();
    }

    private void UpdateState()
    {
        switch (state)
        {
            case BossSlimeState.Idle:
                idleUpdate();
                break;
            case BossSlimeState.Run:
                runUpdate();
                break;
            case BossSlimeState.Jump:
                jumpUpdate();
                break;
            case BossSlimeState.InAir:
                inAirUpdate();
                break;
            case BossSlimeState.Landing:
                landingUpdate();
                break;
            case BossSlimeState.Summon:
                summonUpdate();
                break;
            case BossSlimeState.Death:
                // Death logic could be added here if needed.
                break;
        }
    }

    private void selectState()
    {
        stateComplete = false;
        theRB.velocity = Vector2.zero;

        if (globalCooldownCounter <= 0 && chooseMove())
            return;

        runStart();
        counter = movingCounter;
    }

    // State Start and Update Methods
    private void idleStart()
    {
        Debug.Log("Idle Start");
        state = BossSlimeState.Idle;
        anim.SetBool("isMoving", false);
        movingCounter = 1f;
    }

    private void idleUpdate()
    {
        if (counter <= 0) stateComplete = true;
    }

    private void runStart()
    {
        Debug.Log("Run Start");
        state = BossSlimeState.Run;
        anim.SetBool("isMoving", true);
        movingCounter = Random.Range(3, 5);
    }

    private void runUpdate()
    {
        if (globalCooldownCounter <= 0)
        {
            stateComplete = true;
            return;
        }
        MoveTowardPlayer();
    }

    private void jumpStart()
    {
        Debug.Log("Jump Start");
        anim.SetTrigger("jump");
    }

    private void jumpUpdate()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Boss_Slime_inAir"))
        {
            state = BossSlimeState.InAir;
            inAirStart();
        }
    }

    private void inAirStart()
    {
        // trigger inair animtor will make sprite transparent
        Debug.Log("In Air Start");
        // set attack counter back to zero
        jump.counterCount = 0;
        // turn on shadow
        shadow.gameObject.transform.localScale = Vector3.zero;
        shadow.gameObject.SetActive(true);
    }

    private void inAirUpdate()
    {
        jump.counterCount += Time.deltaTime;
        if (jump.counterCount >= jump.counter)
        {
            shadow.gameObject.transform.localScale = Vector3.MoveTowards(shadow.gameObject.transform.localScale, Vector3.one, jump.speed * Time.deltaTime);
        }
        else
        {
            transform.position = target.position + Vector3.up;
        }
 
       

        if (Vector3.Distance(shadow.gameObject.transform.localScale, Vector3.one) < 0.01f)
        {
            shadow.gameObject.transform.localScale = Vector3.one;
            state = BossSlimeState.Landing;
            shadow.gameObject.SetActive(false);
            anim.SetTrigger("land");
        }
    }

    private void landingUpdate()
    {
        // Placeholder for landing update logic if needed.
    }

    private void summonStart()
    {
        Debug.Log("Summon Start");
        anim.SetTrigger("summon");
    }

    private void summonUpdate()
    {
        // Placeholder for summon update logic if needed.
    }

    private void summonSlime()
    {
        Vector3 spawnOffset = spriteRender.flipX ? new Vector3(-3, -2, 0) : new Vector3(3, -2, 0);
        Instantiate(slimeSummon, transform.position + spawnOffset, Quaternion.identity);
        attackEnd();
    }

    private void attackEnd()
    {
        Debug.Log("Move ended, selecting state...");
        anim.SetBool("isMoving", false);
        state = BossSlimeState.Idle;
        stateComplete = true;
        globalCooldownCounter = globalCooldown;
    }

    private void checkCooldowns()
    {
        foreach (MoveStats move in moveList)
        {
            if (move.isUnconditional && move.cooldownCount <= 0)
                Debug.Log($"{move.moveName} is ready to use");
            else
                move.cooldownCount -= Time.deltaTime;
        }
    }

    private bool chooseMove()
    {
        List<MoveStats> availableMoves = moveList.FindAll(move => move.isUnconditional && move.cooldownCount <= 0);
        if (availableMoves.Count > 0)
        {
            MoveStats chosenMove = availableMoves[Random.Range(0, availableMoves.Count)];
            PerformMove(chosenMove);
            return true;
        }

        Debug.Log("No available move to perform");
        return false;
    }

    private void PerformMove(MoveStats move)
    {
        move.cooldownCount = move.cooldown;
        onCooldown.Add(move);

        switch (move.moveName)
        {
            case "jump":
                state = BossSlimeState.Jump;
                jumpStart();
                break;
            case "summon":
                state = BossSlimeState.Summon;
                summonStart();
                break;
            default:
                Debug.LogWarning("Move not recognized: " + move.moveName);
                break;
        }
        globalCooldownCounter = globalCooldown;
    }

    public override void TakeDamage(float damageToTake, bool shouldKnockBack)
    {
        if (!isInvulnerable)
        {
            health -= damageToTake;
            DamageNumberController.instance.SpawnDamage(damageToTake, transform.position);

            if (health <= 0)
            {
                state = BossSlimeState.Death;
                theRB.velocity = Vector2.zero;
                anim.SetTrigger("isDead");
                isInvulnerable = true;
                isDead = true;
            }
        }
    }
}

// Class for each moveset
[System.Serializable]
public class MoveStats
{
    public string moveName;  // Name of the move/skill

    public bool isUnconditional; // can be activated without any prereqquistes

    public float cooldown; // cooldown counters
    public float cooldownCount = 0f;

    public float damage, speed, range; // move stats

    // additional counter if need
    public float counter;
    public float counterCount = 0f;
}

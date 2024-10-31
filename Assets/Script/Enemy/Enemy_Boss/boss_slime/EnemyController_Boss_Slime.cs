using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VisionOS;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class EnemyController_Boss_Slime : EnemyController
{
    enum boss_SlimeState {idle, run, attack, jump,inAir, landing, summon, death}

    boss_SlimeState state;

    [Header("Internal Status")]
    public bool stateComplete = true;

    // Can it be damaged?
    public bool isinvulnerable = false;
    public bool isDead = false;

    private float counter, movingCounter;

    [Header("Global Cooldowns")]
    public float globalCooldown;
    private float globalCooldownCounter;

    // Move list
    [Header("Move Lists")]
    public List<MoveStats> moveList = new List<MoveStats>();

    // Update is called once per frame
    void Update()
    {
        ChangeSpriteDirection();
        counter -= Time.deltaTime;

        // check if global counter is activated
        if (globalCooldownCounter > 0) 
        {
            globalCooldownCounter -= Time.deltaTime;
        }

        if (isDead == false)
        {
            {
                if (stateComplete == true)
                {
                    selectState();
                }

                UpdateState();
            }
        }

    }

    // State handling
    void UpdateState()
    {
        switch (state)
        {
            case boss_SlimeState.idle:
                idleUpdate();
                break;
            case boss_SlimeState.run:
                runUpdate();
                break;
            case boss_SlimeState.attack:
                attackUpdate();
                break;
            case boss_SlimeState.jump:

                break;
            case boss_SlimeState.inAir:

                break;
            case boss_SlimeState.landing:

                break;
            case boss_SlimeState.summon:

                break;
        }
    }

    void selectState()
    {
        stateComplete = false;

        // reset movement to 0
        theRB.velocity = Vector2.zero;

        bool movePerformed = false;

        // check available move when globalcooldown is ready
        if (globalCooldownCounter <= 0)
        {
            Debug.Log("Selected attack state");
            movePerformed = true;  
            attackStart();
            //movePerformed = chooseMove();
        }

        // Run or Idle when no move available
        if (movePerformed == false)
        {
            runStart();
            // counter for run/idle movement
            counter = movingCounter;
        }


    }

    void idleStart()
    {
        Debug.Log("idle start");

        state = boss_SlimeState.idle;
        anim.SetBool("isMoving", false);

        movingCounter = 1f;
    }

    void idleUpdate()
    {
        Debug.Log("idling");

        if (counter <= 0)
        {
            stateComplete = true;
        }
    }

    void runStart()
    {
        Debug.Log("run start");

        state = boss_SlimeState.run;
        anim.SetBool("isMoving", true);

        movingCounter = Random.Range(3, 5);
    }

    void runUpdate()
    {
        Debug.Log("running");

        if (globalCooldownCounter <= 0)
        {
            stateComplete = true;
            return;
        }
        MoveTowardPlayer();
    }

    void attackStart()
    {
        Debug.Log("attack start");
        state = boss_SlimeState.attack;
        anim.SetTrigger("jump");
        //anim.SetTrigger("summon");

    }

    void attackUpdate()
    {
        Debug.Log("attacking");

    }

    void attackEnd()
    {
        Debug.Log("Move ended, selecting state...");
        anim.SetBool("isMoving", false);
        state = boss_SlimeState.idle;
        stateComplete = true;
        globalCooldownCounter = globalCooldown;
    }

    private void checkCooldowns()
    {
        foreach (MoveStats move in moveList)
        {
            if (move.isUnconditional == true && move.cooldownCount <= 0)
            {
                Debug.Log($"{move.moveName} is ready to use");
            }
            else
            {
                move.cooldown -= Time.deltaTime;
            }
        }

    }

    private bool chooseMove()
    {
        // filter available moves with colldowns ready and that are unconditonal
        List<MoveStats> availableMoves = moveList.FindAll(move => move.isUnconditional == true && move.cooldownCount <= 0 ); 

        if (availableMoves.Count > 0)
        {
            MoveStats chosenMove = availableMoves[Random.Range(0, availableMoves.Count)];
            PerformMove(chosenMove);
            return true;
        }
        else
        {
            Debug.Log("No available move to perform");
            return false;
        }
    }

    private void PerformMove(MoveStats move)
    {
        Debug.Log("Performing move: " + move.moveName);
        // reset cooldown for the move
        move.cooldownCount = move.cooldown;
    }


    public void invulerable()
    {
        isinvulnerable = true;
    }
    
    public void vulerable()
    {
        isinvulnerable = false;
    }
    public override void TakeDamage(float damageToTake, bool shouldKnockBack)
    {
        if (isinvulnerable == false) 
        {
            health -= damageToTake;

            DamageNumberController.instance.SpawnDamage(damageToTake, transform.position);

            if (health <= 0f)
            {
                theRB.velocity = Vector2.zero;
                anim.SetTrigger("isDead");
                isinvulnerable = true;
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
}

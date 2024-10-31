using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyController_Normal : EnemyController
{
    void Update()
    {
        // add basic update logics
        MoveTowardPlayer();
        ChangeSpriteDirection();
        Knockback();
    }

    // Knockback logic, normal enemey only 
    public void Knockback()
    {
        // Is player alive
        if (PlayerController.instance.gameObject.activeSelf == true)
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
        }
        else
        {
            theRB.velocity = Vector2.zero;
        }
    }
}

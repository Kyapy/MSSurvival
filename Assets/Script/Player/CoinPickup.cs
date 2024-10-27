using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{

    public int coinAmount;

    private bool movingToPlayer = false;
    public float moveSpeed;

    public float timeBetweenChecks = 0.2f;
    private float checkCounter;

    private PlayerController player;
    private PlayerStatController playerStats;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.instance;
        playerStats = PlayerStatController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingToPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            checkCounter -= Time.deltaTime;
            if (checkCounter <= 0)
            {
                checkCounter = timeBetweenChecks;

                if (Vector3.Distance(transform.position, player.transform.position) < playerStats.pickUpRange)
                {
                    movingToPlayer = true;
                    moveSpeed += playerStats.moveSpeed;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CoinController.instance.AddCoins(coinAmount);
            Destroy(gameObject);
        }

    }
}

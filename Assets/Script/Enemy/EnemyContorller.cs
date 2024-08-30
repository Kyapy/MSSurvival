using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContorller : MonoBehaviour
{
    public Rigidbody2D theRB;
    private SpriteRenderer spriteRender;
    public float moveSpeed;
    private Transform target;

    public float damage;

    public float hitWaitTime = 1f;
    private float hitConuter;
    // Start is called before the first frame update
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        target = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = (target.position - transform.position).normalized * moveSpeed;

        if (theRB.velocity.x != 0f)
        {
            spriteRender.flipX = theRB.velocity.x > 0f;
        }    

        if (hitConuter > 0f)
        {
            hitConuter -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && hitConuter <= 0f)
        {
            PlayerHealthController.Instance.TakeDamage(damage);

            hitConuter = hitWaitTime;
        }
    }
}

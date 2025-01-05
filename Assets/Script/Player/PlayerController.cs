using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("Player Components")]
    public Rigidbody2D rb;
    private SpriteRenderer spriteRender;
    public Animator anim;
    public bool canMove = true;
    public Vector2 moveInput = Vector2.zero;
    public bool isInvulerable = false;

    public PlayerStatController playerStats;

    [Header("Knockback Settings")]
    public bool isKnockbackActive = false;
    private Vector2 knockbackForce = Vector2.zero;
    private float knockbackDuration = 0f;
    private float knockbackTimer = 0f;

    //public Weapon activeWeapon;
    [Header("Weapon Setting")]
    public List<Weapon> unassignedWeapons;
    public List<Weapon> assignedWeapons;
    public int maxWeapons = 3;
    [HideInInspector]
    public List<Weapon> fullyLevelledWeapons = new List<Weapon>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();

        playerStats = PlayerStatController.instance;
        
        // If player has no weapon, assign random weapon
        if (assignedWeapons.Count == 0)
        {
            AddWeapon(Random.Range(0, unassignedWeapons.Count));
        }
        // If player has pre-assigned weapon, activate them
        else
        {
            for (int i = 0; i < assignedWeapons.Count; i++)
            {
                assignedWeapons[i].gameObject.SetActive(true);
            }
        }
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isKnockbackActive)
        {
            // Apply knockback force
            knockbackForce = Vector2.Lerp(knockbackForce, Vector2.zero, 10f * Time.fixedDeltaTime);

            rb.velocity = knockbackForce;

            // Countdown knockback timer
            knockbackTimer -= Time.fixedDeltaTime;

            if (knockbackTimer <= 0)
            {
                // End knockback
                isKnockbackActive = false;
                rb.velocity = Vector2.zero; // Reset velocity
                canMove = true; // Re-enable movement
            }
            return; // Skip regular movement
        }

        if (canMove)
        {
            // Get player input
            moveInput = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            ).normalized;

            // Apply movement based on player input
            rb.velocity = moveInput * playerStats.moveSpeed;

                    // Player sprite direction
            if (moveInput.x != 0)
            {
                spriteRender.flipX = moveInput.x > 0f;
            }

            // Player running animation
            if (moveInput != Vector2.zero)
            {

                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        knockbackForce = direction.normalized * force;
        knockbackTimer = duration;
        isKnockbackActive = true;
        canMove = false;

        StartCoroutine(invulerableRoutine(playerStats.invulerableDuration));

        Debug.Log($"Knockback Applied: Force = {knockbackForce}, Duration = {knockbackTimer}");
    }

    public IEnumerator invulerableRoutine(float duration, float flickerInterval = 0.1f)
    {
        Debug.Log("invulerable started");
        float timer = 0f;

        while (timer < duration)
        {
            spriteRender.enabled = !spriteRender.enabled;
            yield return new WaitForSeconds(flickerInterval);
            timer += flickerInterval;
        }

        spriteRender.enabled = true;    
        isInvulerable = false;
    }

    // Add weapon to player
    public void AddWeapon(int weaponNumber)
    {
        if (weaponNumber < unassignedWeapons.Count)
        {
            assignedWeapons.Add(unassignedWeapons[weaponNumber]);

            unassignedWeapons[weaponNumber].gameObject.SetActive(true);
            unassignedWeapons.RemoveAt(weaponNumber);
        }
    }

    public void AddWeapon(Weapon weaponToAdd)
    {
        weaponToAdd.gameObject.SetActive(true);

        assignedWeapons.Add(weaponToAdd);
        unassignedWeapons.Remove(weaponToAdd);
    }
}

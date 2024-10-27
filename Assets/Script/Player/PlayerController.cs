using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    private Rigidbody2D rb;
    private SpriteRenderer spriteRender;
    public Animator anim;

    private PlayerStatController playerStats;

    //public Weapon activeWeapon;

    public List<Weapon> unassignedWeapons, assignedWeapons;

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
        // Player input movement
        Vector3 moveInput = new Vector3(0f, 0f, 0f);
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Fix dinagonal movement
        moveInput.Normalize();

        // moveSpeed is referenced fromPlayerStatController.instance
        transform.position += moveInput * playerStats.moveSpeed * Time.deltaTime;

        // Player sprite direction
        if (moveInput.x != 0)
        {
            spriteRender.flipX = moveInput.x > 0f;
        }

        // Player running animation
        if (moveInput != Vector3.zero)
        {

            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
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

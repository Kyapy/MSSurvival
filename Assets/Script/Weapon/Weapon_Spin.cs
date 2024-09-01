using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Spin : MonoBehaviour
{
    public float rotateSpeed;

    public Transform holder, leekToSpawn;

    public float timeBtweenSpawn;
    private float spawnCounter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));

        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0)
        {
            spawnCounter = timeBtweenSpawn;

            Instantiate(leekToSpawn, leekToSpawn.position, leekToSpawn.rotation, holder).gameObject.SetActive(true);

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject enemyToSpawn;

    public float timeToSpawn;
    private float spawnCounter;

    public Transform minSpawn, maxSpawn;

    private Transform target;

    private float despawnDistant;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    public int checkPerFrame;
    private int enemyToCheck;
    private int enemyLeft = 0;

    public List<WaveInfo> waves;
    private int currentWave;
    private float waveCounter;


    // Start is called before the first frame update
    void Start()
    {
        //spawnCounter = timeToSpawn;

        target = FindObjectOfType<PlayerController>().transform;

        despawnDistant = Vector3.Distance(transform.position, maxSpawn.position) + 4f;

        currentWave = -1;
        GoToNextWave();
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerHealthController.Instance.gameObject.activeSelf)
        {
            // Do boss wave stuffs
            if (waves[currentWave].bossWave == true)
            {
                {
                    if ((waves[currentWave].bossSpawnAllowed == true))
                    {
                        if (waveCounter > 0)
                        {
                            GameObject newBoss = Instantiate(waves[currentWave].bossPrefab, BossSpawnPoint(), Quaternion.identity);

                            waveCounter -= 1;
                        }
                        else
                        {
                            return;
                        }
                        
                    }
                    else
                    {
                        StartCoroutine(checkEnemiesLeftRoutine(waves[currentWave]));
                    }
                }
            }

            // Do normal wave stuffs
            else
            {
                if (currentWave < waves.Count)
                {
                    waveCounter -= Time.deltaTime;
                    if (waveCounter <= 0)
                    {
                        GoToNextWave();
                    }

                    spawnCounter -= Time.deltaTime;
                    if (spawnCounter <= 0)
                    {
                        // Spawn assigned enemies in intervals

                        spawnCounter = waves[currentWave].timeBetweenSpawns;

                        GameObject newEnemy = Instantiate(waves[currentWave].enemyToSpawn, SelectSpawnPoint(), Quaternion.identity);

                        spawnedEnemies.Add(newEnemy);
                    }
                }

                // Despawning logic if enemies are off the screen
                transform.position = target.position;

                int checkTarget = enemyToCheck + checkPerFrame;

                while (enemyToCheck < checkTarget)
                {
                    if (enemyToCheck < spawnedEnemies.Count)
                    {
                        if (spawnedEnemies[enemyToCheck] != null)
                        {
                            if (Vector3.Distance(transform.position, spawnedEnemies[enemyToCheck].transform.position) > despawnDistant)
                            {
                                Destroy(spawnedEnemies[enemyToCheck]);

                                spawnedEnemies.RemoveAt(enemyToCheck);
                                checkTarget--;
                            }
                            else
                            {
                                enemyToCheck++;
                            }
                        }
                        else
                        {
                            spawnedEnemies.RemoveAt(enemyToCheck);
                            checkTarget--;
                        }
                    }
                    else
                    {
                        enemyToCheck = 0;
                        checkTarget = 0;
                    }
                }
            }
        }
    }

    public Vector3 SelectSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;

        bool spawnVerticalEdge = Random.Range(0f, 1f) > .5f;

        if (spawnVerticalEdge)
        {
            spawnPoint.y = Random.Range(minSpawn.position.y, maxSpawn.position.y);

            if (Random.Range(0f, 1f) > .5f)
            {
                spawnPoint.x = maxSpawn.position.x;
            }
            else spawnPoint.x = minSpawn.position.x;
        }
        else
        {
            spawnPoint.x = Random.Range(minSpawn.position.x, maxSpawn.position.x);

            if (Random.Range(0f, 1f) > .5f)
            {
                spawnPoint.y = maxSpawn.position.y;
            }
            else spawnPoint.y = minSpawn.position.y;
        }

        return spawnPoint;
    }

    public Vector3 BossSpawnPoint()
    {
        Vector3 absoluteDistance = new Vector3(10f, 0f, 0f);
        Vector3 spawnPoint = PlayerController.instance.transform.position + absoluteDistance;

        return spawnPoint;
    }

    public void GoToNextWave()
    {
        currentWave++;

        if (currentWave >= waves.Count)
        {
            currentWave = waves.Count - 1;
        }

        waveCounter = waves[currentWave].waveLength;
        spawnCounter = waves[currentWave].timeBetweenSpawns;
    }

    // Call this when enemy dies
    public void EnemyDied()
    {
        enemyLeft -= 1;
    }

    // Check all enemies are killed before triggering boss waves
    private IEnumerator checkEnemiesLeftRoutine(WaveInfo BossWave)
    {
        while (true) // Loop indefinitely
        {
            // Wait every 2 seconds
            yield return new WaitForSeconds(2f);

            // Find how many EnemyController object exist
            EnemyController[] enemies = FindObjectsOfType<EnemyController>();

            if (enemies.Length == 0)
            {
                // Allow boss to spawn
                BossWave.bossSpawnAllowed = true;

                // Stop routine 
                yield break;
            }
        }
    }


    [System.Serializable]
    public class WaveInfo
    {
        public bool bossWave;

        // For normal waves:
        public GameObject enemyToSpawn;
        public float waveLength = 1f;
        public float timeBetweenSpawns = 1f;

        // For boss waves:
        public GameObject bossPrefab; // If bossWave = true, spawn this
        public bool bossSpawnAllowed = false; // Allow boss to spawn if pass prechecks
        public GameObject bossIntroCutscene; // a UI panel/animation to enable
    }
}

using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Vector3 spawnAreaCenter;
    public Vector3 spawnAreaSize;
    public float maxDistanceFromNavMesh = 2f;

    [Header("Wave Settings")]
    public Wave[] waves;              // Array of ScriptableObject waves
    public float timeBetweenWaves = 5f;

    private int currentWaveIndex = 1;

    [Header("AutoWave Variables")]
    private bool spawning = false;
    private int enemiesAlive = 0;
    
    private int waveSpawnThreshold = 0;
    [SerializeField]
    private GameObject playerRef;
    [SerializeField]
    private EnemyConfig chaserConfig;
    [SerializeField]
    private EnemyConfig orbiterConfig;
    [SerializeField]
    private EnemyConfig flankerConfig;
    List<EnemyConfig> configList = new List<EnemyConfig>();
    
    
    private void OnEnable()
    {
        EnemyDeathHandler.OnAnyEnemyDied += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        EnemyDeathHandler.OnAnyEnemyDied -= HandleEnemyDeath;
    }


    private void Start()
    {
        configList.Add(chaserConfig);
        configList.Add(orbiterConfig);
        configList.Add(flankerConfig);
        Debug.Log("configList count: " + configList.Count);
    }


    private IEnumerator SpawnWave()
    {
        spawning = true;
        Debug.Log($"Starting wave {currentWaveIndex} spawning...");

        for (int i = 0; i < configList.Count; i++)
        {
            Debug.Log($"Spawning enemies for config {i} ({configList[i].enemyPrefab.name}), count: {configList[i].initSpawnCount * currentWaveIndex}");

            for (int j = 0; j < configList[i].initSpawnCount * currentWaveIndex; j++)
            {
                Vector3 spawnPos = GetRandomNavMeshPosition();
                if (spawnPos != Vector3.zero)
                {
                    Vector3 offset = Vector3.zero;
                    EnemySpawnInfo info = configList[i].enemyPrefab.GetComponent<EnemySpawnInfo>();
                    SpawnHeightEnum spawnHeight = info.spawnHeight;

                    if (spawnHeight == SpawnHeightEnum.Ground)
                    {
                        offset = new Vector3(0, 2, 0);
                    }
                    else if (spawnHeight == SpawnHeightEnum.Random)
                    {
                        int randOffset = Random.Range(3, 15);
                        offset = new Vector3(0, 1 + randOffset, 0);
                    }
                    else if (spawnHeight == SpawnHeightEnum.Sky)
                    {
                        offset = new Vector3(0, 20, 0);
                    }

                    GameObject enemy = Instantiate(configList[i].enemyPrefab, spawnPos + offset, Quaternion.identity);
                    enemy.GetComponent<EnemySpawnInfo>().targetRef = playerRef;
                    enemiesAlive++;

                    Debug.Log($"Spawned enemy {j + 1} of config {i} at position {spawnPos + offset}. Enemies alive: {enemiesAlive}");
                }
                else
                {
                    Debug.LogWarning($"Failed to get valid spawn position for enemy {j + 1} of config {i}");
                }

                yield return new WaitForSeconds(configList[i].spawnInterval);
            }
        }

        currentWaveIndex++;
        spawning = false;
        Debug.Log($"Finished spawning wave {currentWaveIndex}. Ready for next wave.");
    }

    private void Update()
    {
        if (enemiesAlive <= waveSpawnThreshold && !spawning)
        {
            Debug.Log("WaveSpawner Update: Conditions met to start new wave.");
            StartCoroutine(SpawnWave());
        }
    }

    private Vector3 GetRandomNavMeshPosition()
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = spawnAreaCenter + new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                0,
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            );

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, maxDistanceFromNavMesh, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return Vector3.zero; // Failed to find a valid position
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);
    }

    private void HandleEnemyDeath()
    {
        enemiesAlive--;
        Debug.Log($"Enemy died. {enemiesAlive} remaining.");

        if (enemiesAlive <= 0)
        {
            Debug.Log("Wave complete!");
            // Trigger next wave, rewards, etc.
        }
    }
}


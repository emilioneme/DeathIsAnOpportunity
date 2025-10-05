using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Vector3 spawnAreaCenter;
    public Vector3 spawnAreaSize;
    public float maxDistanceFromNavMesh = 2f;

    [Header("Wave Settings")]
    public Wave[] waves;              // Array of ScriptableObject waves
    public float timeBetweenWaves = 5f;

    private int currentWaveIndex = 0;

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];
            Debug.Log($"Spawning wave {currentWaveIndex + 1}");

            // Loop through each enemy type in the wave
            foreach (EnemyConfig enemyConfig in currentWave.enemies)
            {
                for (int i = 0; i < enemyConfig.count; i++)
                {
                    Vector3 spawnPos = GetRandomNavMeshPosition();
                    if (spawnPos != Vector3.zero)
                    {
                        Instantiate(enemyConfig.enemyPrefab, spawnPos, Quaternion.identity);
                    }
                    yield return new WaitForSeconds(enemyConfig.spawnInterval);
                }
                yield return new WaitForSeconds(currentWave.timeBetweenEnemyTypes);
            }

            currentWaveIndex++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("All waves completed!");
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
}


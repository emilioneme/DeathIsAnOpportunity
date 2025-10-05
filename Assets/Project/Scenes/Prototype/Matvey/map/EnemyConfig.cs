using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "Spawner/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    public GameObject enemyPrefab;   // Enemy prefab to spawn
    public int count = 1;            // How many of this enemy in the wave
    public float spawnInterval = 0.5f; // Time between spawns of this enemy type
}


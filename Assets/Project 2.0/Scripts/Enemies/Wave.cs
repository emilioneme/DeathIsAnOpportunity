using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "Spawner/Wave")]
public class Wave : ScriptableObject
{
    public EnemyConfig[] enemies; // Array of different enemy types in this wave
    public float timeBetweenEnemyTypes = 1f; // Optional pause between different enemy types
}


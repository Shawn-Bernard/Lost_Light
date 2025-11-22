using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Setting",menuName = "Game Setting/Spawn Setting" )]
public class SpawnSettings : ScriptableObject
{
    [Header("Spawn Timing")]
    public float spawnRate = 1f;
    public float waveCooldown = 3f;

    [Header("Wave Settings")]
    public bool canSpawn = true;
    public int enemiesPerWave = 4;
    public int enemiesIncreasePerWave = 2;

    [Header("Enemy Info")]
    public Enemy enemyPrefab;
}
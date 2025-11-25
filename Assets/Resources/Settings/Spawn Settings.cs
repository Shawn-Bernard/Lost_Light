using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Setting",menuName = "Game Setting/Spawn Setting" )]
public class SpawnSetting : ScriptableObject
{
    [Header("Spawn Timing")]
    public float spawnRate = 1f;
    public float waveCooldown = 3f;

    [Header("Wave Settings")]
    public bool canSpawn = true;
    public int objectPerWave = 3;
    public int IncreasePerWave = 2;

    [Header("Spawn object Info")]
    public GameObject spawnedPrefab;
}
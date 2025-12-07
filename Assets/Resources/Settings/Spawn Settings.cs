using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "Spawn Setting",menuName = "Game Setting/Spawn Setting" )]
public class SpawnSetting : ScriptableObject
{
    [Header("Spawn Timing")]
    public float waveCooldown = 3f;

    [Header("Wave Settings")]
    public bool canSpawn = true;
    public int objectPerWave = 3;
    public int increasePerWave = 2;
    public int maxSpawnedObject;

    [Header("Spawn object Info")]
    public GameObject spawnedPrefab;

    public GameObject SpawnObject(Vector3 spawnPosition)
    {
        GameObject spawnedObject = Instantiate(spawnedPrefab,spawnPosition,Quaternion.identity);
        spawnedObject.name = spawnedPrefab.name;
        return spawnedObject;
    }
}
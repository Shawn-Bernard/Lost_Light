using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private SpawnSetting enemySpawner;
    [SerializeField] private SpawnSetting batterySpawner;
    [SerializeField] private SpawnDataContainer spawnDataContainer;
    [SerializeField] private MapGenerator mapGenerator;
    private Vector3 playerSpawn;

    [SerializeField] private int enemiesToSpawn;

    private Vector3[] safePositions;
    private PlayerController player;


    private void Awake()
    {
        mapGenerator ??= FindFirstObjectByType<MapGenerator>();
        player ??= FindAnyObjectByType<PlayerController>();
        
    }
    private void Start()
    {
        safePositions = mapGenerator.GetPositionInsideMap();
        SaveAndload();
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnBatteries());
    }

    private Vector3 FindRandomSpawn()
    {
        int spawnIndex = Random.Range(0, safePositions.Length);
        return safePositions[spawnIndex];
    }

    private bool CanSpawn(Vector3 positionToCheck)
    {
        return Vector3.Distance(positionToCheck, player.transform.position) >= spawnDataContainer.RangeCheck;
    }

    private void CheckAndSpawn(List<GameObject> listForObject, SpawnSetting spawnSetting)
    {
        Vector3 spawnPosition = FindRandomSpawn();

        if (!CanSpawn(spawnPosition))
        {
            foreach (Vector3 position in safePositions)
            {
                if (CanSpawn(position))
                {
                    spawnPosition = position;
                    break;
                }
            }
        }
        GameObject SpawnObject = spawnSetting.SpawnObject(spawnPosition);
        listForObject.Add(SpawnObject);
    } 

    private void SaveAndload()
    {
        if (SaveSystem.SaveFileExists())
        {
            // If the save file does exists I use the list of positions and create and adds to the list
            foreach (Vector3 enemy in spawnDataContainer.enemiesPosition)
            {
                spawnDataContainer.enemies.Add(enemySpawner.SpawnObject(enemy));
            }
            foreach (Vector3 battery in spawnDataContainer.batteriesPosition)
            {
                spawnDataContainer.batteries.Add(batterySpawner.SpawnObject(battery));
            }
        }
        else
        {
            // If there is no save file then that means the player already has a saved spawn position
            playerSpawn = FindRandomSpawn();
            player.MovePlayer(playerSpawn);
        }
        enemySpawner = spawnDataContainer.enemySpawner;
        batterySpawner = spawnDataContainer.batterySpawner;

        enemiesToSpawn = spawnDataContainer.enemiesToSpawn;
        

        spawnDataContainer.enemiesToSpawn = enemiesToSpawn;
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(enemySpawner.waveCooldown);
        while (enemySpawner.canSpawn)
        {
            if ((spawnDataContainer.enemies.Count + enemiesToSpawn) < enemySpawner.maxSpawnedObject)
            {
                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    CheckAndSpawn(spawnDataContainer.enemies,enemySpawner);
                }
                // Increase wave size for next loop
                enemiesToSpawn += enemySpawner.increasePerWave;
            }
            else
            {
                Debug.Log("Too much enemies " + spawnDataContainer.enemies.Count + enemiesToSpawn);
            }
            // Wait between waves
            yield return new WaitForSeconds(enemySpawner.waveCooldown);
        }
    }

    private IEnumerator SpawnBatteries()
    {
        yield return new WaitForSeconds(batterySpawner.waveCooldown);
        while (batterySpawner.canSpawn)
        {
            if (spawnDataContainer.batteries.Count <= batterySpawner.maxSpawnedObject)
            {
                CheckAndSpawn(spawnDataContainer.batteries, batterySpawner);
            }
            yield return new WaitForSeconds(batterySpawner.waveCooldown);
        }
    }
}

[System.Serializable]
public struct SpawnerData
{
    public int enemiesToSpawn;
    public EnemySaveData[] Enemies;
    public BatterySaveData[] Batteries;
}
[System.Serializable]
public struct EnemySaveData
{
    public Vector3 position;
}
[System.Serializable]
public struct BatterySaveData
{
    public Vector3 position;
}

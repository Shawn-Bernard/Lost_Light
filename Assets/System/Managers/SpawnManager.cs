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
    [SerializeField] private PlayerSpawnPoint playerSpawn;

    private List<GameObject> enemies = new List<GameObject>();

    private List<GameObject> batteries = new List<GameObject>();

    [SerializeField] private int enemiesToSpawn;
    [SerializeField] private int batteriesToSpawn;

    private void Awake()
    {
        playerSpawn ??= Object.FindAnyObjectByType<PlayerSpawnPoint>();
        mapGenerator ??= FindFirstObjectByType<MapGenerator>();
        SaveAndload();
        enemiesToSpawn = enemySpawner.objectPerWave;
    }

    private void SaveAndload()
    {
        enemySpawner = spawnDataContainer.enemySpawner;
        batterySpawner = spawnDataContainer.batterySpawner;

        enemiesToSpawn = spawnDataContainer.enemiesToSpawn;
        batteriesToSpawn = spawnDataContainer.batteriesToSpawn;

        spawnDataContainer.enemiesToSpawn = enemiesToSpawn;
        spawnDataContainer.batteriesToSpawn = batteriesToSpawn;

        if (SaveSystem.SaveFileExists())
        {
            foreach (Vector3 enemyPos in spawnDataContainer.enemies)
            {
                GameObject enemy = Instantiate(
                    enemySpawner.spawnedPrefab,
                    enemyPos,
                    Quaternion.identity
                );
                enemies.Add(enemy);
                enemy.name = "Enemy";
            }

            foreach (Vector3 batteryPos in spawnDataContainer.batteries)
            {
                GameObject battery = Instantiate(
                    batterySpawner.spawnedPrefab,
                    batteryPos,
                    Quaternion.identity);
                batteries.Add(battery);
                battery.name = "Battery";
            }
        }
    }

    private void Start()
    {
        if (playerSpawn != null)
        {
            playerSpawn.transform.position = mapGenerator.GetPositionInsideMap();
        }
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnBatteries());
    }

    private IEnumerator SpawnEnemies()
    {
        while (enemySpawner.canSpawn)
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                GameObject enemy = Instantiate(
                    enemySpawner.spawnedPrefab,
                    mapGenerator.GetPositionInsideMap(),
                    Quaternion.identity
                );
                enemies.Add(enemy);
                spawnDataContainer.enemies.Add(enemy.transform.position);
                enemy.name = "Enemy";
            }
            // Increase wave size for next loop
            enemiesToSpawn += enemySpawner.IncreasePerWave;
            spawnDataContainer.enemiesToSpawn = enemiesToSpawn;

            // Wait between waves
            yield return new WaitForSeconds(enemySpawner.waveCooldown);
        }
    }

    private IEnumerator SpawnBatteries()
    {
        while (batterySpawner.canSpawn)
        {
            GameObject battery = Instantiate(
                    batterySpawner.spawnedPrefab,
                    mapGenerator.GetPositionInsideMap(),
                    Quaternion.identity);
            batteries.Add(battery);
            spawnDataContainer.batteries.Add(battery.transform.position);
            battery.name = "Battery";
            yield return new WaitForSeconds(batterySpawner.spawnRate);
        }
    }
}

[System.Serializable]
public struct SpawnerData
{
    public int enemiesToSpawn;
    public int batteriesToSpawn;
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

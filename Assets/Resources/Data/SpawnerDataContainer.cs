using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Data", menuName = "Game Data/Spawn Data")]
public class SpawnDataContainer : ScriptableObject
{
    public SpawnSetting enemySpawner;
    public SpawnSetting batterySpawner;

    public int enemiesToSpawn;
    public int batteriesToSpawn;

    public int RangeCheck;

    [HideInInspector] public List<GameObject> enemies = new List<GameObject>();
    [HideInInspector] public List<Vector3> enemiesPosition = new List<Vector3>();

    [HideInInspector] public List<GameObject> batteries = new List<GameObject>();
    [HideInInspector] public List<Vector3> batteriesPosition = new List<Vector3>();

    public void ResetData()
    {
        enemiesToSpawn = enemySpawner.objectPerWave;
        batteriesToSpawn = batterySpawner.objectPerWave;
        enemies = new List<GameObject>();
        batteries = new List<GameObject>();
    }

    public void Save(ref SpawnerData data)
    {
        data.enemiesToSpawn = enemiesToSpawn;
        List<EnemySaveData> enemyList = new List<EnemySaveData>();
        foreach (GameObject enemy in enemies)
        {
            EnemySaveData enemySave = new EnemySaveData();
            enemySave.position = enemy.transform.position;
            enemyList.Add(enemySave);
        }
        data.Enemies = enemyList.ToArray();

        List<BatterySaveData> batteryList = new List<BatterySaveData>();
        foreach (var battery in batteries)
        {
            if (battery != null)
            {
                batteryList.Add(new BatterySaveData
                {
                    position = battery.transform.position
                });
            }
        }
        data.Batteries = batteryList.ToArray();
    }

    public void Load(SpawnerData data)
    {
        enemiesToSpawn = data.enemiesToSpawn;
        enemies = new List<GameObject>();
        enemiesPosition = new List<Vector3>();
        batteries = new List<GameObject>();
        batteriesPosition = new List<Vector3>();

        foreach (EnemySaveData enemy in data.Enemies)
        {
            enemiesPosition.Add(enemy.position);
        }

        foreach (BatterySaveData battery in data.Batteries)
        {
            batteriesPosition.Add(battery.position);
        }
    }
}

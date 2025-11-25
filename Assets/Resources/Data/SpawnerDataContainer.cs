using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Spawn Data", menuName = "Game Data/Spawn Data")]
public class SpawnDataContainer : ScriptableObject
{
    public SpawnSetting enemySpawner;
    public SpawnSetting batterySpawner;

    public int enemiesToSpawn;
    public int batteriesToSpawn;

    [HideInInspector] public List<Vector3> enemies = new List<Vector3>();

    [HideInInspector] public List<Vector3> batteries = new List<Vector3>();

    public void ResetData()
    {
        enemiesToSpawn = enemySpawner.objectPerWave;
        batteriesToSpawn = batterySpawner.objectPerWave;
        enemies.Clear();
        batteries.Clear();
    }

    public void Save(ref SpawnerData data)
    {
        data.enemiesToSpawn = enemiesToSpawn;
        data.batteriesToSpawn = batteriesToSpawn;
        List<EnemySaveData> enemyList = new List<EnemySaveData>();
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemyList.Add(new EnemySaveData
                {
                    position = enemy
                });
            }
        }
        data.Enemies = enemyList.ToArray();

        List<BatterySaveData> batteryList = new List<BatterySaveData>();
        foreach (var battery in batteries)
        {
            if (battery != null)
            {
                batteryList.Add(new BatterySaveData
                {
                    position = battery
                });
            }
        }
        data.Batteries = batteryList.ToArray();
    }

    public void Load(SpawnerData data)
    {
        enemiesToSpawn = data.enemiesToSpawn;
        batteriesToSpawn = data.batteriesToSpawn;
        enemies.Clear();
        batteries.Clear();

        foreach (var batPos in data.Enemies)
        {
            enemies.Add(batPos.position);
        }
        foreach (var batPos in data.Batteries)
        {
            batteries.Add(batPos.position);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private SpawnSettings enemySpawner;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private PlayerSpawnPoint playerSpawn;

    private List<Enemy> enemies;

    [SerializeField] private GameObject player;

    private void Awake()
    {
        enemies = new List<Enemy>();
        if (mapGenerator == null) mapGenerator ??= FindFirstObjectByType<MapGenerator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        playerSpawn.transform.position = mapGenerator.GetPositionInsideMap();
        StartCoroutine(EndlessWaves());
    }

    private IEnumerator EndlessWaves()
    {
        while (enemySpawner.canSpawn)
        {

            Enemy enemy = Instantiate(
                    enemySpawner.enemyPrefab,
                    mapGenerator.GetPositionInsideMap(),
                    Quaternion.identity
                );
            enemy.SetTarget(player);
            enemies.Add(enemy);
            enemy.name = "Enemy";
            Debug.Log("Enemy spawned!");

            yield return new WaitForSeconds(enemySpawner.spawnRate);
            // Increase wave size for next loop
            enemySpawner.enemiesPerWave += enemySpawner.enemiesIncreasePerWave;

            // Wait between waves
            yield return new WaitForSeconds(enemySpawner.waveCooldown);
        }
    }
}

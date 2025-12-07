using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySetting enemySetting;
    [SerializeField] private SpawnDataContainer spawnDataContainer;
    [SerializeField] private NavMeshAgent enemy;
    [SerializeField] private PlayerController player;

    private int caughtRange;

    Vector3 directionToPlayer;

    void Start()
    {
        enemy ??= GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        caughtRange = enemySetting.caughtRange;
        enemy.speed = Random.Range(enemySetting.minSpeed,enemySetting.maxSpeed);
        enemy.angularSpeed = enemySetting.turnSpeed;
    }

    void Update()
    {
        HandleAI();
    }
    /// <summary>
    /// Enemy ai will follow the player within range and wonder if player out of range
    /// </summary>
    public void HandleAI()
    {
        if (player != null) directionToPlayer = player.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        if (enemy != null) enemy.SetDestination(player.transform.position);
        if (distanceToPlayer <= caughtRange)
        {
            if (player != null)
            player.Death();
        }
    }

    public void Death()
    {
        spawnDataContainer.enemies.Remove(gameObject);
        Destroy(gameObject);
    }
}

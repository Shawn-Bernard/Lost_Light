using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySetting enemySetting;
    [SerializeField] private SpawnDataContainer spawnDataContainer;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private PlayerController player;

    private int caughtRange;

    Vector3 directionToPlayer;

    void Start()
    {
        navMeshAgent ??= GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        caughtRange = enemySetting.caughtRange;
        navMeshAgent.speed = Random.Range(enemySetting.minSpeed,enemySetting.maxSpeed);
        navMeshAgent.angularSpeed = enemySetting.turnSpeed;
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
        if (player != null)
        { 
            directionToPlayer =  transform.position - player.transform.position;
            transform.rotation = Quaternion.LookRotation(directionToPlayer);
        }
        float distanceToPlayer = directionToPlayer.magnitude;
        if (navMeshAgent != null) navMeshAgent.SetDestination(player.transform.position);
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

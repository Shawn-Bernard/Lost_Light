using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySetting enemySetting;
    [SerializeField] private NavMeshAgent enemy;
    public GameObject player;

    private int caughtRange;

    Vector3 directionToPlayer;

    void Start()
    {
        enemy ??= GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
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
        directionToPlayer = player.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        enemy.SetDestination(player.transform.position);
        if (distanceToPlayer <= caughtRange)
        {
            GameManager.instance.GameStateManager.SwitchToGameOver();
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}

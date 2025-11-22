using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent enemy;
    public GameObject player;

    public int range = 6;
    public int coneAngle = 80;
    public int caughtRange = 2;
    public int searchRange = 20;

    Vector3 directionToPlayer;

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
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

        if (CanSeePlayer(directionToPlayer, distanceToPlayer))
        {
            Debug.Log("Player seen");
            enemy.SetDestination(player.transform.position);

            if (distanceToPlayer <= caughtRange)
            {
                Debug.Log("Player caught");
            }
        }
        else if (enemy.remainingDistance <= enemy.stoppingDistance)
        {
            PatrolAround();
        }
    }

    /// <summary>
    /// Take in a direction and distance, then shoots a ray cast if within range and angle
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    bool CanSeePlayer(Vector3 direction, float distance)
    {
        if (distance > range) return false;

        float angle = Vector3.Angle(transform.forward, direction.normalized);
        if (angle > coneAngle / 2) return false;

        if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, range))
        {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }
    /// <summary>
    /// Picks a random position around them and moves around 
    /// </summary>
    void PatrolAround()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * searchRange;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, searchRange, NavMesh.AllAreas))
        {
            enemy.SetDestination(hit.position);
        }
    }

    public void Death()
    {
        Debug.Log("IM dead");
    }
}

using UnityEngine;
/// <summary>
/// Find the player and places them in position
/// </summary>
public class PlayerSpawnPoint : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    private void Start()
    {
        controller ??= FindAnyObjectByType<PlayerController>();
        controller.MovePlayer(transform.position);
    }


}

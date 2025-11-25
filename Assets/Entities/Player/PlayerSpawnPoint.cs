using UnityEngine;
/// <summary>
/// Find the player and places them in position (I tried to do it other ways, but character controller hate me)
/// </summary>
public class PlayerSpawnPoint : MonoBehaviour
{
    private PlayerController controller;
    private void Start()
    {
        if (!SaveSystem.SaveFileExists())
        {
            controller ??= FindAnyObjectByType<PlayerController>();
            controller.MovePlayer(transform.position);
        }
    }


}

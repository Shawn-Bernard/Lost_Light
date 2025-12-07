using UnityEngine;
[CreateAssetMenu(fileName = "Player Setting", menuName = "Game Setting/Player Setting")]
public class PlayerSetting : ScriptableObject
{
    [Header("Default Values")]
    public float movementSpeed;
    public float mouseSensitivity;
    public float fireRate;
    public float laserDuration;

}

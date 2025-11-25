using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Setting", menuName = "Game Setting/Enemy Setting")]
public class EnemySetting : ScriptableObject
{
    public float maxSpeed;
    public float minSpeed;
    public float turnSpeed;
    public int range = 6;
    public int coneAngle = 80;
    public int caughtRange = 2;
    public int searchRange = 20;
}

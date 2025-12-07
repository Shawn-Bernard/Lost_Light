using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Setting", menuName = "Game Setting/Enemy Setting")]
public class EnemySetting : ScriptableObject
{
    public float maxSpeed;
    public float minSpeed;
    public float turnSpeed;
    public int caughtRange = 2;
}

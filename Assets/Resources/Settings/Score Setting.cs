using UnityEngine;

[CreateAssetMenu(fileName = "Score Setting", menuName = "Game Setting/Score Setting")]
public class ScoreSetting : ScriptableObject
{
    [Header("Score Timing")]
    public float scoreTickInterval = 5f;
    [Header("Score Amount")]
    public int scorePerTick = 100;
    public int scoreEnemy = 200;
}

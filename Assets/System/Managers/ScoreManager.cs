using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] private GameplayMenuController gameplayMenuController;
    [SerializeField] private GameOverController gameOverController;
    [SerializeField] private ScoreSetting scoreSetting;
    private int score;
    private float scoreTimer;

    private float currentTime;
    private int lastDisplayedSeconds;

    private int highScore;
    private float bestTime;

    private void Awake()
    {
        gameplayMenuController ??= GameManager.instance.UIManager.gameplayMenuController;
        gameOverController ??= GameManager.instance.UIManager.GameOverController;
    }

    /// <summary>
    /// Updates game time UI 
    /// </summary>
    public void handleGameTime()
    {
        currentTime += Time.deltaTime;

        int currentSeconds = Mathf.FloorToInt(currentTime);

        if (currentSeconds != lastDisplayedSeconds)
        {
            lastDisplayedSeconds = currentSeconds;
            gameplayMenuController.UpdateTime(currentTime);

            if (lastDisplayedSeconds > bestTime)
            {
                bestTime = currentTime;
            }
        }
    }
    /// <summary>
    /// Score goes up every timed interval
    /// </summary>
    public void handleScoreTick()
    {
        scoreTimer += Time.deltaTime;
        if (scoreTimer >= scoreSetting.scoreTickInterval)
        {
            score += scoreSetting.scorePerTick;
            scoreTimer -= scoreSetting.scoreTickInterval;
            gameplayMenuController.UpdateScore(score);
            if (score > highScore)
            {
                highScore = score;
                gameOverController.UpdateHighScore(highScore, bestTime);
            }
        }
    }

    public void KilledEnemy()
    {
        score += scoreSetting.scoreEnemy;
        gameplayMenuController.UpdateScore(score);
    }

    public void Save(ref ScoreData data)
    {
        data.score = score;
        data.highScore = highScore;

        data.currentTime = currentTime;
        data.bestTime = bestTime;
    }

    public void Load(ScoreData data)
    {
        score = data.score;
        highScore = data.highScore;

        currentTime = data.currentTime;
        bestTime = data.bestTime;
    }

    public void ResetData()
    {
        score = 0;
        highScore = 0;
        currentTime = 0;
        bestTime = 0;
        gameplayMenuController.UpdateScore(score);
        gameplayMenuController.UpdateTime(currentTime);
    }
}
[System.Serializable]
public struct ScoreData
{
    public int score;
    public int highScore;

    public float currentTime;
    public float bestTime;
}
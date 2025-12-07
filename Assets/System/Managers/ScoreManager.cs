using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] private GameplayMenuController gameplayMenuController;
    [SerializeField] private GameOverController gameOverController;
    [SerializeField] private ScoreSetting scoreSetting;
    private int score;

    private float currentTime;
    private int lastDisplayedSeconds;

    private int highScore;
    private float bestTime;
    private float scoreTimer;

    private void Awake()
    {
        gameplayMenuController ??= GameManager.instance.UIManager.gameplayMenuController;
    }
    /// <summary>
    /// Score goes up every timed interval
    /// </summary>
    public void HandleScore()
    {
        currentTime += Time.deltaTime;

        int currentSeconds = Mathf.FloorToInt(currentTime);

        if (currentSeconds != lastDisplayedSeconds)
        {
            lastDisplayedSeconds = currentSeconds;

            if (lastDisplayedSeconds > bestTime)
            {
                bestTime = currentTime;
            }

            UpdateScore();
        }
        scoreTimer += Time.deltaTime;

        if (scoreTimer >= scoreSetting.scoreTickInterval)
        {
            score += scoreSetting.scorePerTick;

            scoreTimer -= scoreSetting.scoreTickInterval;

            if (score > highScore)
            {
                highScore = score;
            }

            UpdateScore();
        }
    }

    public void KilledEnemy()
    {
        score += scoreSetting.scoreEnemy;
    }

    public void UpdateScore()
    {
        gameplayMenuController.UpdateScore(score, currentTime);
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
        gameplayMenuController.UpdateScore(highScore, bestTime);
    }

    public void ResetData()
    {
        score = 0;
        highScore = 0;
        currentTime = 0;
        bestTime = 0;
        UpdateScore();
    }

    private void OnEnable()
    {
        GameplayState.gameplayStateUpdate += HandleScore;
    }
    private void OnDisable()
    {
        GameplayState.gameplayStateUpdate -= HandleScore;
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
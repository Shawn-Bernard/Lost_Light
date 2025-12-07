using UnityEngine;
using UnityEngine.UIElements;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private UIDocument gameOverMenu;
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private GameStateManager gameStateManager;

    [SerializeField] private GameOverController gameOverController;

    private Button startOverButton;
    private Button mainMenuButton;
    private Button quitButton;
    private Label scoreLabel;
    private Label highScoreLabel;

    private int highScore;
    private float bestTime;

    private void Awake()
    {
        levelManager ??= GameManager.instance.LevelManager;
        gameStateManager ??= GameManager.instance.GameStateManager;
        gameOverController ??= FindAnyObjectByType<GameOverController>();
        gameOverMenu ??= GetComponent<UIDocument>();
        startOverButton = gameOverMenu.rootVisualElement.Q<Button>("StartOverButton");
        mainMenuButton = gameOverMenu.rootVisualElement.Q<Button>("MainMenuButton");
        quitButton = gameOverMenu.rootVisualElement.Q<Button>("QuitButton");
        highScoreLabel = gameOverMenu.rootVisualElement.Q<Label>("HighScore");
        scoreLabel = gameOverMenu.rootVisualElement.Q<Label>("Score");

    }

    private void OnEnable()
    {
        startOverButton.clicked += NewGame;
        mainMenuButton.clicked += MainMenu;
        quitButton.clicked += Quit;
    }

    private void NewGame()
    {
        SaveSystem.ResetGame();
        levelManager.LoadLevel();
        gameStateManager.SwitchToGameplay();
    }

    private void MainMenu()
    {
        levelManager.LoadMainMenu();
        gameStateManager.SwitchToMainMenu();
    }

    private void Quit()
    {
        Application.Quit();
    }
    public void UpdateScore(int score, float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        string timeString = $"{minutes:00}:{seconds:00}";
        scoreLabel.text = $"Score : {score} and lasted {timeString}";
        if (score > highScore && time > bestTime)
        {
            highScore = score;
            bestTime = time;
            UpdateHighScore();
        }
    }

    public void UpdateHighScore()
    {
        int minutes = Mathf.FloorToInt(bestTime / 60);
        int seconds = Mathf.FloorToInt(bestTime % 60);
        string timeString = $"{minutes:00}:{seconds:00}";
        highScoreLabel.text = $"High Score : {highScore} and lasted {timeString}";
    }
}


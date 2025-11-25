using UnityEngine;
using UnityEngine.UIElements;

public class GameOverController : MonoBehaviour
{
    private UIDocument menuUI;

    GameManager gameManager => GameManager.instance;

    UIManager uiManager => GameManager.instance.UIManager;

    InputManager inputManager => GameManager.instance.InputManager;

    LevelManager levelManager => GameManager.instance.LevelManager;

    GameStateManager gameStateManager => GameManager.instance.GameStateManager;
    private Button startOverButton;
    private Button mainMenuButton;
    private Button quitButton;
    private Label highScoreLabel;
    private void Awake()
    {
        menuUI = GetComponent<UIDocument>();
        startOverButton = menuUI.rootVisualElement.Q<Button>("StartOverButton");
        mainMenuButton = menuUI.rootVisualElement.Q<Button>("MainMenuButton");
        quitButton = menuUI.rootVisualElement.Q<Button>("QuitButton");
        highScoreLabel = menuUI.rootVisualElement.Q<Label>("HighScore");
    }

    private void OnEnable()
    {
        startOverButton.clicked += NewGame;
        mainMenuButton.clicked += MainMenu;
        quitButton.clicked += Quit;
    }

    private void NewGame()
    {
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

    public void UpdateHighScore(float score,float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        string timeString = $"{minutes:00}:{seconds:00}";
        highScoreLabel.text = $"High Score : {score} and lasted {timeString}";
    }
}


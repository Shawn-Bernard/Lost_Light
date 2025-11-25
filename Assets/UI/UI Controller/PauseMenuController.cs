using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuController : MonoBehaviour
{
    private UIDocument menuUI;

    GameManager gameManager => GameManager.instance;

    UIManager uiManager => GameManager.instance.UIManager;

    InputManager inputManager => GameManager.instance.InputManager;

    LevelManager levelManager => GameManager.instance.LevelManager;

    GameStateManager gameStateManager => GameManager.instance.GameStateManager;

    private Button ResumeButton;
    private Button optionButton;
    private Button menuButton;
    private void Awake()
    {
        menuUI = GetComponent<UIDocument>();
        ResumeButton = menuUI.rootVisualElement.Q<Button>("ResumeButton");
        menuButton = menuUI.rootVisualElement.Q<Button>("MainMenuButton");

    }

    private void OnEnable()
    {
        ResumeButton.clicked += Resume;
        menuButton.clicked += MainMenu;
    }

    private void Resume()
    {
        gameStateManager.SwitchToGameplay();
    }

    private void MainMenu()
    {
        levelManager.LoadMainMenu();
        gameStateManager.SwitchToMainMenu();
        SaveSystem.Save();
    }
}

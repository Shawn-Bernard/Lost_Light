using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private UIDocument pauseMenu;

    [SerializeField] private LevelManager levelManager;

    [SerializeField] private GameStateManager gameStateManager;

    private Button continueButton;

    private Button ResumeButton;
    private Button optionButton;
    private Button menuButton;
    private void Awake()
    {
        levelManager ??= GameManager.instance.LevelManager;
        gameStateManager ??= GameManager.instance.GameStateManager;
        pauseMenu ??= GetComponent<UIDocument>();
        ResumeButton = pauseMenu.rootVisualElement.Q<Button>("ResumeButton");
        menuButton = pauseMenu.rootVisualElement.Q<Button>("MainMenuButton");

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

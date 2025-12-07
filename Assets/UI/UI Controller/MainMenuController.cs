using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private UIDocument menuUI;

    GameManager gameManager;

    [SerializeField] private UIManager uiManager;

    [SerializeField] private InputManager inputManager;

    [SerializeField] private LevelManager levelManager;

    [SerializeField] private GameStateManager gameStateManager;
    private Button continueButton;
    private Button newGameButton;
    private Button quitButton;
    private void Awake()
    {
        gameManager ??= GameManager.instance;
        uiManager ??= GameManager.instance.UIManager;
        inputManager ??= GameManager.instance.InputManager;
        levelManager ??= GameManager.instance.LevelManager;
        gameStateManager ??= GameManager.instance.GameStateManager;

        menuUI = GetComponent<UIDocument>();
        continueButton = menuUI.rootVisualElement.Q<Button>("ContinueButton");
        newGameButton = menuUI.rootVisualElement.Q<Button>("NewGameButton");
        quitButton = menuUI.rootVisualElement.Q<Button>("QuitButton");
    }

    private void Update()
    {
        if (SaveSystem.SaveFileExists())
        {
            continueButton.style.display = DisplayStyle.Flex;
        }
        else
        {
            continueButton.style.display = DisplayStyle.None;
        }
    }

    private void OnEnable()
    {
        continueButton.clicked += Continue;
        newGameButton.clicked += NewGame;
        quitButton.clicked += Quit;
    }

    private void NewGame()
    {
        SaveSystem.ResetGame();
        if (levelManager != null)
        {
            levelManager.LoadLevel();
        }
        else
        {
            levelManager ??= GameManager.instance.LevelManager;
        }
        if (gameStateManager != null)
        {
            gameStateManager.SwitchToGameplay();
        }
        else
        {
            gameStateManager ??= GameManager.instance.GameStateManager;
        }
    }

    private void Continue()
    {
        if (SaveSystem.SaveFileExists())
        {
            SaveSystem.Load();
            levelManager.LoadLevel();
            gameStateManager.SwitchToGameplay();
        }
    }

    private void Quit()
    {
        Application.Quit();
    }
}

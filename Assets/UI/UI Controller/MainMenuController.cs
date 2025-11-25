using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private UIDocument menuUI;

    GameManager gameManager => GameManager.instance;

    UIManager uiManager => GameManager.instance.UIManager;

    InputManager inputManager => GameManager.instance.InputManager;

    LevelManager levelManager => GameManager.instance.LevelManager;

    GameStateManager gameStateManager => GameManager.instance.GameStateManager;
    private Button continueButton;
    private Button newGameButton;
    private Button quitButton;
    private void Awake()
    {
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
        levelManager.LoadLevel();
        gameStateManager.SwitchToGameplay();
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

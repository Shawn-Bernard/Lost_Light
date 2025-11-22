using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private UIDocument menuUI => GetComponent<UIDocument>();

    GameManager gameManager => GameManager.instance;

    UIManager uiManager => GameManager.instance.UIManager;

    InputManager inputManager => GameManager.instance.InputManager;

    LevelManager levelManager => GameManager.instance.LevelManager;

    GameStateManager gameStateManager => GameManager.instance.GameStateManager;

    private Button startButton;
    private Button optionButton;
    private Button quitButton;

    private void OnEnable()
    {
        startButton = menuUI.rootVisualElement.Q<Button>("PlayButton");
        optionButton = menuUI.rootVisualElement.Q<Button>("OptionButton");
        quitButton = menuUI.rootVisualElement.Q<Button>("QuitButton");

        startButton.clicked += PlayButton;
        optionButton.clicked += OptionButton;
        quitButton.clicked += QuitButton;
    }

    private void OnDisable()
    {
        startButton.clicked -= PlayButton;
        optionButton.clicked -= OptionButton;
        quitButton.clicked -= QuitButton;
    }

    private void PlayButton()
    {
        levelManager.LoadLevel();
        gameStateManager.SwitchToGameplay();
    }

    private void OptionButton()
    {
        //uiManager.en
    }
    private void QuitButton()
    {
        Application.Quit();
    }
}

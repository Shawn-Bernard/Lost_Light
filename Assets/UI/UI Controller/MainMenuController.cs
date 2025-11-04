using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private UIDocument menuUI => GetComponent<UIDocument>();

    GameManager gameManager => GameManager.instance;

    UIManager uiManager => GameManager.instance.UIManager;

    InputManager inputManager => GameManager.instance.InputManager;

    LevelManager levelManager => GameManager.instance.LevelManager;

    private Button startButton;
    private Button optionButton;
    private Button quitButton;

    private Button[] menubuttons;
    private int focusedIndex = 0;

    #region Setup button references and listeners



    #endregion

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
        //levelManager.LoadScene("Level_1");
        Debug.Log("Play button was pressed");
    }

    private void OptionButton()
    {
        Debug.Log("option button was pressed");
    }
    private void QuitButton()
    {
        Debug.Log("quit button was pressed");
    }
}

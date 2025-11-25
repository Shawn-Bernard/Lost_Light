using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameplayMenuController : MonoBehaviour
{
    private UIDocument menuUI;

    GameManager gameManager;

    [SerializeField] private UIManager uiManager;

    InputManager inputManager => GameManager.instance.InputManager;

    GameStateManager gameStateManager => GameManager.instance.GameStateManager;

    private ProgressBar batteryBar;

    private Label scoreLabel;

    private Label batteryCountLabel;

    private Label TimerLabel;

    private void Awake()
    {
        gameManager = GameManager.instance;
        menuUI = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        inputManager.PauseInputEvent += OpenPause;
        scoreLabel = menuUI.rootVisualElement.Q<Label>("ScoreUI");
        batteryBar = menuUI.rootVisualElement.Q<ProgressBar>("BatteryLife");
        batteryCountLabel = menuUI.rootVisualElement.Q<Label>("BatteryCountUI");
        TimerLabel = menuUI.rootVisualElement.Q<Label>("TimerUI");
    }

    public void UpdateBatteryLife(float batteryPercent)
    {
        batteryBar.value = batteryPercent;

        batteryBar.title = $"Battery Power {(int)batteryPercent}%";
    }
    public void UpdateTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        TimerLabel.text = $"{minutes:00}:{seconds:00}";
    }

    public void UpdateScore(int score)
    {
        scoreLabel.text = $"Score : {score}";
    }
    public void UpdateBatteryCount(int totalBatteries)
    {
        batteryCountLabel.text = $"Battery : {totalBatteries}";
    }

    public void OpenPause(InputAction.CallbackContext context)
    {
        if (gameStateManager.IsInGameplay())
        {
            gameStateManager.SwitchToPause();
        }
    }
}

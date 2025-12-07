using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameplayMenuController : MonoBehaviour
{
    [SerializeField] private UIDocument gameplayMenu;

    [SerializeField] private UIManager uiManager;

    [SerializeField] private InputManager inputManager;

    [SerializeField] private GameStateManager gameStateManager;

    [SerializeField] private GameOverController gameOverController;

    private ProgressBar batteryBar;

    private Label scoreLabel;

    private Label batteryCountLabel;

    private Label TimerLabel;

    private void Awake()
    {
        inputManager ??= GameManager.instance.InputManager;
        gameStateManager ??= GameManager.instance.GameStateManager;
        gameOverController ??= FindAnyObjectByType<GameOverController>();
        gameplayMenu ??= GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        inputManager.PauseInputEvent += OpenPause;
        scoreLabel = gameplayMenu.rootVisualElement.Q<Label>("ScoreUI");
        batteryBar = gameplayMenu.rootVisualElement.Q<ProgressBar>("BatteryLife");
        batteryCountLabel = gameplayMenu.rootVisualElement.Q<Label>("BatteryCountUI");
        TimerLabel = gameplayMenu.rootVisualElement.Q<Label>("TimerUI");
    }

    public void UpdateScore(int score, float time)
    {
        scoreLabel.text = $"Score : {score}";

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        TimerLabel.text = $"{minutes:00}:{seconds:00}";

        gameOverController.UpdateScore(score, time);
    }
    public void UpdateBatteryCount(int totalBatteries)
    {
        batteryCountLabel.text = $"Battery : {totalBatteries}";
    }
    public void UpdateBatteryLife(float batteryPercent)
    {
        batteryBar.value = batteryPercent;

        batteryBar.title = $"Battery Power {(int)batteryPercent}%";
    }

    public void OpenPause(InputAction.CallbackContext context)
    {
        if (gameStateManager.IsInGameplay())
        {
            gameStateManager.SwitchToPause();
        }
    }
}

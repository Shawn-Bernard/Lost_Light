using Unity.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [Header("Debug (read only)")]
    [SerializeField] private string LastActiveState;
    [SerializeField] private string currentActiveState;

    [SerializeField] private IState currentState;
    [SerializeField] private IState lastState;

    GameManager gameManager => GameManager.instance;
    #region game states
    // Instantiate game states
    public MainMenuState mainMenuState = MainMenuState.Instance;
    public GameplayState gameplayState = GameplayState.Instance;
    public PauseState pauseState = PauseState.Instance;
    public GameOverState gameoverState = GameOverState.Instance;
    public BootLoadState bootLoadState = BootLoadState.Instance;

    #endregion
    #region State Machine Updates
    private void Start()
    {
        LastActiveState = currentActiveState;
        currentState = bootLoadState;
        currentState.EnterState();
        currentActiveState = currentState.ToString();
    }
    private void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    private void LateUpdate()
    {
        currentState.LateUpdateState();
    }

    #endregion

    private void SwitchStates(IState newState)
    {
        lastState = currentState;
        LastActiveState = currentState.ToString();

        currentState?.ExitState();

        currentState = newState;
        currentActiveState = currentState.ToString();

        currentState.EnterState();
    }

    public void SwitchToMainMenu()
    {
        SwitchStates(mainMenuState);
    }
    public void SwitchToGameplay()
    {
        SwitchStates(gameplayState);
    }
    public void SwitchToPause()
    {
        SwitchStates(pauseState);
    }

    public void SwitchToGameOver()
    {
        SwitchStates(gameoverState);
    }
    public void SwitchToBootLoader()
    {
        SwitchStates(bootLoadState);
    }

}
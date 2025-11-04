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

    #endregion
    #region State Machine Updates
    private void Start()
    {
        LastActiveState = currentActiveState;
        currentState = gameplayState;
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

    public void SwitchStates(IState newState)
    {
        lastState = currentState;
        LastActiveState = currentState.ToString();

        currentState?.ExitState();


        currentState = newState;
        currentActiveState = currentState.ToString();

        currentState.EnterState();
    }

    #region Button Call Methods

    public void StartGame()
    {
        SwitchStates(gameplayState);
        gameManager.LevelManager.LoadSceneWithSpawnPoint("Level1", "SpawnPoint");
    }
    public void Pause()
    {
        if (currentState != gameplayState)
        {
            return;
        }

        if (currentState == gameplayState)
        {
            SwitchStates(pauseState);
            return;
        }
        
    }

    public void Resume()
    {
        if (currentState != pauseState)
        {
            return;
        }

        if (currentState == pauseState)
        {
            SwitchStates(gameplayState);
        }
        
    }
    public void MainMenu()
    {
        SwitchStates(mainMenuState);
    }
    public void Qut()
    {
        Application.Quit();
    }


    #endregion

}

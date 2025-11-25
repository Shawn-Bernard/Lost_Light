using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayState : IState
{
    #region Singleton Instance

    public static GameManager gameManager => GameManager.instance;

    private static readonly GameplayState instance = new GameplayState();

    public static GameplayState Instance = instance;

    #endregion

    private ScoreManager scoreManager;

    private PlayerController playerController;
    public void EnterState() 
    {
        Time.timeScale = 1;
        

        gameManager.UIManager.EnableGameplayMenu();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        scoreManager ??= gameManager.ScoreManager;
        playerController ??= gameManager.PlayerController;
    }

    public void FixedUpdateState()
    {
        playerController.HandleLook();
    }

    public void UpdateState()
    {
        playerController.HandleMovement();
        scoreManager.handleScoreTick();
        scoreManager.handleGameTime();
    }

    public void LateUpdateState()
    {
    }

    public void ExitState()
    {
        Cursor.visible = true;
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
    }
}

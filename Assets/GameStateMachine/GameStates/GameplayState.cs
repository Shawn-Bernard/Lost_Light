using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayState : IState
{
    #region Singleton Instance

    public static GameManager gameManager => GameManager.instance;

    private static readonly GameplayState instance = new GameplayState();

    public static GameplayState Instance = instance;

    #endregion
    public void EnterState() 
    {
        Time.timeScale = 1;

        gameManager.UIManager.EnableGameplayMenu();

        Cursor.visible = false;
    }

    public void FixedUpdateState()
    {
        
    }

    public void UpdateState()
    {
        GameManager.instance.PlayerController.HandleMovement();
    }

    public void LateUpdateState()
    {
        GameManager.instance.PlayerController.HandleLook();
    }

    public void ExitState()
    {
        Cursor.visible = true;
        Time.timeScale = 0;
    }
}

using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameplayState : IState
{
    #region Singleton Instance

    public static GameManager gameManager => GameManager.instance;

    private static readonly GameplayState instance = new GameplayState();

    public static GameplayState Instance = instance;

    #endregion

    public static event Action gameplayStateUpdate;

    private PlayerController playerController;
    public void EnterState() 
    {
        gameManager.UIManager.EnableGameplayMenu();
        Time.timeScale = 1;
        
        
        Cursor.lockState = CursorLockMode.Confined;
        playerController ??= gameManager.PlayerController;
    }

    public void FixedUpdateState()
    {
    }

    public void UpdateState()
    {
        gameplayStateUpdate?.Invoke();
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

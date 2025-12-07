using UnityEngine;

public class GameOverState : IState
{
    #region Singleton Instance

    public static GameManager gameManager => GameManager.instance;

    private static readonly GameOverState instance = new GameOverState();

    public static GameOverState Instance = instance;

    #endregion
    public void EnterState()
    {
        Time.timeScale = 0;
        gameManager.UIManager.EnableGameOverMenu();
    }

    public void ExitState()
    {
        SaveSystem.ResetGame();
    }

    public void FixedUpdateState()
    {

    }

    public void LateUpdateState()
    {

    }

    public void UpdateState()
    {

    }
}

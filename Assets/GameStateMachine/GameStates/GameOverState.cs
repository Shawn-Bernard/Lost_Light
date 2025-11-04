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
        gameManager.UIManager.EnableGameOverMenu();
    }

    public void ExitState()
    {

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

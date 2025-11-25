using UnityEngine;
using UnityEngine.InputSystem;

public class PauseState : IState
{
    #region Singleton Instance

    public static GameManager gameManager => GameManager.instance;

    private static readonly PauseState instance = new PauseState();

    public static PauseState Instance = instance;

    #endregion
    public void EnterState()
    {
        Time.timeScale = 0;
        gameManager.UIManager.EnablePauseMenu();
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

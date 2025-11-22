using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuState : IState
{
    #region Singleton Instance

    public static GameManager gameManager => GameManager.instance;

    private static readonly MainMenuState instance = new MainMenuState();

    public static MainMenuState Instance = instance;

    #endregion
    public void EnterState()
    {
        //gameManager.LevelManager.LoadMainMenu();
        gameManager.UIManager.EnableMainMenu();
    }

    public void FixedUpdateState()
    {

    }

    public void UpdateState()
    {
    }

    public void LateUpdateState()
    {

    }

    public void ExitState()
    {
        //gameManager.PlayerController.gameObject.SetActive(false);
    }
}
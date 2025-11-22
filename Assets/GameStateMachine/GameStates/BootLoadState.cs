using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoadState : IState
{
    #region Singleton Instance

    public static GameManager gameManager => GameManager.instance;

    private static readonly BootLoadState instance = new BootLoadState();

    public static BootLoadState Instance = instance;

    #endregion
    public void EnterState()
    {
        Cursor.visible = false;

        Time.timeScale = 0f;

        if (SceneManager.GetActiveScene().name == "BootLoader")
        {
            gameManager.LevelManager.LoadMainMenu();
            gameManager.GameStateManager.SwitchToMainMenu();
            
        }
        else if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            gameManager.GameStateManager.SwitchToMainMenu();
        }
        else if (SceneManager.GetActiveScene().name == "Level")
        {
            gameManager.GameStateManager.SwitchToGameplay();
        }
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

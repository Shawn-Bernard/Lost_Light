using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void LoadScene(string sceneName)
    {
        //Loading in my scene name with the string from the trigger
        SceneManager.LoadScene(sceneName);


        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

    public void LoadLevel()
    {
        LoadScene("Level");
    }

    public void LoadMainMenu()
    {
        LoadScene("MainMenu");
    }
}

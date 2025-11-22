using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public static class PerformBootLoad
{
    const string sceneName = "BootLoader";
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {

        if (SceneManager.GetActiveScene().name != sceneName)
        {
            for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
            {
                Scene candidateScene = SceneManager.GetSceneAt(sceneIndex);

                if (candidateScene.name == sceneName)
                {
                    return;
                }
            }
            Debug.Log($"Loading BootLoad {sceneName}");

            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

            // Run any resets of game data 
        }
    }

}
public class BootLoader : MonoBehaviour
{
    public static BootLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void Test()
    {
        Debug.Log("BootLoader Scene is active");
    }
}
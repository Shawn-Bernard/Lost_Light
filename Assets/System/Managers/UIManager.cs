using UnityEngine;
using UnityEngine.UIElements;
public class UIManager : MonoBehaviour
{
    [Header("UI Menu Objects")]
    [SerializeField] private UIDocument mainMenuUI;
    [SerializeField] private UIDocument gameplayMenuUI;
    [SerializeField] private UIDocument pauseMenuUI;
    [SerializeField] private UIDocument gameOverMenuUI;

    [Header("UI Menu Controllers")]
    public GameplayMenuController gameplayMenuController;
    public GameOverController GameOverController;
    private void Awake()
    {
        #region UI Document Check
        mainMenuUI ??= FindUIDocument("MainMenu");
        gameplayMenuUI ??= FindUIDocument("GameplayMenu");
        pauseMenuUI ??= FindUIDocument("PauseMenu");
        //optionsMenuUI ??= FindUIDocument("OptionsMenu");
        gameOverMenuUI ??= FindUIDocument("GameOverMenu");
        #endregion

        gameplayMenuController ??= GetComponentInChildren<GameplayMenuController>();
        DisableAllMenus();
    }

    public void DisableAllMenus()
    {
        mainMenuUI.rootVisualElement.style.display = DisplayStyle.None;
        pauseMenuUI.rootVisualElement.style.display = DisplayStyle.None;
        gameplayMenuUI.rootVisualElement.style.display = DisplayStyle.None;
        gameOverMenuUI.rootVisualElement.style.display = DisplayStyle.None;
    }

    public void EnableMainMenu()
    {
        DisableAllMenus();
        mainMenuUI.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    public void EnablePauseMenu()
    {
        DisableAllMenus();
        pauseMenuUI.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    public void EnableGameplayMenu()
    {
        DisableAllMenus();
        gameplayMenuUI.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    public void EnableGameOverMenu()
    {
        DisableAllMenus();
        gameOverMenuUI.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    private UIDocument FindUIDocument(string name)
    {
        UIDocument[] documents = Object.FindObjectsByType<UIDocument>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (UIDocument doc in documents)
        {
            if (doc.name == name)
            {
                //Debug.Log($"UI {doc} - {name} was found");
                return doc;
            }
            else
            {
                //Debug.LogWarning($"UIDocument '{name}' not found in scene.");
            }
        }
        //Debug.LogWarning($"UIDocument '{name}' not found in scene.");
        return null;
    }
}

using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class TriggerSceneChange : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    [SerializeField] private string nextSpawnName;

    GameManager gameManager => GameManager.instance;

    public bool ShowGizmo;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gameManager != null)
            {
                gameManager.LevelManager.LoadSceneWithSpawnPoint(nextSceneName, nextSpawnName);
            }
            else
            {
                Debug.Log("Game manager is null");
            }
            
        }
    }

    private void OnDrawGizmos()
    {
        if (!ShowGizmo)
            return;

        Gizmos.color = Color.green;

        BoxCollider boxCollider = GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            Vector3 worldCenter = boxCollider.transform.TransformPoint(boxCollider.center);
            Vector3 size = Vector3.Scale(boxCollider.size, boxCollider.transform.lossyScale);

            Gizmos.DrawCube(worldCenter, size);
        }
        else
        {
            Debug.LogWarning("BoxCollider is null", this);
        }
    }
}

using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent (typeof(NavMeshSurface))]
public class NavMesh : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMesh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMesh ??= GetComponent<NavMeshSurface>();
        navMesh.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

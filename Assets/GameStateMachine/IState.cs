using UnityEngine;
using UnityEngine.Rendering.Universal;

public interface IState 
{
    void EnterState();

    void FixedUpdateState();

    void UpdateState();

    void LateUpdateState();

    void ExitState();
    
}

using System.Collections.Generic;
using UnityEngine;

public class TriggerStateSwitch : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.GameStateManager.SwitchStates(GameManager.instance.GameStateManager.gameoverState);
        }
        
    }
}

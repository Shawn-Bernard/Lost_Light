using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour,Inputs.IPlayerActions
{
    [SerializeField] private Inputs inputs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        try
        {
            inputs = new Inputs();
            inputs.Player.SetCallbacks(this);
            inputs.Player.Enable();
        }
        catch (Exception exception)
        {
            Debug.Log("input error " + exception);
        }
    }

    #region Input Events

    public event Action<Vector2> MoveInputEvent;

    public event Action<Vector2> LookInputEvent;

    public event Action<InputAction.CallbackContext> AttackInputEvent;

    #endregion

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInputEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInputEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        AttackInputEvent?.Invoke(context);
    }

    void OnEnable()
    {
        if (inputs != null)
        {
            inputs.Player.Enable();
        }
    }

    void OnDestroy()
    {
        if (inputs != null)
        {
            inputs.Player.Disable();
        }
    }
}

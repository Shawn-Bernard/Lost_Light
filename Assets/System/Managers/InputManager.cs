using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour,Inputs.IPlayerActions
{

    private Inputs inputs;

    void Awake()
    {
        try
        {
            inputs = new Inputs();

            inputs.Player.SetCallbacks(this);
            inputs.Player.Enable();
        }
        catch (Exception exception) 
        {
            Debug.LogError("Input error" + exception);
        }
        
    }

    #region Input Events

    public event Action<Vector2> MoveInputEvent;

    public event Action<Vector2> LookInputEvent;

    public event Action<InputAction.CallbackContext> JumpInputEvent;
    public event Action<InputAction.CallbackContext> SprintInputEvent;
    public event Action<InputAction.CallbackContext> CrouchInputEvent;
    public event Action<InputAction.CallbackContext> InteractInputEvent;

    #endregion

    #region Input Callbacks

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInputEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInputEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInputEvent?.Invoke(context);
        }
        if (context.performed)
        {
            Debug.Log("Jump performed");
        }

    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        SprintInputEvent?.Invoke(context);
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        CrouchInputEvent?.Invoke(context);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        InteractInputEvent?.Invoke(context);
    }

    #endregion


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

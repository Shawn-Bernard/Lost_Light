using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float mouseSensitivity = 5f;
    


    [SerializeField] private CharacterController characterController;

    [SerializeField] private Vector2 moveInput;

    [SerializeField] private Vector2 lookInput;

    public Vector3 positon;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputManager = GameManager.instance.InputManager;

    }

    public void HandleMovement()
    {
        Vector3 moveInputDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 worldMoveDirection = transform.TransformDirection(moveInputDirection);


        Vector3 horizontalMoveDirection = worldMoveDirection * speed;

        Vector3 movement = horizontalMoveDirection;

        characterController.Move(movement * Time.deltaTime);
    }
    public void HandleLook()
    {
        float LookX = lookInput.x * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * LookX);
    }


    private void SetMoveInput(Vector2 inputValue)
    {
        moveInput = new Vector2(inputValue.x, inputValue.y);
    }

    private void SetLookInput(Vector2 inputValue)
    {
        lookInput = new Vector2(inputValue.x, inputValue.y);
    }

    private void SetAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Physics.Raycast(transform.position, transform.position + Vector3.forward, out RaycastHit hitInfo, 5f))
            {
                Debug.Log($"Shot has hit {hitInfo.collider.gameObject.name}");
                if (hitInfo.collider.CompareTag("Enemy"))
                {
                    Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
                    enemy.Death();
                }
            }
        }
    }
    /// <summary>
    /// This disables characterController and then moves player into position and re-enables it  
    /// </summary>
    /// <param name="setPosition"></param>
    public void MovePlayer(Vector3 setPosition)
    {
        characterController.enabled = false;
        transform.position = setPosition;
        characterController.enabled = true;
    }

    private void OnEnable()
    {
        inputManager.MoveInputEvent += SetMoveInput;
        inputManager.LookInputEvent += SetLookInput;
        inputManager.AttackInputEvent += SetAttackInput;
    }

    private void OnDisable()
    {
        inputManager.MoveInputEvent -= SetMoveInput;
        inputManager.LookInputEvent -= SetLookInput;
        inputManager.AttackInputEvent -= SetAttackInput;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.forward);
    }
}

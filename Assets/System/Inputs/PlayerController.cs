using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputManager inputManager = GameManager.instance.InputManager;

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float currentSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 5f;

    private Vector3 velocity;

    private CharacterController characterController;

    [SerializeField] private Vector2 moveInput;

    [SerializeField] private Vector2 lookInput;

    public GameObject attackModel;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Start()
    {
    }

    private void Update()
    {
        HandleMovement();
    }
    void FixedUpdate()
    {
        HandleLook();
    }

    public void HandleMovement()
    {
        Vector3 moveInputDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 worldMoveDirection = transform.TransformDirection(moveInputDirection);


        Vector3 horizontalMoveDirection = worldMoveDirection * currentSpeed;

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
            Instantiate(attackModel, transform.position + Vector3.forward, Quaternion.identity);
            if (Physics.Raycast(transform.position, transform.position + Vector3.forward, out RaycastHit hitInfo, 5f))
            {
                //Debug.Log("Player hit");
            }
            Debug.Log("Attack button");
        }
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

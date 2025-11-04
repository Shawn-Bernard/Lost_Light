using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum MovementState
    {
        Idle,
        Walking,
        Sprinting,
        Crouching,
        Jumping,
        Falling
    }

    public MovementState currentMovementState;

    private InputManager InputManager => GameManager.instance.InputManager;

    private Rigidbody rb => characterController.attachedRigidbody;
    CharacterController characterController => GetComponent<CharacterController>();

    [Header("Input Vectors")]
    [SerializeField] private Vector2 moveInput;

    [SerializeField] private Vector2 lookInput;

    [Header("Move & Look locks")]
    public bool canMove;
    public bool canLook;

    [SerializeField] private bool JumpEnabled = true;
    [SerializeField] private bool SprintEnabled;
    [SerializeField] private bool crouchEnabled;
    [SerializeField] private bool crouchInput = false;

    [Header("Movement Speed")]

    [SerializeField] private float currentSpeed;
    public float movementSpeed = 5;

    private float speedTransitionDuration;
    [SerializeField] private float walkSpeed = 4.0f;
    [SerializeField] private float sprintSpeed = 7.0f;

    [Header("Mouse Sensivivity")]
    public float MouseSensivivity = 05f;


    [Header("Move & Look locks")]
    private float lowerLookLimit = -60;
    private float UpperLookLimit = 60;

    private Vector3 velocity;

    public GameObject cameraRoot;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchSpeed = 2.0f;
    private float standingHeight;
    private Vector3 standingCenter;
    private float standingCamY;
    private bool isObstructed = false;

    [SerializeField] private float crouchTransitionDuration = 0.2f; // Time in seconds for crouch/stand transition (approximate completion)
    [SerializeField] private float crouchingHeight = 1.0f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private float crouchingCamY = 0.75f;

    private float targetHeight;
    private Vector3 targetCenter;
    private float targetCamY; // Target Y position for camera root during crouch transition

    private int playerLayerMask;

    [Header("Jump & gravity settings")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float gravity = 30f;
    [SerializeField] private float jumpHeight = 2.0f;

    [SerializeField] private float jumpCooldown = 0.2f;
    [SerializeField] private float jumpTimer;
    [SerializeField] private float groundCheckRadius = .1f;
    [SerializeField] private bool jumpRequested = false;

    public Transform spawnPosition;

    private void Awake()
    {
        playerLayerMask = ~LayerMask.GetMask("Player");

        #region Initialize Default values

        currentMovementState = MovementState.Idle;
        // Initialize crouch variables
        standingHeight = characterController.height;
        standingCenter = characterController.center;
        standingCamY = cameraRoot.transform.localPosition.y;

        targetHeight = standingHeight;
        targetCenter = standingCenter;
        targetCamY = cameraRoot.transform.localPosition.y;

        crouchEnabled = false;
        SprintEnabled = false;

        #endregion
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //HandleMovement();
    }

    private void LateUpdate()
    {
        //HandleLook();
    }

    public void HandleMovement()
    {

        //if (!canMove) return;
        DetermineMovementState();

        // Ground check
        GroundedCheck();



        // Handle crouch
        HandleCrouch();


        ApplyMovement();
        
    }

    private void DetermineMovementState()
    {
        // if the player is not grounded they are either jumping or falling 
        if (isGrounded == false)
        {
            if (velocity.y > 0.1f)
            {
                currentMovementState = MovementState.Jumping;
            }
            else if (velocity.y < 0)
            {
                currentMovementState = MovementState.Falling;
            }
        }
        else if (isGrounded == true)
        {
            if (crouchEnabled == true || isObstructed == true)
            {
                currentMovementState = MovementState.Crouching;
            }
            else if (SprintEnabled == true && currentMovementState != MovementState.Crouching)
            {
                currentMovementState = MovementState.Sprinting;
            }
            else if (moveInput.magnitude > 0.1f && SprintEnabled == false && crouchEnabled == false)
            {
                currentMovementState = MovementState.Walking;
            }
            else if (moveInput.magnitude <= 0.1f && SprintEnabled == false && crouchEnabled == false)
            {
                currentMovementState = MovementState.Idle;
            }

        }
    }

    private void ApplyMovement()
    {
        //Step 1 Getting input direction
        Vector3 moveInputDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 worldMoveDirection = transform.TransformDirection(moveInputDirection);

        //Step 2 determine move speed
        float targetSpeed = walkSpeed;

        switch (currentMovementState)
        {
            case MovementState.Crouching:
                {
                    targetSpeed = crouchSpeed;
                    break;
                }
            case MovementState.Sprinting:
                {
                    targetSpeed = sprintSpeed;
                    break;
                }
            default:
                {
                    targetSpeed = walkSpeed;
                    break;
                }
        }

        if (SprintEnabled)
        {
            targetSpeed = sprintSpeed;
        }
        else
        {
            targetSpeed = walkSpeed;
        }
        //Step 3 smoothly interpolate current speed towards  
        float lerpSpeed = 1f - Mathf.Pow(0.01f, Time.deltaTime / speedTransitionDuration);
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, lerpSpeed);
        //Step 4 handle horizontal movement


        //Step 5 handle jumping and gravity
        ApplyJumpAndGravity();

        Vector3 horizontalMoveDirection = worldMoveDirection * currentSpeed;

        //Step 6 Combine horizontal and vertical movement

        Vector3 movement = horizontalMoveDirection;
        movement.y = velocity.y;
        // Step 7 apply fianl movement
        characterController.Move(movement * Time.deltaTime);
    }
    public void HandleLook()
    {
        float LookX = lookInput.x * MouseSensivivity * Time.deltaTime;
        float LookY = lookInput.y * MouseSensivivity * Time.deltaTime;
        transform.Rotate(Vector3.up * LookX);

        Vector3 currentAngles = cameraRoot.transform.localEulerAngles;
        float newRotationX = currentAngles.x - LookY;

        newRotationX = (newRotationX > 180) ? newRotationX - 360 : newRotationX;
        newRotationX = Math.Clamp(newRotationX, lowerLookLimit, UpperLookLimit);

        cameraRoot.transform.localEulerAngles = new Vector3(newRotationX, 0, 0);
    }

    private void ApplyJumpAndGravity()
    {
        if (JumpEnabled == true) // checks if jump is enabled
        {
            if (jumpRequested == true)
            {
                // Calculate the itial upward velocity to reach the desired jump height
                velocity.y = MathF.Sqrt(2f * jumpHeight * gravity);

                jumpRequested = false;
                jumpTimer = jumpCooldown;
            }
            
        }

        // Apply gravity based on the player's current state (grounded or in air).
        if (isGrounded && velocity.y < 0)
        {
            // If grounded and moving downwards (due to accumulated gravity from previous frames),
            // snap velocity to a small negative value. This keeps the character firmly on the ground
            // without allowing gravity to build up indefinitely, preventing "bouncing" or
            // incorrect ground detection issues.

            velocity.y = -1f;
        }
        else  // If not grounded (in the air):
        {
            // apply standard gravity
            velocity.y -= gravity * Time.deltaTime;
        }




        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    private void HandleCrouch()
    {


        bool shouldCrouch = crouchInput == true;

        // if airborne and was crouching, maintain crouch state (prevents standing up from crouch while walking off a ledge)
        bool wasAlreadyCrouching = characterController.height < (standingHeight - 0.05f);


        if (isGrounded == false && wasAlreadyCrouching)
        {
            shouldCrouch = true; // Maintain crouch state if airborne (walking off ledge while crouching)
        }

        if (shouldCrouch)
        {
            targetHeight = crouchingHeight;
            targetCenter = crouchingCenter;
            targetCamY = crouchingCamY;
            isObstructed = false; // No obstruction when intentionally crouching
        }
        else
        {
            // float maxAllowedHeight = GetMaxAllowedHeight();

            float maxAllowedHeight = GetMaxHeight();

            if (maxAllowedHeight >= standingHeight - 0.05f)
            {
                // No obstruction, allow immediate transition to standing
                targetHeight = standingHeight;
                targetCenter = standingCenter;
                targetCamY = standingCamY;
                isObstructed = false;
            }

            else
            {
                // Obstruction detected, limit height and center
                targetHeight = Mathf.Min(standingHeight, maxAllowedHeight);
                float standRatio = Mathf.Clamp01((targetHeight - crouchingHeight) / (standingHeight - crouchingHeight));
                targetCenter = Vector3.Lerp(crouchingCenter, standingCenter, standRatio);
                targetCamY = Mathf.Lerp(crouchingCamY, standingCamY, standRatio);
                isObstructed = true;
            }
        }

        // Calculate lerp speed based on desired duration
        // This formula ensures the transition approximately reaches 99% of the target in 'crouchTransitionDuration' seconds.
        float lerpSpeed = 1f - Mathf.Pow(0.01f, Time.deltaTime / crouchTransitionDuration);

        // Smoothly transition to targets
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, lerpSpeed);
        characterController.center = Vector3.Lerp(characterController.center, targetCenter, lerpSpeed);

        Vector3 currentCamPos = cameraRoot.transform.localPosition;
        cameraRoot.transform.localPosition = new Vector3(currentCamPos.x, Mathf.Lerp(currentCamPos.y, targetCamY, lerpSpeed), currentCamPos.z);

    }
    public void GroundedCheck()
    {
        isGrounded = characterController.isGrounded;
    }

    private float GetMaxHeight()
    {
        RaycastHit raycastHit;
        float maxCheckDistance = standingHeight + 0.15f;

        if (Physics.Raycast(transform.position,cameraRoot.transform.up,out raycastHit, maxCheckDistance, playerLayerMask))
        {
            float maxHeight = raycastHit.distance - 01f;

            maxHeight = Mathf.Max(maxHeight, crouchingHeight);

            return maxHeight;
        }

        return standingHeight;

    }

    public void MovePlayerToSpawn(Transform spawnPoint)
    {
        Debug.Log("spawnpoint happened");
        characterController.enabled = false;

        transform.position = spawnPosition.position;
        
        characterController.enabled = true;

    }
    #region Inputs 
    private void SetMoveInput(Vector2 inputValue)
    {
        moveInput = new Vector2 (inputValue.x, inputValue.y);
    }

    private void SetLookInput(Vector2 inputValue)
    {
        lookInput = new Vector2 (inputValue.x, inputValue.y);
    }

    private void SetJumpInput(InputAction.CallbackContext context)
    {
        if (JumpEnabled == true)
        {
            if (context.started && isGrounded && jumpTimer <= 0)
            {
                Debug.Log("Jump Started");

                jumpRequested = true;

                jumpTimer = 0.1f;
            }
        }
        
    }

    private void SetCrouchInput(InputAction.CallbackContext context)
    {
        //if (!crouchEnabled) return;

        if (context.performed)
        {
            crouchInput = !crouchInput;
            Debug.Log("Crouch button hit");
        }
        
    }

    private void SetSprintInput(InputAction.CallbackContext context)
    {
        //if (!SprintEnabled) return;

        if (context.started)
        {
            SprintEnabled = true;
        }
        else if (context.canceled)
        {
            SprintEnabled = false;
        }
            Debug.Log("Sprint button hit");
    }
    #endregion
    private void OnEnable()
    {
        InputManager.MoveInputEvent += SetMoveInput;
        InputManager.LookInputEvent += SetLookInput;

        InputManager.JumpInputEvent += SetJumpInput;

        InputManager.CrouchInputEvent += SetCrouchInput;

        InputManager.SprintInputEvent += SetSprintInput;
    }


    private void OnDestroy()
    {
        InputManager.MoveInputEvent -= SetMoveInput;
        InputManager.LookInputEvent -= SetLookInput;

        InputManager.JumpInputEvent -= SetJumpInput;

        InputManager.CrouchInputEvent -= SetCrouchInput;

        InputManager.SprintInputEvent -= SetSprintInput;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(LineRenderer))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSetting playerSetting;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private ScoreManager scoreManager;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private LineRenderer lineRenderer;


    private Vector2 moveInput;

    private Vector2 lookInput;

    private float nextFireTime;
    private float movementSpeed;
    private float mouseSensitivity;
    private float fireRate;
    private float laserDuration;

    private void Awake()
    {
        soundManager ??= GameManager.instance.SoundManager;
        scoreManager ??= GameManager.instance.ScoreManager;
        inputManager ??= GameManager.instance.InputManager;
        gameStateManager ??= GameManager.instance.GameStateManager;
        characterController ??= GetComponent<CharacterController>();
        lineRenderer ??= GetComponent<LineRenderer>();
        ApplySetting();
    }

    private void Update()
    {
        if (nextFireTime > 0)
        {
            nextFireTime -= Time.deltaTime;
        }
    }

    private void ShootingEffect(Vector3 endPosition)
    {
        if (soundManager != null) 
        {
            soundManager.PlayShoot(transform.position);
        }
        StartCoroutine(AnimateLaser(endPosition));
    }

    private IEnumerator AnimateLaser(Vector3 endPosition)
    {
        if (lineRenderer != null) 
        {
            lineRenderer.enabled = true;

            Vector3 startPosition = transform.position;

            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, endPosition);


            yield return new WaitForSeconds(laserDuration);

            lineRenderer.enabled = false;
        }
    }

    #region Handlers

    public void HandleMovement()
    {
        Vector3 moveInputDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 worldMoveDirection = transform.TransformDirection(moveInputDirection);


        Vector3 horizontalMoveDirection = worldMoveDirection * playerSetting.movementSpeed;

        Vector3 movement = horizontalMoveDirection;

        characterController.Move(movement * Time.deltaTime);
    }
    public void HandleLook()
    {
        float LookX = lookInput.x * playerSetting.mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * LookX);
    }

    #endregion

    #region Setting Inputs

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
            if (nextFireTime <= 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    ShootingEffect(hit.point);
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        Enemy enemy = hit.collider.GetComponent<Enemy>();
                        if (enemy != null)
                        {
                            enemy.Death();
                            scoreManager.KilledEnemy();
                            soundManager.PlayHit(hit.point);
                        }
                    }
                }
                nextFireTime = fireRate;
            }
        }
    }

    #endregion

    /// <summary>
    /// This disables characterController and then moves player into position and re-enables it  
    /// </summary>
    /// <param name="setPosition"></param>
    public void MovePlayer(Vector3 setPosition)
    {
        if (characterController != null) 
        {
            characterController.enabled = false;
            transform.position = setPosition;
            characterController.enabled = true;
        }
    }

    public void Death()
    {
        gameStateManager.SwitchToGameOver();
    }

    private void OnEnable()
    {
        GameplayState.gameplayStateUpdate += HandleMovement;
        GameplayState.gameplayStateUpdate += HandleLook;
        inputManager.MoveInputEvent += SetMoveInput;
        inputManager.LookInputEvent += SetLookInput;
        inputManager.AttackInputEvent += SetAttackInput;
    }

    private void OnDisable()
    {
        GameplayState.gameplayStateUpdate -= HandleMovement;
        GameplayState.gameplayStateUpdate -= HandleLook;
        inputManager.MoveInputEvent -= SetMoveInput;
        inputManager.LookInputEvent -= SetLookInput;
        inputManager.AttackInputEvent -= SetAttackInput;
    }

    public void Save(ref PlayerData data)
    {
        data.position = transform.position;
        data.movementSpeed = movementSpeed;
        data.mouseSensitivity = mouseSensitivity;
        data.fireRate = fireRate;
    }

    public void Load(PlayerData data)
    {
        MovePlayer(data.position);
        playerSetting.movementSpeed = data.movementSpeed;
        playerSetting.mouseSensitivity = data.mouseSensitivity;
        playerSetting.fireRate = data.fireRate;
        ApplySetting();
    }
    public void ResetData()
    {
        ApplySetting();
    }
    private void ApplySetting()
    {
        movementSpeed = playerSetting.movementSpeed;
        mouseSensitivity = playerSetting.mouseSensitivity;
        fireRate = playerSetting.fireRate;
        laserDuration = playerSetting.laserDuration;
    }
}

[System.Serializable]
public struct PlayerData
{
    public Vector3 position;
    public float movementSpeed;
    public float mouseSensitivity;
    public float fireRate;
}

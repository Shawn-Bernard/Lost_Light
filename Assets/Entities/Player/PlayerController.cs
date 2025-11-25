using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerSetting playerSetting;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private ScoreManager scoreManager;

    private Vector2 moveInput;

    private Vector2 lookInput;

    private float nextFireTime;
    private float movementSpeed;
    private float mouseSensitivity;
    private float fireRate;

    private void Awake()
    {
        soundManager ??= GameManager.instance.SoundManager;
        scoreManager ??= GameManager.instance.ScoreManager;
        inputManager ??= GameManager.instance.InputManager;
        characterController ??= GetComponent<CharacterController>();
        ApplySetting();
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
            if (Time.time >= nextFireTime)
            {
                soundManager.PlayShoot(transform.position);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        scoreManager.KilledEnemy();
                        soundManager.PlayHit(transform.position);
                        hit.collider.GetComponent<Enemy>().Death();
                    }
                }
                nextFireTime = Time.time + (1f / playerSetting.fireRate);
            }
            else
            {
                Debug.Log($"Can't shoot yet {nextFireTime}");
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
        playerSetting.movementSpeed = playerSetting.defaultMovementSpeed;
        playerSetting.mouseSensitivity = playerSetting.defaultMouseSensitivity;
        playerSetting.fireRate = playerSetting.defaultFireRate;
        ApplySetting();
    }
    private void ApplySetting()
    {
        movementSpeed = playerSetting.movementSpeed;
        mouseSensitivity = playerSetting.mouseSensitivity;
        fireRate = playerSetting.fireRate;
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

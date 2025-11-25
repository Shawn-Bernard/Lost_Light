using System;
using UnityEngine;

public class BatteryController : MonoBehaviour
{
    [SerializeField] private GameplayMenuController gameplayMenu;
    [SerializeField] private BatterySetting batterySetting;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private GameStateManager gameStateManager;

    private int currentBatteryCount;
    private int batteryCapacity;

    private float batteryLife;
    private float maxBatteryLife;

    private float drainRate;

    private void Awake()
    {
        soundManager ??= GameManager.instance.SoundManager;
    }
    void Start()
    {
        ApplySetting();
        gameplayMenu ??= GameManager.instance.UIManager.gameplayMenuController;
        gameplayMenu.UpdateBatteryCount(currentBatteryCount);
        gameStateManager ??= GameManager.instance.GameStateManager;
    }

    // Update is called once per frame
    void Update()
    {
        HandleBatteryLife();
    }

    public void HandleBatteryLife()
    {
        batteryLife -= Time.deltaTime * drainRate;
        if (batteryLife <= 0)
        {
            currentBatteryCount--;
            batteryLife = maxBatteryLife;
            gameplayMenu.UpdateBatteryCount(currentBatteryCount);
        }

        if (batteryLife <= 0 && batteryCapacity <= 0)
        {
            gameStateManager.SwitchToGameOver();
        }
        gameplayMenu.UpdateBatteryLife(batteryLife);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            soundManager.PlayPickup(transform.position);
            currentBatteryCount++;
            gameplayMenu.UpdateBatteryCount(currentBatteryCount);
            Destroy(other.gameObject);
        }
    }
    public void Save(ref BatteryData data)
    {
        data.batteryCount = currentBatteryCount;
        data.batteryLife = batteryLife;
    }

    public void Load(BatteryData data)
    {
        currentBatteryCount = data.batteryCount;
        batteryLife = data.batteryLife;
    }

    public void ResetData()
    {
        batterySetting.startBatteryCount = batterySetting.defaultStartBatteryCount;
        ApplySetting();
    }

    private void ApplySetting()
    {
        currentBatteryCount = batterySetting.startBatteryCount;
        batteryCapacity = batterySetting.batteryCapacity;
        maxBatteryLife = batterySetting.maxBatteryLife;
        drainRate = batterySetting.drainRate;
        batteryLife = Mathf.Clamp(batteryLife, 0, maxBatteryLife);
        batteryLife = maxBatteryLife;
        gameplayMenu.UpdateBatteryCount(currentBatteryCount);
    }
}

[System.Serializable]
public struct BatteryData
{
    public int batteryCount;
    public float batteryLife;
}

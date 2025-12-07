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
    private int lastIntBatteryLife;
    private float maxBatteryLife;

    private float drainRate;

    private void Awake()
    {
        soundManager ??= GameManager.instance.SoundManager;
        gameplayMenu ??= GameManager.instance.UIManager.gameplayMenuController;
        gameStateManager ??= GameManager.instance.GameStateManager;
    }
    void Start()
    {
        ApplySetting();
        UpdateBatteryCount();
    }

    // Update is called once per frame
    void Update()
    {
        HandleBatteryLife();
    }

    public void HandleBatteryLife()
    {
        batteryLife -= Time.deltaTime * drainRate;
        int intBatteryLife = Mathf.FloorToInt(batteryLife);

        if (intBatteryLife != lastIntBatteryLife)
        {
            lastIntBatteryLife = intBatteryLife;
            UpdateBatteryLife();
        }
        if (batteryLife <= 0)
        {
            ConsumeBattery();
        }
    }

    private void AddBattery()
    {
        if (currentBatteryCount < batteryCapacity)
        {
            currentBatteryCount++;
            UpdateBatteryCount();
        }
    }

    private void ConsumeBattery()
    {
        currentBatteryCount--;
        batteryLife = maxBatteryLife;
        EmptyBatteryCheck();
    }

    private void EmptyBatteryCheck()
    {
        if (batteryLife <= 0 && batteryCapacity <= 0)
        {
            gameStateManager.SwitchToGameOver();
        }
    }

    private void UpdateBatteryLife()
    {
        gameplayMenu.UpdateBatteryLife(lastIntBatteryLife);
    }

    private void UpdateBatteryCount()
    {
        gameplayMenu.UpdateBatteryCount(currentBatteryCount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            soundManager.PlayPickup(other.transform.position);
            AddBattery();
            Destroy(other.gameObject);
        }
    }
    #region Save and load for data
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
    #endregion
}

[System.Serializable]
public struct BatteryData
{
    public int batteryCount;
    public float batteryLife;
}

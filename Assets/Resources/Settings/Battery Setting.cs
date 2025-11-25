using UnityEngine;

[CreateAssetMenu(fileName = "Battery Setting", menuName = "Game Setting/Battery Setting")]
public class BatterySetting : ScriptableObject
{
    public int defaultStartBatteryCount;
    public int startBatteryCount;
    public int batteryCapacity;
    public float maxBatteryLife;
    public float drainRate;
}

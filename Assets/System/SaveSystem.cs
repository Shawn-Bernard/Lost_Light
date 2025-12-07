using System;
using System.IO;
using UnityEngine;
public class SaveSystem : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BatteryController batteryController;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private MapGenerator mapSetting;

    public PlayerController PlayerController => playerController;


    private static SaveData saveData = new SaveData();

    private GameManager gameManager => GameManager.instance;
    [System.Serializable]
    public struct SaveData
    {
        public PlayerData playerData;
        public BatteryData batteryData;
        public ScoreData scoreData;
        public SpawnerData spawnerData;
        public MapData mapData;
    }
    /// <summary>
    /// Returns the saved file
    /// </summary>
    /// <returns></returns>
    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + "/save" + ".data";
        return saveFile;
    }
    /// <summary>
    /// Check if save file exist
    /// </summary>
    /// <returns></returns>
    public static bool SaveFileExists()
    {
        return File.Exists(SaveFileName());
    }
    /// <summary>
    /// Deletes save file if it exist
    /// </summary>
    public static void DeleteSave()
    {
        string path = SaveFileName();
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
    /// <summary>
    /// Calls handle save data and crates a json file
    /// </summary>
    public static void Save()
    {
        HandleSaveData();

        File.WriteAllText(SaveFileName(),JsonUtility.ToJson(saveData,true));
    }
    /// <summary>
    /// Calls the save method to apply data
    /// </summary>
    private static void HandleSaveData()
    {
        GameManager.instance.PlayerController.Save( ref saveData.playerData);
        GameManager.instance.BatteryController.Save(ref saveData.batteryData);
        GameManager.instance.ScoreManager.Save( ref saveData.scoreData);
        GameManager.instance.SpawnDataContainer.Save(ref saveData.spawnerData);
        GameManager.instance.MapSetting.Save(ref saveData.mapData);
    }
    /// <summary>
    /// Applies the saved to the game save data in save system
    /// </summary>
    public static void Load()
    {
        string saveContent = File.ReadAllText(SaveFileName());

        saveData = JsonUtility.FromJson<SaveData>(saveContent);
        HandleLoadData();
    }

    private static void HandleLoadData()
    {
        GameManager.instance.PlayerController.Load(saveData.playerData);
        GameManager.instance.BatteryController.Load(saveData.batteryData);
        GameManager.instance.ScoreManager.Load(saveData.scoreData);
        GameManager.instance.SpawnDataContainer.Load(saveData.spawnerData);
        GameManager.instance.MapSetting.Load(saveData.mapData);
    }

    public static void ResetGame()
    {
        DeleteSave();
        GameManager.instance.PlayerController.ResetData();
        GameManager.instance.BatteryController.ResetData();
        GameManager.instance.ScoreManager.ResetData();
        GameManager.instance.SpawnDataContainer.ResetData();
        GameManager.instance.MapSetting.ResetData();
    }
}

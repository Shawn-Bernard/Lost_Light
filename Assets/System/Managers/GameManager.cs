using UnityEditor.VersionControl;
using UnityEngine;

// This help load first 
//[DefaultExecutionOrder(-100)]

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }

    [Header("Manager References")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private BatteryController batteryController;
    [SerializeField] private SpawnDataContainer spawnDataContainer;
    [SerializeField] private MapSetting mapSetting;


    public InputManager InputManager => inputManager;

    public GameStateManager GameStateManager => gameStateManager;

    public PlayerController PlayerController => playerController;

    public UIManager UIManager => uiManager;

    public LevelManager LevelManager => levelManager;

    public ScoreManager ScoreManager => scoreManager;

    public SoundManager SoundManager => soundManager;

    public BatteryController BatteryController => batteryController;

    public SpawnDataContainer SpawnDataContainer => spawnDataContainer;

    public MapSetting MapSetting => mapSetting;
    void Awake()
    {
        #region Singleton
        // ??= meaning, if (inputManager == null) inputManager = GetComponentInChildren<InputManager>();
        inputManager ??= GetComponentInChildren<InputManager>();
        gameStateManager ??= GetComponentInChildren<GameStateManager>();
        playerController ??= GetComponentInChildren<PlayerController>();
        uiManager ??= GetComponentInChildren<UIManager>();
        levelManager ??= GetComponentInChildren<LevelManager>();
        scoreManager ??= GetComponentInChildren<ScoreManager>();
        soundManager ??= GetComponentInChildren<SoundManager>();
        batteryController ??= GetComponentInChildren<BatteryController>();

        spawnDataContainer ??= Resources.Load<SpawnDataContainer>("Data/Spawn Data");
        mapSetting ??= Resources.Load<MapSetting>("Settings/Map Setting");

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        #endregion
        
    }
}

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

[DisallowMultipleComponent]
public class GameBehaviour : MonoBehaviour, IDataPersistence
{
    public static GameBehaviour Instance;
    public bool _isGame;
    public List<Texture> _costumes;
    public RuntimeAnimatorController _philipController;
    public Material _philipMaterial;
    public Texture SelectedCostume { get; private set; }
    public bool PlayerIsDead { get; private set; }
    public bool NoCheckpoints { get; private set; } = false;
    
    public int _costumeID;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        Application.targetFrameRate = 60;
        SetQualitySettings();
        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        LoadGame();
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public Vector3 ConvertTimerToVector3(float time)
    {
        float m = Mathf.FloorToInt(time / 60000);
        float s = Mathf.FloorToInt((time - m * 60000) / 1000);
        float ms = Mathf.FloorToInt(time - m * 60000 - s * 1000);
        return new Vector3(m, s, ms);
    }
    public float ConvertTimerToFloat(Vector3 time)
    {
        return (time.x * 60 + time.y) * 1000 + time.z;
    }
    void SetQualitySettings()
    {
        QualitySettings.vSyncCount = 0;
    }
    public void SaveGame()
    {
        DataPersistenceManager.Instance.SaveGame();
    }
    public void LoadGame()
    {
        DataPersistenceManager.Instance.LoadGame();
    }
    public void OnResetProgressPress()
    {
        DataPersistenceManager.Instance.NewGame();
        SaveGame();
    }
    public void SetCostume(int cID = 0)
    {
        _costumeID = cID;
        SelectedCostume = _costumes[cID];
        SaveGame();
    }
    public void SetPlayerDeath(bool _isDead)
    {
        PlayerIsDead = _isDead;
    }
    public void SetNoCheckpoints(bool _noCheckpoints)
    {
        NoCheckpoints = _noCheckpoints;
        SaveGame();
    }
    public void SaveData(GameData data)
    {
        data.selectedCostume = _costumeID;
        data.noCheckpoints = NoCheckpoints;
    }
    public void LoadData(GameData data)
    {
        _costumeID = data.selectedCostume;
        SelectedCostume = _costumes[data.selectedCostume];
        NoCheckpoints = data.noCheckpoints;
    }
}

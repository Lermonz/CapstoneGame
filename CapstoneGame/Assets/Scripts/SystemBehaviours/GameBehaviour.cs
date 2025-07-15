using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DisallowMultipleComponent]
public class GameBehaviour : MonoBehaviour
{
    public static GameBehaviour Instance;
    public bool _isGame;
    public List<Texture> _costumes;
    public Texture SelectedCostume { get; private set; }
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
        //Cursor.lockState = CursorLockMode.Locked;
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
        SelectedCostume = _costumes[cID];
    }
}

using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string _fileName;
    
    private GameData _gameData;
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _dataHandler;

    public static DataPersistenceManager Instance {get; private set;}
    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        else
            Instance = this;
        DontDestroyOnLoad(this.gameObject);
        this._dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName);
    }
    private void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        this._dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame(){
        this._gameData = new GameData();
    }
    public void SaveGame(){
        if(this._gameData == null) {
            Debug.LogWarning("No data found when trying to save.");
            return;
        }
        // pass the data to other scripts so they can update it
        foreach(IDataPersistence dataPersistenceObj in _dataPersistenceObjects) {
            dataPersistenceObj.SaveData(_gameData);
        }

        // save that data to a file using the data handler
        _dataHandler.Save(_gameData);
    }
    public void LoadGame(){
        // Load any saved data from a file using data handler
        this._gameData = _dataHandler.Load();

        if (this._gameData == null) {
            NewGame();
        }
        // push the loaded data to all scripts that need it
        foreach(IDataPersistence dataPersistenceObj in _dataPersistenceObjects) {
            dataPersistenceObj.LoadData(_gameData);
        }
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects() 
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = 
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}

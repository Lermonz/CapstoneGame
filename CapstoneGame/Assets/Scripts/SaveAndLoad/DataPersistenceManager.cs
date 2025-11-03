using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

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
        LoadGame();
    }

    public void NewGame(){
        this._gameData = new GameData();
    }
    public void SetLevelTimes()
    {
        this._gameData.levelDevTimes = new GameData().levelDevTimes;
        this._gameData.levelDiamonds = new GameData().levelDiamonds;
        this._gameData.levelGolds = new GameData().levelGolds;
        this._gameData.levelSilvers = new GameData().levelSilvers;
        this._gameData.levelBronzes = new GameData().levelBronzes;
        SaveGame();
    }
    public void SaveGame()
    {
        Debug.Log("game is saved :)");
        this._dataPersistenceObjects = FindAllDataPersistenceObjects();
        if (this._gameData == null)
        {
            Debug.LogWarning("No data found when trying to save.");
            return;
        }
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            //Debug.Log("DataPersistenceManager SaveData foreach SAVE\n"+dataPersistenceObj);
            dataPersistenceObj.SaveData(_gameData);
            //Debug.Log("GAME SAVED");
        }

        // save that data to a file using the data handler
        _dataHandler.Save(_gameData);
    }
    public void LoadGame(){
        this._dataPersistenceObjects = FindAllDataPersistenceObjects();
        // Load any saved data from a file using data handler
        this._gameData = _dataHandler.Load();

        if (this._gameData == null) {
            NewGame();
        }
        // push the loaded data to all scripts that need it
        foreach(IDataPersistence dataPersistenceObj in _dataPersistenceObjects) {
            //Debug.Log("GAME LOADED");
            dataPersistenceObj.LoadData(_gameData);
        }
    }
    public void LoadSingleObjectData(IDataPersistence dataGameObject)
    {
        if (this._gameData == null)
        {
            NewGame();
        }
        //push loaded data to the script that called it
        dataGameObject.LoadData(_gameData);
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects =
            //FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        //Debug.Log("why no list of IDataPersistence??? " + (new List<IDataPersistence>(dataPersistenceObjects)));
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}

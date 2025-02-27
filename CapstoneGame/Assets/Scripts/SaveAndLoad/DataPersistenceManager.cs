using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData _gameData;
    private List<IDataPersistence> _dataPersistenceObjects;

    public static DataPersistenceManager Instance;
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    void Start()
    {
        this._dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame(){
        this._gameData = new GameData();
    }
    public void SaveGame(){
        // TODO - pass the data to other scripts so they can update it
        foreach(IDataPersistence dataPersistenceObj in _dataPersistenceObjects) {
            dataPersistenceObj.LoadData(_gameData);
        }
        Debug.Log("Saved PB: "+ _gameData.personalBest);

        // TODO - save that data to a file using the data handler
    }
    public void LoadGame(){
        // TODO - Load any saved data from a file using data handler
        if (this._gameData == null) {
            NewGame();
        }
        // TODO - push the loaded data to all scripts that need it
        foreach(IDataPersistence dataPersistenceObj in _dataPersistenceObjects) {
            dataPersistenceObj.SaveData(ref _gameData);
        }
        Debug.Log("Loaded PB: "+ _gameData.personalBest);
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects() {
        IEnumerable<IDataPersistence> dataPersistenceObjects = 
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string _dataDirPath = "";
    private string _dataFileName = "";
    public FileDataHandler(string dataDirPath, string dataFileName){
        this._dataDirPath = dataDirPath;
        this._dataFileName = dataFileName;
    }
    public GameData Load() {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath)) {
            try
            {
                // Load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // deserialize the data from the Json back into the C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data to file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }
    public void Save(GameData data) {
        // Path.Combine accounts for different OS's having different path separators
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        try {
            //create the directory the file will be written to if it doesn't exist already
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize the C# game data object into JSON
            string dataToStore = JsonUtility.ToJson(data,true); // can add a second input "true" to make it easy to read
        
            // write the serialize data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) {
                using (StreamWriter writer = new StreamWriter(stream)) {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e) {
            Debug.LogError("Error occured when trying to save data to file: "+fullPath+"\n"+e);
        }
    }
}

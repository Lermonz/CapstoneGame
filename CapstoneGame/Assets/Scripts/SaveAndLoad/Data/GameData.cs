using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //PLAYER DATA
    public SerializableDictionary<string, float> personalBest;
    public SerializableDictionary<string, string> medals;

    //GLOBAL LEVEL DATA
    public SerializableDictionary<string, float> levelGolds;
    public SerializableDictionary<string, float> levelSilvers;
    public SerializableDictionary<string, float> levelBronzes;
    // default initialized values of the constructor
    public GameData() {
        personalBest = new SerializableDictionary<string, float>();
        medals = new SerializableDictionary<string, string>();
        levelGolds = new SerializableDictionary<string, float> {
            {"1", 6400}, {"2", 6400}, {"3", 6400}, {"4", 6400}, 
            {"5", 6400}, {"6", 6400}, {"7", 6400}, {"8", 6400}, 
            {"9", 6400},{"10", 6400}
        };
        levelSilvers = new SerializableDictionary<string, float> {
            {"1", 8400}, {"2", 8400}, {"3", 8400}, {"4", 8400}, 
            {"5", 8400}, {"6", 8400}, {"7", 8400}, {"8", 8400}, 
            {"9", 8400}, {"10", 8400}
        };
        levelBronzes = new SerializableDictionary<string, float> {
            {"1", 11000}, {"2", 11000}, {"3", 11000}, {"4", 11000}, 
            {"5", 11000}, {"6", 11000}, {"7", 11000}, {"8", 11000}, 
            {"9", 11000},{"10", 11000}
        };
    }
}

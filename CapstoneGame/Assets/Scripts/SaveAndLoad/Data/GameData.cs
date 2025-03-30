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
    public SerializableDictionary<string, bool> unlockedLevels;
    // default initialized values of the constructor
    public GameData() {
        personalBest = new SerializableDictionary<string, float>();
        medals = new SerializableDictionary<string, string>();
        unlockedLevels = new SerializableDictionary<string, bool>();
        levelGolds = new SerializableDictionary<string, float> {
            {"1", 7000}, {"2", 9500}, {"3", 9000}, {"4", 8800}, 
            {"5", 14200}, {"6", 14600}, {"7", 9200}, {"8", 17500}, 
            {"9", 6200},{"10", 18500},{"11",10000}
        };
        levelSilvers = new SerializableDictionary<string, float> {
            {"1", 8500}, {"2", 11000}, {"3", 10500}, {"4", 10200}, 
            {"5", 16000}, {"6", 16500}, {"7", 10800}, {"8", 19500}, 
            {"9", 8000}, {"10", 20000},{"11",12500}
        };
        levelBronzes = new SerializableDictionary<string, float> {
            {"1", 11000}, {"2", 13000}, {"3", 13000}, {"4", 13000}, 
            {"5", 18000}, {"6", 18500}, {"7", 12000}, {"8", 21000}, 
            {"9", 10000},{"10", 24000},{"11",16000}
        };
    }
}

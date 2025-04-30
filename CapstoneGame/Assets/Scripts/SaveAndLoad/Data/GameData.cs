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
            {"1", 7700}, {"2", 9500}, {"3", 10400}, {"4", 11200}, 
            {"5", 11800}, {"6", 8500}, {"7", 12000}, {"8", 11400}, 
            {"9", 11800},{"10", 14000},{"11",7650},{"12",8200},
            {"13",6000},{"14",12800},{"15",12000},{"16",13500},
            {"17",155000},{"18",13800},{"19",14500},{"20",20000}
        };
        levelSilvers = new SerializableDictionary<string, float> {
            {"1", 8700}, {"2", 10800}, {"3", 12200}, {"4", 13000}, 
            {"5", 13700}, {"6", 10200}, {"7", 14400}, {"8", 14800}, 
            {"9", 14800}, {"10", 18500},{"11",9400},{"12",11000},
            {"13",12000},{"14",14400},{"15",15200},{"16",15000},
            {"17",19600},{"18",15800},{"19",16200},{"20",24500}
        };
        levelBronzes = new SerializableDictionary<string, float> {
            {"1", 10500}, {"2", 12500}, {"3", 14000}, {"4", 14500}, 
            {"5", 15500}, {"6", 13200}, {"7", 18000}, {"8", 17400}, 
            {"9", 19000},{"10", 22000},{"11",11000},{"12",14200},
            {"13",20000},{"14",18000},{"15",18800},{"16",18000},
            {"17",22000},{"18",19500},{"19",20000},{"20",30000}
        };
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //PLAYER DATA
    public SerializableDictionary<string, float> personalBest;
    public SerializableDictionary<string, string> medals;
    public SerializableDictionary<string, bool> costumes;
    public int selectedCostume;

    //CONTROLS DATA
    public string rebinds;
    public int musicVolume;
    public int soundVolume;

    //GLOBAL LEVEL DATA
    public SerializableDictionary<string, float> levelGolds;
    public SerializableDictionary<string, float> levelSilvers;
    public SerializableDictionary<string, float> levelBronzes;
    public SerializableDictionary<string, bool> unlockedLevels;
    // default initialized values of the constructor
    public GameData()
    {
        rebinds = string.Empty;
        selectedCostume = 0;
        musicVolume = 69;
        costumes = new SerializableDictionary<string, bool>
        {
            {"Vanilla", true}, {"Green", false}, {"Chocolate", false}, {"Lava-Dipped", false},
            {"Gameboy", false}, {"Transgender", false}, {"Golden", false}, {"Philip", false}
         };
        personalBest = new SerializableDictionary<string, float>();
        medals = new SerializableDictionary<string, string>();
        unlockedLevels = new SerializableDictionary<string, bool>();
        levelGolds = new SerializableDictionary<string, float> {
            {"1", 8900}, {"2", 9850}, {"3", 14000}, {"4", 10600},
            {"5", 8200}, {"6", 12300}, {"7", 10000}, {"8", 16000},
            {"9", 16400},{"10", 14400},{"11",12900},{"12",7500},
            {"13",9000},{"14",16200},{"15",12800},{"16",17000},
            {"17",16000},{"18",11800},{"19",31000},{"20",33500}
        };
        levelSilvers = new SerializableDictionary<string, float> {
            {"1", 10200}, {"2", 11500}, {"3", 15900}, {"4", 12800},
            {"5", 10000}, {"6", 14500}, {"7", 13000}, {"8", 20500},
            {"9", 18400}, {"10", 18000},{"11",14500},{"12",9500},
            {"13",13500},{"14",19750},{"15",14000},{"16",20500},
            {"17",19600},{"18",15000},{"19",45000},{"20",42000}
        };
        levelBronzes = new SerializableDictionary<string, float> {
            {"1", 12300}, {"2", 13500}, {"3", 18400}, {"4", 15000},
            {"5", 12200}, {"6", 17300}, {"7", 18000}, {"8", 26000},
            {"9", 22000},{"10", 21500},{"11",16800},{"12",11500},
            {"13",20000},{"14",23500},{"15",16950},{"16",25000},
            {"17",22200},{"18",20000},{"19",60000},{"20",50000}
        };
    }
}

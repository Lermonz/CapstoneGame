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
    public SerializableDictionary<string, float> levelDiamonds;
    public SerializableDictionary<string, float> levelGolds;
    public SerializableDictionary<string, float> levelSilvers;
    public SerializableDictionary<string, float> levelBronzes;
    public SerializableDictionary<string, bool> unlockedLevels;
    // default initialized values of the constructor
    public GameData()
    {
        rebinds = string.Empty;
        musicVolume = 69;
        costumes = new SerializableDictionary<string, bool>
        {
            {"Vanilla", true}, {"Green", false}, {"Invert", false}, {"Gameboy", false},
            {"Pilot", false}, {"Diva", false}, {"Transgender", false}, {"Bug", false},
            {"Mountain", false}, {"Seer", false}, {"Rainbow", false}, {"Philip", false}
         };
        personalBest = new SerializableDictionary<string, float>();
        medals = new SerializableDictionary<string, string>();
        unlockedLevels = new SerializableDictionary<string, bool>();
        levelDiamonds = new SerializableDictionary<string, float> {
            {"1", 14767}, {"2", 16883}, {"3", 11149}, {"4", 13266},
            {"5", 12783}, {"6", 22033}, {"7", 18015}, {"8", 22800},
            {"9", 5399},{"10", 28656},{"11",12900},{"12",7500},
            {"13",9000},{"14",16200},{"15",12800},{"16",17000},
            {"17",16000},{"18",11800},{"19",31000},{"20",33500},
            {"21",16000},{"22",11800},{"23",31000},{"24",33500},
            {"25",16000},{"26",11800},{"27",31000},{"28",33500},
            {"29",16000},{"30",11800}
        };
        levelGolds = new SerializableDictionary<string, float> {
            {"1", 15500}, {"2", 17500}, {"3", 12000}, {"4", 13900},
            {"5", 13600}, {"6", 23200}, {"7", 18900}, {"8", 28000},
            {"9", 19700},{"10", 34400},{"11",12900},{"12",7500},
            {"13",9000},{"14",16200},{"15",12800},{"16",17000},
            {"17",16000},{"18",11800},{"19",31000},{"20",33500},
            {"21",16000},{"22",11800},{"23",31000},{"24",33500},
            {"25",16000},{"26",11800},{"27",31000},{"28",33500},
            {"29",16000},{"30",11800}
        };
        levelSilvers = new SerializableDictionary<string, float> {
            {"1", 18500}, {"2", 23000}, {"3", 15900}, {"4", 17700},
            {"5", 18000}, {"6", 27500}, {"7", 24200}, {"8", 39300},
            {"9", 26800}, {"10", 40000},{"11",14500},{"12",9500},
            {"13",13500},{"14",19750},{"15",14000},{"16",20500},
            {"17",19600},{"18",15000},{"19",45000},{"20",42000},
            {"21",16000},{"22",11800},{"23",31000},{"24",33500},
            {"25",16000},{"26",11800},{"27",31000},{"28",33500},
            {"29",16000},{"30",11800}
        };
        levelBronzes = new SerializableDictionary<string, float> {
            {"1", 24000}, {"2", 29600}, {"3", 22300}, {"4",27000},
            {"5", 27700}, {"6", 31500}, {"7", 30500}, {"8", 52000},
            {"9", 35600},{"10", 48000},{"11",16800},{"12",11500},
            {"13",20000},{"14",23500},{"15",16950},{"16",25000},
            {"17",22200},{"18",20000},{"19",60000},{"20",50000},
            {"21",16000},{"22",11800},{"23",31000},{"24",33500},
            {"25",16000},{"26",11800},{"27",31000},{"28",33500},
            {"29",16000},{"30",11800}
        };
    }
}

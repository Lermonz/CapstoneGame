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

    //RESOLUTION DATA
    public Vector2 windowedResolution;
    public bool fullscreen;

    //OTHER PREFERENCES
    public bool noCheckpoints;

    //GLOBAL LEVEL DATA
    public SerializableDictionary<string, float> levelDevTimes;
    public SerializableDictionary<string, float> levelDiamonds;
    public SerializableDictionary<string, float> levelGolds;
    public SerializableDictionary<string, float> levelSilvers;
    public SerializableDictionary<string, float> levelBronzes;
    public SerializableDictionary<string, bool> unlockedLevels;
    // default initialized values of the constructor
    public GameData()
    {
        rebinds = string.Empty;
        fullscreen = true;
        noCheckpoints = false;
        selectedCostume = 0;
        windowedResolution = new Vector2(1600, 900);
        musicVolume = 60;
        soundVolume = 60;
        costumes = new SerializableDictionary<string, bool>
        {
            {"Vanilla", true}, {"Green", false}, {"Invert", false}, {"Gameboy", false},
            {"Pilot", false}, {"Diva", false}, {"Transgender", false}, {"Bug", false},
            {"Mountain", false}, {"Seer", false}, {"Rainbow", false}, {"Philip", false}
         };
        personalBest = new SerializableDictionary<string, float>();
        medals = new SerializableDictionary<string, string>();
        unlockedLevels = new SerializableDictionary<string, bool>();
        levelDevTimes = new SerializableDictionary<string, float> {
            {"1", 10316}, {"2", 11841}, {"3", 8582}, {"4", 9550},
            {"5", 9901}, {"6", 15366}, {"7", 13339}, {"8", 22800},
            {"9", 6081},{"10", 25750},{"11",11799},{"16",10050},
            {"12",11514},{"13",16144},{"14",17565},{"15",21315},
            {"17",15901},{"18",21535},{"19",26602},{"20",43647},
            {"21",22436},{"22",22882},{"23",22487},{"24",34131},
            {"25",29676},{"26",24735},{"27",19583},{"28",20699},
            {"29",29225},{"30",13082}
        };
        levelDiamonds = new SerializableDictionary<string, float> {
            {"1", 12300}, {"2", 15000}, {"3", 10300}, {"4", 12100},
            {"5", 11700}, {"6", 19500}, {"7", 18200}, {"8", 24000},
            {"9", 13600},{"10", 30000},{"11",14600},{"16",13500},
            {"12",12850},{"13",19300},{"14",23900},{"15",26000},
            {"17",19300},{"18",24500},{"19",34000},{"20",50000},
            {"21",24100},{"22",25500},{"23",24200},{"24",37300},
            {"25",32500},{"26",26800},{"27",24400},{"28",23800},
            {"29",31800},{"30",25000}
        };
        levelGolds = new SerializableDictionary<string, float> {
            {"1", 14300}, {"2", 17800}, {"3", 12700}, {"4", 13900},
            {"5", 14100}, {"6", 22400}, {"7", 20900}, {"8", 28000},
            {"9", 19700},{"10", 34500},{"11",18100},{"16",16800},
            {"12",14600},{"13",23500},{"14",28200},{"15",29000},
            {"17",21700},{"18",28000},{"19",41200},{"20",65000},
            {"21",26700},{"22",30400},{"23",29200},{"24",39400},
            {"25",37500},{"26",30000},{"27",27600},{"28",28000},
            {"29",34100},{"30",35000}
        };
        levelSilvers = new SerializableDictionary<string, float> {
            {"1", 18500}, {"2", 23200}, {"3", 16200}, {"4", 19000},
            {"5", 18500}, {"6", 27200}, {"7", 26400}, {"8", 39300},
            {"9", 26800}, {"10", 40000},{"11",24000},{"16",21200},
            {"12",18300},{"13",30700},{"14",34000},{"15",34500},
            {"17",28000},{"18",40000},{"19",49500},{"20",90000},
            {"21",31600},{"22",38400},{"23",37600},{"24",52000},
            {"25",45200},{"26",35600},{"27",34400},{"28",38200},
            {"29",39200},{"30",45000}
        };
        levelBronzes = new SerializableDictionary<string, float> {
            {"1", 24000}, {"2", 31700}, {"3", 24300}, {"4",28600},
            {"5", 27700}, {"6", 34400}, {"7", 34000}, {"8", 52000},
            {"9", 35600},{"10", 48000},{"11",30500},{"16",29000},
            {"12",26000},{"13",40000},{"14",42000},{"15",40500},
            {"17",35500},{"18",55000},{"19",65000},{"20",130000},
            {"21",38500},{"22",48000},{"23",47400},{"24",70000},
            {"25",54000},{"26",42800},{"27",45500},{"28",50500},
            {"29",48700},{"30",55000}
        };
    }
}

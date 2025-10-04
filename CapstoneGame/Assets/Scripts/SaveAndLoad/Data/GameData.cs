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
        fullscreen = true;
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
        levelDiamonds = new SerializableDictionary<string, float> {
            {"1", 12799}, {"2", 16583}, {"3", 8748}, {"4", 10533},
            {"5", 11250}, {"6", 17948}, {"7", 18015}, {"8", 22800},
            {"9", 5399},{"10", 28656},{"11",14900},{"12",11067},
            {"13",12267},{"14",18900},{"15",22366},{"16",25499},
            {"17",18383},{"18",21535},{"19",39799},{"20",46265},
            {"21",23316},{"22",22882},{"23",24184},{"24",34131},
            {"25",31100},{"26",24735},{"27",19583},{"28",20699},
            {"29",30886},{"30",13082}
        };
        levelGolds = new SerializableDictionary<string, float> {
            {"1", 16000}, {"2", 17800}, {"3", 12700}, {"4", 14700},
            {"5", 14100}, {"6", 21400}, {"7", 20900}, {"8", 28000},
            {"9", 19700},{"10", 34400},{"11",19800},{"12",15600},
            {"13",14600},{"14",22500},{"15",28100},{"16",33200},
            {"17",22700},{"18",27600},{"19",46600},{"20",60000},
            {"21",26200},{"22",29700},{"23",29200},{"24",38500},
            {"25",37600},{"26",30000},{"27",25700},{"28",28000},
            {"29",33600},{"30",35000}
        };
        levelSilvers = new SerializableDictionary<string, float> {
            {"1", 20000}, {"2", 23200}, {"3", 16200}, {"4", 19000},
            {"5", 18500}, {"6", 27200}, {"7", 26400}, {"8", 39300},
            {"9", 26800}, {"10", 40000},{"11",24400},{"12",20700},
            {"13",18300},{"14",28200},{"15",34600},{"16",39400},
            {"17",29800},{"18",38000},{"19",63000},{"20",87000},
            {"21",31600},{"22",38400},{"23",37600},{"24",52000},
            {"25",45200},{"26",35600},{"27",33400},{"28",38200},
            {"29",39200},{"30",45000}
        };
        levelBronzes = new SerializableDictionary<string, float> {
            {"1", 28700}, {"2", 31700}, {"3", 24300}, {"4",28600},
            {"5", 27700}, {"6", 34400}, {"7", 34000}, {"8", 52000},
            {"9", 35600},{"10", 48000},{"11",31000},{"12",28600},
            {"13",26000},{"14",36200},{"15",44000},{"16",50200},
            {"17",38300},{"18",50000},{"19",90000},{"20",110000},
            {"21",38500},{"22",48000},{"23",47000},{"24",70000},
            {"25",54000},{"26",42800},{"27",45500},{"28",50500},
            {"29",48700},{"30",55000}
        };
    }
}

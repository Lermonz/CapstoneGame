using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public SerializableDictionary<string, float> personalBest;
    // default initialized values of the constructor
    public GameData() {
        personalBest = new SerializableDictionary<string, float>();
    }
}

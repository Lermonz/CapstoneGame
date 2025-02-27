using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float personalBest;
    // default initialized values of the constructor
    public GameData() {
        this.personalBest = 99*60000+59*1000+999;
    }
}

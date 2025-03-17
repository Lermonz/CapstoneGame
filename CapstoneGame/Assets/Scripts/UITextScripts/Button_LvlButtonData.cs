using System;
using UnityEngine;
[Serializable]
public class Button_LvlButtonData 
{
    public LevelUnlockType _unlockType;
    public string _levelID = "";
    public int _world = 0;
    [Header("Requirements")]
    public int _goldTotalReq = 0;
    public string _requiredLevelID = "";
}

using UnityEngine;
using TMPro;
using System;

public class CheckTimeFromLevel : MonoBehaviour, IDataPersistence
{
    //These two set in unity editor per instance
    public TimeType _timeType;
    public string _levelID;
    TMP_Text _text;
    LevelManager _levelManager;
    void Awake() {
        _text = GetComponent<TMP_Text>();
        try {
            _levelManager = GameObject.Find("LevelBehaviour").GetComponent<LevelManager>();
            _levelID = _levelManager._levelID;
        }
        catch {
            //Debug.Log("levelmanager not found");
        }
    }
    public void LoadData(GameData data) {
        switch (_timeType) {
            case TimeType.PB:
                if (!data.personalBest.ContainsKey(_levelID)) {
                    _text.text = "none";
                }
                else {
                    _text.text = DataToStringText(data.personalBest[_levelID]);
                }
                break;
            case TimeType.Diamond:
                _text.text = DataToStringText(data.levelDiamonds[_levelID]);
                break;
            case TimeType.Gold:
                _text.text = DataToStringText(data.levelGolds[_levelID]);
                break;
            case TimeType.Silver:
                _text.text = DataToStringText(data.levelSilvers[_levelID]);
                break;
            case TimeType.Bronze:
                _text.text = DataToStringText(data.levelBronzes[_levelID]);
                break;
            default:
                break;
        }
    }
    string DataToStringText(float time) {
        Vector3 time3 = GameBehaviour.Instance.ConvertTimerToVector3(time);
        return string.Format("{0:00}:{1:00}.{2:000}",time3.x,time3.y,time3.z);
    }
    public void SaveData(GameData data) {
        return;
    }
}

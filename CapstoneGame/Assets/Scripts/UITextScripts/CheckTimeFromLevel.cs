using UnityEngine;
using TMPro;
using System;

public class CheckTimeFromLevel : MonoBehaviour, IDataPersistence
{
    //These two set in unity editor per instance
    public TimeType _timeType;
    [SerializeField] string _levelID;
    [SerializeField] TMP_Text _text;
    [SerializeField] LevelButtonManager _buttonManager;
    void OnEnable()
    {
        if (_text == null)
        {
            _text = this.GetComponent<TMP_Text>();
        }
        if (_levelID == "levelmanager")
        {
            _levelID = LevelManager.Instance._levelID;
        }
        else if (_levelID == "levelbutton" && _buttonManager != null)
        {
            _levelID = _buttonManager._buttonData._levelID;
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

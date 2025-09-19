using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class LevelButtonManager : MonoBehaviour, IDataPersistence
{
    public Button_LvlButtonData _buttonData;
    [SerializeField]
    private Button _levelButton;
    [SerializeField] CheckNewMedal _medalImage;
    public int _totalCount;
    bool _shouldBeUnlocked = false;
    private void Awake()
    {
        _levelButton = this.GetComponent<Button>();
    }
    public void LoadData(GameData data){
        CheckUnlock(data);
    }
    public void SaveData(GameData data) {
        CheckUnlock(data);
    }
    private void CheckUnlock(GameData data) {
        _totalCount = 0;
        switch(_buttonData._unlockType) {
        case LevelUnlockType.OnWorld:
            data.unlockedLevels[_buttonData._levelID] = true;
            break;
        case LevelUnlockType.TotalGolds:
            TotalGolds(data);
            data.unlockedLevels[
                _buttonData._levelID] = 
                _totalCount >= 
                _buttonData._goldTotalReq;
            break;
        case LevelUnlockType.GoldsInWorld:
            TotalGoldsInWorld(data);
            data.unlockedLevels[_buttonData._levelID] = _totalCount >= _buttonData._goldTotalReq;
            break;
        case LevelUnlockType.SpecificGold:
            data.unlockedLevels[_buttonData._levelID] = data.medals[_buttonData._requiredLevelID] == "gold";
            break;
        case LevelUnlockType.PrevClear:
            data.unlockedLevels[_buttonData._levelID] = data.personalBest.ContainsKey(_buttonData._requiredLevelID);
            break;
        default:
            break;
        }
        MakeButtonInteractable(data);
    }
    private void MakeButtonInteractable(GameData data) {
        if (_medalImage != null)
        { 
            _medalImage.MedalLoad(data.medals.ContainsKey(_buttonData._levelID) ? data.medals[_buttonData._levelID] : "none");
        }
        _levelButton.interactable = data.unlockedLevels[_buttonData._levelID];
        _shouldBeUnlocked = _levelButton.interactable;
        //Debug.Log("should be unlocked?: "+_shouldBeUnlocked+",  "+_buttonData._levelID);
    }
    private void TotalGolds(GameData data) {
        _totalCount = 0;
        foreach(KeyValuePair<string, string> item in data.medals) {
            if(item.Value == "gold") {
                _totalCount++;
            }
        }
    }
    private void TotalGoldsInWorld(GameData data) {
        _totalCount = 0;
        for(int i = 0+(_buttonData._world-1)*10; i < 10+(_buttonData._world-1)*10; i++) {
            if(data.medals.ElementAt(i).Value == "gold") {
                _totalCount++;
            }
        }
    }
    // void Update() {
    //     if(_shouldBeUnlocked && !GameBehaviour.Instance._isGame) {
    //         _levelButton.interactable = ScrollRectSnap.Instance._currentWorld == _buttonData._world-1;
    //     }
    // }
}

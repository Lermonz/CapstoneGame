using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class LevelButtonManager : MonoBehaviour, IDataPersistence
{
    public Button_LvlButtonData _buttonData;
    [SerializeField]
    private Button _levelButton;
    public int _totalGolds;
    public bool IsSelected {get; private set;} = false;
    private void Awake()
    {
        _levelButton = this.GetComponent<Button>();
    }
    public void LoadData(GameData data)
    {
        _totalGolds = 0;
        switch(_buttonData._unlockType) {
        case LevelUnlockType.OnWorld:
            _levelButton.interactable = true;
            break;
        case LevelUnlockType.TotalGolds:
            foreach(KeyValuePair<string, string> item in data.medals) {
               if(item.Value == "gold") {
                  _totalGolds++;
               }
            }
            _levelButton.interactable = _totalGolds >= _buttonData._goldTotalReq;
            break;
        case LevelUnlockType.GoldsInWorld:
            for(int i = 0+(_buttonData._world-1)*10; i < 10+(_buttonData._world-1)*10; i++) {
               if(data.medals.ElementAt(i).Value == "gold") {
                  _totalGolds++;
               }
            }
            _levelButton.interactable = _totalGolds >= _buttonData._goldTotalReq;
            break;
        case LevelUnlockType.SpecificGold:
            _levelButton.interactable = data.medals[_buttonData._requiredLevelID] == "gold";
            break;
        default:
            break;
      }
    }
    public void SaveData(GameData data) {
        return;
    }
}

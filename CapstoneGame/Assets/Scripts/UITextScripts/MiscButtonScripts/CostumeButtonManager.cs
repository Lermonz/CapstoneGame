using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class CostumeButtonManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] LevelUnlockType _unlockType;
    [SerializeField] ButtonSelectionHandler _selectionHandler;
    [SerializeField] int _world;
    [SerializeField] string _costumeID;
    [SerializeField] string _unlockedDescription;
    [SerializeField] string _lockedDescription;
    [SerializeField] TextMeshProUGUI _descriptionObject;
    Button _button;
    bool _isUnlocked;
    void Awake()
    {
        _button = this.GetComponent<Button>();
    }
    void Update()
    {
        if (_selectionHandler.IsSelected)
        {
            if (_isUnlocked)
            {
                SetDescription(_unlockedDescription);
            }
            else {
                SetDescription(_lockedDescription);
            }
        }
    }
    public void SaveData(GameData data) { return; }
    public void LoadData(GameData data)
    {
        switch (_unlockType)
        {
            case LevelUnlockType.CostumeSecret:
                _isUnlocked = data.costumes[_costumeID];
                break;
            case LevelUnlockType.GoldsInWorld:
                _isUnlocked = TotalGoldsInWorld(data, _world) >= 10;
                break;
            default:
                _isUnlocked = true;
                break;
        }
        MakeButtonInteractable();
    }
    void MakeButtonInteractable()
    {
        _button.interactable = _isUnlocked;
    }
    void SetDescription(string descriptionText)
    {
        _descriptionObject.text = descriptionText;
    }
    private int TotalGoldsInWorld(GameData data, int world)
    {
        int totalGoldCount = 0;
        for (int i = 0 + (world - 1) * 10; i < 10 + (world - 1) * 10; i++)
        {
            if (data.medals.ElementAt(i).Value == "gold")
            {
                totalGoldCount++;
            }
        }
        return totalGoldCount;
    }
    // only use CostumeSecret or GoldsInWorld from this enum for the switch:case
    // if CostumeSecret, check GameData data for if the specified id for that CostumeSecret has been collected
    // if GoldsInWorld, copy paste the check from LevelButtonManager
}

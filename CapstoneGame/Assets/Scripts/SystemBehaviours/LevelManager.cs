using System.Collections;
using System.Data.Common;
using UnityEngine;

[DisallowMultipleComponent]
public class LevelManager : MonoBehaviour, IDataPersistence
{
    public static LevelManager Instance;

    public string _levelID;
    public float _personalBest = 99*60000+59*1000+999;
    [Header("Level Times (in ms)")]
    [Tooltip("minute : second : millisecond")]
    public Vector3 _goldTime;
    public Vector3 _silverTime;
    public Vector3 _bronzeTime;
    [Header("Target Stuff")]
    public int _targetReq;
    int _targetsDestroyed;
    public int TargetsDestroyed { get => _targetsDestroyed; set {_targetsDestroyed = value;}}
    public bool _canExit;
    public bool _stopTimer;
    public bool _countdownDone = false;
    bool _hasPlayedSoundEffect = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    void Start() {
        Time.timeScale = 1f;
        _stopTimer = true;
        InputManager.Instance.DisablePlayerInput();
        StartCoroutine(Countdown());
    }
    public void HitTarget() {
        TargetsDestroyed++;
    }
    void Update() {
        // if you've destroyed enough targets for this level, you can touch the goal
        if (_targetsDestroyed >= _targetReq)
        {
            _canExit = true;
            if (!_hasPlayedSoundEffect)
            {
                AkSoundEngine.PostEvent("AllCrystals", gameObject);
                _hasPlayedSoundEffect = true;
            }
        }
    }
    IEnumerator Countdown() {
        yield return new WaitForSeconds(2f);
        _stopTimer = false;
        _countdownDone = true;
        InputManager.Instance.EnablePlayerInput();
        InputManager.Instance._freezeVelocity = false;
    }
    public void FreezePlayerAndTimer(bool doIt = true) {
        _stopTimer = doIt;
        InputManager.Instance._freezeVelocity = doIt;
    }
    private void SetNewPB(float thisRunTime) {
        this._personalBest = thisRunTime;
    }
    public void CheckPB() {
        float finalTime = GameBehaviour.Instance.ConvertTimerToFloat(
            new Vector3(Timer.Instance.m,
                    Timer.Instance.s,
                    Timer.Instance.ms));
        if (finalTime < this._personalBest) {
            SetNewPB(finalTime);
        }
    }
    public void LoadData(GameData data) {
        Debug.Log("does load data in level manager get called??");
        this._bronzeTime = GameBehaviour.Instance.ConvertTimerToVector3(data.levelBronzes[_levelID]);
        this._silverTime = GameBehaviour.Instance.ConvertTimerToVector3(data.levelSilvers[_levelID]);
        this._goldTime = GameBehaviour.Instance.ConvertTimerToVector3(data.levelGolds[_levelID]);
        if(data.personalBest.ContainsKey(_levelID)){
            data.personalBest.TryGetValue(_levelID, out this._personalBest);
        }
        else{
            this._personalBest = 5999999;
        }
        //this._personalBest = data.personalBestOLD;
    }
    public void SaveData(GameData data)
    {
        Debug.Log("SAVE THE FUCKIGN DATAAAAAAAAAAAAAAAAAAAAAAAAA");
        if (data.personalBest.ContainsKey(_levelID))
        {
            data.personalBest.Remove(_levelID);
        }
        data.personalBest.Add(_levelID, this._personalBest);
        if (data.personalBest[_levelID] <= data.levelGolds[_levelID])
        {
            UpdateMedals(data, "gold");
        }
        else if (data.personalBest[_levelID] <= data.levelSilvers[_levelID])
        {
            UpdateMedals(data, "silver");
        }
        else if (data.personalBest[_levelID] <= data.levelBronzes[_levelID])
        {
            UpdateMedals(data, "bronze");
        }
        else
        {
            UpdateMedals(data, "none");
        }
        
        //data.personalBestOLD = this._personalBest;
    }
    public void UpdateMedals(GameData data, string medal) {
        Debug.Log("levelmanager.updatemedals");
        if (data.medals.ContainsKey(_levelID))
        {
            Debug.Log("Show medal correct please");
            GameObject.Find("RewardStar").GetComponent<CheckNewMedal>().MedalLoad(data.medals[_levelID], medal);
            data.medals.Remove(_levelID);
        }
        data.medals.Add(_levelID, medal);
    }
}
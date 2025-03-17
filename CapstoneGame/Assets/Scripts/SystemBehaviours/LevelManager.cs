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
        if(_targetsDestroyed >= _targetReq) {
            _canExit = true;
        }
    }
    IEnumerator Countdown() {
        yield return new WaitForSeconds(2f);
        _stopTimer = false;
        _countdownDone = true;
        InputManager.Instance.EnablePlayerInput();
        InputManager.Instance._freezeVelocity = false;
    }
    public void FreezePlayerAndTimer() {
        _stopTimer = true;
        InputManager.Instance._freezeVelocity = true;
    }
    public Vector3 ConvertTimerToVector3(float time) {
        float m = Mathf.FloorToInt(time/60000);
        float s = Mathf.FloorToInt((time-m*60000)/1000);
        float ms = Mathf.FloorToInt(time - m*60000 - s*1000);
        return new Vector3(m,s,ms);
    }
    public float ConvertTimerToFloat(Vector3 time) {
        return (time.x*60+time.y)*1000+time.z;
    }
    private void SetNewPB(float thisRunTime) {
        this._personalBest = thisRunTime;
    }
    public void CheckPB() {
        float finalTime = ConvertTimerToFloat(
            new Vector3(Timer.Instance.m,
                    Timer.Instance.s,
                    Timer.Instance.ms));
        if (finalTime < this._personalBest) {
            SetNewPB(finalTime);
        }
    }
    public void LoadData(GameData data) {
        this._bronzeTime = ConvertTimerToVector3(data.levelBronzes[_levelID]);
        this._silverTime = ConvertTimerToVector3(data.levelSilvers[_levelID]);
        this._goldTime = ConvertTimerToVector3(data.levelGolds[_levelID]);
        if(data.personalBest.ContainsKey(_levelID)){
            data.personalBest.TryGetValue(_levelID, out this._personalBest);
        }
        else{
            this._personalBest = 5999999;
        }
        //this._personalBest = data.personalBestOLD;
    }
    public void SaveData(GameData data) {
        if(data.personalBest.ContainsKey(_levelID)){
            data.personalBest.Remove(_levelID);
        }
        data.personalBest.Add(_levelID, this._personalBest);
        if(this._personalBest <= data.levelGolds[_levelID]) {
            UpdateMedals(data, "gold");
        }
        else if(this._personalBest <= data.levelSilvers[_levelID]) {
            UpdateMedals(data, "silver");
        }
        else if(this._personalBest <= data.levelBronzes[_levelID]) {
            UpdateMedals(data, "bronze");
        }
        //data.personalBestOLD = this._personalBest;
    }
    public void UpdateMedals(GameData data, string medal) {
        if(data.medals.ContainsKey(_levelID)){
            data.medals.Remove(_levelID);
        }
        data.medals.Add(_levelID, medal);
    }
}
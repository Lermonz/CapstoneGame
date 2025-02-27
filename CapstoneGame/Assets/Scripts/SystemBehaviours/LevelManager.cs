using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class LevelManager : MonoBehaviour, IDataPersistence
{
    public static LevelManager Instance;

    public int _currentLevel;
    public float _personalBest;
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
    public void LoadData(GameData data) {
        this._personalBest = data.personalBest;
    }
    public void SaveData(ref GameData data) {
        data.personalBest = this._personalBest;
    }
}

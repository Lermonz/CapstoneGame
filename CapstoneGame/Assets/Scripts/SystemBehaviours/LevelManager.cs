using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int _currentLevel;
    [Header("Level Times (in ms)")]
    [Tooltip("m*6000 : s*100 : ms*1")]
    public int _goldTime;
    public int _silverTime;
    public int _bronzeTime;
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
}

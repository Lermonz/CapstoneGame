using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int _currentLevel;
    public int _targetReq;
    public int _targetsDestroyed;
    public int TargetsDestroyed { get => _targetsDestroyed; set {_targetsDestroyed = value;}}
    public bool _canExit;
    public bool _stopTimer;
    public bool _countdownDone = false;

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    void Start() {
        _stopTimer = true;
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
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(2f);
        _stopTimer = false;
        _countdownDone = true;
        Time.timeScale = 1f;

    }
}

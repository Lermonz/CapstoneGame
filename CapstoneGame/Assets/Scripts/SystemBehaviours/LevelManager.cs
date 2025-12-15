using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class LevelManager : MonoBehaviour, IDataPersistence
{
    public static LevelManager Instance;

    public string _levelID;
    public float _personalBest = 99*60000+59*1000+999;
    [Header("Level Times (in ms)")]
    [Tooltip("minute : second : millisecond")]
    public Vector3 _developerTime;
    public Vector3 _diamondTime;
    public Vector3 _goldTime;
    public Vector3 _silverTime;
    public Vector3 _bronzeTime;
    [Header("Target Stuff")]
    public int _targetReq;
    int _targetsDestroyed;
    public int TargetsDestroyed { get => _targetsDestroyed; set {_targetsDestroyed = value;}}
    public bool _canExit;
    public bool _stopTimer;
    public Action _countdownStart;
    public bool _countdownDone = false;
    bool _hasPlayedSoundEffect = false;
    bool _canReset = false;
    //Vector2 _checkpoint;
    public Vector3 Checkpoint { get; private set; }
    public string _medalInLevel { get; private set; }
    [Header("Object References")]
    [SerializeField] CheckNewMedal[] _medalSprites;
    [SerializeField] ScaleScreenWipeMask _screenWipe;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _countdown;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    void Start()
    {
        _player.SetActive(false);
        if(_screenWipe != null)
        {
            _screenWipe._StartSceneAnims += OnFinishScreenWipe;
        }
        else
        {
            OnFinishScreenWipe();
            Debug.Log("else triggered");
        }
        Time.timeScale = 1f;
        _stopTimer = true;
        InputManager.Instance.DisablePlayerInput();
        InputManager.Instance.QuickResetPressEvent += OnQuickResetPress;
    }
    void OnFinishScreenWipe()
    {
        _player.SetActive(true);
        Debug.Log(Time.frameCount+" OnFinishScreenWipe was invoked");
        _countdownStart?.Invoke();
    }
    void OnDestroy()
    {
        InputManager.Instance.QuickResetPressEvent -= OnQuickResetPress;
        _screenWipe._StartSceneAnims -= OnFinishScreenWipe;
    }
    public void HitTarget(Vector2 position, Vector2 offset)
    {
        TargetsDestroyed++;
        SetRespawnPoint(position, offset.x, offset.y);
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
    public void SetRespawnPoint(Vector2 position, float offsetX = 0, float offsetY = 0)
    {
        Vector2 offset = new Vector2(offsetX, offsetY);
        Checkpoint = position + offset;
    }
    void OnQuickResetPress()
    {
        if(!_canReset) { return; }
        _canReset = false;
        this.GetComponent<SelectLevel>().Reload(false);
    }
    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1f);
        InputManager.Instance.EnablePlayerInput();
        InputManager.Instance._freezeVelocity = false;
        yield return new WaitForSeconds(0.5f);
        _stopTimer = false;
        _canReset = true;
    }
    public void FreezePlayerAndTimer(bool doIt = true) {
        Debug.Log("PlayerFroze");
        _stopTimer = doIt;
        InputManager.Instance._freezeVelocity = doIt;
    }
    public void StopTimer(bool stopped = true)
    {
        _stopTimer = stopped;
    }
    public void CanReset()
    {
        _canReset = true;
    }
    private void SetNewPB(float thisRunTime)
    {
        this._personalBest = thisRunTime;
    }
    public bool CheckPB()
    {
        float finalTime = GameBehaviour.Instance.ConvertTimerToFloat(
            new Vector3(Timer.Instance.m,
                    Timer.Instance.s,
                    Timer.Instance.ms));
        if (finalTime < this._personalBest)
        {
            SetNewPB(finalTime);
            return true;
        }
        return false;
    }
    public void LoadData(GameData data) {
        this._bronzeTime = GameBehaviour.Instance.ConvertTimerToVector3(data.levelBronzes[_levelID]);
        this._silverTime = GameBehaviour.Instance.ConvertTimerToVector3(data.levelSilvers[_levelID]);
        this._goldTime = GameBehaviour.Instance.ConvertTimerToVector3(data.levelGolds[_levelID]);
        this._diamondTime = GameBehaviour.Instance.ConvertTimerToVector3(data.levelDiamonds[_levelID]);
        this._developerTime = GameBehaviour.Instance.ConvertTimerToVector3(data.levelDevTimes[_levelID]);
        if (data.personalBest.ContainsKey(_levelID))
        {
            data.personalBest.TryGetValue(_levelID, out this._personalBest);
            if (data.medals.ContainsKey(_levelID)) { _medalInLevel = data.medals[_levelID]; }
        }
        else
        {
            this._personalBest = 5999999;
        }
        //this._personalBest = data.personalBestOLD;
    }
    public void SaveData(GameData data)
    {
        if (_canExit)
        {
            if (data.personalBest.ContainsKey(_levelID))
            {
                data.personalBest.Remove(_levelID);
            }
            data.personalBest.Add(_levelID, this._personalBest);
            if (data.personalBest[_levelID] <= data.levelDevTimes[_levelID])
            {
                UpdateMedals(data, "developer");
            }
            else if (data.personalBest[_levelID] <= data.levelDiamonds[_levelID])
            {
                UpdateMedals(data, "diamond");
            }
            else if (data.personalBest[_levelID] <= data.levelGolds[_levelID])
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
        }
        
        //data.personalBestOLD = this._personalBest;
    }
    public void UpdateMedals(GameData data, string medal) {
        _medalInLevel = medal;
        if (GameObject.Find("RewardStar") != null)
        {
            if (data.medals.ContainsKey(_levelID))
            {
                foreach (CheckNewMedal MedalSprite in _medalSprites)
                {
                    MedalSprite.MedalLoad(data.medals[_levelID], medal);
                }
                data.medals.Remove(_levelID);
            }
            else
            {
                foreach (CheckNewMedal MedalSprite in _medalSprites)
                {
                    MedalSprite.MedalLoad("none", medal);
                }
            }
            data.medals.Add(_levelID, medal);
        }
    }
}
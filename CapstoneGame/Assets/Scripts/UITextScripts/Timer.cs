using UnityEngine;

[DisallowMultipleComponent]
public class Timer : MonoBehaviour
{
    public static Timer Instance;
    public float _totalFrames { get; private set; }
    public int m { get; private set; }
    public int s { get; private set; }
    public int ms { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    void Start()
    {
        _totalFrames = 0;
    }
    void Update()
    {
        if(!LevelManager.Instance._stopTimer) {
            _totalFrames+=Time.deltaTime;
            m = Mathf.FloorToInt(_totalFrames/60);
            s = Mathf.FloorToInt(_totalFrames%60);
            ms = Mathf.FloorToInt(_totalFrames%1*1000);
        }
    }
}

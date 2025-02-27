using UnityEngine;

[DisallowMultipleComponent]
public class Timer : MonoBehaviour
{
    public static Timer Instance;
    public float _totalFrames = 0;
    public int m = 0;
    public int s = 0;
    public int ms = 0;
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
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

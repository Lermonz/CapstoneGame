using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
[DisallowMultipleComponent]
public class Timer : MonoBehaviour
{
    float _totalFrames = 0;
    public int FinalTime;
    int ms = 0;
    int s = 0;
    int m = 0;
    TMP_Text _timer;
    void Start() {
        _timer = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if(!LevelManager.Instance._stopTimer) {
            _totalFrames+=Time.deltaTime;
            m = Mathf.FloorToInt(_totalFrames/60);
            s = Mathf.FloorToInt(_totalFrames%60);
            ms = Mathf.FloorToInt(_totalFrames%1*100);
            _timer.text = string.Format("{0:00}:{1:00}:{2:00}", m, s, ms);
        }
        else {
            FinalTime = (m*60+s)*100+ms;
        }
    }
}

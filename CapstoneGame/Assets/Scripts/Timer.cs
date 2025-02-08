using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class Timer : MonoBehaviour
{
    float _totalFrames = 0;
    public static int ms = 0;
    public static int s = 0;
    public static int m = 0;
    public TMP_Text _timer;
    void Update()
    {
        if(!GameBehaviour.Instance._stopTimer) {
            _totalFrames+=Time.deltaTime;
            m = Mathf.FloorToInt(_totalFrames/60);
            s = Mathf.FloorToInt(_totalFrames%60);
            ms = Mathf.FloorToInt(_totalFrames%1*100);
            _timer.text = string.Format("{0:00}:{1:00}:{2:00}", m, s, ms);
        }
    }
}

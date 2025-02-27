using UnityEngine;
using TMPro;

public class TimerText : MonoBehaviour
{
    [SerializeField] TMP_Text _minutesText;
    [SerializeField] TMP_Text _secondsText;
    [SerializeField] TMP_Text _millisecondsText;
    void Update()
    {
        //_timer.text = string.Format("{0:00}:{1:00}:{2:00}", m, s, ms);
        _minutesText.text = string.Format("{0:00}",Timer.Instance.m);
        _secondsText.text = string.Format("{0:00}",Timer.Instance.s);
        _millisecondsText.text = string.Format("{0:000}",Timer.Instance.ms);
    }
}

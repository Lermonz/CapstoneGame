using UnityEngine;
using TMPro;

public class MusicVolume : MonoBehaviour
{
    [SerializeField] AK.Wwise.RTPC _rtpc;
    [SerializeField] TMP_Text _text;
    int _volume;
    void Start()
    {
        _volume = 70;
    }
    public void IncreaseVolume() {
        if (_volume < 100)
            _volume += 10;
        SetRTPC();
    }
    public void DecreaseVolume() {
        if (_volume > 0)
            _volume -= 10;
        SetRTPC();
    }
    void SetRTPC() {
        _rtpc.SetGlobalValue(_volume);
        _text.text = ""+_volume;
    }
}

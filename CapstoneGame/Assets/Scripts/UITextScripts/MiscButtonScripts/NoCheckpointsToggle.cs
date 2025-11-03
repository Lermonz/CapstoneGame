using UnityEngine;
using TMPro;

public class NoCheckpointsToggle : MonoBehaviour
{
    bool _noCheckpoints;
    [SerializeField] TMP_Text _text;
    void OnEnable()
    {
        _noCheckpoints = GameBehaviour.Instance.NoCheckpoints;
        SetText(_noCheckpoints);
    }
    public void OnToggle()
    {
        _noCheckpoints = !_noCheckpoints;
        SetText(_noCheckpoints);
        PlayAudio(_noCheckpoints);
        GameBehaviour.Instance.SetNoCheckpoints(_noCheckpoints);
    }
    void SetText(bool isOff)
    {
        _text.text = isOff ? "OFF" : "ON";
    }
    void PlayAudio(bool isOff)
    {
        MainMenuManager.Instance.FindAudioPlayerForButtons(isOff ? 10 : 9);
    }
}

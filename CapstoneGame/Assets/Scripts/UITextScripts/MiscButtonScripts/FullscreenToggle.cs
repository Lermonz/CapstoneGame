using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FullscreenToggle : MonoBehaviour, IDataPersistence
{
    public bool IsFullscreen { get; private set; }
    Vector2 _windowedResolution = new Vector2(1600, 900);
    [SerializeField] TMP_Text _text;
    public void LoadData(GameData data)
    {
        IsFullscreen = data.fullscreen;
        if (data.windowedResolution.x < 480) { data.windowedResolution.x = 480; }
        if (data.windowedResolution.y < 270) { data.windowedResolution.y = 270; }
        _windowedResolution = data.windowedResolution;
    }
    public void SaveData(GameData data)
    {
        data.fullscreen = IsFullscreen;
        if (_windowedResolution.x < 480) { _windowedResolution.x = 480; }
        if (_windowedResolution.y < 270) { _windowedResolution.y = 270; }
        data.windowedResolution = _windowedResolution;
    }
    void OnEnable()
    {
        DataPersistenceManager.Instance.LoadGame();
        _text.enabled = IsFullscreen;
    }
    public void OnToggle()
    {
        if (!IsFullscreen) { _windowedResolution = new Vector2(Screen.width, Screen.height); }
        IsFullscreen = !IsFullscreen;
        _text.enabled = IsFullscreen;
        PlayAudio(IsFullscreen);
        MainMenuManager.Instance.SetFullscreen(IsFullscreen, _windowedResolution);
    }
    void PlayAudio(bool _isFullscreen)
    {
        MainMenuManager.Instance.FindAudioPlayerForButtons(_isFullscreen ? 9 : 10);
    }
}

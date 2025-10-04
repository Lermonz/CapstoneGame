using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FullscreenToggle : MonoBehaviour, IDataPersistence
{
    public bool IsFullscreen { get; private set; }
    Vector2 _windowedResolution;
    [SerializeField] TMP_Text _text;
    public void LoadData(GameData data)
    {
        IsFullscreen = data.fullscreen;
        _windowedResolution = data.windowedResolution;
    }
    public void SaveData(GameData data)
    {
        data.fullscreen = IsFullscreen;
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
        MainMenuManager.Instance.SetFullscreen(IsFullscreen, _windowedResolution);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour, IDataPersistence
{
    public static MusicManager Instance;
    [SerializeField] AK.Wwise.RTPC _akMusicVolume;
    [SerializeField] AK.Wwise.RTPC _akSoundVolume;
    int _musicVolume;
    int _soundVolume;
    int trackToPlay;
    bool playingTrack1 = false;
    bool playingTrack2 = false;
    // bool playingTrack3 = false;
    // bool playingTrackMenu = false;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void SetRTPC(AK.Wwise.RTPC rtpc, int volume)
    {
        rtpc.SetGlobalValue(volume);
    }
    public void SetVolume(AudioMixType audioType, int volumeChange)
    {
        switch (audioType)
        {
            case AudioMixType.Music:
                _musicVolume += volumeChange;
                SetRTPC(_akMusicVolume, _musicVolume);
                break;
            case AudioMixType.Sound:
                _soundVolume += volumeChange;
                SetRTPC(_akSoundVolume, _soundVolume);
                break;
        }
    }
    public int GetVolume(AudioMixType audioType)
    {
        switch (audioType)
        {
            case AudioMixType.Music:
                return _musicVolume;
            case AudioMixType.Sound:
                return _soundVolume;
            default:
                return 70;
        }
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int sceneID = SceneManager.GetActiveScene().buildIndex;
        if (sceneID == 0)
        {
            trackToPlay = 0;
        }
        else if (sceneID <= 10)
        {
            trackToPlay = 1;
        }
        else if (sceneID <= 20) {
            trackToPlay = 2;
        }
        else {
            trackToPlay = 1;
        }
        Debug.Log(sceneID + " <- sceneID\n" + trackToPlay + " <- trackToPlay");
        if (trackToPlay == 0)
        {
            if (playingTrack1)
            {
                AkSoundEngine.PostEvent("StopWorld1", gameObject);
                playingTrack1 = false;
            }
            if (playingTrack2)
            {
                AkSoundEngine.PostEvent("StopWorld2", gameObject);
                playingTrack2 = false;
            }
            AkSoundEngine.PostEvent("PlayMenu", gameObject);
        }
        else if (trackToPlay == 1 && !playingTrack1)
        {
            //Debug.Log("PlayWorld1");
            AkSoundEngine.PostEvent("StopMenu", gameObject);
            AkSoundEngine.PostEvent("PlayWorld1", gameObject);
            playingTrack1 = true;
        }
        else if (trackToPlay == 2 && !playingTrack2)
        {
            //Debug.Log("PlayWorld1");
            AkSoundEngine.PostEvent("StopMenu", gameObject);
            AkSoundEngine.PostEvent("PlayWorld2", gameObject);
            playingTrack2 = true;
        }
    }
    public void SaveData(GameData data)
    {
        data.musicVolume = _musicVolume;
        data.soundVolume = _soundVolume;
    }
    public void LoadData(GameData data)
    {
        _akMusicVolume.SetGlobalValue(data.musicVolume);
        _akSoundVolume.SetGlobalValue(data.soundVolume);
        _musicVolume = data.musicVolume;
        _soundVolume = data.soundVolume;
    }
}

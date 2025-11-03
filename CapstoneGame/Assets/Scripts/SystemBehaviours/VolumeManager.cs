using UnityEngine;

public class VolumeManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] AK.Wwise.RTPC _akMusicVolume;
    [SerializeField] AK.Wwise.RTPC _akSoundVolume;
    int _musicVolume;
    int _soundVolume;
    void SetRTPC(AK.Wwise.RTPC rtpc, int volume)
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
        Debug.Log("Set volume to: " + volumeChange);
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

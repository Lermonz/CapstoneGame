using UnityEngine;
using TMPro;

public class MusicVolume : MonoBehaviour
{
    [SerializeField] AudioMixType _audioType;
    [SerializeField] TMP_Text _text;
    [SerializeField] VolumeManager _volumeManager;
    void Start()
    {
        if (_volumeManager == null)
        {
            _volumeManager = this.gameObject.AddComponent<VolumeManager>();
        }
        UpdateText();
    }
    public void IncreaseVolume()
    {
        if (_volumeManager.GetVolume(_audioType) < 100)
        {
            _volumeManager.SetVolume(_audioType, 10);
            UpdateText();
        }
    }
    public void DecreaseVolume()
    {
        if (_volumeManager.GetVolume(_audioType) > 0)
        {
            _volumeManager.SetVolume(_audioType, -10);
            UpdateText();
        }
    }
    void UpdateText()
    {
        _text.text = "" + _volumeManager.GetVolume(_audioType);
    }
}

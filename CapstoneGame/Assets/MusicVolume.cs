using UnityEngine;
using TMPro;

public class MusicVolume : MonoBehaviour
{
    [SerializeField] AudioMixType _audioType;
    [SerializeField] TMP_Text _text;
    void Start()
    {
        UpdateText();   
    }
    public void IncreaseVolume()
    {
        if (MusicManager.Instance.GetVolume(_audioType) < 100)
        {
            MusicManager.Instance.SetVolume(_audioType, 10);
            UpdateText();
        }
    }
    public void DecreaseVolume()
    {
        if (MusicManager.Instance.GetVolume(_audioType) > 0)
        {
            MusicManager.Instance.SetVolume(_audioType, -10);
            UpdateText();
        }
    }
    void UpdateText()
    {
        _text.text = "" + MusicManager.Instance.GetVolume(_audioType);
    }
}

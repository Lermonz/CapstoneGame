using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class Player_SFXPlayer : MonoBehaviour
{
    AudioSource _source;
    public AudioClip _jumpSFX;
    public AudioClip _boostSFX;
    public AudioClip _spinSFX;
    public AudioClip _fastFallSFX;
    void Start()
    {
        _source = GetComponent<AudioSource>();   
    }
    public void SetAndPlayOneShot(AudioClip clip) {
        _source.PlayOneShot(clip);
    }
}

using UnityEngine;

public class AudioSelecterButtons : MonoBehaviour
{
    public AudioClip[] _clips;
    public string[] _events;
    private AudioSource _source;
    void Start() {
        _source = this.GetComponent<AudioSource>();   
    }
    public void PlaySFX(int fileNum) {
        AkSoundEngine.PostEvent(_events[fileNum], this.gameObject);
        //_source.PlayOneShot(_clips[fileNum]);
    }
}

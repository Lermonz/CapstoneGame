using UnityEngine;

public class AudioSelecterButtons : MonoBehaviour
{
    public static AudioSelecterButtons Instance;
    public AudioClip[] _clips;
    public string[] _events;
    private AudioSource _source;
    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
            return;
        }
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start() {
        _source = this.GetComponent<AudioSource>();   
    }
    public void PlaySFX(int fileNum) {
        AkSoundEngine.PostEvent(_events[fileNum], this.gameObject);
        //_source.PlayOneShot(_clips[fileNum]);
    }
}

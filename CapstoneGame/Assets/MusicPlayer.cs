using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    int trackToPlay;
    bool playingTrack1 = false;
    // bool playingTrack2 = false;
    // bool playingTrack3 = false;
    // bool playingTrackMenu = false;
    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        int sceneID = SceneManager.GetActiveScene().buildIndex;
        if(sceneID == 0) {
            trackToPlay = 0;
        }
        else if(sceneID != 0) {
            trackToPlay = 1;
        }
        Debug.Log(sceneID +" <- sceneID\n"+trackToPlay+" <- trackToPlay");
        if(trackToPlay == 0) {
            if(playingTrack1) {
                //Debug.Log("StopyWorld1");
                AkSoundEngine.PostEvent("StopWorld1",gameObject);
                playingTrack1 = false;
            }
            AkSoundEngine.PostEvent("PlayMenu",gameObject);
        }
        else if(trackToPlay == 1 && !playingTrack1) {
            //Debug.Log("PlayWorld1");
            AkSoundEngine.PostEvent("StopMenu",gameObject);
            AkSoundEngine.PostEvent("PlayWorld1",gameObject);
            playingTrack1 = true;
        }
    }
}

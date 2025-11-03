using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;
    int trackToPlay;
    bool playingTrack1 = false;
    bool playingTrack2 = false;
    bool playingTrack3 = false;
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
        else if (sceneID <= 20)
        {
            trackToPlay = 2;
        }
        else if (sceneID <= 30)
        {
            trackToPlay = 3;
        }
        else
        {
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
            if (playingTrack3)
            {
                AkSoundEngine.PostEvent("StopWorld3", gameObject);
                playingTrack3 = false;
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
        else if (trackToPlay == 3 && !playingTrack3)
        {
            //Debug.Log("PlayWorld1");
            AkSoundEngine.PostEvent("StopMenu", gameObject);
            AkSoundEngine.PostEvent("PlayWorld3", gameObject);
            playingTrack3 = true;
        }
    }
    // void OnApplicationFocus(bool hasFocus)
    // {
    //     //if !hasFocus, pause current playing event, if hasFocus, play
    //     if (!hasFocus) { AkSoundEngine.Suspend(); }
    //     else { AkSoundEngine.WakeupFromSuspend(); }
    // }
}

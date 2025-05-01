using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

[DisallowMultipleComponent]
public class GameBehaviour : MonoBehaviour
{
    public static GameBehaviour Instance;
    public bool _isGame;
    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start() {
        Application.targetFrameRate = 60;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void ExitGame() {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else 
            Application.Quit();
        #endif
    }
    public Vector3 ConvertTimerToVector3(float time) {
        float m = Mathf.FloorToInt(time/60000);
        float s = Mathf.FloorToInt((time-m*60000)/1000);
        float ms = Mathf.FloorToInt(time - m*60000 - s*1000);
        return new Vector3(m,s,ms);
    }
    public float ConvertTimerToFloat(Vector3 time) {
        return (time.x*60+time.y)*1000+time.z;
    }
}

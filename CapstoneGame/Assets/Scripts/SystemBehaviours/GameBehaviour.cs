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
    }
    public void ExitGame() {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else 
            Application.Quit();
        #endif
    }
}

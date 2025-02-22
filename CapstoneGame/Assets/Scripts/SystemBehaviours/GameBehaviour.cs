using UnityEngine;

[DisallowMultipleComponent]
public class GameBehaviour : MonoBehaviour
{
    public static GameBehaviour Instance;
    public bool _isGame;
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start() {
        Application.targetFrameRate = 60;
    }
}

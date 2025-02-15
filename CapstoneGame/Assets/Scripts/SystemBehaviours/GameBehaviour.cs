using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    public static GameBehaviour Instance;
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

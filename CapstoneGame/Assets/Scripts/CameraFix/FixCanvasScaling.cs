using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixCanvasScaling : MonoBehaviour
{
    public static FixCanvasScaling Instance;
    [SerializeField] CanvasScaler[] _canvasScalers;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
            Instance = this;
    }
    public void SetCanvasScalerMatch(float value)
    {
        if (_canvasScalers != null)
        {
            for (int i = 0; i < _canvasScalers.Length; i++)
            {
                _canvasScalers[i].matchWidthOrHeight = value;
            }
        }
    }
}

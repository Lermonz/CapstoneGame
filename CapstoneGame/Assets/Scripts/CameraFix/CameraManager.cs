using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] _cameras;
    public CinemachineVirtualCamera startCamera;
    public CinemachineVirtualCamera winCamera;
    private CinemachineVirtualCamera currentCamera;
    public static CameraManager Instance;
    [SerializeField] Player _player;
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
    void Start()
    {
        SwitchCamera(startCamera);
    }
    public void SwitchCamera(CinemachineVirtualCamera newCamera)
    {
        if (newCamera == currentCamera) { return; }
        currentCamera = newCamera;
        SetCurrentCameraEnabled();
    }
    public void SwitchToWinCamera()
    {
        currentCamera = winCamera;
        currentCamera.enabled = true;
        SetCurrentCameraEnabled();
    }
    void SetCurrentCameraEnabled()
    {
        for(int i = 0; i < _cameras.Length; i++)
        {
            if (_cameras[i] == currentCamera)
            {
                _cameras[i].enabled = true;
                _player.SetVcam(_cameras[i]);
            }
            else
            {
                _cameras[i].enabled = false;
            }
        }
    }
}

using Cinemachine;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _newCamera;
    public void OnContact()
    {
        CameraManager.Instance.SwitchCamera(_newCamera);
    }
}

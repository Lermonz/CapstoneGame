using Cinemachine;
using UnityEngine;

public class PauseCameraConfine : MonoBehaviour
{
    [SerializeField] CinemachineConfiner2D _selfConfiner;
    [SerializeField] CinemachineConfiner2D _referenceConfiner;
    void OnEnable()
    {
        _selfConfiner.m_BoundingShape2D = _referenceConfiner.m_BoundingShape2D;
    }
}

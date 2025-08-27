using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ChangeConfiner : MonoBehaviour
{
    public PolygonCollider2D _room;
    public CinemachineConfiner2D _confiner;
    public void ChangeCameraConfiner()
    {
        Debug.Log("Player Detected in Zone");
        _confiner.m_BoundingShape2D = _room;
    }
}

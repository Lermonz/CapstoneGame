using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CircularPath : MonoBehaviour
{
    [SerializeField] GameObject _movingObj;
    public Vector2 _radius;
    //public Vector3 _center;
    [SerializeField] float _speed;
    [SerializeField] float _offset;
    [SerializeField] bool _clockwise;
    [SerializeField] CirclePath _pathVisuals;
    float currentTime = 0;
    void Start()
    {
        currentTime = _offset * Mathf.Deg2Rad;
        if (_movingObj == null)
        {
            _movingObj = this.gameObject;
        }
        if (_pathVisuals != null)
        {
            _pathVisuals.DrawCircle(_radius);
        }
    }
    void Update()
    {
        currentTime += Time.deltaTime * (_speed * 0.1f) * (_clockwise ? -1 : 1);
        _movingObj.transform.localPosition = new Vector3(Mathf.Cos(currentTime) * _radius.x, Mathf.Sin(currentTime) * _radius.y);
        if (currentTime > 6.283 || currentTime < -6.283)
        {
            currentTime = 0;
        }
    }

}
[CustomEditor(typeof(CircularPath))]
public class RadiusView : Editor
{
    void OnSceneGUI() {
        CircularPath _circularPath = (CircularPath)target;
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(_circularPath.transform.position, Vector3.forward, _circularPath._radius.x, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTimeWithCurve : MonoBehaviour
{
    [SerializeField] GameObject _movingObj;
    [SerializeField] AnimationCurve _curve;
    public Vector3 _startRotation;
    public Vector3 _endRotation;
    [SerializeField] float _speed = 1;
    float currentTime = 0;
    void Start()
    {
        if (_movingObj == null)
        {
            _movingObj = this.gameObject;
        }
    }
    void Update()
    {
        currentTime += Time.deltaTime * (_speed*0.1f);
        _movingObj.transform.localEulerAngles = Vector3.LerpUnclamped(_startRotation, _endRotation, _curve.Evaluate(currentTime));
    }
}

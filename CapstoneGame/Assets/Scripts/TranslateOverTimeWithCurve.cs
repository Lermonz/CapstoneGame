using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateOverTimeWithCurve : MonoBehaviour
{
    [SerializeField] GameObject _movingObj;
    [SerializeField] AnimationCurve _curve;
    public Vector3 _startPosition;
    public Vector3 _endPosition;
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
        _movingObj.transform.localPosition = Vector3.LerpUnclamped(_startPosition, _endPosition, _curve.Evaluate(currentTime));
    }

}

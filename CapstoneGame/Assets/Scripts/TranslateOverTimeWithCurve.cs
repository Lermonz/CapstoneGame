using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateOverTimeWithCurve : MonoBehaviour
{
    [SerializeField] AnimationCurve _curve;
    [SerializeField] Vector3 _startPosition;
    [SerializeField] Vector3 _endPosition;
    [SerializeField] float _speed = 1;
    float currentTime = 0;
    void Update()
    {
        currentTime += Time.deltaTime * (_speed*0.1f);
        this.transform.position = Vector3.LerpUnclamped(_startPosition, _endPosition, _curve.Evaluate(currentTime));
    }

}

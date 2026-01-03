using System;
using System.Collections;
using UnityEngine;

public class ScaleOnEnable : MonoBehaviour
{
    [SerializeField] GameObject _movingObj;
    [SerializeField] AnimationCurve _curve;
    [SerializeField] bool _move = false;
    [SerializeField] bool _rotate = false;
    [SerializeField] bool _scale = false;
    [Header("Position")]
    [SerializeField] Vector3 _startPos;
    [SerializeField] Vector3 _endPos;
    [Header("Rotation")]
    [SerializeField] Vector3 _startRot;
    [SerializeField] Vector3 _endRot;
    [Header("Scale")]
    [SerializeField] Vector3 _startScale;
    [SerializeField] Vector3 _endScale;
    [SerializeField] float _durationFrames = 5;
    private event Action EnabledAnimationTrigger;
    void Awake()
    {
        if (_movingObj == null)
        {
            _movingObj = this.gameObject;
        }
    }
    void OnEnable()
    {
        if(_move) {EnabledAnimationTrigger += PosAnimTrigger;}
        if(_rotate) {EnabledAnimationTrigger += RotAnimTrigger;}
        if(_scale) {EnabledAnimationTrigger += ScaleAnimTrigger;}
        EnabledAnimationTrigger?.Invoke();
    }
    void PosAnimTrigger()
    {
        StartCoroutine(PositionAnimation());
    }
    void RotAnimTrigger()
    {
        StartCoroutine(RotateAnimation());
    }
    void ScaleAnimTrigger()
    {
        StartCoroutine(ScaleAnimation());
    }
    IEnumerator PositionAnimation()
    {
        _movingObj.transform.localPosition = _startPos;
        for(int i = 0; i < _durationFrames; i++)
        {
            _movingObj.transform.localPosition = Vector3.LerpUnclamped(_startPos, _endPos, _curve.Evaluate(i/(_durationFrames-1)));
            yield return null;
        }
        EnabledAnimationTrigger -= PosAnimTrigger;
    }
    IEnumerator RotateAnimation()
    {
        _movingObj.transform.localEulerAngles = _startRot;
        for(int i = 0; i < _durationFrames; i++)
        {
            _movingObj.transform.localEulerAngles = Vector3.LerpUnclamped(_startRot, _endRot, _curve.Evaluate(i/(_durationFrames-1)));
            yield return null;
        }
        EnabledAnimationTrigger -= RotAnimTrigger;
    }
    IEnumerator ScaleAnimation()
    {
        _movingObj.transform.localScale = _startScale;
        for(int i = 0; i < _durationFrames; i++)
        {
            _movingObj.transform.localScale = Vector3.LerpUnclamped(_startScale, _endScale, _curve.Evaluate(i/(_durationFrames-1)));
            yield return null;
        }
        EnabledAnimationTrigger -= ScaleAnimTrigger;
    }
    //would like to expand this to include options to translate and rotate from this script too
    //use an event (like bufferedInputs in Player.cs) to assign which coroutines to trigger on enable
    //would prefer to make the input fields for all of the options foldable menus, seems difficult
}

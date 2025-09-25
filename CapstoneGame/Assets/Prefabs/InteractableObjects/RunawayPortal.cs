using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunawayPortal : MonoBehaviour
{
    bool _runaway;
    [SerializeField] float _stoppingX;
    [SerializeField] float _boostThreshold;
    [SerializeField] float _boostMaximum;
    [SerializeField] float _boostBuffer;
    [SerializeField] float _boostExponent;
    [SerializeField] float _slowdownDiffX;
    [SerializeField] float _baseSpeed;
    [SerializeField] float _baseSlowSpeed;
    [SerializeField] GameObject _wings;
    float diffX;
    bool _startedFlying = false;
    void Update()
    {
        _runaway = LevelManager.Instance._canExit && this.transform.position.x < _stoppingX;
        if (LevelManager.Instance._canExit && !_startedFlying)
        {
            StartCoroutine(ExpandWings());
        }
        if (_runaway)
        {
            diffX = this.transform.position.x - GameObject.Find("Player").transform.position.x;
            if (diffX < 1) { diffX = 1; }
            float boostX = diffX < _boostThreshold ? Mathf.Pow(-(diffX - _boostThreshold) * _boostBuffer,_boostExponent) : 0;
            //float boostX = diffX < _boostThreshold ? _boostMaximum / Mathf.Pow(Mathf.Abs(diffX)+_boostBuffer,_boostExponent) : 0;
            float realSlowSpeed = this.transform.position.x > -15 ? _baseSlowSpeed : _baseSpeed;
            this.transform.Translate((Vector3.right * (diffX > _slowdownDiffX ? realSlowSpeed : _baseSpeed) + new Vector3(boostX, 0, 0)) * Time.deltaTime);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(LevelManager.Instance._canExit)
                _stoppingX = -999;
        }
    }
    IEnumerator ExpandWings()
    {
        _startedFlying = true;
        _wings.SetActive(true);
        float time = 15;
        for (int i = 0; i <= time; i++)
        {
            _wings.transform.localScale = Vector3.Slerp(Vector3.zero, Vector3.one * 1.5f, i / time);
            yield return null;
        }
    }
}

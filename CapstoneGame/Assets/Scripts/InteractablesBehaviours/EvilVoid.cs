using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilVoid : MonoBehaviour
{
    [SerializeField] PortalBehaviour _exitPortal;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] float _baseSpeed;
    [SerializeField] float _linearSpeed;
    [SerializeField] float _growthSpeed;
    [SerializeField] float _growthExponent;
    [SerializeField] float _thresholdDistance;
    [SerializeField] float _slowdownMult;
    [SerializeField] bool _horizontal;
    float _speed;
    float diffX;
    float _acc = 0.1f;
    float _max;
    Transform _player;
    void Start()
    {
        _renderer.material.SetFloat("_Size", this.transform.localScale.y);
        _player = GameObject.Find("Player").transform;
    }
    void Update()
    {
        if (!_exitPortal.HasWon)
        {
            diffX = _horizontal ? Mathf.Abs(this.transform.position.x - _player.position.x) : Mathf.Abs(this.transform.position.y - _player.position.y);
            if (diffX >= _thresholdDistance)
            {
                _max = Mathf.Pow((diffX-_thresholdDistance)*_growthSpeed, _growthExponent) + _baseSpeed+_linearSpeed;
            }
            else if (diffX >= 1)
            {
                _max = _baseSpeed+(diffX-_thresholdDistance)*_linearSpeed;
            }
            else
            {
                _max = _baseSpeed * _slowdownMult;
            }
            if (_speed < _max)
            {
                _speed += _acc;
            }
            else
            {
                _speed -= _acc * 0.5f;
            }
        }
        else { _speed = -2; }
        transform.Translate(Vector2.right * _speed * Time.deltaTime);
    }
}

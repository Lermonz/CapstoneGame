using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilVoid : MonoBehaviour
{
    [SerializeField] PortalBehaviour _exitPortal;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] Transform _player;
    [SerializeField] float _maxSpeed;
    [SerializeField] float _baseSpeed;
    [SerializeField] float _linearSpeed;
    [SerializeField] float _growthSpeed;
    [SerializeField] float _growthExponent;
    [SerializeField] float _thresholdDistance;
    [SerializeField] float _slowdownMult;
    [SerializeField] bool _horizontal;
    [SerializeField] float _startingDistanceFromPlayer;
    float _speed;
    float diffX;
    float _acc = 0.1f;
    float _max;
    bool _resetOnRespawn;

    void Start()
    {
        _renderer.material.SetFloat("_Size", this.transform.localScale.y);
    }
    void Update()
    {
        if (!_exitPortal.HasWon)
        {
            diffX = _horizontal ? Mathf.Abs(this.transform.position.x - _player.position.x) : Mathf.Abs(this.transform.position.y - _player.position.y);
            if (diffX >= _thresholdDistance)
            {
                _max = Mathf.Pow((diffX - _thresholdDistance) * _growthSpeed, _growthExponent) + _baseSpeed;
            }
            else if (diffX >= 1)
            {
                _max = _baseSpeed + (diffX - _thresholdDistance) * _linearSpeed;
            }
            else
            {
                _max = _baseSpeed * _slowdownMult;
            }
            if(_max >= _maxSpeed) { _max = _maxSpeed; }
            if (_speed < _max)
            {
                _speed += _acc;
            }
            else
            {
                _speed -= _acc;
            }
        }
        else { _speed = -2; }
        if (GameBehaviour.Instance.PlayerIsDead)
        {
            _resetOnRespawn = true;
        }
        else if (_resetOnRespawn)
        {
            RespawnLocation();
            _resetOnRespawn = false;
        }
        transform.Translate(Vector2.right * _speed * Time.deltaTime);
    }
    void RespawnLocation()
    {
        _speed = 0;
        transform.position = _horizontal ? new Vector3(_player.position.x - _startingDistanceFromPlayer, this.transform.position.y, this.transform.position.z)
        : new Vector3(this.transform.position.x, _player.position.y - _startingDistanceFromPlayer, this.transform.position.z);
    }
}

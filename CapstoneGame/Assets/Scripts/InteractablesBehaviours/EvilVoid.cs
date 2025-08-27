using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilVoid : MonoBehaviour
{
    [SerializeField] PortalBehaviour _exitPortal;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] float _baseSpeed;
    [SerializeField] float _growthSpeed;
    [SerializeField] float _thresholdDistance;
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
                _max = Mathf.Pow(diffX * 0.5f - (_thresholdDistance * 0.5f - 0.5f), _growthSpeed) + _baseSpeed;
            }
            else if (diffX >= 1)
            {
                _max = _baseSpeed;
            }
            else
            {
                _max = _baseSpeed * 0.5f;
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

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent (typeof (Controller2D))]
[RequireComponent (typeof (SpriteRenderer))]
public class Player : MonoBehaviour
{
    Controller2D _controller;
    SpriteRenderer _renderer;
    Vector2 _velocity;
    float _jumpVelocity = 8.5f;
    float _doubleJumpVelocity = 6f;
    float _gravity = -0.05f;
    float _baseAccel = 10f;
    float _baseDecel = 16f;
    float _skidDecel = 30f;
    float _maxRunSpeed = 5f;

    bool _canBoost = true;

    bool _boostDeceling;
    float _boostSpeed = 12f;
    float _boostDecel = 36f;

    Vector2 _dpad;
    bool _buttonA;
    bool _buttonB;
    
    void Start()
    {
        _controller = GetComponent<Controller2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }
    void Update() {
        float delta = Time.deltaTime;
        _dpad = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _buttonA = Input.GetButtonDown("Boost");
        _buttonB = Input.GetButtonDown("Spin");
        if ((_dpad.x > 0 && _renderer.flipX) || (_dpad.x < 0 && !_renderer.flipX)) {
            _renderer.flipX = !_renderer.flipX;
        }
        
        if(Mathf.Abs(_velocity.x) < _maxRunSpeed) {
            Accelerate(ref _velocity.x, _dpad.x, _baseAccel, delta);
        }
        if(Mathf.Abs(_velocity.x) >= _maxRunSpeed) {
            Accelerate(ref _velocity.x, _dpad.x, _baseAccel, delta, true);
            //_velocity.x -= _dpad.x * _baseAccel * delta;
        }
        if(Mathf.Sign(_dpad.x) != Mathf.Sign(_velocity.x)) {
            Accelerate(ref _velocity.x, _dpad.x, _skidDecel, delta);
            //_velocity.x += _dpad.x * _skidDecel * delta;
        }
        if(_dpad.x == 0 && _velocity.x != 0 && _controller._isGrounded) {
            Accelerate(ref _velocity.x, Mathf.Sign(_velocity.x), _baseDecel, delta, true);
            //_velocity.x -= Mathf.Sign(_velocity.x) * _baseDecel * delta;
        }
        if(_canBoost && _buttonA) {
            float facingDirection = _renderer.flipX ? -1 : 1;
            if(_dpad.x != 0) {
                facingDirection = Mathf.Sign(_dpad.x);
            }
            _velocity.x += _boostSpeed * facingDirection;
            StartCoroutine(BoostCooldown());
        }
        if(_boostDeceling) {
            Accelerate(ref _velocity.x, Mathf.Sign(_velocity.x), _boostDecel, delta, true);
            //_velocity.x -= Mathf.Sign(_velocity.x) * _boostDecel * delta;
        }
        
        if(_controller._isGrounded) {
            if(_dpad.y != 0) {
                _velocity.y = _jumpVelocity;
            }
            else {
                _velocity.y = 0;
            }
        }
        if(!_controller._isGrounded && _buttonB) {
            _velocity.y = _doubleJumpVelocity;
        }
        _velocity.y += _gravity;
        if(Mathf.Abs(_velocity.x) < 0.02) {
            _velocity.x = 0;
        }
        _controller.Move(_velocity * delta);
    }
    IEnumerator BoostCooldown() {
        _canBoost = false;
        _gravity = 0;
        _velocity.y = Mathf.Clamp(_velocity.y, -0.2f, 0.2f);
        yield return new WaitForSeconds(0.15f);
        _gravity = -0.05f;
        _boostDeceling = true;
        yield return new WaitForSeconds(0.25f);
        _boostDeceling = false;
        yield return new WaitForSeconds(0.8f);
        _canBoost = true;
    }
    void Accelerate(ref float axis, float input, float mult, float delta, bool decel = false) {
        int dir = decel ? -1 : 1;
        axis += input * mult * delta * dir;
    }
}

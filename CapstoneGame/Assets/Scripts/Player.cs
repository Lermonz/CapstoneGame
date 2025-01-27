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
    public GameObject _hitBox;
    float _jumpVelocity = 8.5f;
    float _doubleJumpVelocity = 6f;
    float _gravity = -0.05f;
    float _baseAccel = 10f;
    float _baseDecel = 16f;
    float _skidDecel = 30f;
    float _maxRunSpeed = 5f;

    bool _canBoost = true;
    bool _canSpin = true;
    bool _canDoubleJump = true;

    bool _boostDeceling;
    float _boostSpeed = 12f;
    float _boostDecel = 30f;

    Vector2 _dpad;
    bool _buttonA;
    bool _buttonB;

    float _spinCooldown = 1.1f;
    float _doubleJumpCooldown = 0.6f;
    
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
        
        // if not at max run speed yet, accelerate to max run speed
        if(Mathf.Abs(_velocity.x) < _maxRunSpeed) {
            Accelerate(ref _velocity.x, _dpad.x, _baseAccel, delta);
        }
        // if over max run speed, decelerate to max run speed
        if(Mathf.Abs(_velocity.x) >= _maxRunSpeed) {
            Accelerate(ref _velocity.x, _dpad.x, _baseAccel, delta, true);
            //_velocity.x -= _dpad.x * _baseAccel * delta;
        }
        // deceleration for skidding on the ground
        if(Mathf.Sign(_dpad.x) != Mathf.Sign(_velocity.x)) {
            Accelerate(ref _velocity.x, _dpad.x, _skidDecel, delta);
            //_velocity.x += _dpad.x * _skidDecel * delta;
        }
        // deceleration for when you are no longer pressing a direction
        if(_dpad.x == 0 && _velocity.x != 0 && _controller._isGrounded) {
            Accelerate(ref _velocity.x, Mathf.Sign(_velocity.x), _baseDecel, delta, true);
        }

        // Boost
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
        }

        // Spin
        if(_buttonB && _canSpin) {
            Hitbox(1.5f, 0.8f);
            if(!_controller._isGrounded && _canDoubleJump) {
                _velocity.y = _doubleJumpVelocity;
            }
            StartCoroutine(SpinCooldown());
        }

        //Refresh DoubleJump Property
        if(_controller._isGrounded && !_canDoubleJump) {
            StartCoroutine(DoubleJumpCooldown());
        }
        
        // Jump
        if(_controller._isGrounded) {
            if(_dpad.y != 0) {
                _velocity.y = _jumpVelocity;
            }
            else {
                _velocity.y = 0;
            }
        }
        
        _velocity.y += _gravity;
        if(Mathf.Abs(_velocity.x) < 0.02) {
            _velocity.x = 0;
        }
        _controller.Move(_velocity * delta);
    }
    IEnumerator BoostCooldown() {
        _canBoost = false;
        _canSpin = false;
        _canDoubleJump = false;
        _gravity = 0;
        _velocity.y = Mathf.Clamp(_velocity.y, -0.2f, 0.2f);
        yield return new WaitForSeconds(0.15f);
        _gravity = -0.05f;
        _canSpin = true;
        _boostDeceling = true;
        yield return new WaitForSeconds(0.25f);
        _boostDeceling = false;
        yield return new WaitForSeconds(0.8f);
        _canBoost = true;
    }
    IEnumerator SpinCooldown() {
        Debug.Log("SpinCooldown");
        _canSpin = false;
        _canDoubleJump = false;
        yield return new WaitForSeconds(_spinCooldown);
        _canSpin = true;
    }
    IEnumerator DoubleJumpCooldown() {
        yield return new WaitForSeconds(_doubleJumpCooldown);
        _canDoubleJump = true;
    }
    void Accelerate(ref float axis, float input, float mult, float delta, bool decel = false) {
        int dir = decel ? -1 : 1;
        axis += input * mult * delta * dir;
    }
    void Hitbox(float width, float height) {
        GameObject HitBoxObject;
        HitBoxObject = Instantiate(_hitBox, this.transform);
        HitBoxObject.transform.localScale = new Vector3(width, height, 20);
    }
}

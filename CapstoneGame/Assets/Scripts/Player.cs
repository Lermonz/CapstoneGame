using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
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
    float _jumpVelocity = 9f;
    float _doubleJumpVelocity = 6.5f;
    float _downBoostVelocity = -10f;
    float _gravity = -0.3f;
    float _baseAccel = 10f;
    float _baseDecel = 16f;
    float _baseAirDecel = 2.5f;
    float _skidDecel = 30f;
    float _maxRunSpeed = 5f;

    bool _canJump = true;
    bool _canBoost = true;
    bool _canSpin = true;
    bool _canDoubleJump = true;
    bool _canDownBoost = true;

    bool _ignoreWallVerticalClamp = false;

    bool _boostDeceling;
    float _boostSpeed = 12f;
    float _boostDecel = 30f;

    Vector2 _dpad;
    bool _buttonA;
    bool _buttonB;

    float _spinCooldown = 66f;
    float _doubleJumpCooldown = 8f;
    
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
        if(_controller._isGrounded) {
            _canJump = true;
            _velocity.y = 0;
            _canDownBoost = false;
        }
        // Jump
        if(_canJump && _dpad.y > 0) {
            _velocity.y = _jumpVelocity;
            _canDownBoost = true;
            _canJump = false;
        }
        if(!_controller._isGrounded && _canJump) {
            _canDownBoost = true;
            StartCoroutine(CoyoteTime());
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
        if(_dpad.x == 0 && _velocity.x != 0) {
            if(_controller._isGrounded) {
                Accelerate(ref _velocity.x, Mathf.Sign(_velocity.x), _baseDecel, delta, true);
            }
            else {
                Accelerate(ref _velocity.x, Mathf.Sign(_velocity.x), _baseAirDecel, delta, true);
            }
        }

        // Boost
        if(_canBoost && _buttonA) {
            float facingDirection = _renderer.flipX ? -1 : 1;
            if(_dpad.x != 0) {
                facingDirection = Mathf.Sign(_dpad.x);
            }
            _velocity.x += _boostSpeed * facingDirection;
            StartCoroutine(BoostCoroutine());
        }
        if(_boostDeceling) {
            Accelerate(ref _velocity.x, Mathf.Sign(_velocity.x), _boostDecel, delta, true);
        }
        if(_controller._hitWall) {
            StopCoroutine(BoostCoroutine());
            _velocity.x = Mathf.Clamp(_velocity.x,-5,5);
            _boostDeceling = false;
        }

        // Spin
        if(_buttonB && _canSpin) {
            Hitbox(1.5f, 0.8f);
            if(!_controller._isGrounded && _canDoubleJump) {
                _canDoubleJump = false;
                _velocity.y = _doubleJumpVelocity;
            }
            StartCoroutine(SpinCooldown(_spinCooldown));
        }

        //Refresh DoubleJump Property
        if(_controller._isGrounded && !_canDoubleJump) {
            StartCoroutine(DoubleJumpCooldown(_doubleJumpCooldown));
            StartCoroutine(SpinCooldown(_doubleJumpCooldown));
        }
        
        
        if(_dpad.y < 0 && _canDownBoost) {
            _velocity.y = _downBoostVelocity;
            _canDownBoost = false;
        }
        if(_controller._hitCeiling) {
            _velocity.y = 0;
        }
        
        _velocity.y += _gravity;
        if(Mathf.Abs(_velocity.x) < 0.02) {
            _velocity.x = 0;
        }
        _controller.Move(_velocity * delta);
    }
    IEnumerator BoostCoroutine() {
        _canBoost = false;
        bool spinOverride = false;
        if(_canSpin)
            spinOverride = true;
        if(spinOverride)
            _canSpin = false;
        _canDownBoost = false;
        _gravity *= 0.25f;
        _velocity.y = Mathf.Clamp(_velocity.y, -0.4f, 2f);
        yield return new WaitForSeconds(0.15f);
        _gravity *= 4f;
        if(spinOverride) {
            _canSpin = true;
            spinOverride = false;
        }
        _canDownBoost = true;
        _boostDeceling = true;
        yield return new WaitForSeconds(0.25f);
        _boostDeceling = false;
        if(!_controller._isGrounded) {
            StartCoroutine(BoostCooldown(60f));
        }
        else
            StartCoroutine(BoostCooldown(12f));
    }
    IEnumerator SpinCooldown(float cooldown) {
        Debug.Log("SpinCooldown");
        for(int i = 0; i < cooldown; i++) {
            _canSpin = false;
            yield return null;
        }
        _canSpin = true;
    }
    IEnumerator DoubleJumpCooldown(float cooldown) {
        for(int i = 0; i < cooldown; i++) {
            yield return null;
        }
        _canDoubleJump = true;
    }
    IEnumerator BoostCooldown(float cooldown) {
        for(int i = 0; i < cooldown; i++) {
            yield return null;
        }
        _canBoost = true;
    }
    IEnumerator CoyoteTime() {
        yield return new WaitForSeconds(0.1f);
        _canJump = false;
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
    void OnGUI() {
        string CoordText = GUI.TextArea(new Rect(0, 0, 150, 150), 
        ("XPos: "+this.transform.position.x.ToString("#.00")+
        "\nY Pos: "+this.transform.position.y.ToString("#.00")+
        "\nX Vel: "+_velocity.x.ToString("#.00")+
        "\nY Vel: "+_velocity.y.ToString("#.00")+
        "\nCan Boost: "+_canBoost+
        "\nCan Spin: "+_canSpin+
        "\nCan Double Jump: "+_canDoubleJump+
        "\nCan Fast Fall: "+_canDownBoost));
    }
}

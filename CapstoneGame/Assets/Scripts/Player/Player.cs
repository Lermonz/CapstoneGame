using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (VFXPlayer))]
public class Player : MonoBehaviour
{
    Controller2D _controller;
    SpriteRenderer _renderer;
    VFXPlayer _vfxPlayer;
    Animator _animator;

    Vector2 _velocity;
    public GameObject _hitBox;

    float _jumpVelocity = 14f;
    float _doubleJumpVelocity = 8.5f;
    float _downBoostVelocity = -10f;
    float _baseAccel = 18f;
    float _baseDecel = 20f;
    float _baseAirDecel = 2.8f;
    float _skidDecel = 40f;
    float _maxRunSpeed = 5.5f;
    const float _gravity = -29f;
    float _gravityMult = 1;
    bool _inBlackHole = false;
    float _terminalVelocity;
    const float _termV = -20;

    bool _canJump = true;
    bool _canBoost = true;
    bool _canSpin = true;
    bool _canDoubleJump = true;
    bool _canDownBoost = true;

    bool _boostDeceling;
    float _boostSpeed = 12f;
    float _boostDecel = 36f;

    bool _isGravityFlipped = false;

    float _spinCooldown = 42f;
    float _doubleJumpCooldown = 8f;

    bool _jumpOverride = false;
    
    void Start()
    {
        _controller = GetComponent<Controller2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _vfxPlayer = GetComponent<VFXPlayer>();
        _animator = GetComponent<Animator>();
        //_particles = GetComponent<ParticleSystem>();
        _terminalVelocity = _termV;
    }
    void Update() {
        float delta = Time.deltaTime;
        _animator.SetBool("Running", InputManager.Instance.HorizontalInput != 0 && _controller._isGrounded);
        _animator.SetBool("Jumping", _velocity.y > 0.2 && !_controller._isGrounded);
        _animator.SetBool("Falling", _velocity.y < -0.2 && !_controller._isGrounded);
        //_dpad = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if ((InputManager.Instance.HorizontalInput > 0 && _renderer.flipX) || (InputManager.Instance.HorizontalInput < 0 && !_renderer.flipX)) {
            _renderer.flipX = !_renderer.flipX;
        }
        if(_controller._isGrounded) {
            _canJump = !_jumpOverride;
            _velocity.y = 0;
            _canDownBoost = false;
        }
        if(_controller._hitCeiling) {
            _velocity.y = 0;
        }
        // Jump
        if(_canJump && InputManager.Instance.JumpInput) {
            _velocity.y = _jumpVelocity;
            //_sfxPlayer.SetAndPlayOneShot(_sfxPlayer._jumpSFX);
            AkSoundEngine.PostEvent("Player_Jump", gameObject);
            _canDownBoost = true;
            _canJump = false;
        }
        if(_velocity.y >= 4 && InputManager.Instance.JumpRelease) {
            StartCoroutine(ShortenJumpTo(2,4f));
        }
        if(!_controller._isGrounded && _canJump) {
            _canDownBoost = true;
            StartCoroutine(CoyoteTime());
        } 
        
        // if not at max run speed yet, accelerate to max run speed
        if(Mathf.Abs(_velocity.x) < _maxRunSpeed) {
            Accelerate(ref _velocity.x, InputManager.Instance.HorizontalInput, _baseAccel, delta);
        }
        // if over max run speed, decelerate to max run speed
        if(Mathf.Abs(_velocity.x) >= _maxRunSpeed) {
            Accelerate(ref _velocity.x, InputManager.Instance.HorizontalInput, _baseAccel, delta, true);
            //_velocity.x -= _dpad.x * _baseAccel * delta;
        }
        // deceleration for skidding on the ground
        if(Mathf.Sign(InputManager.Instance.HorizontalInput) != Mathf.Sign(_velocity.x)) {
            Accelerate(ref _velocity.x, InputManager.Instance.HorizontalInput, _skidDecel, delta);
            //_velocity.x += _dpad.x * _skidDecel * delta;
        }
        // deceleration for when you are no longer pressing a direction
        if(InputManager.Instance.HorizontalInput == 0 && _velocity.x != 0) {
            if(_controller._isGrounded) {
                Accelerate(ref _velocity.x, Mathf.Sign(_velocity.x), _baseDecel, delta, true);
            }
            else {
                Accelerate(ref _velocity.x, Mathf.Sign(_velocity.x), _baseAirDecel, delta, true);
            }
            if(Mathf.Abs(_velocity.x) <= 1) {
                _velocity.x = 0;
            }
        }
        //TREATING GRAVITY LIKE ACCELERATION
        if(!PauseMenu.Instance._isPaused && !_inBlackHole) {
            if(_velocity.y > _terminalVelocity) {
                Accelerate(ref _velocity.y, _gravityMult, _gravity, delta);
            }
            if(_velocity.y <= _terminalVelocity) {
                Accelerate(ref _velocity.y, _gravityMult, _gravity*2, delta, true);
                //_velocity.x -= _dpad.x * _baseAccel * delta;
            }
        }
        /// BOOST  ///
        if(_canBoost && InputManager.Instance.BoostInput) {
            float facingDirection = _renderer.flipX ? -1 : 1;
            if(InputManager.Instance.HorizontalInput != 0) {
                facingDirection = Mathf.Sign(InputManager.Instance.HorizontalInput);
            }
            _velocity.x += _boostSpeed * facingDirection;
            //_sfxPlayer.SetAndPlayOneShot(_sfxPlayer._boostSFX);
            AkSoundEngine.PostEvent("Player_Dash", gameObject);
            _vfxPlayer.Boost_AfterImage(_renderer.flipX);
            StartCoroutine(BoostCoroutine());
        }
        if(_boostDeceling) {
            Accelerate(ref _velocity.x, Mathf.Sign(_velocity.x), _boostDecel, delta, true);
        }
        // when player hits wall they can bounce off of it at high enough speeds
        if(_controller._hitWall) {
            if(Mathf.Abs(_velocity.x) > _maxRunSpeed * 1.5f) {
                _velocity.x = -_velocity.x*0.4f;
                _velocity.y = _isGravityFlipped ? -Mathf.Abs(_velocity.x) : Mathf.Abs(_velocity.x);
                _velocity.y *= 0.6f; //scale bounce with gravity changes
            }
            else
                _velocity.x = Mathf.Clamp(_velocity.x,-5,5);
            _boostDeceling = false;
        }

        /// SPIN ///
        if(InputManager.Instance.SpinInput && _canSpin) {
            Hitbox(1.5f, 0.8f);
            //_sfxPlayer.SetAndPlayOneShot(_sfxPlayer._spinSFX);
            AkSoundEngine.PostEvent("Player_Attack", gameObject);
            _vfxPlayer.Spin_Sparkle();
            if(!_controller._isGrounded && _canDoubleJump) {
                _canDoubleJump = false;
                _vfxPlayer.Woosh(-0.5f);
                _velocity.y = _doubleJumpVelocity;
            }
            StartCoroutine(SpinCooldown(_spinCooldown));
        }

        //Refresh DoubleJump Property
        if(_controller._isGrounded && !_canDoubleJump) {
            StopCoroutine(SpinCooldown(0));
            StartCoroutine(DoubleJumpCooldown(_doubleJumpCooldown));
            StartCoroutine(SpinCooldown(_doubleJumpCooldown));
        }
        
        //Fast Fall / Down Boost
        if(InputManager.Instance.DownInput && _canDownBoost) {
            _velocity.y += _downBoostVelocity;
            //_sfxPlayer.SetAndPlayOneShot(_sfxPlayer._fastFallSFX);
            AkSoundEngine.PostEvent("Player_FastFall", gameObject);
            _vfxPlayer.Woosh(0.5f);
            _canDownBoost = false;
        }
        
        // if(_inBlackHole)
        //     _velocity.y += _gravity;
        if(Mathf.Abs(_velocity.x) < 0.02) {
            _velocity.x = 0;
        }
        _controller.Move(_velocity * delta);
    }
    IEnumerator ShortenJumpTo(int delay, float amount) {
        for(int i = 0; i < delay; i++) {
            yield return null;
        }
        _velocity.y = _isGravityFlipped ? -amount : amount;
    }
    IEnumerator BoostCoroutine() {
        _canBoost = false;
        if(_canSpin || _canJump)
            _jumpOverride = true;
        if(_jumpOverride) {
            _canJump = false;
            _canSpin = false;
        }
        _canDownBoost = false;
        StartCoroutine(NegateGravityFor(12));
        _velocity.y = Mathf.Clamp(_velocity.y, -0.4f, 0.4f);
        yield return new WaitForSeconds(0.15f);
        if(_jumpOverride) {
            _canSpin = true;
            _jumpOverride = false;
        }
        _canDownBoost = true;
        _boostDeceling = true;
        yield return new WaitForSeconds(0.21f);
        _boostDeceling = false;
        StartCoroutine(BoostCooldown(60f));
    }
    IEnumerator NegateGravityFor(int delay) {
        _gravityMult *= 0.125f;
        for(int i = 0; i < delay; i++) {
            yield return null;
        }
        _gravityMult *= 8f;
    }
    IEnumerator BoostDecelCoroutine(int delay, int amount) {
        for(int i = 0; i < delay; i++) {
            yield return null;
        }
        _boostDeceling = true;
        StartCoroutine(BoostDecelCoroutine(amount));
    }
    IEnumerator BoostDecelCoroutine(int amount) {
        for(int i = 0; i < amount; i++) {
            yield return null;
        }
        _boostDeceling = false;
    }
    IEnumerator SpinCooldown(float cooldown) {
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
        _canSpin = true;
        _canDoubleJump = true;
    }
    IEnumerator BoostCooldown(float cooldown) {
        for(int i = 0; i < cooldown; i++) {
            if(_controller._isGrounded && i < cooldown - 12)
                i = (int)cooldown - 12;
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
    /// COLLISIONS WITH OBJECTS ///
    // GRAVITY FIELD
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("GravityFlip")) {
            FlipGravity();
        }
        if(other.gameObject.CompareTag("Boost")) {
            StartCoroutine(BoostObjectPull(other.gameObject.transform.position, 
                other.gameObject.GetComponent<BoostObjectBehaviour>().BoostInDirection()));
            StartCoroutine(BoostDecelCoroutine(26,13));
        }
    }
    void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.CompareTag("BlackHole")) {
            _terminalVelocity = 0;
            _inBlackHole = true;
            //Strength of black hole pull is increased when player is closer to it
            float strength = 1/Vector2.Distance(other.gameObject.transform.position,this.transform.position);
            //Debug.Log("Strength: "+strength);
            PullTowards(other.gameObject.transform.position, strength);
        }
    }
    void PullTowards(Vector2 goal, float str) {
        _velocity.x += (goal.x-this.transform.position.x)*str;
        _velocity.y += (goal.y-this.transform.position.y)*str;
    }
    IEnumerator BoostObjectPull(Vector3 position, Vector2 boost) {
        SetAllPlayerActions(false, false, false, false);
        float elapsedTime = 0;
        float waitTime = 6f;
        while(elapsedTime < waitTime) {
            transform.position = Vector3.Lerp(transform.position, position, (elapsedTime / (waitTime)));
            elapsedTime++;
            yield return null;
        }
        elapsedTime = 0;
        waitTime = 6f;
        while(elapsedTime < waitTime) {
            InputManager.Instance._freezeVelocity = true;
            elapsedTime++;
            yield return null;
        }
        InputManager.Instance._freezeVelocity = false;
        SetAllPlayerActions();
        ForceVelocityToVector(boost);

    }
    // GRAVITY FIELD EXIT
    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("GravityFlip")) {
            FlipGravity();
            _renderer.flipY = false;
        }
        if(other.gameObject.CompareTag("BlackHole")) {
            _terminalVelocity = _termV;
            _inBlackHole = false;
        }
    }
    void ForceVelocityToVector(Vector2 v) {
        _velocity = v;
    }
    void FlipGravity() {
        _isGravityFlipped = !_isGravityFlipped;
        _controller._groundIsDown = -_controller._groundIsDown;
        _jumpVelocity = -_jumpVelocity;
        _doubleJumpVelocity = -_doubleJumpVelocity;
        _downBoostVelocity = -_downBoostVelocity;
        _gravityMult = -_gravityMult;
        _renderer.flipY = true;
    }
    void SetAllPlayerActions(bool spin = true, bool doubleJump = true, bool boost = true, bool fastFall = true) {
        _canSpin = spin;
        _canDoubleJump = doubleJump;
        _canBoost = boost;
        _canDownBoost = fastFall;
    }
    // void OnGUI() {
    //     string CoordText = GUI.TextArea(new Rect(0, 0, 150, 150), 
    //     ("XPos: "+this.transform.position.x.ToString("#.00")+
    //     "\nY Pos: "+this.transform.position.y.ToString("#.00")+
    //     "\nX Vel: "+_velocity.x.ToString("#.00")+
    //     "\nY Vel: "+_velocity.y.ToString("#.00")+
    //     "\nIs Ground: "+_controller._isGrounded+
    //     "\nGravity Mult: "+_gravityMult));
    // }
}

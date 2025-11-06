using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (VFXPlayer))]
public class Player : MonoBehaviour
{
    Controller2D _controller;
    [SerializeField] SpriteRenderer _renderer;
    VFXPlayer _vfxPlayer;
    Animator _animator;
    CinemachineVirtualCamera _vcam;
    [SerializeField] Transform _cameraFollow;

    Vector2 _velocity;
    Vector2 _velocitySend;
    public GameObject _hitBox;

    float _horizontalInput;

    float _jumpVelocity = 14f;
    float _doubleJumpVelocity = 10.5f;
    float _downBoostVelocity = -24f;
    float _baseAccel = 18f;
    float _baseDecel = 20f;
    float _baseAirDecel = 2.8f;
    float _skidDecel = 40f;
    float _maxRunSpeed = 5.4f;
    const float _gravity = -32f;
    float _gravityMult = 1;
    bool _inBlackHole = false;
    float _blackHoleBaseStrength = 1.16f;
    float _terminalVelocity;
    const float _termV = -18;
    Vector2 _wind;
    float _windMult = 1;

    bool _canJump = true;
    bool _canBoost = true;
    bool _canSpin = true;
    bool _canDoubleJump = true;
    bool _canDownBoost = true;
    bool _canDownBoostReal = true;
    bool _isJumping = false;
    bool _canCrouch = true;
    bool _isCrouching = false;
    bool _canTeleport = true;
    bool _teleporting = false;
    bool _dontLockOut = false;
    bool _invulnerable;
    bool _hasWon = false;

    bool _grabbedMode = false;
    float _grabbedMaxSpeed = 4.5f;
    float _grabbedSpeed;
    float _grabbedAccel = 0.5f;
    GrabberBehavior _grabbedBy;

    float _bounceVelocity = 16f;

    bool _boostDeceling;
    float _boostSpeed = 11.5f;
    float _boostDecel = 55f;

    public bool _isGravityFlipped = false;

    float _spinCooldown = 42f;
    float _doubleJumpCooldown = 9f;
    float _spinBoost = 0; // real value is in spinCoroutine
    bool _specialJump;

    bool _jumpOverride = false;
    bool _overrideXInput = false;
    bool _dead;
    bool _isPhilip;
    public Action SwapState;

    void Start()
    {
        _controller = GetComponent<Controller2D>();
        //_renderer = GetComponent<SpriteRenderer>();
        _vfxPlayer = GetComponent<VFXPlayer>();
        _animator = GetComponent<Animator>();
        _vcam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        //_particles = GetComponent<ParticleSystem>();
        _terminalVelocity = _termV;
        _dead = false;
        _invulnerable = false;
        _hasWon = false;
        SetCostume();
        LevelManager.Instance.SetRespawnPoint(this.transform.position);
        //Set Actions
        InputManager.Instance.JumpPressEvent += OnJumpPress;
        InputManager.Instance.JumpReleaseEvent += OnJumpRelease;
        InputManager.Instance.BoostPressEvent += Boost;
        InputManager.Instance.SpinPressEvent += Spin;
        _controller.BecameGrounded += OnGrounded;
        _controller.BecameAirBorne += OnAirborne;
    }
    void OnDestroy()
    {
        InputManager.Instance.JumpPressEvent -= OnJumpPress;
        InputManager.Instance.JumpReleaseEvent -= OnJumpRelease;
        InputManager.Instance.BoostPressEvent -= Boost;
        InputManager.Instance.SpinPressEvent -= Spin;
        _controller.BecameGrounded -= OnGrounded;
        _controller.BecameAirBorne -= OnAirborne;
    }
    void SetCostume()
    {
        Debug.Log("Costume Texture Name: " + GameBehaviour.Instance.SelectedCostume.ToString());
        if (GameBehaviour.Instance.SelectedCostume.ToString() == "philip (UnityEngine.Texture2D)")
        {
            _animator.runtimeAnimatorController = GameBehaviour.Instance._philipController;
            _renderer.material = GameBehaviour.Instance._philipMaterial;
            _isPhilip = true;
        }
        else
        {
            _renderer.material.SetTexture("_Palette", GameBehaviour.Instance.SelectedCostume);
        }
    }
    void OnGrounded()
    {
        Debug.Log("Became Grounded");
        _wind.y = 0;
        _isJumping = false;
        _canJump = true;
        _velocity.y = 0;
        _canDownBoost = false;
        if(!_canDoubleJump)
        {
            StopCoroutine(SpinCooldown(0));
            StartCoroutine(DoubleJumpCooldown(_doubleJumpCooldown));
            StartCoroutine(SpinCooldown(_doubleJumpCooldown));
        }
    }
    void OnAirborne()
    {
        if(_canJump) { StartCoroutine(CoyoteTime()); }
    }
    void Update()
    {
        _horizontalInput = _overrideXInput ? 0 : InputManager.Instance.Dpad.x;
        float delta = Time.deltaTime;
        _isCrouching = InputManager.Instance.Dpad.y < 0 && _controller._isGrounded && _canCrouch;
        //Debug.Log("Dead: "+_dead);
        if (!PauseMenu.Instance._isPausedPhysics)
        {
            _animator.SetBool("Crouch", _isCrouching);
            _animator.SetBool("Running", _horizontalInput != 0 && _controller._isGrounded);
            _animator.SetBool("Jumping", _velocity.y >= 0 && !_controller._isGrounded && !_dead);
            _animator.SetBool("Falling", _velocity.y < 0 && !_controller._isGrounded && !_dead);
        }
        //_dpad = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if ((_horizontalInput > 0 && _renderer.flipX) || (_horizontalInput < 0 && !_renderer.flipX))
        {
            _renderer.flipX = !_renderer.flipX;
            _cameraFollow.localPosition *= -1;
        }
        if (_isCrouching)
        {
            _horizontalInput = 0;
            _controller.CrouchHurtbox();
            _windMult = 0.2f;
        }
        else
        {
            _controller.RegularHurtbox();
            if (!_grabbedMode) { _windMult = 1f; }
        }
        // if (_controller._isGrounded)
        // {
        //     _wind.y = 0;
        //     _isJumping = false;
        //     _canJump = !_jumpOverride;
        //     _canDownBoost = false;
        // }
        if (_controller._hitCeiling)
        {
            EndGrabbedMode();
            _isJumping = false;
            _velocity.y = _wind.y > 0 ? -2 : 0;
            _wind.y = 0;
        }
        _velocity.x += _controller._conveyerSpeed * delta;
        if (_boostDeceling)
        {
            Accelerate(ref _velocity.x, Mathf.Sign(_velocity.x), _boostDecel, delta, true);
        }
        else
        {
            // these are put in an else because otherwise they stack with boostdeceling and i dont want that
            // if not at max run speed yet, accelerate to max run speed
            if (Mathf.Abs(_velocity.x) < _maxRunSpeed)
            {
                Accelerate(ref _velocity.x, _horizontalInput, _baseAccel, delta, false);
            }
            // if over max run speed, decelerate to max run speed
            if (Mathf.Abs(_velocity.x) >= _maxRunSpeed)
            {
                Accelerate(ref _velocity.x, _horizontalInput, _baseAccel, delta, true);
                //_velocity.x -= _dpad.x * _baseAccel * delta;
            }
            // deceleration for skidding on the ground
            if (Mathf.Sign(_horizontalInput) != Mathf.Sign(_velocity.x))
            {
                Accelerate(ref _velocity.x, _horizontalInput, _skidDecel, delta, false);
                //_velocity.x += _dpad.x * _skidDecel * delta;
            }
        }
        // deceleration for when you are no longer pressing a direction
        if (_horizontalInput == 0 && _velocity.x != 0)
        {
            if (_controller._isGrounded)
            {
                Accelerate(ref _velocity.x, Mathf.Sign(_velocity.x), _baseDecel, delta, true);
            }
            else
            {
                Accelerate(ref _velocity.x, Mathf.Sign(_velocity.x), _baseAirDecel, delta, true);
            }
            if (!_inBlackHole && Mathf.Abs(_velocity.x) <= 1)
            {
                _velocity.x = 0;
            }
        }
        //TREATING GRAVITY LIKE ACCELERATION
        if (!_controller._isGrounded && !PauseMenu.Instance._isPaused && !_inBlackHole && !_dead && !_grabbedMode && !_teleporting && !_hasWon)
        {
            if (_isGravityFlipped ? _velocity.y < _terminalVelocity : _velocity.y > _terminalVelocity)
            {
                Accelerate(ref _velocity.y, _gravityMult, _gravity, delta);
            }
            if (_isGravityFlipped ? _velocity.y >= _terminalVelocity : _velocity.y <= _terminalVelocity)
            {
                Accelerate(ref _velocity.y, _gravityMult, _gravity * 2, delta, true);
                //_velocity.x -= _dpad.x * _baseAccel * delta;
            }
        }
        if (_grabbedMode)
        {
            Accelerate(ref _velocity.y, 1, _grabbedSpeed, delta);
            Debug.Log(_grabbedSpeed + " : " + _grabbedAccel);
            if (_grabbedSpeed < (_grabbedMaxSpeed + _wind.y))
            {
                Accelerate(ref _grabbedSpeed, 1, _grabbedAccel, 1);
            }
        }
        // when player hits wall they can bounce off of it at high enough speeds
        if (_controller._hitWall)
        {
            EndGrabbedMode();
            if (Mathf.Abs(_velocity.x) > _maxRunSpeed * 1.5f && Mathf.Abs(_velocity.y) < 5)
            {
                _vfxPlayer.DustEffect(0, 0.05f * Mathf.Sign(_velocity.x), Mathf.Sign(_velocity.x));
                _velocity.x = -_velocity.x * 0.4f;
                _velocity.y = _isGravityFlipped ? -Mathf.Abs(_velocity.x) : Mathf.Abs(_velocity.x);
                _velocity.y *= 0.9f; //scale bounce with gravity changes
            }
            else
                _velocity.x = Mathf.Clamp(_velocity.x, -5, 5);
            _boostDeceling = false;
        }
        if (!_inBlackHole && Mathf.Abs(_velocity.x) < 0.02)
        {
            _velocity.x = 0;
        }
        if (!_dead && !_hasWon)
        {
            _velocitySend = _velocity + _wind * _windMult + Vector2.right * _controller._conveyerSpeed;
        }
        else
        {
            _velocitySend = _velocity;
        }
        if (_velocitySend.y != 0) { Debug.Log("velocity send:" + _velocitySend.y); }
        _controller.Move(_velocitySend * delta);
    }
    void OnJumpPress()
    {
        Debug.Log("Can Jump?" + _canJump);
        if (!_canJump) { return; }
        AkSoundEngine.PostEvent("Player_Jump", gameObject);
        _isJumping = true;
        Jump(_jumpVelocity);
    }
    void OnJumpRelease()
    {
        if(!_isJumping || _specialJump) { return; }
        _isJumping = false;
        if (Mathf.Abs(_velocity.y) >= 7)
        {
            _velocity.y *= 0.4f;
        }
    }
    void Boost()
    {
        if(!_canBoost) { return; }
        float facingDirection = _renderer.flipX ? -1 : 1;
        EndGrabbedMode();
        if (_horizontalInput != 0)
        {
            facingDirection = Mathf.Sign(_horizontalInput);
        }
        if (Mathf.Sign(_velocity.x) != facingDirection)
        {
            _velocity.x = -_velocity.x;
        }
        _velocity.x *= 0.25f;
        _velocity.x += (_boostSpeed + _maxRunSpeed * 0.75f) * facingDirection;
        //Mathf.Clamp(_velocity.x, (_boostSpeed + 3) * facingDirection, Mathf.Infinity*facingDirection);
        //_sfxPlayer.SetAndPlayOneShot(_sfxPlayer._boostSFX);
        AkSoundEngine.PostEvent("Player_Dash", gameObject);
        if (_isPhilip && (_renderer.flipX != _isGravityFlipped))
        {
            StartCoroutine(PhilipFlip());
        }
        _vfxPlayer.Boost_AfterImage(_renderer.flipX);
        _animator.Play("Player_Dash");
        StartCoroutine(BoostCoroutine());
    }
    void Spin()
    {
        if(!_canSpin) { return; }
        EndGrabbedMode();
        _isJumping = false;
        Hitbox(1.5f, 0.8f);
        StartCoroutine(NotCrouchedFor(16));
        AkSoundEngine.PostEvent("Player_Attack", gameObject);
        _vfxPlayer.Spin_Sparkle();
        _animator.Play("Player_Spin");
        SwapState?.Invoke();
        //SwapBlocks(FindAllOnOffBlockObjects());
        if (!_controller._isGrounded && _canDoubleJump)
        {
            _canDoubleJump = false;
            _vfxPlayer.Woosh(-0.5f);
            _velocity.y = _doubleJumpVelocity;
            _canDownBoost = true;
        }
        StartCoroutine(SpinCooldown(_spinCooldown, _controller._isGrounded));
    }
    IEnumerator OverrideXInput(int delay) {
        _overrideXInput = true;
        for (int i = 0; i < delay; i++) {
            yield return null;
        }
        _overrideXInput = false;
    }
    IEnumerator ShortenJumpTo(int delay, float amount) {
        for(int i = 0; i < delay; i++) {
            yield return null;
        }
        _velocity.y = _isGravityFlipped ? -amount : amount;
    }
    IEnumerator BoostCoroutine() {
        _canBoost = false;
        StartCoroutine(LockOut(6,false,false,true));
        StartCoroutine(NegateGravityFor(12));
        StartCoroutine(OverrideXInput(9));
        _velocity.y = Mathf.Clamp(_velocity.y, -0.4f, 0.4f);
        yield return new WaitForSeconds(0.15f);
        StartCoroutine(BoostDecelCoroutine(1,9));
        StartCoroutine(BoostCooldown(70));
    }
    IEnumerator LockOut(int delay, bool ignoreSpinReset = false, bool ignoreDownBoostReset = false, bool ignoreBoostReset = false)
    {
        if (_canSpin || _canJump)
            _jumpOverride = true;
        if (_jumpOverride)
        {
            _canJump = false;
            _canSpin = false;
        }
        _canDownBoost = false;
        _canBoost = false;
        for (int i = 0; i < delay; i++)
        {
            yield return null;
        }
        if (_jumpOverride)
        {
            _canSpin = !ignoreSpinReset;
            _canJump = _controller._isGrounded;
            _jumpOverride = false;
        }
        _canDownBoost = !ignoreDownBoostReset;
        //if (!ignoreBoostReset) { RegenerateBoost(); } 
        _canBoost = !ignoreBoostReset;
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
    IEnumerator SpinCooldown(float cooldown, bool specialChance = false)
    {
        if (_controller._isGrounded)
            cooldown -= 8;
        for (int i = 0; i < cooldown; i++)
        {
            if (specialChance && i < 3)
            {
                _spinBoost = 2f;
            }
            else
                _spinBoost = 0;
            _canSpin = false;
            yield return null;
        }
        _canSpin = true;
        _spinBoost = 0;
    }
    IEnumerator DoubleJumpCooldown(float cooldown) {
        for(int i = 0; i < cooldown; i++) {
            yield return null;
        }
        _canSpin = true;
        _canDoubleJump = true;
    }
    IEnumerator BoostCooldown(int cooldown) {
        int adjuster = 12;
        bool didVFX = false;
        if (_controller._isGrounded)
            adjuster += 14;
        for(int i = 0; i < cooldown; i++) {
            if(_canBoost) { i = cooldown; }
            if(_controller._isGrounded && i < cooldown - adjuster)
                i = (int)cooldown - adjuster;
            if (cooldown - i < 6 && !didVFX)
            {
                if (!_dead && !_hasWon) { _vfxPlayer.RegenerateBoost(); }
                didVFX = true;
            }
            yield return null;
        }
        _canBoost = true;
    }
    IEnumerator CoyoteTime() {
        Debug.Log("CoyoteTime");
        yield return new WaitForSeconds(0.1f);
        if (!_controller._isGrounded) { _canJump = false; }
    }
    IEnumerator NotCrouchedFor(int delay)
    {
        _canCrouch = false;
        for (int i = 0; i < delay; i++)
        {
            yield return null;
        }
        _canCrouch = true;
    }
    IEnumerator PhilipFlip()
    {
        _renderer.flipX = !_renderer.flipX;
        _renderer.flipY = !_isGravityFlipped;
        for (int i = 0; i < 14; i++)
        {
            yield return null;
        }
        _renderer.flipX = !_renderer.flipX;
        _renderer.flipY = _isGravityFlipped;
    }
    IEnumerator TeleportCooldown() {
        _canTeleport = false;
        for(int i = 0; i < 42; i++) {
            yield return null;
        }
        _canTeleport = true;
    }
    void Jump(float jumpVelocity)
    {
        _velocity.y = jumpVelocity + (_isGravityFlipped ? -(Mathf.Abs(_velocity.x) * 0.125f + _spinBoost) : (Mathf.Abs(_velocity.x) * 0.125f + _spinBoost));
        _specialJump = false;
        if (_spinBoost != 0)
        {
            _specialJump = true;
            _vfxPlayer.SpecialJump();
            AkSoundEngine.PostEvent("Player_JumpSpecial", gameObject);
        }
        //_sfxPlayer.SetAndPlayOneShot(_sfxPlayer._jumpSFX);
        _canDownBoost = true;
        _canJump = false;
        //_isJumping = true;
    }
    void Accelerate(ref float axis, float input, float mult, float delta, bool decel = false, float conveyer = 0) {
        int dir = decel ? -1 : 1;
        axis += (input * mult * dir + conveyer) * delta;
    }
    void Hitbox(float width, float height) {
        GameObject HitBoxObject;
        HitBoxObject = Instantiate(_hitBox, this.transform);
        HitBoxObject.transform.localScale = new Vector3(width, height, 20);
    }
    // void RegenerateBoost()
    // {
    //     _canBoost = true;
    //     _vfxPlayer.RegenerateBoost();
    // }
    IEnumerator Teleport(Teleporter teleporter)
    {
        teleporter.Teleport();
        _renderer.enabled = false;
        _controller._collider.enabled = false;
        InputManager.Instance._freezeVelocity = true;
        _teleporting = true;
        _vcam.Follow = teleporter._teleportTravelCameraFollow.transform;
        float offsetY = _controller._vcamTransposer.m_TrackedObjectOffset.y;
        _controller._vcamTransposer.m_TrackedObjectOffset.y = 0;
        for (int i = 0; i < teleporter._time + 3; i++)
        {
            yield return null;
        }
        _renderer.enabled = true;
        _controller._collider.enabled = true;
        InputManager.Instance._freezeVelocity = false;
        _teleporting = false;
        _vcam.Follow = _cameraFollow;
        _controller._vcamTransposer.m_TrackedObjectOffset.y = offsetY;
        this.transform.position = teleporter.LinkedTo.position;
        StartCoroutine(TeleportCooldown());
    }
    void EndGrabbedMode()
    {
        _grabbedMode = false;
        _windMult = 1;
        _skidDecel = 40f;
        _baseAccel = 18f;
        _maxRunSpeed = 5.8f;
        if (_grabbedBy != null)
        {
            _grabbedBy.Detach(new Vector2(_velocity.x * 0.5f, 9));
        }
        _grabbedBy = null;
    }
    private List<OnOffBlockBehaviour> FindAllOnOffBlockObjects() 
    {
        IEnumerable<OnOffBlockBehaviour> OnOffObjects =
            FindObjectsByType<OnOffBlockBehaviour>(FindObjectsSortMode.None);
        return new List<OnOffBlockBehaviour>(OnOffObjects);
    }
    void SwapBlocks(List<OnOffBlockBehaviour> AllBlocks) {
        foreach (OnOffBlockBehaviour OnOff in AllBlocks)
        {
            OnOff.SwapState();
        }
    }
    /// COLLISIONS WITH OBJECTS ///
    // GRAVITY FIELD
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("GravityFlip"))
        {
            FlipGravity();
            _terminalVelocity = -_termV;
        }
        if (other.gameObject.CompareTag("GravityTriggerTrigger"))
        {
            _dontLockOut = true;
        }
        if (other.gameObject.CompareTag("Grabber") && !_grabbedMode)
        {
            _velocity.y = Mathf.Clamp(_velocity.y, _terminalVelocity, 0);
            _velocity.y *= 0.12f;
            _grabbedSpeed = -3f;
            _grabbedMode = true;
            _grabbedBy = other.gameObject.GetComponent<GrabberBehavior>();
            _grabbedBy.Attach();
            _windMult = 0.5f;
            _skidDecel = 80f;
            _baseAccel = 24f;
            _maxRunSpeed = 6.8f;
            StartCoroutine(LockOut(5));
            StartCoroutine(DoubleJumpCooldown(5));
        }
        if (other.gameObject.CompareTag("Boost"))
        {
            _isJumping = false;
            StartCoroutine(BoostObjectPull(other.gameObject.transform.position,
                other.gameObject.GetComponent<BoostObjectBehaviour>().BoostInDirection()));
            StartCoroutine(BoostDecelCoroutine(15, 9));
            StartCoroutine(NegateGravityFor(19 * (int)Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad * other.gameObject.transform.localEulerAngles.z))));
        }
        if (other.gameObject.CompareTag("Teleport") && _canTeleport)
        {
            StartCoroutine(Teleport(other.gameObject.GetComponent<Teleporter>()));
            // this.transform.position = other.gameObject.GetComponent<Teleporter>().LinkedTo.transform.position;
        }
        if (other.gameObject.CompareTag("Bounce"))
        {
            _isJumping = false;
            AkSoundEngine.PostEvent("Spring_Object", other.gameObject);
            Jump(_bounceVelocity);
            SetAllPlayerActions();
        }
        if (other.gameObject.CompareTag("BlackHole"))
        {
            _velocity.x *= 0.8f;
            _velocity.y *= 0.5f;
        }
        if (other.gameObject.CompareTag("SecretOrb"))
        {
            other.GetComponent<SecretOrb>().TouchedByPlayer();
        }
        if (other.gameObject.CompareTag("ConfinerChange"))
        {
            other.GetComponent<ChangeConfiner>().ChangeCameraConfiner();
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BlackHole"))
        {
            //_terminalVelocity = 0;
            _inBlackHole = true;
            //Strength of black hole pull is decreased when player is further from it
            float strength = _blackHoleBaseStrength - 0.35f*Vector2.Distance(other.gameObject.transform.position, this.transform.position);
            // float strength = _blackHoleStrength (2.85 in this instance) - 2 * Mathf.Pow(x,0.2); x is the distance
            _blackHoleBaseStrength += _blackHoleBaseStrength*0.08f*Time.deltaTime;
            //dont pull player into the ground from below
            if ((this.transform.position.y - other.gameObject.transform.position.y) > 0 && _controller._isGrounded)
            {
                ExitBlackHole();
            }
            //Debug.Log("Distance: "+Vector2.Distance(other.gameObject.transform.position, this.transform.position)+"\nStrength: "+strength);
            PullTowards(other.gameObject.transform.position, strength);
        }
        if (other.gameObject.CompareTag("Wind"))
        {
            _wind = other.gameObject.GetComponent<BoostObjectBehaviour>().BoostInDirection();
        }
    }
    public void PullTowards(Vector2 goal, float str) {
        _velocity.x += (goal.x-this.transform.position.x)*str;
        _velocity.y += (goal.y-this.transform.position.y)*str;
    }
    IEnumerator BoostObjectPull(Vector3 position, Vector2 boost) {
        SetAllPlayerActions(false, false, false, false);
        float elapsedTime = 0;
        float waitTime = 4f;
        while(elapsedTime < waitTime) {
            transform.position = Vector3.Lerp(transform.position, position, (elapsedTime / (waitTime)));
            elapsedTime++;
            yield return null;
        }
        elapsedTime = 0;
        waitTime = 2f;
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
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("GravityFlip"))
        {
            FlipGravity();
            _renderer.flipY = false;
            _terminalVelocity = _termV;
        }
        if (other.gameObject.CompareTag("GravityTriggerTrigger"))
        {
            _dontLockOut = false;
        }
        if (other.gameObject.CompareTag("BlackHole"))
        {
            //_terminalVelocity = _termV;
            ExitBlackHole();
        }
        if (other.gameObject.CompareTag("Wind"))
        {
            _wind = Vector2.zero;
        }
    }
    void ExitBlackHole() {
        _inBlackHole = false;
            _blackHoleBaseStrength = 1.16f;
    }
    void ForceVelocityToVector(Vector2 v) {
        AkSoundEngine.PostEvent("Boost_Object", gameObject);
        _velocity = v;
    }
    void FlipGravity()
    {
        int lockOutFrames = 7;
        if (!_dontLockOut)
        {
            _velocity.y = 0;
            if (this.gameObject.activeSelf) { StartCoroutine(NegateGravityFor(6)); }
        }
        else
        {
            lockOutFrames = 2;
            _canDoubleJump = true;
            if (Mathf.Abs(_velocity.y) < 12)
            {
                _velocity.y = 12 * Mathf.Sign(_velocity.y);
            }
        }
        if (this.gameObject.activeSelf) { StartCoroutine(LockOut(lockOutFrames)); }
        _controller._vcamTransposer.m_TrackedObjectOffset.y = -_controller._vcamTransposer.m_TrackedObjectOffset.y;
        _isGravityFlipped = !_isGravityFlipped;
        _controller._groundIsDown = -_controller._groundIsDown;
        _jumpVelocity = -_jumpVelocity;
        _doubleJumpVelocity = -_doubleJumpVelocity;
        _downBoostVelocity = -_downBoostVelocity;
        _gravityMult = -_gravityMult;
        _renderer.flipY = true;
        this.GetComponent<BoxCollider2D>().offset = -this.GetComponent<BoxCollider2D>().offset;
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
    //     "\nY Vel: "+_velocity.y.ToString("#.00")
    //     // "\nIs Ground: "+_controller._isGrounded+
    //     // "\nGravity Mult: "+_gravityMult
    //     ));
    // }
    public void DeathNormal(float delay, bool sceneResets) {
        if (!_invulnerable)
        {
            _invulnerable = true;
            _animator.SetTrigger("DeathNormal");
            InputManager.Instance.NegateAllInput();
            LevelManager.Instance.FreezePlayerAndTimer();
            PlayerIsDead(delay,sceneResets);
        }
        // freeze player movement
        // trigger animation for dying to spikes
    }
    public void DeathBlackHole(float delay, bool sceneResets) {
        if (!_invulnerable)
        {
            _invulnerable = true;
            _blackHoleBaseStrength += 2;
            if (!_dead)
            {
                _animator.SetTrigger("DeathBlackHole");
                PlayerIsDead(delay, sceneResets);
            }
        }
        // negate player control
            // trigger animation for dying to black hole (shrink and rotate into it)
        }
    IEnumerator DelayRespawn(float delay)
    {
        //this coroutine only occurs if the scene does not reset
        yield return new WaitForSeconds(delay);
        PauseMenu.Instance.OnResetLevel();
        for (int i = 0; i < 30; i++)
        {
            yield return null;
        }
        Respawn();
    }
    void Respawn()
    {
        _animator.SetTrigger("Respawns");
        _dead = false;
        GameBehaviour.Instance.SetPlayerDeath(_dead);
        _velocity = Vector2.zero;
        this.transform.position = LevelManager.Instance.Checkpoint;
        StartCoroutine(ReturnPlayerControl());
        StartCoroutine(Invulnerability(90));
    }
    IEnumerator Invulnerability(int delay)
    {
        _invulnerable = true;
        for (int i = 0; i < delay; i++)
        {
            yield return null;
        }
        _invulnerable = false;
    }
    public void SetInvulnerable(bool invul = true)
    {
        _invulnerable = invul;
    }
    public void SetHasWon(bool value = true)
    {
        _hasWon = value;
        if (value)
        {
            EndGrabbedMode();
            _velocity = Vector2.zero;
        }
    }
    IEnumerator ReturnPlayerControl()
    {
        yield return new WaitForSeconds(1.02f);
        LevelManager.Instance.FreezePlayerAndTimer(false);
        InputManager.Instance.EnablePlayerInput();
    }
    void PlayerIsDead(float delay, bool sceneResets)
    {
        _velocity = Vector2.zero;
        _dead = true;
        EndGrabbedMode();
        GameBehaviour.Instance.SetPlayerDeath(_dead);
        AkSoundEngine.PostEvent("Player_Die", gameObject);
        if (!sceneResets) { StartCoroutine(DelayRespawn(delay)); }
    }
}

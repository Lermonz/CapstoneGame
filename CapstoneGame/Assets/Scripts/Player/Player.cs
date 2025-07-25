using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    float _horizontalInput;

    float _jumpVelocity = 14f;
    float _doubleJumpVelocity = 10.5f;
    float _downBoostVelocity = -20f;
    float _baseAccel = 18f;
    float _baseDecel = 20f;
    float _baseAirDecel = 2.8f;
    float _skidDecel = 40f;
    float _maxRunSpeed = 5.8f;
    const float _gravity = -32f;
    float _gravityMult = 1;
    bool _inBlackHole = false;
    float _terminalVelocity;
    const float _termV = -20;
    Vector2 _wind;

    bool _canJump = true;
    bool _canBoost = true;
    bool _canSpin = true;
    bool _canDoubleJump = true;
    bool _canDownBoost = true;
    bool _canDownBoostReal = true;
    bool _isJumping = false;
    bool _canTeleport = true;
    bool _dontLockOut = false;

    bool _grabbedMode = false;
    float _grabbedMaxSpeed = 10.2f;
    float _grabbedSpeed;
    float _grabbedAccel = 124;
    GrabberBehavior _grabbedBy;

    float _bounceVelocity = 18f;

    bool _boostDeceling;
    float _boostSpeed = 12f;
    float _boostDecel = 50f;

    bool _isGravityFlipped = false;

    float _spinCooldown = 42f;
    float _doubleJumpCooldown = 9f;
    float _spinBoost = 0; // real value is in spinCoroutine

    bool _jumpOverride = false;
    bool _overrideXInput = false;
    bool _dead;

    void Start()
    {
        _controller = GetComponent<Controller2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _vfxPlayer = GetComponent<VFXPlayer>();
        _animator = GetComponent<Animator>();
        //_particles = GetComponent<ParticleSystem>();
        _terminalVelocity = _termV;
        _dead = false;
        SetCostume();
    }
    void SetCostume()
    {
        _renderer.material.SetTexture("_Palette", GameBehaviour.Instance.SelectedCostume);
    }
    void Update() {
        _horizontalInput = _overrideXInput ? 0 : InputManager.Instance.HorizontalInput;
        float delta = Time.deltaTime;
        //Debug.Log("Dead: "+_dead);
        _animator.SetBool("Running", _horizontalInput != 0 && _controller._isGrounded);
        _animator.SetBool("Jumping", _velocity.y >= 0 && !_controller._isGrounded && !_dead);
        _animator.SetBool("Falling", _velocity.y < 0 && !_controller._isGrounded && !_dead);
        //_dpad = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if ((_horizontalInput > 0 && _renderer.flipX) || (_horizontalInput < 0 && !_renderer.flipX)) {
            _renderer.flipX = !_renderer.flipX;
        }
        if(_controller._isGrounded) {
            _wind.y = 0;
            _isJumping = false;
            _canJump = !_jumpOverride;
            _velocity.y = 0;
            _canDownBoost = false;
        }
        if (_controller._hitCeiling)
        {
            EndGrabbedMode();
            _isJumping = false;
            _velocity.y = _wind.y > 0 ? -2 : 0;
            _wind.y = 0;
        }
        _canDownBoostReal = (_isGravityFlipped ? (_velocity.y > -2) : (_velocity.y < 2)) && _canDownBoost;
        // Jump
        if (_canJump && InputManager.Instance.JumpInput)
        {
            AkSoundEngine.PostEvent("Player_Jump", gameObject);
            Jump(_jumpVelocity);
        }
        if(_isJumping && InputManager.Instance.JumpRelease) {
            _isJumping = false;
            if (!_isGravityFlipped && _velocity.y >= 7) {
                _velocity.y *= 0.4f;
            }
            else if (_isGravityFlipped && _velocity.y <= -7) {
                _velocity.y *= 0.4f;
            }
        }
        if(!_controller._isGrounded && _canJump) {
            _canDownBoost = true;
            StartCoroutine(CoyoteTime());
        } 
        _velocity.x += _controller._conveyerSpeed * delta;
        if(_boostDeceling) {
            Accelerate(ref _velocity.x, Mathf.Sign(_velocity.x), _boostDecel, delta, true);
        }
        else {
        // these are put in an else because otherwise they stack with boostdeceling and i dont want that
            // if not at max run speed yet, accelerate to max run speed
            if(Mathf.Abs(_velocity.x) < _maxRunSpeed) {
                Accelerate(ref _velocity.x, _horizontalInput, _baseAccel, delta, false);
            }
            // if over max run speed, decelerate to max run speed
            if(Mathf.Abs(_velocity.x) >= _maxRunSpeed) {
                Accelerate(ref _velocity.x, _horizontalInput, _baseAccel, delta, true);
                //_velocity.x -= _dpad.x * _baseAccel * delta;
            }
            // deceleration for skidding on the ground
            if(Mathf.Sign(_horizontalInput) != Mathf.Sign(_velocity.x)) {
                Accelerate(ref _velocity.x, _horizontalInput, _skidDecel, delta, false);
                //_velocity.x += _dpad.x * _skidDecel * delta;
            }
        }
        // deceleration for when you are no longer pressing a direction
        if(_horizontalInput == 0 && _velocity.x != 0) {
            if(_controller._isGrounded) {
                Accelerate(ref _velocity.x, Mathf.Sign(_velocity.x), _baseDecel, delta, true);
            }
            else{
                Accelerate(ref _velocity.x, Mathf.Sign(_velocity.x), _baseAirDecel, delta, true);
            }
            if(!_inBlackHole && Mathf.Abs(_velocity.x) <= 1) {
                _velocity.x = 0;
            }
        }
        //TREATING GRAVITY LIKE ACCELERATION
        if(!PauseMenu.Instance._isPaused && !_inBlackHole && !_dead && !_grabbedMode) {
            if(_isGravityFlipped ? _velocity.y < _terminalVelocity : _velocity.y > _terminalVelocity) {
                Accelerate(ref _velocity.y, _gravityMult, _gravity, delta);
            }
            if(_isGravityFlipped ? _velocity.y >= _terminalVelocity : _velocity.y <= _terminalVelocity) {
                Accelerate(ref _velocity.y, _gravityMult, _gravity*2, delta, true);
                //_velocity.x -= _dpad.x * _baseAccel * delta;
            }
        }
        if (_grabbedMode)
        {
            Debug.Log("GrabbedMode "+_grabbedSpeed);
            Accelerate(ref _velocity.y, 1, _grabbedSpeed, delta);
            if (_grabbedSpeed < _grabbedMaxSpeed)
            {
                Accelerate(ref _grabbedSpeed, 1, _grabbedAccel, delta);
            }
        }
        /// BOOST  ///
        if (_canBoost && InputManager.Instance.BoostInput)
        {
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
            _velocity.x += _boostSpeed * facingDirection;
            //Mathf.Clamp(_velocity.x, (_boostSpeed + 3) * facingDirection, Mathf.Infinity*facingDirection);
            //_sfxPlayer.SetAndPlayOneShot(_sfxPlayer._boostSFX);
            AkSoundEngine.PostEvent("Player_Dash", gameObject);
            _vfxPlayer.Boost_AfterImage(_renderer.flipX);
            _animator.Play("Player_Dash");
            StartCoroutine(BoostCoroutine());
        }
        
        // when player hits wall they can bounce off of it at high enough speeds
        if(_controller._hitWall) {
            EndGrabbedMode();
            if (Mathf.Abs(_velocity.x) > _maxRunSpeed * 1.5f)
            {
                _vfxPlayer.DustEffect(0,0.05f*Mathf.Sign(_velocity.x),Mathf.Sign(_velocity.x));
                _velocity.x = -_velocity.x * 0.4f;
                _velocity.y = _isGravityFlipped ? -Mathf.Abs(_velocity.x) : Mathf.Abs(_velocity.x);
                _velocity.y *= 0.9f; //scale bounce with gravity changes
            }
            else
                _velocity.x = Mathf.Clamp(_velocity.x, -5, 5);
            _boostDeceling = false;
        }

        /// SPIN ///
        if(InputManager.Instance.SpinInput && _canSpin) {
            EndGrabbedMode();
            _isJumping = false;
            Hitbox(1.5f, 0.8f);
            //_sfxPlayer.SetAndPlayOneShot(_sfxPlayer._spinSFX);
            AkSoundEngine.PostEvent("Player_Attack", gameObject);
            _vfxPlayer.Spin_Sparkle();
            _animator.Play("Player_Spin");
            SwapBlocks(FindAllOnOffBlockObjects());
            if (!_controller._isGrounded && _canDoubleJump)
            {
                _canDoubleJump = false;
                _vfxPlayer.Woosh(-0.5f);
                _velocity.y = _doubleJumpVelocity;
                _canDownBoost = true;
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
        if(InputManager.Instance.DownInput && _canDownBoostReal) {
            _grabbedMode = false;
            // //_sfxPlayer.SetAndPlayOneShot(_sfxPlayer._fastFallSFX);
            AkSoundEngine.PostEvent("Player_FastFall", gameObject);
            _vfxPlayer.Woosh(0.5f);
            _velocity.y = _downBoostVelocity;
        }

        // if(_inBlackHole)
        //     _velocity.y += _gravity;
        //Conveyer belt
        if (!_inBlackHole && Mathf.Abs(_velocity.x) < 0.02)
        {
            _velocity.x = 0;
        }
        if(!_dead) {
            _controller.Move((_velocity + _wind + Vector2.right*_controller._conveyerSpeed) * delta);
        }
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
        StartCoroutine(BoostDecelCoroutine(1,10));
        StartCoroutine(BoostCooldown(70f));
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
            _jumpOverride = false;
        }
        _canDownBoost = !ignoreDownBoostReset;
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
    IEnumerator SpinCooldown(float cooldown)
    {
        if (_controller._isGrounded)
            cooldown -= 8;
        for (int i = 0; i < cooldown; i++)
        {
            if (i < 3)
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
    IEnumerator BoostCooldown(float cooldown) {
        int adjuster = 12;
        if(_controller._isGrounded)
            adjuster += 14;
        for(int i = 0; i < cooldown; i++) {
            if(_controller._isGrounded && i < cooldown - adjuster)
                i = (int)cooldown - adjuster;
            yield return null;
        }
        _canBoost = true;
    }
    IEnumerator CoyoteTime() {
        yield return new WaitForSeconds(0.1f);
        _canJump = false;
    }
    IEnumerator TeleportCooldown() {
        _canTeleport = false;
        for(int i = 0; i < 32; i++) {
            yield return null;
        }
        _canTeleport = true;
    }
    void Jump(float jumpVelocity)
    {
        _velocity.y = jumpVelocity + (_isGravityFlipped ? -(Mathf.Abs(_velocity.x) * 0.125f + _spinBoost) : (Mathf.Abs(_velocity.x) * 0.125f + _spinBoost));
        //_sfxPlayer.SetAndPlayOneShot(_sfxPlayer._jumpSFX);
        _canDownBoost = true;
        _canJump = false;
        _isJumping = true;
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
    void Teleport(Teleporter teleporter) {
        this.transform.position = teleporter.LinkedTo.transform.position;
        teleporter.PlaySFX();
    }
    void EndGrabbedMode()
    {
        _grabbedMode = false;
        if (_grabbedBy != null)
        {
            _grabbedBy.Detach(new Vector2(_velocity.x*0.5f, 9));
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
        if (other.gameObject.CompareTag("Grabber"))
        {
            _velocity.y = Mathf.Clamp(_velocity.y, _terminalVelocity, 0);
            _velocity.y *= 0.2f;
            _grabbedSpeed = -20;
            _grabbedMode = true;
            _grabbedBy = other.gameObject.GetComponent<GrabberBehavior>();
            _grabbedBy.Attach();
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
            StartCoroutine(TeleportCooldown());
            Teleport(other.gameObject.GetComponent<Teleporter>());
            // this.transform.position = other.gameObject.GetComponent<Teleporter>().LinkedTo.transform.position;
        }
        if (other.gameObject.CompareTag("Bounce"))
        {
            Jump(_bounceVelocity);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BlackHole"))
        {
            //_terminalVelocity = 0;
            _inBlackHole = true;
            //Strength of black hole pull is increased when player is closer to it
            float strength = 0.1f + 0.84f / Vector2.Distance(other.gameObject.transform.position, this.transform.position);
            //Debug.Log("Strength: "+strength);
            PullTowards(other.gameObject.transform.position, strength);
        }
        if (other.gameObject.CompareTag("Wind"))
        {
            _wind = other.gameObject.GetComponent<BoostObjectBehaviour>().BoostInDirection();
        }
    }
    void PullTowards(Vector2 goal, float str) {
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
            _inBlackHole = false;
        }
        if (other.gameObject.CompareTag("Wind"))
        {
            _wind = Vector2.zero;
        }
    }
    void ForceVelocityToVector(Vector2 v) {
        AkSoundEngine.PostEvent("Boost_Object", gameObject);
        _velocity = v;
    }
    void FlipGravity() {
        if(!_dontLockOut) {
            _velocity.y = 0;
            StartCoroutine(NegateGravityFor(6));
        }
        StartCoroutine(LockOut(8));
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
    void OnGUI() {
        string CoordText = GUI.TextArea(new Rect(0, 0, 150, 150), 
        ("XPos: "+this.transform.position.x.ToString("#.00")+
        "\nY Pos: "+this.transform.position.y.ToString("#.00")+
        "\nX Vel: "+_velocity.x.ToString("#.00")+
        "\nY Vel: "+_velocity.y.ToString("#.00")
        // "\nIs Ground: "+_controller._isGrounded+
        // "\nGravity Mult: "+_gravityMult
        ));
    }
    public void DeathNormal() {
        _animator.SetTrigger("DeathNormal");
        InputManager.Instance.NegateAllInput();
        LevelManager.Instance.FreezePlayerAndTimer();
        PlayerIsDead();
        // freeze player movement
        // trigger animation for dying to spikes
    }
    public void DeathBlackHole() {
        if(!_dead) {
            _animator.SetTrigger("DeathBlackHole");
            PlayerIsDead();
        }
        // negate player control
        // trigger animation for dying to black hole (shrink and rotate into it)
    }
    void PlayerIsDead() {
        _dead = true;
        AkSoundEngine.PostEvent("Player_Die", gameObject);
    }
}

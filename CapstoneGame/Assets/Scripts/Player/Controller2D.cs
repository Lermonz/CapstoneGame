using System;
using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    const float boundInset = 0.02f;

    public int _horzRayCount = 4;
    public int _vertRayCount = 4;
    float _horzRaySpacing;
    float _vertRaySpacing;

    float _vertElseCount;
    float _horzElseCount;


    public LayerMask collMask;
    public LayerMask getOutMask;
    public CinemachineFramingTransposer _vcamTransposer;

    public bool _isGrounded = true;
    bool _setToBeGrounded = true;
    public Action BecameGrounded;
    public Action BecameAirBorne;
    public bool _hitCeiling;
    public bool _hitWall;
    public bool _canDoubleJump;
    bool _destructable;
    bool _touchConveyer;
    public float _conveyerSpeed;
    DestructableBlockBehaviour _destructableBlock;
    bool _touchFallingBlock;
    FallingBlock _fallingBlock;

    float _vertMult = 0.95f;

    public int _groundIsDown = 1;

    public BoxCollider2D _collider;
    RayCastOrigins _raycastOrigins;
    Vector2 _hurtboxSize;
    Vector2 _hurtboxOffset;
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _vcamTransposer = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        CalcRays();
        _hurtboxOffset = _collider.offset;
        _hurtboxSize = _collider.size;
    }
    public void Move(Vector2 velocity)
    {
        if (InputManager.Instance._freezeVelocity)
        {
            velocity = Vector2.zero;
        }
        if (!PauseMenu.Instance._isPaused)
        {
            RayCastUpdate();
            HorzCollisions(ref velocity);
            ReverseHorzCollision(ref velocity);
            VertCollisions(ref velocity);
            transform.Translate(velocity);
        }
    }
    public void PullTowards(Vector2 goal, float str)
    {
        Vector2 dir = new Vector2(goal.x - this.transform.position.x, goal.y - this.transform.position.y);
        transform.Translate(dir * str);
    }
    void VertCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y - boundInset * _groundIsDown);
        float rayLength = Mathf.Abs(velocity.y) + boundInset*5;
        _destructable = false;
        _vertElseCount = 0;

        for (int i = 0; i < _vertRayCount; i++)
        {
            float _tempVertRaySpacing = directionY == -_groundIsDown ? _vertRaySpacing * 0.9f : _vertRaySpacing * 0.85f;
            Vector2 rayOrigin = directionY == -1 ? _raycastOrigins.botleft : _raycastOrigins.topleft;
            rayOrigin += directionY == -_groundIsDown ? (Vector2.right * 0.05f) : (Vector2.right * 0.075f);
            //rayOrigin.y += directionY * boundInset;
            rayOrigin += Vector2.right * i * _tempVertRaySpacing;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - boundInset) * directionY;
                //rayLength = hit.distance + boundInset;
                if (directionY == _groundIsDown)
                {
                    _hitCeiling = true;
                }
                else
                {
                    if (!_isGrounded && !LevelManager.Instance._stopTimer)
                    {
                        this.GetComponent<VFXPlayer>().DustEffect(-0.55f * _groundIsDown);
                    }
                    _setToBeGrounded = true;
                    //_vcamTransposer.m_DeadZoneHeight = 0;
                }
                if (hit.collider.CompareTag("DestructableBlock"))
                {
                    _destructable = true;
                    _destructableBlock = hit.collider.GetComponent<DestructableBlockBehaviour>();
                }
                if (_touchConveyer) { _conveyerSpeed = hit.collider.GetComponent<ConveyerBehaviour>()._speed; }
                else { _conveyerSpeed = 0; }
                if (hit.collider.CompareTag("Conveyer"))
                {
                    _touchConveyer = true;
                }
                if (hit.collider.CompareTag("FallingBlock"))
                {
                    _touchFallingBlock = true;
                    _fallingBlock = hit.collider.GetComponent<FallingBlock>();
                }else {_touchFallingBlock = false;}
                //Debug.Log("conveyer? " + _touchConveyer + " " + _conveyerSpeed);

            }
            else
            {
                _vertElseCount++;
                //_vcamTransposer.m_DeadZoneHeight = 0.1f;
            }
        }
        if (_vertElseCount >= _vertRayCount && !PauseMenu.Instance._isPausedPhysics)
        {
            _touchConveyer = false;
            _setToBeGrounded = false;
            _hitCeiling = false;
            _destructable = false;
            _touchFallingBlock = false;
        }
        // if (_hitWall && _vertElseCount > 1)
        // {
        //     _setToBeGrounded = false; Debug.Log("Hit Wall cause not grounded");
        // }
        if (_destructable)
        {
            TouchedDestructableBlock(ref _destructableBlock);
            if(!_hitCeiling)
            {
                velocity.y -= boundInset + _destructableBlock._fallSpeed * _destructableBlock._delayOverStep;
            }
        }
        if (_touchFallingBlock && !_hitCeiling)
        {
            if (_fallingBlock.MoveDown())
            {
                velocity.y -= boundInset + _fallingBlock._fallSpeed * Time.deltaTime;
            }
        }
        if (_fallingBlock != null)
        {
            _fallingBlock.SetIsGoingDown(_touchFallingBlock);
        }
        if (_setToBeGrounded && !_isGrounded)
        {
            BecameGrounded?.Invoke();
        }
        if (!_setToBeGrounded && _isGrounded)
        {
            BecameAirBorne?.Invoke();
        }
        _isGrounded = _setToBeGrounded;
    }
    void HorzCollisions(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + boundInset;
        _horzElseCount = 0;
        float hitDistanceSum = 0;
        _destructable = false;

        for (int i = 1; i < _vertRayCount; i++)
        {
            Vector2 rayOrigin = directionX == -1 ? _raycastOrigins.botleft : _raycastOrigins.botright;
            //rayOrigin.x += boundInset * directionX;
            if (_groundIsDown == -1) { rayOrigin = directionX == -1 ? _raycastOrigins.topleft : _raycastOrigins.topright; }
            rayOrigin += Vector2.up * (_horzRaySpacing * _vertMult * i * _groundIsDown);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - boundInset) * directionX;
                //velocity.y = Mathf.Clamp(velocity.y, -Mathf.Infinity, 0);
                rayLength = hit.distance;
                _hitWall = true;
                if (hit.collider.CompareTag("DestructableBlock"))
                {
                    _destructable = true;
                    _destructableBlock = hit.collider.GetComponent<DestructableBlockBehaviour>();
                }
                hitDistanceSum += hit.distance;
            }
            else
            {
                _horzElseCount++;
            }
        }
        if (_horzElseCount >= _horzRayCount - 1)
        {
            _hitWall = false;
            _destructable = false;
        }
        if (_horzElseCount == 0 && hitDistanceSum < 0.01f)
        {
            GetOutOfWallCollision(ref velocity);
        }
        if (_hitWall && _destructable)
        {
            TouchedDestructableBlock(ref _destructableBlock);
        }

    }
    void ReverseHorzCollision(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + boundInset;
        Vector2 rayOrigin = directionX == 1 ? _raycastOrigins.botleft : _raycastOrigins.botright;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collMask);
        Vector2 rayOrigin2 = directionX == 1 ? _raycastOrigins.topleft : _raycastOrigins.topright;
        RaycastHit2D hit2 = Physics2D.Raycast(rayOrigin2, Vector2.right * directionX, rayLength, collMask);
        Debug.DrawRay(rayOrigin, Vector2.left * directionX * rayLength, Color.red);
        Debug.DrawRay(rayOrigin2, Vector2.left * directionX * rayLength, Color.red);
        if (hit && hit2)
        {
            if (hit.distance == 0 && hit2.distance == 0)
            {
                GetOutOfWallCollision(ref velocity);
            }
            velocity.x = (hit.distance + 0.15f) * directionX;
        }
    }
    void TouchedDestructableBlock(ref DestructableBlockBehaviour block)
    {
        if (!block._beenTouched)
        {
            block.BeenTouched();
        }
    }
    void GetOutOfWallCollision(ref Vector2 velocity)
    {
        RaycastHit2D escapeRight = Physics2D.Raycast(_raycastOrigins.botright, Vector2.right, 4, getOutMask);
        RaycastHit2D escapeLeft = Physics2D.Raycast(_raycastOrigins.botleft, Vector2.left, 4, getOutMask);
        Debug.DrawRay(_raycastOrigins.botright, Vector2.right * 4, Color.red);
        Debug.DrawRay(_raycastOrigins.botleft, Vector2.left * 4, Color.red);
        float shortestDistance = escapeRight.distance < escapeLeft.distance ? escapeRight.distance : -escapeLeft.distance;
        velocity.x = shortestDistance;
        Debug.Log("escapeRight: " + escapeRight.distance + "\nescapeLeft: " + escapeLeft.distance + "\nshortest: " + shortestDistance + "\nvelocity.x:" + velocity.x);
    }
    void RayCastUpdate()
    {
        Bounds bounds = getBounds();

        _raycastOrigins.botleft = new Vector2(bounds.min.x, bounds.min.y);
        _raycastOrigins.botright = new Vector2(bounds.max.x, bounds.min.y);
        _raycastOrigins.topleft = new Vector2(bounds.min.x, bounds.max.y);
        _raycastOrigins.topright = new Vector2(bounds.max.x, bounds.max.y);
        //_center = new Vector2((bounds.max.x+bounds.min.x)/2, (bounds.min.y+bounds.max.y)/2);
    }
    void CalcRays()
    {
        Bounds bounds = getBounds();

        _horzRayCount = Mathf.Clamp(_horzRayCount, 2, 20);
        _vertRayCount = Mathf.Clamp(_vertRayCount, 2, 20);

        _horzRaySpacing = bounds.size.y / (_horzRayCount - 1);
        _vertRaySpacing = bounds.size.x / (_vertRayCount - 1);
    }
    Bounds getBounds()
    {
        Bounds bounds = _collider.bounds;
        bounds.Expand(boundInset * -2);
        return bounds;
    }
    public struct RayCastOrigins
    {
        public Vector2 topleft, topright;
        public Vector2 botleft, botright;
    }
    public void CrouchHurtbox()
    {
        Vector2 offset = new Vector2(0, -0.25f);
        Vector2 size = new Vector2(_hurtboxSize.x, 0.5f);
        SetHurtbox(offset, size);
    }
    public void RegularHurtbox()
    {
        SetHurtbox(_hurtboxOffset, _hurtboxSize);
    }
    void SetHurtbox(Vector2 offset, Vector2 size)
    {
        _collider.offset = _groundIsDown * offset;
        _collider.size = size;
    }
}

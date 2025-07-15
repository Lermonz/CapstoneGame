using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
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
    CinemachineFramingTransposer _vcamTransposer;

    public bool _isGrounded = true;
    public bool _hitCeiling;
    public bool _hitWall;
    public bool _canDoubleJump;
    bool _destructable;
    DestructableBlockBehaviour _destructableBlock;

    float _vertMult = 0.95f;

    public int _groundIsDown = 1;

    BoxCollider2D _collider;
    RayCastOrigins _raycastOrigins;
    Vector2 _center;
    void Start() {
        _collider = GetComponent<BoxCollider2D>();
        _vcamTransposer = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        CalcRays();
    }
    public void Move(Vector2 velocity) {
        if(InputManager.Instance._freezeVelocity) {
            velocity = Vector2.zero;
        }
        RayCastUpdate();
        VertCollisions(ref velocity);
        HorzCollisions(ref velocity);
        ReverseHorzCollision(ref velocity);
        if (!PauseMenu.Instance._isPaused)
        {
            transform.Translate(velocity);
        }
    }
    public void PullTowards(Vector2 goal, float str) {
        Vector2 dir = new Vector2(goal.x-this.transform.position.x,goal.y-this.transform.position.y);
        transform.Translate(dir*str);
    }
    void VertCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + boundInset;
        _destructable = false;
        _vertElseCount = 0;

        for (int i = 0; i < _vertRayCount; i++)
        {
            float _tempVertRaySpacing = directionY == -1 ? _vertRaySpacing * 0.9f : _vertRaySpacing * 0.7f;
            Vector2 rayOrigin = directionY == -1 ? _raycastOrigins.botleft : _raycastOrigins.topleft;
            rayOrigin += directionY == -_groundIsDown ? (Vector2.right * 0.05f) : (Vector2.right * 0.15f + Vector2.up * -0.1f);
            rayOrigin += Vector2.right * i * _tempVertRaySpacing;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - boundInset) * directionY;
                rayLength = hit.distance;
                if (directionY == _groundIsDown)
                {
                    _hitCeiling = true;
                }
                else
                {
                    if (!_isGrounded)
                    {
                        this.GetComponent<VFXPlayer>().DustEffect(-0.45f * _groundIsDown);
                    }
                    _isGrounded = true;
                    _vcamTransposer.m_DeadZoneHeight = 0;
                }
                if (hit.collider.CompareTag("DestructableBlock"))
                {
                    _destructable = true;
                    _destructableBlock = hit.collider.GetComponent<DestructableBlockBehaviour>();
                }

            }
            else
            {
                _vertElseCount++;
                _vcamTransposer.m_DeadZoneHeight = 0.2f;
            }
        }
        if (_vertElseCount >= _vertRayCount)
        {
            _isGrounded = false;
            _hitCeiling = false;
            _destructable = false;
        }
        if (_hitWall && _vertElseCount > 0)
        {
            _isGrounded = false;
        }
        if (_destructable)
        {
            TouchedDestructableBlock(ref _destructableBlock);
        }
    }
    void HorzCollisions(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + boundInset;
        _horzElseCount = 0;
        _destructable = false;

        for (int i = 1; i < _vertRayCount; i++)
        {
            Vector2 rayOrigin = directionX == -1 ? _raycastOrigins.botleft : _raycastOrigins.botright;
            rayOrigin += Vector2.up * (_horzRaySpacing * _vertMult * i);
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
            }
            else
            {
                _horzElseCount++;
            }
        }
        if (_horzElseCount >= _horzRayCount - 2)
        {
            _hitWall = false;
            _destructable = false;
        }
        if (_horzElseCount == 0)
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
        Vector2 rayOrigin = directionX == 1 ? _raycastOrigins.botleft + Vector2.right * 0.25f : _raycastOrigins.botright - Vector2.right * 0.25f;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collMask);
        Vector2 rayOrigin2 = directionX == 1 ? _raycastOrigins.topleft + Vector2.right * 0.25f : _raycastOrigins.topright - Vector2.right * 0.25f;
        RaycastHit2D hit2 = Physics2D.Raycast(rayOrigin2, Vector2.right * directionX, rayLength, collMask);
        if (hit && hit2)
        {
            if (hit.distance == 0 && hit2.distance == 0)
            {
                GetOutOfWallCollision(ref velocity);
            }
            else
            {
                velocity.x = (hit.distance + 0.25f) * directionX;
            }
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
        Debug.DrawRay(_raycastOrigins.botright, Vector2.right* 4, Color.red);
        Debug.DrawRay(_raycastOrigins.botleft, Vector2.left* 4, Color.red);
        float shortestDistance = escapeRight.distance < escapeLeft.distance ? escapeRight.distance : -escapeLeft.distance;
        velocity.x = shortestDistance;
        Debug.Log("escapeRight: " + escapeRight.distance + "\nescapeLeft: " + escapeLeft.distance + "\nshortest: " + shortestDistance + "\nvelocity.x:" + velocity.x);
    }
    void RayCastUpdate() {
        Bounds bounds = getBounds();

        _raycastOrigins.botleft = new Vector2(bounds.min.x, bounds.min.y);
        _raycastOrigins.botright = new Vector2(bounds.max.x, bounds.min.y);
        _raycastOrigins.topleft = new Vector2(bounds.min.x, bounds.max.y);
        _raycastOrigins.topright = new Vector2(bounds.max.x, bounds.max.y);
        //_center = new Vector2((bounds.max.x+bounds.min.x)/2, (bounds.min.y+bounds.max.y)/2);
    }
    void CalcRays(){
        Bounds bounds = getBounds();

        _horzRayCount = Mathf.Clamp(_horzRayCount,2,20);
        _vertRayCount = Mathf.Clamp(_vertRayCount,2,20);

        _horzRaySpacing = bounds.size.y / (_horzRayCount-1);
        _vertRaySpacing = bounds.size.y / (_vertRayCount-1);
    }
    Bounds getBounds() {
        Bounds bounds = _collider.bounds;
        bounds.Expand(boundInset*-2);
        return bounds;
    }
    public struct RayCastOrigins {
        public Vector2 topleft, topright;
        public Vector2 botleft, botright;
    }
}

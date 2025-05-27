using System.Collections;
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

    public bool _isGrounded = true;
    public bool _hitCeiling;
    public bool _hitWall;
    public bool _canDoubleJump;

    float _vertMult = 0.95f;

    public int _groundIsDown = 1;

    BoxCollider2D _collider;
    RayCastOrigins _raycastOrigins;
    void Start() {
        _collider = GetComponent<BoxCollider2D>();
        CalcRays();
    }
    public void Move(Vector2 velocity) {
        if(InputManager.Instance._freezeVelocity) {
            velocity = Vector2.zero;
        }
        RayCastUpdate();
        VertCollisions(ref velocity);
        HorzCollisions(ref velocity);
        if(!PauseMenu.Instance._isPaused) {
            transform.Translate(velocity);
        }
    }
    public void PullTowards(Vector2 goal, float str) {
        Vector2 dir = new Vector2(goal.x-this.transform.position.x,goal.y-this.transform.position.y);
        transform.Translate(dir*str);
    }
    void VertCollisions(ref Vector2 velocity) {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y)+boundInset;

        _vertElseCount = 0;

        for(int i = 0; i < _vertRayCount; i++) {
            float _tempVertRaySpacing = directionY == -1 ? _vertRaySpacing * 0.9f : _vertRaySpacing * 0.7f;
            Vector2 rayOrigin = directionY == -1 ? _raycastOrigins.botleft + Vector2.right*0.05f : _raycastOrigins.topleft + Vector2.right*0.15f;
            rayOrigin += Vector2.right * i * _tempVertRaySpacing;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if(hit) {
                velocity.y = (hit.distance-boundInset) * directionY;
                rayLength = hit.distance;
                if(directionY == _groundIsDown) {
                    _hitCeiling = true;
                }
                else {
                    if (!_isGrounded) {
                        this.GetComponent<VFXPlayer>().DustEffect(-0.45f*_groundIsDown);
                    }
                    _isGrounded = true;
                }
                
            }
            else {
                _vertElseCount++;
            }
        }
        if(_vertElseCount >= _vertRayCount) {
            _isGrounded = false;
            _hitCeiling = false;
        }
        if(_hitWall && _vertElseCount > 0) {
            _isGrounded = false;
        }
    }
    void HorzCollisions(ref Vector2 velocity) {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x)+boundInset;
        _horzElseCount = 0;

        for(int i = 1; i < _vertRayCount; i++) {
            Vector2 rayOrigin = directionX == -1 ? _raycastOrigins.botleft : _raycastOrigins.botright;
            rayOrigin += Vector2.up * (_horzRaySpacing * _vertMult * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionX * rayLength, Color.red);

            if(hit) {
                velocity.x = (hit.distance-boundInset) * directionX;
                //velocity.y = Mathf.Clamp(velocity.y, -Mathf.Infinity, 0);
                rayLength = hit.distance;
                _hitWall = true;
            }
            else {
                _horzElseCount++;
            }
        }
        if(_horzElseCount >= _horzRayCount-2) {
            _hitWall = false;
        }

    }
    void RayCastUpdate() {
        Bounds bounds = getBounds();

        _raycastOrigins.botleft = new Vector2(bounds.min.x,bounds.min.y);
        _raycastOrigins.botright = new Vector2(bounds.max.x,bounds.min.y);
        _raycastOrigins.topleft = new Vector2(bounds.min.x,bounds.max.y);
        _raycastOrigins.topright = new Vector2(bounds.max.x,bounds.max.y);
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

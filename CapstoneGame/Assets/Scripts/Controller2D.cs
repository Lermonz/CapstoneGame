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

    public LayerMask collMask;

    public bool _isGrounded;
    public bool _hitCeiling;
    public bool _canDoubleJump;

    BoxCollider2D _collider;
    RayCastOrigins _raycastOrigins;
    void Start() {
        _collider = GetComponent<BoxCollider2D>();
        CalcRays();
    }
    public void Move(Vector2 velocity) {
        RayCastUpdate();
        VertCollisions(ref velocity);
        HorzCollisions(ref velocity);
        transform.Translate(velocity);
    }
    void VertCollisions(ref Vector2 velocity) {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y)+boundInset;

        _vertElseCount = 0;

        for(int i = 0; i < _vertRayCount; i++) {
            Vector2 rayOrigin = directionY == -1 ? _raycastOrigins.botleft : _raycastOrigins.topleft;
            rayOrigin += Vector2.right * (_vertRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if(hit) {
                velocity.y = (hit.distance-boundInset) * directionY;
                rayLength = hit.distance;
                if(directionY == 1) {
                    _hitCeiling = true;
                }
                else {
                    _isGrounded = true;
                }
                
            }
            else {
                _vertElseCount++;
            }
        }
        if(_vertElseCount >= _vertRayCount) {
            Debug.Log("Airborne");
            _isGrounded = false;
            _hitCeiling = false;
        }
    }
    void HorzCollisions(ref Vector2 velocity) {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x)+boundInset;

        for(int i = 0; i < _vertRayCount; i++) {
            Vector2 rayOrigin = directionX == -1 ? _raycastOrigins.botleft : _raycastOrigins.botright;
            rayOrigin += Vector2.up * (_horzRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionX * rayLength, Color.red);

            if(hit) {
                velocity.x = (hit.distance-boundInset) * directionX;
                rayLength = hit.distance;
            }
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

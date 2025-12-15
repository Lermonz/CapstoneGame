using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    Vector3 _home;
    Vector3 _rayOrigin;
    public float _fallSpeed = 2;
    [SerializeField] Vector3 rayOffset;
    Vector2 _moveBy;
    public LayerMask collMask;
    bool _isGoingDown;
    bool _resetOnRespawn = false;
    float _ySize;
    void Start()
    {
        _home = this.transform.position;
        _moveBy = new Vector2(0, -_fallSpeed);
        _ySize = this.GetComponent<NineSlicedResizing>()._size.y;
    }
    void Update()
    {
        if (this.transform.position != _home && !_isGoingDown)
        {
            MoveUp();
        }
        if (GameBehaviour.Instance.PlayerIsDead)
        {
            _resetOnRespawn = true;
        }
        else if (_resetOnRespawn)
        {
            this.transform.position = _home;
            _resetOnRespawn = false;
        }
    }
    public bool MoveDown()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position+rayOffset, Vector2.down, _ySize * 0.5f, collMask);
        if (!hit)
        {
            this.transform.Translate(_moveBy * Time.deltaTime);
            return true;
        }
        return false;
    }
    void MoveUp()
    {
        this.transform.Translate(_moveBy * Time.deltaTime * -0.75f);
        if (Mathf.Sign(_home.y - this.transform.position.y) != Mathf.Sign(_fallSpeed))
        {
            this.transform.position = _home;
        }
    }
    public void SetIsGoingDown(bool goingDown)
    {
        _isGoingDown = goingDown;
    }
}

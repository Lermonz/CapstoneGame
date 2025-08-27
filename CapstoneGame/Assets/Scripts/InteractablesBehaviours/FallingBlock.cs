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
    void Start()
    {
        _home = this.transform.position;
        _moveBy = new Vector2(0, -_fallSpeed);
    }
    void Update()
    {
        if (this.transform.position != _home && !_isGoingDown)
        {
            MoveUp();
        }
        if (this.transform.position.y > _home.y)
        {
            this.transform.position = _home;
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
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position+rayOffset, Vector2.down, this.transform.localScale.y * 0.5f, collMask);
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
    }
    public void SetIsGoingDown(bool goingDown)
    {
        _isGoingDown = goingDown;
    }
}

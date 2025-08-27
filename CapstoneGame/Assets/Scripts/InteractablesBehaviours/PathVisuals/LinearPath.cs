using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearPath : MonoBehaviour
{
    public TranslateOverTimeWithCurve _path;
    [SerializeField] SpriteRenderer _renderer;
    Vector3 _size;
    void Start()
    {
        if (_path != null)
        {
            _size.x = _path._endPosition.x - _path._startPosition.x;
            _size.y = _path._endPosition.y - _path._startPosition.y;
            this.transform.position += _size*0.5f;
            _size.x = Mathf.Abs(_size.x) + 0.15f;
            _size.y = Mathf.Abs(_size.y) + 0.15f;
            _renderer.size = _size;
        }
    }
}

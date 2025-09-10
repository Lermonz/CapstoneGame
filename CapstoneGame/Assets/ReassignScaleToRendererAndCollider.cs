using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReassignScaleToRendererAndCollider : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] BoxCollider2D _collider;
    public Vector2 _size;
    void Awake()
    {
        _size = this.transform.localScale;
        _renderer.size = _size;
        _collider.size = _size;
        this.transform.localScale = Vector3.one;
    }
}

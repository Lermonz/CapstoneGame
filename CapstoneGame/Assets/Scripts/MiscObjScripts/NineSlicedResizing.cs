using UnityEngine;

public class NineSlicedResizing : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] BoxCollider2D _collider;
    [SerializeField] BoxCollider2D _overlapCollider;
    public Vector2 _size { get; private set; }
    void Start()
    {
        _size = this.transform.localScale;
        _renderer.size = _size;
        _collider.size = _size;
        if(_overlapCollider != null)
        {
            _overlapCollider.size = _size;
        }
        this.transform.localScale = Vector3.one;
    }
}

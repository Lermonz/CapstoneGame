using System.Collections;
using UnityEngine;

public class DestructableBlockBehaviour : MonoBehaviour
{
    [SerializeField] Vector2 _size = new Vector2(2, 2);
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] BoxCollider2D _collider;
    [SerializeField] SpriteRenderer _outlineRenderer;
    [SerializeField] ParticleSystem _particles;
    Vector3 _startPos;
    public bool _beenTouched;
    bool _blockIsGone;
    bool _invulnerable = false;
    public bool _checkOverlap;
    bool _canRespawn;
    ParticleSystem.ShapeModule shapeMod;
    void Awake()
    {
        _size = this.transform.localScale;
        this.transform.localScale = Vector3.one;
        _startPos = this.transform.position;
        _renderer.size = _size;
        _collider.size = _size;
        shapeMod = _particles.shape;
        shapeMod.scale = _size;
        _outlineRenderer.size = _size * 0.9f;
        var emission = _particles.emission;
        emission.rateOverTime = (_size.x * _size.y) * 8 + 3;
    }
    public void BeenTouched()
    {
        if (!_invulnerable)
        {
            _renderer.color = new Color(0.75f, 0.75f, 0.79f, 1f);
            StartCoroutine(Deactivate(0.3f));
            _beenTouched = true;
        }
    }
    void Update()
    {
        if (_beenTouched)
        {
            this.transform.position -= Vector3.up * 0.25f * Time.deltaTime;
        }
        if (_blockIsGone)
        {
            StartCoroutine(Reactivate(3));
        }
        if (_canRespawn && !_checkOverlap)
        {
            RespawnBlock();
        }
    }
    void RespawnBlock()
    {
        _canRespawn = false;
        StartCoroutine(Invulnerability(3));
        _renderer.enabled = true;
        _collider.enabled = true;
        _renderer.color = Color.white;
        shapeMod.radius = 0.75f;
        shapeMod.radiusThickness = 0.75f;
        _particles.Play();
    }
    IEnumerator Deactivate(float delay)
    {
        yield return new WaitForSeconds(delay);
        shapeMod.radius = 0.55f;
        shapeMod.radiusThickness = 0.5f;
        _particles.Play();
        _renderer.enabled = false;
        _collider.enabled = false;
        _blockIsGone = true;
    }
    IEnumerator Reactivate(float delay)
    {
        _blockIsGone = false;
        _beenTouched = false;
        this.transform.position = _startPos;
        yield return new WaitForSeconds(delay);
        _canRespawn = true;
    }
    IEnumerator Invulnerability(float delay)
    {
        _invulnerable = true;
        for (int i = 0; i < delay; i++)
        {
            yield return null;
        }
        _invulnerable = false;
    }
}

using System.Collections;
using UnityEngine;

public class DestructableBlockBehaviour : MonoBehaviour
{
    Vector2 _size = new Vector2(2, 2);
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] BoxCollider2D _collider;
    [SerializeField] SpriteRenderer _outlineRenderer;
    [SerializeField] ParticleSystem _particles;
    Vector3 _startPos;
    public bool _beenTouched;
    public float _fallSpeed = 0.2f;
    public float _delayOverStep { get; private set; }
    bool _blockIsGone;
    bool _invulnerable = false;
    public bool _checkOverlap;
    [SerializeField] bool _flippedGravity;
    bool _canRespawn;
    ParticleSystem.ShapeModule shapeMod;
    void Awake()
    {
        _size = this.transform.localScale;
        _startPos = this.transform.position;
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
        int currentStep = 0;
        int steps = 10;
        _delayOverStep = delay / steps;
        AkSoundEngine.PostEvent("Fragile_Step", gameObject);
        while (currentStep < steps)
        {
            yield return new WaitForSeconds(_delayOverStep);
            currentStep++;
            this.transform.Translate(Vector3.up * _fallSpeed * _delayOverStep * (_flippedGravity ? 1 : -1));
        }
        AkSoundEngine.PostEvent("Fragile_Break", gameObject);
        shapeMod.radius = 0.55f;
        shapeMod.radiusThickness = 0.5f;
        _particles.Play();
        _renderer.enabled = false;
        _collider.enabled = false;
        StartCoroutine(Reactivate(2.1f));
        _blockIsGone = true;
    }
    IEnumerator Reactivate(float delay)
    {
        _blockIsGone = false;
        _beenTouched = false;
        this.transform.position = _startPos;
        yield return new WaitForSeconds(delay);
        StartCoroutine(CheckToRespawn());
    }
    IEnumerator CheckToRespawn()
    {
        while (_checkOverlap)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
        RespawnBlock();
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

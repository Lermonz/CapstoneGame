using System.Collections;
using UnityEngine;

public class DestructableBlockBehaviour : MonoBehaviour
{
    [SerializeField] Vector2 _size = new Vector2(2,2);
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] BoxCollider2D _collider;
    Vector3 _startPos;
    public bool _beenTouched;
    bool _blockIsGone;
    void Awake()
    {
        _size = this.transform.localScale;
        this.transform.localScale = Vector3.one;
        _renderer = this.gameObject.GetComponent<SpriteRenderer>();
        _collider = this.gameObject.GetComponent<BoxCollider2D>();
        _startPos = this.transform.position;
    }
    void Start()
    {
        _renderer.size = _size;
        _collider.size = _size;
    }
    public void BeenTouched()
    {
        Debug.Log("Fragile left");
        this.gameObject.GetComponent<SpriteRenderer>().color *= 0.75f;
        StartCoroutine(Deactivate(0.9f));
        _beenTouched = true;
    }
    void Update()
    {
        if (_beenTouched)
        {
            this.transform.position -= Vector3.up * 0.25f * Time.deltaTime;
        }
        if (_blockIsGone)
        {
            StartCoroutine(Reactivate(4));
        }
    }
    IEnumerator Deactivate(float delay)
    {
        Debug.Log("getmeoutta here");
        yield return new WaitForSeconds(delay);
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
        _renderer.enabled = true;
        _collider.enabled = true;
        _renderer.color = new Color(0.6f, 0.63f, 0.66f, 1);
    }
}

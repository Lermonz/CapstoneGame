using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffBlockBehaviour : MonoBehaviour
{
    // public enum State
    // {
    //     On,
    //     Off
    // }
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] BoxCollider2D _collider;
    [SerializeField] bool _state;
    void Start()
    {
        ActivateState();
    }
    void ActivateState()
    {
        if (_state)
        {
            _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 1);
            _collider.enabled = true;
        }
        else
        {
            _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 0.2f);
            _collider.enabled = false;
        }
    }
    public void SwapState()
    {
        _state = !_state;
        ActivateState();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilVoid : MonoBehaviour
{
    [SerializeField] PortalBehaviour _exitPortal;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] float _speed;
    float diffX;
    float _acc = 0.1f;
    float _max;
    void Start()
    {
        _renderer.material.SetFloat("_Size", this.transform.localScale.y);
    }
    void Update()
    {
        if (!_exitPortal.HasWon)
        {
            diffX = Mathf.Abs(this.transform.position.x - GameObject.Find("Player").transform.position.x);
            if (diffX >= 9)
            {
                _max = Mathf.Pow(diffX * 0.5f - 4f, 0.7f) + 1.1f;
            }
            else if (diffX >= 1)
            {
                _max = 1.1f;
            }
            else
            {
                _max = 0.6f;
            }
            if (_speed < _max)
            {
                _speed += _acc;
            }
            else
            {
                _speed -= _acc * 0.5f;
            }
        }
        else { _speed = -2; }
        transform.Translate(Vector2.right*_speed * Time.deltaTime);
    }
}

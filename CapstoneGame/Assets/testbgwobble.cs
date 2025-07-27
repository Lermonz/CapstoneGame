using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testbgwobble : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] float _size;
    void Start()
    {
        _renderer.material.SetFloat("_Size", _size);
    }
}

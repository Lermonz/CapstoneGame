using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilVoid : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    void Start()
    {
        _renderer.material.SetFloat("_Size", this.transform.localScale.y);
    }
    void Update()
    {
        
    }
}

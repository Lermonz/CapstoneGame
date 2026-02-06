using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBehaviour : MonoBehaviour
{
    public float _speed {get; private set;}
    [SerializeField] float _strength = 3;
    [SerializeField] bool _isForward = true;
    void Start()
    {
        _speed = _isForward ? _strength : -_strength;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    [SerializeField] Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    
}

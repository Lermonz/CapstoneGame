using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfMinButton : MonoBehaviour
{
    Animator _animator;
    int _levelIDAsInt;
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _levelIDAsInt = int.Parse(this.GetComponent<LevelButtonManager>()._buttonData._levelID);
    }
    void Update()
    {
        if(ScrollRectSnap.Instance._minButtonNum == _levelIDAsInt%10-1)
            Debug.Log("MinButton is this: "+(_levelIDAsInt%10-1));
       // _animator.SetBool("MinButton", ScrollRectSnap.Instance._minButtonNum == _levelIDAsInt%10-1);
    }
}

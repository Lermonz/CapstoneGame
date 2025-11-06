using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleScreenWipeMask : MonoBehaviour
{
    [SerializeField] float _time;
    [SerializeField] float _holdTime;
    [SerializeField] GameObject _parent;
    public void ScaleDown()
    {
        this.transform.localScale = Vector3.one*7;
        StartCoroutine(ScalingAnim(Vector3.zero, true));
    }
    public void ScaleUp()
    {
        this.transform.localScale = Vector3.zero;
        StartCoroutine(ScalingAnim(Vector3.one*7, false));
    }
    IEnumerator ScalingAnim(Vector3 endScale, bool goAgain)
    {
        Vector3 startScale = this.transform.localScale;
        for (int i = 0; i < _time; i++)
        {
            this.transform.localScale = Vector3.Lerp(startScale, endScale, i / _time);
            yield return null;
        }
        this.transform.localScale = endScale;
        for (int i = 0; i < _holdTime; i++)
        {
            yield return null;
        }
        if (goAgain) { ScaleUp(); }
        else{ _parent.SetActive(false); }
    }
}

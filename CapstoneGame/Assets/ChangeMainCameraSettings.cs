using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ChangeMainCameraSettings : MonoBehaviour
{
    [SerializeField] CinemachineBrain _brain;
    public void SetEasing(float time)
    {
        _brain.m_DefaultBlend.m_Time = time;
    }
    public void DelaySetEasing(float time)
    {
        StartCoroutine(EasingDelayCoroutine(time));
    }
    IEnumerator EasingDelayCoroutine(float time)
    {
        yield return new WaitForSecondsRealtime(_brain.m_DefaultBlend.m_Time + .2f);
        SetEasing(time);
    }
}

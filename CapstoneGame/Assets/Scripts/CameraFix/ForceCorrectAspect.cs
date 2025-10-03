using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCorrectAspect : MonoBehaviour
{
    const float _targetAspect = 16.0f / 9.0f;
    Vector2 _resolution;
    void Start()
    {
        _resolution = new Vector2(Screen.width, Screen.height);
        Adjust();
    }
    void Update()
    {
        if (_resolution.x != Screen.width || _resolution.y != Screen.height)
        {
            Adjust();
            _resolution.x = Screen.width;
            _resolution.y = Screen.height;
        }
    }
    void Adjust()
    {
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / _targetAspect;
        Camera camera = this.GetComponent<Camera>();
        if (scaleHeight < 1)
        {
            Rect rect = camera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1 - scaleHeight) / 2.0f;
            camera.rect = rect;
            FixCanvasScaling.Instance.SetCanvasScalerMatch(0);
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = camera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1 - scaleWidth) / 2.0f;
            rect.y = 0;
            camera.rect = rect;
            FixCanvasScaling.Instance.SetCanvasScalerMatch(1);
        }
    }
}

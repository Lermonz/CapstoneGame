using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePath : MonoBehaviour
{
    [SerializeField] LineRenderer _circleRenderer;
    [SerializeField] int _steps;

    public void DrawCircle(Vector2 radius)
    {
        _circleRenderer.loop = true;
        _circleRenderer.positionCount = _steps;
        float angle = 0;
        for (int i = 0; i < _steps; i++)
        {
            float x = radius.x * Mathf.Cos(angle);
            float y = radius.y * Mathf.Sin(angle);
            _circleRenderer.SetPosition(i, new Vector3(x, y, 0));
            angle += 2 * Mathf.PI / _steps;
        }
    }
}

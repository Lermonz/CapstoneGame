using System;
using UnityEngine;

public class BoostObjectBehaviour : MonoBehaviour
{
    public float _force = 13;
    [SerializeField] float diagonalMult = 0.2f;
    public Vector2 BoostInDirection() {
        Vector2 v = new Vector2(_force,_force);
        float delta = this.transform.localEulerAngles.z+45;
        delta = Mathf.Deg2Rad * delta;
        float difference = Mathf.Abs(Mathf.Abs(Mathf.Cos(delta)) - Mathf.Abs(Mathf.Sin(delta)));
        float mult = 1 + difference * diagonalMult;
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        )*mult;
    }
}

using UnityEngine;

public class BoostObjectBehaviour : MonoBehaviour
{
    public float _force = 12;
    public Vector2 BoostInDirection() {
        Vector2 v = new Vector2(_force,_force);
        float delta = this.transform.localEulerAngles.z+45;
        delta = Mathf.Deg2Rad * delta;
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
}

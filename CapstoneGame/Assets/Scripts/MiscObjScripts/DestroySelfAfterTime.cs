using UnityEngine;

public class DestroySelfAfterTime : MonoBehaviour
{
    public float _time;
    void Start() {
        Destroy(this.gameObject, _time);
    }
}

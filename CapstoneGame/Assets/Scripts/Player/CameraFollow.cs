using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector2 _startPos;
    public float _speed = 1;
    float _z;
    [SerializeField] private GameObject _toFollow;
    void Awake() {
        _startPos = this.transform.position;
        _z = this.transform.position.z;
    }
    void Update(){
        this.transform.position = new Vector3(_toFollow.transform.position.x*_speed,_toFollow.transform.position.y*_speed,_z);
    }
}

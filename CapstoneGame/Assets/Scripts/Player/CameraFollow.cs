using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector2 _startPos;
    public float _speed = 1;
    float _z;
    [SerializeField] Vector2 _offset;
    [SerializeField] private GameObject _toFollow;
    void Awake() {
        _startPos = this.transform.position;
        _z = this.transform.position.z;
    }
    void Start()
    {
        if (_toFollow == null)
        {
            _toFollow = GameObject.Find("Main Camera");
        }
    }
    void Update(){
        this.transform.position = new Vector3(_toFollow.transform.position.x*_speed+_offset.x,_toFollow.transform.position.y*_speed+_offset.y,_z);
    }
}

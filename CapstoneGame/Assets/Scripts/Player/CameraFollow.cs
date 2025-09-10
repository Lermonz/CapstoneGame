using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector2 _startPos;
    public float _speed = 1;
    float _z;
    [SerializeField] Vector2 _offset;
    [SerializeField] private GameObject _toFollow;
    [SerializeField] private GameObject _toFollowY;
    void Awake()
    {
        _startPos = this.transform.position;
        _z = this.transform.position.z;
    }
    void Start()
    {
        if (_toFollow == null)
        {
            _toFollow = GameObject.Find("Main Camera");
        }
        if (_toFollowY == null)
        {
            _toFollowY = _toFollow;
        }
    }
    void Update()
    {
        if(!PauseMenu.Instance._isPaused) {
            this.transform.position = new Vector3(_toFollow.transform.position.x * _speed + _offset.x, _toFollowY.transform.position.y * _speed + _offset.y, _z);
        }
    }
}

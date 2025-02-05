using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector2 _startPos;
    float _z;
    GameObject _player;
    void Awake() {
        _player = GameObject.Find("Player");
        _startPos = this.transform.position;
        _z = this.transform.position.z;
    }
    void Update(){
        this.transform.position = new Vector3(_player.transform.position.x,0,_z);
    }
}

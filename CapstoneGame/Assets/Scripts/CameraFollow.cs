using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector2 _startPos;
    GameObject _player;
    void Awake() {
        _player = GameObject.Find("Player");
        _startPos = this.transform.position;
    }
    void Update(){
        this.transform.position = Vector2.right * _player.transform.position.x;
    }
}

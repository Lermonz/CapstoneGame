using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHoleBehaviour : MonoBehaviour
{
    SelectLevel _sceneManager;
    void Start() {
        _sceneManager = GetComponent<SelectLevel>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")) {
            InputManager.Instance.NegateAllInput();
            // LevelManager.Instance.FreezePlayerAndTimer();
            other.GetComponent<Player>().DeathBlackHole();
            this.GetComponent<CircleCollider2D>().radius = 1;
            _sceneManager.Reload();
        }
    }
    // void OnTriggerStay2D(Collider2D other) {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("PlayerTriggerStay " + other.gameObject.transform.position);
    //         other.GetComponent<Player>().PullTowards(this.transform.position, 2f);
    //         //other.gameObject.transform.position = Vector3.Lerp(other.gameObject.transform.position, this.transform.position, elapsedFrames/totalFrames);
    //         //elapsedFrames++;
    //     }
    // }
}

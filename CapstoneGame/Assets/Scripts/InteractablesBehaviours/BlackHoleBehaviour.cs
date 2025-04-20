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
    int elapsedFrames = 0;
    int totalFrames = 60;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")) {
            InputManager.Instance.NegateAllInput();
            // LevelManager.Instance.FreezePlayerAndTimer();
            other.GetComponent<Player>().DeathBlackHole();
            _sceneManager.Reload();
        }
    }
    void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")) {
            other.gameObject.transform.position = Vector3.Lerp(other.gameObject.transform.position, this.transform.position, elapsedFrames/totalFrames);
            elapsedFrames++;
        }
    }
}

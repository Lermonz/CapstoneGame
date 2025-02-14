using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehaviour : MonoBehaviour
{
    public GameObject _winMenu;
    private float _scale;
    void Start()
    {
        _scale = transform.localScale.x;
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")) {
            if(GameBehaviour.Instance._canExit) {
                ExitSuccess();
            }
            else
                ExitFail();
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        ResetTransform();
    }
    void ExitSuccess() {
        Time.timeScale = 0;
        _winMenu.SetActive(true);
    }
    void ExitFail() {
        this.transform.localScale = new Vector3(_scale*0.5f,_scale*0.5f,_scale*0.5f);
    }
    void ResetTransform() {
        this.transform.localScale = new Vector3(_scale,_scale,_scale);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehaviour : MonoBehaviour
{
    public SpriteRenderer[] _renderers;
    public Transform _transform;
    private bool dontRetrigger = false;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player") && !dontRetrigger) {
            if(LevelManager.Instance._canExit) {
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
        dontRetrigger = true;
        InputManager.Instance.DisablePlayerInput();
        LevelManager.Instance.FreezePlayerAndTimer();
        PauseMenu.Instance.OnWin();
    }
    void ExitFail() {
        foreach(SpriteRenderer i in _renderers) {
            i.color = new Color (0.5f,0.5f,0.5f,1);
        }
        _transform.localScale = Vector3.one*0.8f;
    }
    void ResetTransform() {
        foreach(SpriteRenderer i in _renderers) {
            i.color = Color.white;
        }
        _transform.localScale = Vector3.one;
    }
}

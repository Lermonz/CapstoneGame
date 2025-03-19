using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehaviour : MonoBehaviour
{
    public SpriteRenderer[] _renderers;
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
            i.color = Color.black;
        }
    }
    void ResetTransform() {
        foreach(SpriteRenderer i in _renderers) {
            i.color = Color.white;
        }
    }
}

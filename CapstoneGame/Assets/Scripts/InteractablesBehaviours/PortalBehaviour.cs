using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehaviour : MonoBehaviour
{
    public SpriteRenderer[] _renderers;
    private bool dontRetrigger = false;
    public bool HasWon { get => dontRetrigger; set {dontRetrigger = value;}}
    bool _canExit;
    void Update()
    {
        _canExit = LevelManager.Instance._canExit;
        this.GetComponent<Animator>().SetBool("canExit", _canExit);
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player") && !dontRetrigger) {
            if(_canExit) {
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
            i.color = new Color (0.6f,0.6f,0.6f,1);
        }
        this.transform.localScale *= 0.8f;
    }
    void ResetTransform() {
        foreach(SpriteRenderer i in _renderers) {
            i.color = Color.white;
        }
        this.transform.localScale = new Vector3(0.85f,0.85f,1);
    }
}

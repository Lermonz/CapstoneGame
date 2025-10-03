using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehaviour : MonoBehaviour
{
    public SpriteRenderer[] _renderers;
    Player _player;
    private bool dontRetrigger = false;
    public bool HasWon { get => dontRetrigger; set {dontRetrigger = value;}}
    bool _canExit;
    void Update()
    {
        _canExit = LevelManager.Instance._canExit;
        this.GetComponent<Animator>().SetBool("canExit", _canExit);
        if (dontRetrigger && _player != null)
        {
            float xDiff = this.transform.position.x - _player.transform.position.x;
            float yDiff = this.transform.position.y - _player.transform.position.y;
            _player.transform.position += new Vector3(xDiff, yDiff, 0)*3f*Time.deltaTime;
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player") && !dontRetrigger) {
            if(_canExit) {
                _player = other.gameObject.GetComponent<Player>();
                _player.SetInvulnerable();
                _player.SetHasWon();
                ExitSuccess();
            }
            else
                ExitFail();
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if(!dontRetrigger)
            ResetTransform();
    }
    void ExitSuccess() {
        dontRetrigger = true;
        InputManager.Instance.DisablePlayerInput();
        LevelManager.Instance.StopTimer();
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

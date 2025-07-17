using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunawayPortal : MonoBehaviour
{
    bool _runaway;
    [SerializeField] float _stoppingX;
    float diffX;
    void Update()
    {
        _runaway = LevelManager.Instance._canExit && this.transform.position.x < _stoppingX;
        if (_runaway)
        {
            diffX = this.transform.position.x - GameObject.Find("Player").transform.position.x;
            if (diffX < 1) { diffX = 1; }
            float boostX = diffX < 6 ? 18 / Mathf.Abs(diffX) : 0;
            Debug.Log("diffX: " + diffX + "\nboostX: " + boostX);
            this.transform.Translate((Vector3.right * 8f + new Vector3(boostX, 0, 0)) * Time.deltaTime);
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            _stoppingX = -999;
        }
    }
}

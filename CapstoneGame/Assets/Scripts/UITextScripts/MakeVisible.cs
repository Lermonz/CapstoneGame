using UnityEngine;
using TMPro;
using System.Collections;

public class MakeVisible : MonoBehaviour
{
    public TMP_Text _text;
    bool _appear = false;
    float _elapsedTime = 0;
    float _waitTime = 70f;
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            _appear = true;
            _elapsedTime = 0;
            _waitTime = 35f;
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            _appear = false;
            _elapsedTime = 0;
            _waitTime = 25f;
        }
    }
    void Update()
    {
        if (_elapsedTime <= _waitTime) {
            if (_appear) {
                _text.alpha = (_elapsedTime*0.75f / _waitTime);
                _elapsedTime++;
            }
            else {
                float startNum = _text.alpha;
                _text.alpha = startNum - (_elapsedTime*0.75f / _waitTime);
                _elapsedTime++;
            }
        }
    }
}

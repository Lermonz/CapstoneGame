using UnityEngine;
using TMPro;
using System.Collections;

public class MakeVisible : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] SpriteRenderer _controllerIcon;
    bool _appear = false;
    float _elapsedTime = 0;
    float _waitTime = 70f;
    Color _iconColor = Color.white;
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") && !LevelManager.Instance._canExit) {
            _appear = true;
            _elapsedTime = 0;
            _waitTime = 35f;
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            _appear = false;
            _elapsedTime = 0;
            _waitTime = 30f;
        }
    }
    void Update()
    {
        if (_elapsedTime <= _waitTime) {
            if (_appear)
            {
                _text.alpha = _elapsedTime * 0.75f / _waitTime;
                _iconColor.a = _elapsedTime * 0.75f / _waitTime;
                _elapsedTime++;
            }
            else
            {
                float startNum = _text.alpha;
                _text.alpha = startNum - (_elapsedTime * 0.75f / _waitTime);
                _iconColor.a = startNum - (_elapsedTime * 0.75f / _waitTime);
                _elapsedTime++;
            }
            if (_controllerIcon != null) { _controllerIcon.color = _iconColor; }
        }
    }
}

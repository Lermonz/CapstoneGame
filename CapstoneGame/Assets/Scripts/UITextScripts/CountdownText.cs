using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TMP_Text))]
[DisallowMultipleComponent]
public class CountdownText : MonoBehaviour
{
    TMP_Text _text;
    [SerializeField] Animator _animator;
    void Start() {
        _text = GetComponent<TextMeshProUGUI>();
        LevelManager.Instance._countdownStart += CountStart;
    }
    void CountStart()
    {
        StartCoroutine(Countdown());
    }
    void OnDestroy()
    {
        LevelManager.Instance._countdownStart -= CountStart;
    }
    IEnumerator Countdown()
    {
        _animator.SetTrigger("CountStart");
        yield return new WaitForSeconds(1f);
        InputManager.Instance.EnablePlayerInput();
        InputManager.Instance._freezeVelocity = false;
        _text.text = "Go!";
        yield return new WaitForSeconds(0.5f);
        LevelManager.Instance.StopTimer(false);
        LevelManager.Instance.CanReset();
        Destroy(this.gameObject);
    }
}

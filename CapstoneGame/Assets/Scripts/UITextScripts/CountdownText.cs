using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
[DisallowMultipleComponent]
public class CountdownText : MonoBehaviour
{
    TMP_Text _text;
    void Start() {
        _text = GetComponent<TextMeshProUGUI>();
        LevelManager.Instance._countdownFinish += CountGo;
    }
    void CountGo() {
        _text.text = "Go!";
        LevelManager.Instance._countdownFinish -= CountGo;
        Destroy(this.gameObject, 0.5f);
    }
}

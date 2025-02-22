using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
[DisallowMultipleComponent]
public class CountdownText : MonoBehaviour
{
    TMP_Text _text;
    void Start() {
        _text = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if(LevelManager.Instance._countdownDone) {
            CountGo();
        }
    }
    void CountGo() {
        _text.text = "Go!";
        Destroy(this.gameObject, 0.6f);
    }
}

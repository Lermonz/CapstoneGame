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
        if(GameBehaviour.Instance._countdownDone) {
            CountGo();
        }
    }
    void CountGo() {
        _text.text = "Go!";
        Destroy(this.gameObject, 1.2f);
    }
}

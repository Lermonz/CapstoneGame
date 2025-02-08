using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
[DisallowMultipleComponent]
public class TargetsLeft : MonoBehaviour
{
    TMP_Text _text;
    void Start() {
        _text = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        _text.text = string.Format(GameBehaviour.Instance._targetsDestroyed+"/"+GameBehaviour.Instance._targetReq+" Targets");
    }
}

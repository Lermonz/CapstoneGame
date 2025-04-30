using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class tutorialText : MonoBehaviour
{
    private TMP_Text _textText;
    public InputActionReference _action;
    void Awake() {
        _textText = this.GetComponent<TMP_Text>();
    }
    void Start()
    {
        this._textText.text = _textText.text.Replace("[", ""+_action.action.GetBindingDisplayString(0));
        // if(_action.action.GetBindingDisplayString(1) != null) {
        //     this._textText.text = _textText.text.Replace("]", ""+_action.action.GetBindingDisplayString(1));
        // }
    }
}

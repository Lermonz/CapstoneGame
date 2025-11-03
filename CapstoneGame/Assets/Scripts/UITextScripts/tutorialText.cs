using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class tutorialText : MonoBehaviour
{
    private TMP_Text _textText;
    [SerializeField] SpriteRenderer _controllerIcon;
    [SerializeField] GamepadIconsHolder _icons;
    public InputActionReference _action;
    private string _currentControlScheme;
    private string _replacementString = "[";
    private string _constantString = "Press ";
    void Awake()
    {
        _textText = this.GetComponent<TMP_Text>();
    }
    void Start()
    {
        _replacementString = _constantString + _replacementString;
        UpdateBindingDisplay();
        StartCoroutine(CheckControlScheme());
    }
    void UpdateBindingDisplay()
    {
        if (InputManager.Instance.GetControlScheme() == "Gamepad")
        {
            _controllerIcon.enabled = true;
            _action.action.GetBindingDisplayString(1, out string device, out string controlPath);
            Debug.Log(controlPath);
            _controllerIcon.sprite = _icons._icons.GetSprite(controlPath);
            this._textText.text = _textText.text.Replace("" + _replacementString, _constantString+"  ");
            _replacementString = _constantString+"  ";
        }
        else
        {
            _controllerIcon.enabled = false;
            this._textText.text = _textText.text.Replace("" + _replacementString, _constantString + _action.action.GetBindingDisplayString(0));
            _replacementString = _constantString+_action.action.GetBindingDisplayString(0);
        }
    }
    IEnumerator CheckControlScheme()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            if (_currentControlScheme != InputManager.Instance.GetControlScheme())
            {
                UpdateBindingDisplay();
            }
        }
    }
}

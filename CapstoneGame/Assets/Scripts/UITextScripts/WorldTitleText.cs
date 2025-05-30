using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldTitleText : MonoBehaviour
{
    private TMP_Text _textText;
    void Awake() {
        _textText = this.GetComponent<TMP_Text>();
    }
    void Start()
    {
        this._textText.text = _textText.text.Replace("[", ""+(MainMenuManager.Instance.CurrentWorld+1));
    }
}

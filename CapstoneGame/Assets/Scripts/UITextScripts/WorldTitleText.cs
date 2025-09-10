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
    void Update()
    {
        _textText.text = "World " + (MainMenuManager.Instance.CurrentWorld + 1);
        //Debug.Log("Current world according to Text:" + (MainMenuManager.Instance.CurrentWorld + 1));
    }
}

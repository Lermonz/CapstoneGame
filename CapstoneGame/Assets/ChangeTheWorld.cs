using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTheWorld : MonoBehaviour
{
    [SerializeField] bool _increase;
    void Update() {
        if (this.GetComponent<ButtonSelectionHandler>().IsSelected) {
            if (_increase)
            {
                MainMenuManager.Instance.OnNextWorldPress();
            }
            else
            {
                MainMenuManager.Instance.OnPrevWorldPress();
            }
        }
    }
}

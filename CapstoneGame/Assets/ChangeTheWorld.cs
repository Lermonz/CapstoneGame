using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTheWorld : MonoBehaviour
{
    [SerializeField] bool _increase;
    bool _canChangeWorld = true;
    [SerializeField] ButtonHighlighSFX _highlightSFX;
    void Update()
    {
        if (_canChangeWorld && this.GetComponent<ButtonSelectionHandler>().IsSelected)
        {
            if (_increase)
            {
                MainMenuManager.Instance.OnNextWorldPress();
            }
            else
            {
                MainMenuManager.Instance.OnPrevWorldPress();
            }
            StartCoroutine(ChangeWorldLockOut());
        }
    }
    IEnumerator ChangeWorldLockOut()
    {
        _canChangeWorld = false;
        if(_highlightSFX != null) { _highlightSFX.CanPlayHighlightSFX(false); }
        yield return new WaitForSecondsRealtime(0.5f);
        _canChangeWorld = true;
        if(_highlightSFX != null) { _highlightSFX.CanPlayHighlightSFX(true); }
    }
}

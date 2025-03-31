using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighlighSFX : MonoBehaviour, ISelectHandler
{
    bool canPlaySFX = false;
    void OnEnable()
    {
        StartCoroutine(DelayForAudio());
    }
    void OnDisable()
    {
        canPlaySFX = false;
    }
    public void OnSelect (BaseEventData eventData) 
	{
        if(canPlaySFX)
            AkSoundEngine.PostEvent("ButtonHighlight",gameObject);
    }
    IEnumerator DelayForAudio() {
        yield return null;
        yield return null;
        canPlaySFX = true;
    }
}

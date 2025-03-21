using UnityEngine;
using TMPro;
using System.Collections;

public class MakeVisible : MonoBehaviour
{
    public TMP_Text _text;
    bool prioritizeDisappear;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")) {
            StartCoroutine(TextAppear());
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player") && !prioritizeDisappear) {
            StartCoroutine(TextDisappear());
        }
    }
    IEnumerator TextAppear() {
        float elapsedTime = 0;
        float waitTime = 35f;
        while(elapsedTime <= waitTime) {
            if(prioritizeDisappear)
                elapsedTime = waitTime;
            _text.alpha = (elapsedTime / waitTime);
            elapsedTime++;
            yield return null;
        }
    }
    IEnumerator TextDisappear() {
        prioritizeDisappear = true;
        yield return null;
        float elapsedTime = 0;
        float waitTime = 25f;
        while(elapsedTime <= waitTime) {
            _text.alpha = 1-(elapsedTime / waitTime);
            elapsedTime++;
            yield return null;
        }
        prioritizeDisappear = false;
    }
}

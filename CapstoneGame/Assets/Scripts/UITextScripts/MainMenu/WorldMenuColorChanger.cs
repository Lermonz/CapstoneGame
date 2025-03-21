using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldMenuColorChanger : MonoBehaviour
{
    public bool isText;
    public SpriteRenderer _renderer;
    public TMP_Text _text;
    public WorldSelectData _data;
    public void ChangeColor(int worldFrom, int worldTo) {
        if(isText) {
            _text.color = _data._worldColors[worldTo];
        }
        else {
            StartCoroutine(ChangeColorCoroutine(worldFrom, worldTo));
        }
    }
    IEnumerator ChangeColorCoroutine(int worldFrom, int worldTo) {
        float elapsedTime = 0;
        float waitTime = 20f;
        while(elapsedTime < waitTime) {
                _renderer.color = Color.Lerp(_data._worldColors[worldFrom], _data._worldColors[worldTo], (elapsedTime / (waitTime)));
            elapsedTime++;
            yield return null;
        }
    }
}

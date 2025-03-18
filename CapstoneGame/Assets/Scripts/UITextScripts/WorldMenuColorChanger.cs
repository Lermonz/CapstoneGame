using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMenuColorChanger : MonoBehaviour
{
    public WorldSelectData _data;
    public SpriteRenderer _renderer;
    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    public void ChangeColor(int worldFrom, int worldTo) {
        StartCoroutine(ChangeColorCoroutine(worldFrom, worldTo));
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

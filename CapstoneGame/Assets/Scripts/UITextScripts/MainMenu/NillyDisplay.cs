using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NillyDisplay : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] SpriteRenderer _shadowRenderer;
    public static NillyDisplay Instance;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
            Instance = this;
    }
    public void ShowNilly(bool show)
    {
        _renderer.enabled = show;
        _shadowRenderer.enabled = show;
    }
    public void ShowCostume(int cID)
    {
        if (cID < 0)
        {
            _renderer.color = Color.black;
            return;
        }
        _renderer.color = Color.white;
        _renderer.material.SetTexture("_Palette", GameBehaviour.Instance._costumes[cID]);
    }
}

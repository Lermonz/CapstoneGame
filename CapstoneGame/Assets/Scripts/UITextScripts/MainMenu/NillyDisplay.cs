using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NillyDisplay : MonoBehaviour
{
    [SerializeField] Vector3 _nillySize;
    [SerializeField] Vector3 _nillyShadowPosition;
    [SerializeField] Vector3 _philipShadowPosition;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] SpriteRenderer _shadowRenderer;
    [SerializeField] Sprite _nillySprite;
    [SerializeField] Sprite _philipSprite;
    Material _nillyMaterial;
    int _costumeBeingShown = 99;
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
    void Start()
    {
        _nillyMaterial = _renderer.material;
    }
    public void ShowNilly(bool show)
    {
        _renderer.enabled = show;
        _shadowRenderer.enabled = show;
    }
    public void ShowCostume(int cID)
    {
        if(cID == _costumeBeingShown) { return; }
        _costumeBeingShown = cID;
        if (cID == 11)
        {
            _renderer.sprite = _philipSprite;
            _shadowRenderer.sprite = _philipSprite;
            _renderer.material = GameBehaviour.Instance._philipMaterial;
            _renderer.transform.localScale = _nillySize * 0.32f;
            _shadowRenderer.transform.localPosition = _philipShadowPosition;
        }
        else
        {
            _renderer.sprite = _nillySprite;
            _shadowRenderer.sprite = _nillySprite;
            _renderer.transform.localScale = _nillySize;
            _shadowRenderer.transform.localPosition = _nillyShadowPosition;
            _renderer.material = _nillyMaterial;
            if (cID < 0)
            {
                _renderer.color = Color.black;
                return;
            }
            _renderer.material.SetTexture("_Palette", GameBehaviour.Instance._costumes[cID]);
        }
        _renderer.color = Color.white;
    }
}

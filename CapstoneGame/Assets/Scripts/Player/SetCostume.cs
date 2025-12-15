using UnityEngine;

public class SetCostume : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
        SetPlayerCostume();
    }
    void SetPlayerCostume()
    {
        if (GameBehaviour.Instance.SelectedCostume.ToString() == "philip (UnityEngine.Texture2D)")
        {
            _animator.runtimeAnimatorController = GameBehaviour.Instance._philipController;
            _renderer.material = GameBehaviour.Instance._philipMaterial;
        }
        else
        {
            _renderer.material.SetTexture("_Palette", GameBehaviour.Instance.SelectedCostume);
        }
    }
}

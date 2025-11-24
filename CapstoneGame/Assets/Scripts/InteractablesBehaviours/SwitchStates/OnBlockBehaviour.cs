using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class OnBlockBehaviour : MonoBehaviour
{
    BoxCollider2D _collider;
    Animator _animator;
    void Start()
    {
        _collider = this.GetComponent<BoxCollider2D>();
        _animator = this.GetComponent<Animator>();
        GlobalOnOffState.Instance.OnState += ActivateBlock;
        GlobalOnOffState.Instance.OffState += DeactivateBlock;
        ActivateBlock();
    }
    void OnDestroy()
    {
        GlobalOnOffState.Instance.OnState -= ActivateBlock;
        GlobalOnOffState.Instance.OffState -= DeactivateBlock;
    }
    void ActivateBlock()
    {
        _collider.enabled = true;
        _animator.SetTrigger("On");
    }
    void DeactivateBlock()
    {
        _collider.enabled = false;
        _animator.SetTrigger("Off");
    }
}

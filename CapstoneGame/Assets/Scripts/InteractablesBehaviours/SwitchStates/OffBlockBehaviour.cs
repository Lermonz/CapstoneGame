using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class OffBlockBehaviour : MonoBehaviour
{
    BoxCollider2D _collider;
    Animator _animator;
    void Start()
    {
        _collider = this.GetComponent<BoxCollider2D>();
        _animator = this.GetComponent<Animator>();
        GlobalOnOffState.Instance.OnState += DeactivateBlock;
        GlobalOnOffState.Instance.OffState += ActivateBlock;
        ActivateBlock();
    }
    void OnDestroy()
    {
        GlobalOnOffState.Instance.OnState -= DeactivateBlock;
        GlobalOnOffState.Instance.OffState -= ActivateBlock;
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

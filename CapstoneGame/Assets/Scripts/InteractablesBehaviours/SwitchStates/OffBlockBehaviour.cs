using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class OffBlockBehaviour : MonoBehaviour
{
    BoxCollider2D _collider;
    void Start()
    {
        _collider = this.GetComponent<BoxCollider2D>();
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
        //animate turn on
    }
    void DeactivateBlock()
    {
        _collider.enabled = false;
        // animate turn off
    }
}

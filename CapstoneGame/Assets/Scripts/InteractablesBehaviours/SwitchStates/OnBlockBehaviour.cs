using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class OnBlockBehaviour : MonoBehaviour
{
    BoxCollider2D _collider;
    void Start()
    {
        _collider = this.GetComponent<BoxCollider2D>();
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
        //animate turn on
    }
    void DeactivateBlock()
    {
        _collider.enabled = false;
        // animate turn off
    }
}

using UnityEngine;

public class DestructableBlockBehaviour : MonoBehaviour
{
    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Fragile left");     
        Destroy(this.gameObject, 0.2f);
    }
}

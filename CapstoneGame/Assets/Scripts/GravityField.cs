using UnityEngine;

public class GravityField : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")) {
            
        }
    }
}

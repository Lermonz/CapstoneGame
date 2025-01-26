using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("got collision");
        if(other.gameObject.CompareTag("Hitbox")) {
            Debug.Log("got hit");
            GotHit();
        }
    }
    public void GotHit() {
        this.gameObject.transform.localScale -= Vector3.up;
        Destroy(gameObject, 0.5f);
    }
}

using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Hitbox")) {
            GotHit();
        }
    }
    public void GotHit() {
        this.gameObject.transform.localScale -= Vector3.up;
        LevelManager.Instance.HitTarget();
        Destroy(gameObject, 0.5f);
    }
}

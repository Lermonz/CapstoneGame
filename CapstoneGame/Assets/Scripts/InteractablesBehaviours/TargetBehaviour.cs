using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class TargetBehaviour : MonoBehaviour
{
    public Vector2 _checkpointOffset;
    [SerializeField] Animator _animator;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Hitbox")) {
            GotHit();
        }
    }
    public void GotHit() {
        this.GetComponent<PolygonCollider2D>().enabled = false;
        //this.GetComponent<AudioSource>().PlayOneShot(this.GetComponent<AudioSource>().clip);
        AkSoundEngine.PostEvent("Crystal_Break", gameObject);
        StartCoroutine(BreakApartAnim());
        LevelManager.Instance.HitTarget(this.transform.position, _checkpointOffset);
    }
    IEnumerator BreakApartAnim() {
        _animator.SetTrigger("Destroyed");
        Destroy(gameObject, 1f);
        InputManager.Instance._freezeVelocity = true;
        for (int i = 0; i < 2; i++)
        {
            yield return null;
        }
        InputManager.Instance._freezeVelocity = false;
        for (int i = 0; i < 2; i++)
        {
            yield return null;
        }
        //this.transform.localEulerAngles = new Vector3(90, 0, 0);
        this.GetComponent<ParticleSystem>().Play();
        
    }
}

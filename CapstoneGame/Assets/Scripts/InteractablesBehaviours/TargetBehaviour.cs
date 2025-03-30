using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class TargetBehaviour : MonoBehaviour
{
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
        LevelManager.Instance.HitTarget();
    }
    IEnumerator BreakApartAnim() {
        float realX = this.transform.position.x;
        for(int i = 0; i < 3; i++) {
            //this.transform.position = new Vector3(realX+Mathf.Sin(Time.deltaTime*100)*0.5f,this.transform.position.y,this.transform.position.z);
            yield return null;
        }
        this.transform.localEulerAngles = new Vector3(90, 0, 0);
        this.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject, 1f);
    }
}

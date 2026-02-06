using System.Collections;
using UnityEngine;

public class LinearTranslateWithLifetime : MonoBehaviour
{
    public float speed = 2;
    public float direction = 0;
    public float lifetime = 5;
    Vector2 force;
    public ParticleSystem dustBurst;
    void Start()
    {
        float delta = Mathf.Deg2Rad*direction;
        force = speed * new Vector2(-Mathf.Cos(delta),-Mathf.Sin(delta));
        StartCoroutine(MovingTime());
    }
    void Update()
    {
        transform.Translate(force * Time.deltaTime);
    }
    IEnumerator MovingTime()
    {
        yield return new WaitForSeconds(lifetime);
        Instantiate(dustBurst, this.transform.position, this.transform.rotation);
        DestroySprite();
        Destroy(this.gameObject,3f);
    }
    void DestroySprite()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

}

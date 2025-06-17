using UnityEngine;

public class DestructableBlockBehaviour : MonoBehaviour
{
    public bool _beenTouched;
    public void BeenTouched()
    {
        _beenTouched = true;
        Debug.Log("Fragile left");
        this.gameObject.GetComponent<SpriteRenderer>().color *= 0.75f;
        Destroy(this.gameObject, 0.9f);
    }
    void Update()
    {
        if (_beenTouched)
        {
            this.transform.position -= Vector3.up * 0.25f * Time.deltaTime;
        }
    }
}

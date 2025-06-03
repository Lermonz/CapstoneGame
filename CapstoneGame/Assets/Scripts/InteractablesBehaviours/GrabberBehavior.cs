using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberBehavior : MonoBehaviour
{
    bool _attach;
    Vector3 _home;
    public void Attach()
    {
        _attach = true;
    }
    public void Detach(Vector2 velocity)
    {
        _attach = false;
        StartCoroutine(FlyOffAndReset(velocity));
    }
    void Start()
    {
        _home = this.transform.position;
    }
    void Update()
    {
        if (_attach)
        {
            this.transform.position = GameObject.Find("Player").transform.position + Vector3.up * 0.9f;
        }
    }
    IEnumerator FlyOffAndReset(Vector2 velocity)
    {
        for (int i = 0; i < 120; i++)
        {
            this.transform.Translate(velocity * Time.deltaTime);
            yield return null;
        }
        this.transform.position = _home;
    }
}

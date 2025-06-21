using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapFix : MonoBehaviour
{
    [SerializeField] GameObject escapeRight;
    [SerializeField] GameObject escapeLeft;
    [SerializeField] BoxCollider2D _parent;
    void Start()
    {
        this.transform.localScale = new Vector3(_parent.size.x, _parent.size.y,1);
        escapeRight.transform.localPosition = new Vector3((_parent.size.x + 1) / _parent.size.x, 0, 0);
        escapeLeft.transform.localPosition = new Vector3(-(_parent.size.x + 1) / _parent.size.x, 0, 0);
        escapeRight.GetComponent<BoxCollider2D>().enabled = false;
        escapeLeft.GetComponent<BoxCollider2D>().enabled = false;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("overlapfix trigger enter");
        if (collision.CompareTag("Player"))
        {
            escapeRight.GetComponent<BoxCollider2D>().enabled = true;
            escapeLeft.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("overlapfix trigger exit");
        if (collision.CompareTag("Player"))
        {
            escapeRight.GetComponent<BoxCollider2D>().enabled = false;
            escapeLeft.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    
}

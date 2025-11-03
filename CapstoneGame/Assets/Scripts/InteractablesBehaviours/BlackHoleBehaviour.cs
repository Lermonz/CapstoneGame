using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHoleBehaviour : MonoBehaviour
{
    SelectLevel _sceneManager;
    public float _delay;
    [SerializeField] bool _reset;

    void Start()
    {
        _sceneManager = GetComponent<SelectLevel>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InputManager.Instance.NegateAllInput();
            bool reset = (LevelManager.Instance.TargetsDestroyed == 0 || GameBehaviour.Instance.NoCheckpoints) ? true : _reset;
            LevelManager.Instance.StopTimer();
            other.GetComponent<Player>().DeathBlackHole(_delay, reset);
            this.GetComponent<CircleCollider2D>().radius = 1;
            if (reset) { _sceneManager.Reload(); }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.GetComponent<CircleCollider2D>().radius = 0.05f;
        }
    }
    // void OnTriggerStay2D(Collider2D other) {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("PlayerTriggerStay " + other.gameObject.transform.position);
    //         other.GetComponent<Player>().PullTowards(this.transform.position, 2f);
    //         //other.gameObject.transform.position = Vector3.Lerp(other.gameObject.transform.position, this.transform.position, elapsedFrames/totalFrames);
    //         //elapsedFrames++;
    //     }
    // }
}

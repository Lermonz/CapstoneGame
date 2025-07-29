using UnityEngine;

[RequireComponent (typeof(SelectLevel))]
public class DeathBoxBehaviour : MonoBehaviour
{
    SelectLevel _sceneManager;
    public float _delay = 0.5f;
    [SerializeField] bool _reset;
    void Start() {
        _sceneManager = GetComponent<SelectLevel>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().DeathNormal(_delay,_reset);
            if(_reset) {_sceneManager.Reload(); }
            //trigger death screen wipe animation
        }
    }
}

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
            bool reset = (LevelManager.Instance.TargetsDestroyed == 0 || GameBehaviour.Instance.NoCheckpoints) ? true : _reset;
            other.GetComponent<Player>().DeathNormal(_delay,reset);
            if(reset) {_sceneManager.Reload(); }
            //trigger death screen wipe animation
        }
    }
}

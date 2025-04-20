using UnityEngine;

[RequireComponent (typeof(SelectLevel))]
public class DeathBoxBehaviour : MonoBehaviour
{
    SelectLevel _sceneManager;
    void Start() {
        _sceneManager = GetComponent<SelectLevel>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            other.GetComponent<Player>().DeathNormal();
            _sceneManager.Reload();
        }
    }
}

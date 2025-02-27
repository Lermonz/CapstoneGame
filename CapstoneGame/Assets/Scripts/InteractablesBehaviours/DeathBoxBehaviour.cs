using UnityEngine;

[RequireComponent (typeof(SelectLevel))]
public class DeathBoxBehaviour : MonoBehaviour
{
    SelectLevel _sceneManager;
    void Start() {
        _sceneManager = GetComponent<SelectLevel>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("player died");
        if(other.CompareTag("Player")) {
            Debug.Log("for real");
            _sceneManager.Reload();
        }
    }
}

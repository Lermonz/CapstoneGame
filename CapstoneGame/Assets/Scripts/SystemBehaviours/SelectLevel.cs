using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    public void LoadLevel (int level) {
        SceneManager.LoadScene(level);
    }
    public void Reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

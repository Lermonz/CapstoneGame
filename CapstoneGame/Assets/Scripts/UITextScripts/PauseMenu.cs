using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool _isPaused = false;
    public GameObject _pauseMenu;
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Pause();
        }
    }
    public void Pause() {
        _isPaused = !_isPaused;
        _pauseMenu.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0 : 1;
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool _isPaused = false;
    public GameObject _pauseMenu;
    public GameObject _settingsMenu;

    [SerializeField] private GameObject _mainFirstButton;
    [SerializeField] private GameObject _settingsFirstButton;

    private void Update() {
        if(InputManager.Instance.MenuOpenCloseInput) {
            if(!_isPaused)
                Pause();
            else
                Unpause();
        }
    }
    public void Pause() {
        _isPaused = true;
        OpenMainPauseMenu();
        Time.timeScale = 0;
    }
    public void Unpause() {
        _isPaused = false;
        CloseMenus();
        Time.timeScale = 1;
    }
    private void CloseMenus() {
        _pauseMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
    private void OpenMainPauseMenu() {
        _pauseMenu.SetActive(true);
        _settingsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_mainFirstButton);
    }
    private void OpenSettingsMenu() {
        _pauseMenu.SetActive(false);
        _settingsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_settingsFirstButton);
    }
    public void OnSettingsPress() {
        OpenSettingsMenu();
    }
    public void OnBackPress() {
        OpenMainPauseMenu();
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool _isPaused = false;
    [Header ("Panels")]
    public GameObject _pauseMenu;
    public GameObject _settingsMenu;
    public GameObject _controlsMenu;

    [Header ("Event System wants a first selected button")]
    [SerializeField] private GameObject _mainFirstButton;
    [SerializeField] private GameObject _settingsFirstButton;
    [SerializeField] private GameObject _controlsFirstButton;

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
        _controlsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
    private void OpenMainPauseMenu() {
        CloseMenus();
        _pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_mainFirstButton);
    }
    private void OpenSettingsMenu() {
        CloseMenus();
        _settingsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_settingsFirstButton);
    }
    private void OpenControlsMenu() {
        CloseMenus();
        _controlsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_controlsFirstButton);
    }
    public void OnSettingsPress() {
        OpenSettingsMenu();
    }
    public void OnControlsPress() {
        OpenControlsMenu();
    }
    public void OnBackPress() {
        OpenMainPauseMenu();
    }
}

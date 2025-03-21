using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public bool _isPaused = false;
    public static PauseMenu Instance;
    [Header ("Panels")]
    public GameObject _pauseMenu;
    public GameObject _settingsMenu;
    public GameObject _controlsMenu;
    public GameObject _winMenu;

    [Header ("Event System wants a first selected button")]
    [SerializeField] private GameObject _mainFirstButton;
    [SerializeField] private GameObject _settingsFirstButton;
    [SerializeField] private GameObject _controlsFirstButton;
    [SerializeField] private GameObject _winFirstButton;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        _isPaused = false;
    }
    private void Update() {
        if(InputManager.Instance.MenuOpenInput) {
            if(!_isPaused)
                Pause();
        }
        if(InputManager.Instance.UIMenuCloseInput) {
            if(_isPaused)
                Unpause();
        }
        /*
        if(EventSystem.current.currentSelectedGameObject == null) {
            EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
        }*/
    }
    public void Pause() {
        _isPaused = true;
        OpenMainPauseMenu();
        FindAudioPlayerForButtons(6);
        InputManager.Instance.DisablePlayerInput();
        Time.timeScale = 0;
    }
    public void Unpause() {
        _isPaused = false;
        FindAudioPlayerForButtons(5);
        CloseMenus();
        InputManager.Instance.EnablePlayerInput();
        Time.timeScale = 1;
    }
    private void CloseMenus() {
        _pauseMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _controlsMenu.SetActive(false);
        _winMenu.SetActive(false);
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
    private void OpenWinMenu() {
        CloseMenus();
        _winMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_winFirstButton);
    }
    public void OnSettingsPress() {
        OpenSettingsMenu();
    }
    public void OnControlsPress() {
        OpenControlsMenu();
    }
    public void OnWin() {
        OpenWinMenu();
    }
    public void OnBackPress() {
        OpenMainPauseMenu();
    }
    public void FindAudioPlayerForButtons(int fileNum) {
        AudioSelecterButtons.Instance.PlaySFX(fileNum);
    }
}

using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public bool _isPaused = false;
    public static PauseMenu Instance;
    public CinemachineVirtualCamera _vcam;

    [Header("Panels")]
    public GameObject _pauseMenu;
    public GameObject _settingsMenu;
    public GameObject _controlsMenu;
    public GameObject _winMenu;
    public ScaleScreenWipeMask _screenWipe;

    [Header("Event System wants a first selected button")]
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
    private void Update()
    {
        if (InputManager.Instance.MenuOpenInput)
        {
            if (!_isPaused)
                Pause();
            else if (_isPaused)
            {
                Debug.Log("Unpaused via MenuOpenInput");
                Unpause();
            }

        }
        if (InputManager.Instance.UIMenuCloseInput)
        {
            if (_isPaused)
            {
                Debug.Log("Unpaused via UIMenuCloseInput");
                Unpause();
            }
        }
        /*
        if(EventSystem.current.currentSelectedGameObject == null) {
            EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
        }*/
    }
    public void Pause()
    {
        MoveCameraOffset();
        _isPaused = true;
        OpenMainPauseMenu();
        FindAudioPlayerForButtons(6);
        DataPersistenceManager.Instance.LoadGame();
        InputManager.Instance.DisablePlayerInput();
        LevelManager.Instance.FreezePlayerAndTimer();
        //Time.timeScale = 0;
    }
    public void Unpause()
    {
        ResetCameraOffset();
        _isPaused = false;
        FindAudioPlayerForButtons(5); // change back to 5?
        CloseMenus();
        InputManager.Instance.EnablePlayerInput();
        LevelManager.Instance.FreezePlayerAndTimer(false);
        //Time.timeScale = 1;
    }
    private void CloseMenus()
    {
        _pauseMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _controlsMenu.SetActive(false);
        _winMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
    private void OpenMainPauseMenu()
    {
        CloseMenus();
        _pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_mainFirstButton);
    }
    private void OpenSettingsMenu()
    {
        CloseMenus();
        _settingsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_settingsFirstButton);
    }
    private void OpenControlsMenu()
    {
        CloseMenus();
        _controlsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_controlsFirstButton);
    }
    private void OpenWinMenu()
    {
        CloseMenus();
        _winMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_winFirstButton);
    }
    public void OnSettingsPress()
    {
        OpenSettingsMenu();
    }
    public void OnControlsPress()
    {
        OpenControlsMenu();
    }
    public void OnWin()
    {
        OpenWinMenu();
    }
    public void OnBackPress()
    {
        OpenMainPauseMenu();
    }
    public void OnPlayerDeath()
    {
        _screenWipe.ScaleDown();
    }
    public void FindAudioPlayerForButtons(int fileNum)
    {
        //AudioSelecterButtons.Instance.PlaySFX(fileNum);
    }
    void MoveCameraOffset()
    {
        var transposer = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_TrackedObjectOffset.x = 6;
    }
    void ResetCameraOffset()
    {
        _vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.x = 0;
    }
}

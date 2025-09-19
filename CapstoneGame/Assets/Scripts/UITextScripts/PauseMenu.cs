using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public bool _isPaused = false;
    public bool _isPausedPhysics = false;
    public static PauseMenu Instance;
    public CinemachineVirtualCamera _vcam;
    public CinemachineVirtualCamera _vcamPaused;

    [Header("Panels")]
    public GameObject _pauseMenu;
    public GameObject _settingsMenu;
    public GameObject _controlsMenu;
    public GameObject _winMenu;
    public GameObject _collectedOrb;
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
    void Start()
    {
        _vcam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        _vcamPaused = GameObject.Find("Virtual Camera Paused").GetComponent<CinemachineVirtualCamera>();
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
        _isPausedPhysics = true;
        OpenMainPauseMenu();
        FindAudioPlayerForButtons(6);
        DataPersistenceManager.Instance.LoadGame();
        InputManager.Instance.DisablePlayerInput();
        //LevelManager.Instance.FreezePlayerAndTimer();
        Time.timeScale = 0;
    }
    public void Unpause()
    {
        ResetCameraOffset();
        FindAudioPlayerForButtons(5);
        CloseMenus();
        InputManager.Instance.EnablePlayerInput();
        LevelManager.Instance.FreezePlayerAndTimer(false);
        Time.timeScale = 1;
        _isPaused = false;
        StartCoroutine(MaintainPrePausedState());
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
    public void OnResetLevel()
    {
        _screenWipe.ScaleDown();
    }
    public void CollectedOrb()
    {
        StartCoroutine(CollectedOrbTextAnimation());
    }
    public void FindAudioPlayerForButtons(int fileNum)
    {
        //AudioSelecterButtons.Instance.PlaySFX(fileNum);
    }
    void MoveCameraOffset()
    {
        _vcam.enabled = false;
        _vcamPaused.enabled = true;
        // var transposer = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        // transposer.m_TrackedObjectOffset.x = 6;
    }
    void ResetCameraOffset()
    {
        _vcamPaused.enabled = false;
        _vcam.enabled = true;
        //_vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.x = 0;
    }
    IEnumerator MaintainPrePausedState()
    {
        yield return null;
        _isPausedPhysics = false;
    }
    IEnumerator CollectedOrbTextAnimation()
    {
        _collectedOrb.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(0.15f);
        _collectedOrb.SetActive(true);
        for (int i = 0; i < 6; i++)
        {
            _collectedOrb.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, i / 6f);
            yield return null;
        }
        _collectedOrb.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(4);
        for (int i = 0; i < 6; i++)
        {
            _collectedOrb.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i / 6f);
            yield return null;
        }
        _collectedOrb.SetActive(false);
    }
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject _mainMenu;
    public GameObject _costumesMenu;
    public GameObject _settingsMenu;
    public GameObject _controlsMenu;
    public GameObject _controlsKeyboard;
    public GameObject _controlsGamepad;
    public GameObject _levelsMenu;
    public GameObject _resetDataMenu;
    public GameObject _exitGameMenu;
    [SerializeField] ScaleScreenWipeMask _screenWipe;
    [SerializeField] GameObject _screenWipeParent;
    [SerializeField] AudioSelecterButtons _buttonAudio;

    [Header("Event System wants a first selected button")]
    [SerializeField] private GameObject _mainFirstButton;
    [SerializeField] private GameObject _costumesFirstButton;
    [SerializeField] private GameObject _settingsFirstButton;
    [SerializeField] private GameObject _controlsKeyboardFirstButton;
    [SerializeField] private GameObject _controlsGamepadFirstButton;
    [SerializeField] private GameObject _levelsFirstButton;
    [SerializeField] private GameObject _resetDataFirstButton;
    [SerializeField] private GameObject _exitGameFirstButton;

    public WorldMenuColorChanger[] _colorChangers;
    public int CurrentWorld { get; private set; }
    int[] _lastSelectedLevelButton;
    int _lastSelectedCostumeButton;
    public MenuScreens _currentScreen { get; private set; }
    bool _noCancelling = false;

    public static MainMenuManager Instance;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
            Instance = this;
    }
    private void Start()
    {
        DataPersistenceManager.Instance.SetLevelTimes();
        InputManager.Instance.DisablePlayerInput();
        OpenKeyboardControls();
        OpenMainMenu();
        Time.timeScale = 1f;
        CurrentWorld = 0;
        _lastSelectedLevelButton = new int[] { 0, 0, 0 };
    }
    void Update()
    {
        if (InputManager.Instance.UICancel && !_noCancelling)
        {
            OnCancelInput();
        }
        // if(_levelsMenu.GetComponent<Canvas>().enabled) {
        //     if(EventSystem.current.currentSelectedGameObject == null) {
        //         EventSystem.current.SetSelectedGameObject(_levelsFirstButton);
        //     }
        // }
    }

    private void CloseMenus()
    {
        StartCoroutine(CantCancelTooFast());
        _mainMenu.SetActive(false);
        _costumesMenu.SetActive(false);
        NillyDisplay.Instance.ShowNilly(false);
        _settingsMenu.SetActive(false);
        _controlsMenu.SetActive(false);
        _resetDataMenu.SetActive(false);
        _exitGameMenu.SetActive(false);
        _levelsMenu.SetActive(false);
        //_levelsMenu.GetComponent<Canvas>().enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
    }
    private void OpenMainMenu()
    {
        CloseMenus();
        _mainMenu.SetActive(true);
        _currentScreen = MenuScreens.Title;
        EventSystem.current.SetSelectedGameObject(_mainFirstButton);
    }
    private void OpenLevelsMenu()
    {
        CloseMenus();
        //_levelsMenu.SetActive(true);
        _levelsMenu.SetActive(true);
        _currentScreen = MenuScreens.LevelSelect;
        DataPersistenceManager.Instance.LoadGame();
        SetLastSelectedButton();
        SetFirstLevelsButton();
        EventSystem.current.SetSelectedGameObject(_levelsFirstButton);
    }
    private void OpenCostumesMenu()
    {
        CloseMenus();
        _costumesMenu.SetActive(true);
        _currentScreen = MenuScreens.Costumes;
        NillyDisplay.Instance.ShowNilly(true);
        DataPersistenceManager.Instance.LoadGame();
        SetFirstCostumesButton();
        EventSystem.current.SetSelectedGameObject(_costumesFirstButton);
    }
    private void OpenSettingsMenu()
    {
        CloseMenus();
        _settingsMenu.SetActive(true);
        _currentScreen = MenuScreens.Settings;
        EventSystem.current.SetSelectedGameObject(_settingsFirstButton);
    }
    private void OpenControlsMenu()
    {
        CloseMenus();
        _controlsMenu.SetActive(true);
        OpenKeyboardControls();
        _currentScreen = MenuScreens.Controls;
        EventSystem.current.SetSelectedGameObject(_controlsKeyboardFirstButton);
    }
    private void OpenResetDataMenu()
    {
        StartCoroutine(CantCancelTooFast());
        _resetDataMenu.SetActive(true);
        _currentScreen = MenuScreens.ResetDataCheck;
        EventSystem.current.SetSelectedGameObject(_resetDataFirstButton);
    }
    private void OpenExitGameMenu()
    {
        StartCoroutine(CantCancelTooFast());
        _exitGameMenu.SetActive(true);
        _currentScreen = MenuScreens.QuitCheck;
        EventSystem.current.SetSelectedGameObject(_exitGameFirstButton);
    }
    private void OpenKeyboardControls()
    {
        _controlsGamepad.SetActive(false);
        _controlsKeyboard.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_controlsKeyboardFirstButton);
    }
    private void OpenGamepadControls()
    {
        StartCoroutine(CantCancelTooFast());
        _controlsKeyboard.SetActive(false);
        _controlsGamepad.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_controlsGamepadFirstButton);
    }
    private void IncrementWorld()
    {
        SetLastSelectedButton();
        if (CurrentWorld < ScrollRectSnap.Instance._panelForLevels.Length - 1)
        {
            foreach (WorldMenuColorChanger i in _colorChangers)
            {
                i.ChangeColor(CurrentWorld, CurrentWorld + 1);
            }
            CurrentWorld++;
            ScrollRectSnap.Instance.SnapToWorld(CurrentWorld);
        }
        SetFirstLevelsButton();
        //DebugIt();
        EventSystem.current.SetSelectedGameObject(_levelsFirstButton);
    }
    private void DecrementWorld()
    {
        SetLastSelectedButton();
        if (CurrentWorld > 0)
        {
            foreach (WorldMenuColorChanger i in _colorChangers)
            {
                i.ChangeColor(CurrentWorld, CurrentWorld - 1);
            }
            CurrentWorld--;
            ScrollRectSnap.Instance.SnapToWorld(CurrentWorld);
        }
        SetFirstLevelsButton();
        //DebugIt();
        EventSystem.current.SetSelectedGameObject(_levelsFirstButton);
    }
    void DebugIt()
    {
        Debug.Log("Actual World:" + CurrentWorld);
    }
    void SetFirstLevelsButton()
    {
        _levelsFirstButton = ScrollRectSnap.Instance._buttonsArray[CurrentWorld][_lastSelectedLevelButton[CurrentWorld]].gameObject;
    }
    void SetLastSelectedButton()
    {
        _lastSelectedLevelButton[CurrentWorld] = ScrollRectSnap.Instance._minButtonNum;
    }
    void SetFirstCostumesButton()
    {
        _costumesFirstButton = ScrollRectSnapCostumes.Instance._buttonsArray[GameBehaviour.Instance._costumeID].gameObject;
    }
    public void OnCostumesPress()
    {
        OpenCostumesMenu();
    }
    public void OnSettingsPress()
    {
        OpenSettingsMenu();
    }
    public void OnControlsPress()
    {
        OpenControlsMenu();
    }
    public void OnLevelsPress()
    {
        OpenLevelsMenu();
    }
    public void OnResetDataPress()
    {
        OpenResetDataMenu();
    }
    public void OnQuitPress()
    {
        OpenExitGameMenu();
    }
    public void OnBackPress()
    {
        OpenMainMenu();
    }
    public void OnNextWorldPress()
    {
        IncrementWorld();
    }
    public void OnPrevWorldPress()
    {
        DecrementWorld();
    }
    public void OnKeyboardControlsPress()
    {
        OpenKeyboardControls();
    }
    public void OnGamepadControlsPress()
    {
        OpenGamepadControls();
    }
    public void OnRebindButtonPress()
    {
        InputManager.Instance.NegateAllInput();
        Debug.Log("RebindButtonPressed");
    }
    public void FindAudioPlayerForButtons(int fileNum)
    {
        _buttonAudio.PlaySFX(fileNum);
    }
    public void OnLoadNewLevel()
    {
        _screenWipeParent.SetActive(true);
        _screenWipe.ScaleDown();
    }
    // METHODS THAT REDIRECT TO SCRIPTS THAT PERSIST BETWEEN SCENES (dont destroy on loads)
    // so that buttons on the menu can access them
    public void OnQuit()
    {
        GameBehaviour.Instance.ExitGame();
    }
    public void OnResetData()
    {
        GameBehaviour.Instance.OnResetProgressPress();
        OpenSettingsMenu();
    }
    public void OnCostumeChangeButton(int cID)
    {
        GameBehaviour.Instance.SetCostume(cID);
    }
    public void OnCancelInput()
    {
        int audioNumber = 4;
        SaveGame();
        switch (_currentScreen)
        {
            case MenuScreens.Title:
                audioNumber = 7;
                OnQuitPress();
                break;
            case MenuScreens.Costumes:
                OnBackPress();
                break;
            case MenuScreens.Settings:
                OnBackPress();
                break;
            case MenuScreens.Controls:
                OnSettingsPress();
                break;
            case MenuScreens.LevelSelect:
                OnBackPress();
                break;
            case MenuScreens.ResetDataCheck:
                OnSettingsPress();
                break;
            case MenuScreens.QuitCheck:
                OnBackPress();
                break;
        }
        FindAudioPlayerForButtons(audioNumber);
    }
    public void SetNoCancelling(bool x)
    {
        _noCancelling = x;
    }
    public void SetFullscreen(bool isFullscreen, Vector2 windowedResolution)
    {
        if (isFullscreen)
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        }
        else
        {
            if(windowedResolution.x < 480) { windowedResolution.x = 480; }
            if(windowedResolution.y < 270) { windowedResolution.y = 270; }
            Screen.SetResolution((int)windowedResolution.x, (int)windowedResolution.y, false);
        }
        SaveGame();
    }
    public void SaveGame()
    {
        DataPersistenceManager.Instance.SaveGame();
    }
    public void LoadGame()
    {
        DataPersistenceManager.Instance.LoadGame();
    }
    IEnumerator CantCancelTooFast()
    {
        SetNoCancelling(true);
        yield return new WaitForSecondsRealtime(0.2f);
        SetNoCancelling(false);
    }
}

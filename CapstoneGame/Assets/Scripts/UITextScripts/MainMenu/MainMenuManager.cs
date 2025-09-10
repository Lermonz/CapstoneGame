using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject _mainMenu;
    public GameObject _costumesMenu;
    public GameObject _settingsMenu;
    public GameObject _controlsMenu;
    public GameObject _levelsMenu;
    public GameObject _resetDataMenu;

    [Header("Event System wants a first selected button")]
    [SerializeField] private GameObject _mainFirstButton;
    [SerializeField] private GameObject _costumesFirstButton;
    [SerializeField] private GameObject _settingsFirstButton;
    [SerializeField] private GameObject _controlsFirstButton;
    [SerializeField] private GameObject _levelsFirstButton;
    [SerializeField] private GameObject _resetDataFirstButton;

    public WorldMenuColorChanger[] _colorChangers;
    public int CurrentWorld { get; private set; }
    int[] _lastSelectedLevelButton;

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
        OpenMainMenu();
        Time.timeScale = 1f;
        CurrentWorld = 0;
        _lastSelectedLevelButton = new int[] { 0, 0, 0 };
    }
    // void Update()
    // {
    //     if(_levelsMenu.GetComponent<Canvas>().enabled) {
    //         if(EventSystem.current.currentSelectedGameObject == null) {
    //             EventSystem.current.SetSelectedGameObject(_levelsFirstButton);
    //         }
    //     }
    // }

    private void CloseMenus()
    {
        _mainMenu.SetActive(false);
        _costumesMenu.SetActive(false);
        NillyDisplay.Instance.ShowNilly(false);
        _settingsMenu.SetActive(false);
        _controlsMenu.SetActive(false);
        _resetDataMenu.SetActive(false);
        _levelsMenu.SetActive(false);
        //_levelsMenu.GetComponent<Canvas>().enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
    }
    private void OpenMainMenu()
    {
        CloseMenus();
        _mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_mainFirstButton);
    }
    private void OpenLevelsMenu()
    {
        CloseMenus();
        //_levelsMenu.SetActive(true);
        _levelsMenu.SetActive(true);
        DataPersistenceManager.Instance.LoadGame();
        EventSystem.current.SetSelectedGameObject(_levelsFirstButton);
    }
    private void OpenCostumesMenu()
    {
        CloseMenus();
        _costumesMenu.SetActive(true);
        NillyDisplay.Instance.ShowNilly(true);
        DataPersistenceManager.Instance.LoadGame();
        EventSystem.current.SetSelectedGameObject(_costumesFirstButton);
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
    private void OpenResetDataMenu()
    {
        _resetDataMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_resetDataFirstButton);
    }
    private void IncrementWorld()
    {
        _lastSelectedLevelButton[CurrentWorld] = ScrollRectSnap.Instance._minButtonNum;
        foreach (WorldMenuColorChanger i in _colorChangers)
        {
            i.ChangeColor(CurrentWorld, CurrentWorld + 1);
        }
        CurrentWorld++;
        DebugIt();
        EventSystem.current.SetSelectedGameObject(ScrollRectSnap.Instance._buttonsArray[CurrentWorld][_lastSelectedLevelButton[CurrentWorld]].gameObject);
    }
    private void DecrementWorld()
    {
        _lastSelectedLevelButton[CurrentWorld] = ScrollRectSnap.Instance._minButtonNum;
        foreach (WorldMenuColorChanger i in _colorChangers)
        {
            i.ChangeColor(CurrentWorld, CurrentWorld - 1);
        }
        CurrentWorld--;
        DebugIt();
        EventSystem.current.SetSelectedGameObject(ScrollRectSnap.Instance._buttonsArray[CurrentWorld][_lastSelectedLevelButton[CurrentWorld]].gameObject);
    }
    void DebugIt()
    {
        Debug.Log("Actual World:" + CurrentWorld);
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
    public void FindAudioPlayerForButtons(int fileNum)
    {
        AudioSelecterButtons.Instance.PlaySFX(fileNum);
    }
    // METHODS THAT REDIRECT TO SCRIPTS THAT PERSIST BETWEEN SCENES (dont destroy on loads)
    // so that buttons on the menu can access them
    public void OnQuitButton()
    {
        GameBehaviour.Instance.ExitGame();
    }
    public void OnCostumeChangeButton(int cID)
    {
        GameBehaviour.Instance.SetCostume(cID);
    }
    public void SaveGame()
    {
        DataPersistenceManager.Instance.SaveGame();
    }
    public void LoadGame()
    {
        DataPersistenceManager.Instance.LoadGame();
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [Header ("Panels")]
    public GameObject _mainMenu;
    public GameObject _settingsMenu;
    public GameObject _controlsMenu;
    public GameObject _levelsMenu;

    [Header ("Event System wants a first selected button")]
    [SerializeField] private GameObject _mainFirstButton;
    [SerializeField] private GameObject _settingsFirstButton;
    [SerializeField] private GameObject _controlsFirstButton;
    [SerializeField] private GameObject _levelsFirstButton;

    private void Start() {
        OpenMainMenu();
        Time.timeScale = 1f;
    }
    void Update()
    {
        if(_levelsMenu.activeSelf) {
            if(EventSystem.current.currentSelectedGameObject == null) {
                EventSystem.current.SetSelectedGameObject(_levelsFirstButton);
            }
        }
    }

    private void CloseMenus() {
        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _controlsMenu.SetActive(false);
        _levelsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
    private void OpenMainMenu() {
        CloseMenus();
        _mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_mainFirstButton);
    }
    private void OpenLevelsMenu() {
        CloseMenus();
        _levelsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_levelsFirstButton);
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
    public void OnLevelsPress() {
        OpenLevelsMenu();
    }
    public void OnBackPress() {
        OpenMainMenu();
    }
    public void OnResetProgressPress() {
        DataPersistenceManager.Instance.NewGame();
        DataPersistenceManager.Instance.SaveGame();
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour
{
    public RectTransform[] _panelForLevels;
    public RectTransform _panelForWorlds;
    public Button[] _world1Levels;
    public Button[] _world2Levels;
    public Button[] _world3Levels;
    Button[] _button;
    public Button[][] _buttonsArray;
    public RectTransform _centerForLevels;

    float[] _distance;
    bool dragging = false;
    int _buttonDistance;    //distance between the buttons
    public int _minButtonNum;      //to hold number of the butten with smallest distance to center=
    public int _currentWorld;
    int _worldDistance;

    public static ScrollRectSnap Instance;
    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        else
            Instance = this;
    }

    void Start() {
        int buttonLength = _world1Levels.Length;
        _distance = new float[buttonLength];
        _buttonsArray = new Button[][] {_world1Levels, _world2Levels, _world3Levels};

        //Get distance between buttons
        _buttonDistance = (int)Mathf.Abs(_world1Levels[1].GetComponent<RectTransform>().anchoredPosition.y - _world1Levels[0].GetComponent<RectTransform>().anchoredPosition.y);
        if (_panelForLevels.Length > 1)
        {  
            _worldDistance = (int)Mathf.Abs(_panelForLevels[1].anchoredPosition.x - _panelForLevels[0].anchoredPosition.x);
        }
    }
    void Update()
    {
        _currentWorld = MainMenuManager.Instance.CurrentWorld;
        _button = _buttonsArray[_currentWorld];
        if (MainMenuManager.Instance._currentScreen == MenuScreens.LevelSelect)
        {
            float compareTo = _centerForLevels.transform.position.y;
            if (_minButtonNum < _button.Length - 1 && InputManager.Instance.MouseScrollInput.y < 0)
            {
                _button[_minButtonNum + 1].Select();
            }
            else if (_minButtonNum > 0 && InputManager.Instance.MouseScrollInput.y > 0)
            {
                _button[_minButtonNum - 1].Select();
            }
            for (int i = 0; i < _button.Length; i++)
            {
                _distance[i] = Mathf.Abs(compareTo - _button[i].transform.position.y);
                _distance[i] = Mathf.Abs(compareTo - _button[i].transform.position.y);
            }
            float minDistance = Mathf.Min(_distance);

            for (int i = 0; i < _button.Length; i++)
            {
                if (minDistance == _distance[i])
                {
                    _minButtonNum = i;
                }
            }
            for (int i = 0; i < _button.Length; i++)
            {
                if (_button[i].GetComponent<ButtonSelectionHandler>().IsSelected)
                {
                    _minButtonNum = i;
                }
            }
            if (!dragging)
            {
                LerpToButton(_minButtonNum * _buttonDistance);
            }
        }
    }
    void LerpToButton(int position)
    {
        float newY = Mathf.Lerp(_panelForLevels[_currentWorld].anchoredPosition.y, position, Time.deltaTime * 15f);
        float yDiff = Mathf.Abs(position - newY);
        //Debug.Log("newY --> " + newY + "\nyDiff --> " + yDiff);
        if (yDiff < 0.1f && yDiff > 0)
        {
            newY = position;
        }
        Vector2 newPosition = new Vector2(_panelForLevels[_currentWorld].anchoredPosition.x, newY);
        _panelForLevels[_currentWorld].anchoredPosition = newPosition;
    }
    public void StartDrag() {
        dragging = true;
    }
    public void StopDrag() {
        dragging = false;
    }
    IEnumerator LerpToWorld(int index)
    {
        int time = 30;
        for (int i = 0; i < time; i++)
        {
            float newX = Mathf.Lerp(_panelForWorlds.anchoredPosition.x, index, Time.deltaTime * time);
            Vector2 newPosition = new Vector2(newX, _panelForWorlds.anchoredPosition.y);
            _panelForWorlds.anchoredPosition = newPosition;
            yield return null;
        }
        
    }
    public void SnapToWorld(int CurrentWorld) {
        //_currentWorld++;
        StartCoroutine(LerpToWorld(CurrentWorld * -_worldDistance));
    }
}
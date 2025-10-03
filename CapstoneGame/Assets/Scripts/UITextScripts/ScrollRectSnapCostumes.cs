using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnapCostumes : MonoBehaviour
{
    public RectTransform _panelForCostumes;
    public Button[] _buttonsArray;
    public RectTransform _centerForLevels;

    float[] _distance;
    bool dragging = false;
    int _buttonDistance;    //distance between the buttons
    public int _minButtonNum;      //to hold number of the butten with smallest distance to center=
    public int _currentWorld;
    int _worldDistance;

    void Start() {
        int buttonLength = _buttonsArray.Length;
        _distance = new float[buttonLength];

        //Get distance between buttons
        _buttonDistance = (int)Mathf.Abs(_buttonsArray[1].GetComponent<RectTransform>().anchoredPosition.y - _buttonsArray[0].GetComponent<RectTransform>().anchoredPosition.y);
    }
    void Update() {
        if (MainMenuManager.Instance._currentScreen == MenuScreens.Costumes)
        {
            for (int i = 0; i < _buttonsArray.Length; i++)
            {
                _distance[i] = Mathf.Abs(_centerForLevels.transform.position.y - _buttonsArray[i].transform.position.y);
                _distance[i] = Mathf.Abs(_centerForLevels.transform.position.y - _buttonsArray[i].transform.position.y);
            }
            float minDistance = Mathf.Min(_distance);
            if (_minButtonNum < _buttonsArray.Length - 1 && InputManager.Instance.MouseScrollInput.y < 0)
            {
                _buttonsArray[_minButtonNum + 1].Select();
            }
            else if (_minButtonNum > 0 && InputManager.Instance.MouseScrollInput.y > 0)
            {
                _buttonsArray[_minButtonNum - 1].Select();
            }
            for (int i = 0; i < _buttonsArray.Length; i++)
            {
                if (minDistance == _distance[i])
                {
                    _minButtonNum = i;
                }
            }
            for (int i = 0; i < _buttonsArray.Length; i++)
            {
                if (_buttonsArray[i].GetComponent<ButtonSelectionHandler>().IsSelected)
                {
                    NillyDisplay.Instance.ShowCostume(_buttonsArray[i].interactable ? i : -1);
                    _minButtonNum = i;
                }
            }
            if (!dragging)
            {
                LerpToButton(_minButtonNum * _buttonDistance);
            }
        }
    }
    void LerpToButton(int position) {
        float newY = Mathf.Lerp(_panelForCostumes.anchoredPosition.y, position, Time.deltaTime * 15f);
        Vector2 newPosition = new Vector2 (_panelForCostumes.anchoredPosition.x, newY);
        _panelForCostumes.anchoredPosition = newPosition;
    }
    public void StartDrag() {
        dragging = true;
    }
    public void StopDrag() {
        dragging = false;
    }
}
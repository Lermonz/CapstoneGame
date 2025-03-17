using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour
{
    public RectTransform _panel;
    public Button[] _button;
    public RectTransform _center;

    float[] _distance;
    bool dragging = false;
    int _buttonDistance;    //distance between the buttons
    int _minButtonNum;      //to hold number of the butten with smallest distance to center
    int _selectedButtonNum;

    void Start()
    {
        int buttonLength = _button.Length;
        _distance = new float[buttonLength];

        //Get distance between buttons
        _buttonDistance = (int)Mathf.Abs(_button[1].GetComponent<RectTransform>().anchoredPosition.y - _button[0].GetComponent<RectTransform>().anchoredPosition.y);
    }
    void Update()
    {
        for(int i = 0; i<_button.Length; i++) {
            _distance[i] = Mathf.Abs(_center.transform.position.y - _button[i].transform.position.y);
        }
        float minDistance = Mathf.Min(_distance);

        for(int i = 0; i < _button.Length; i++) {
            if(minDistance == _distance[i]) {
                _minButtonNum = i;
            }
        }
        for(int i = 0; i < _button.Length; i++) {
            if(_button[i].GetComponent<ButtonSelectionHandler>().IsSelected) {
                _minButtonNum = i;
            }
        }
        if(!dragging){
            LerpToButton(_minButtonNum * _buttonDistance);
        }
    }
    void LerpToButton(int position) {
        float newY = Mathf.Lerp(_panel.anchoredPosition.y, position, Time.deltaTime * 15f);
        Vector2 newPosition = new Vector2 (_panel.anchoredPosition.x, newY);
        _panel.anchoredPosition = newPosition;
    }
    public void StartDrag() {
        dragging = true;
    }
    public void StopDrag() {
        dragging = false;
    }
}
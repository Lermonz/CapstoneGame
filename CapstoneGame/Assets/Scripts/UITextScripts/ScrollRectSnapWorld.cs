using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnapWorld : MonoBehaviour
{
    public RectTransform _panel;
    public Button[] _button;
    public RectTransform _center;

    float[] _distance;
    int _worldDistance;    //distance between the buttons
    int _minButtonNum;      //to hold number of the butten with smallest distance to center
    int _selectedButtonNum;
    int _currentWorld;

    void Start()
    {
        int buttonLength = _button.Length;
        _distance = new float[buttonLength];
        _currentWorld = 0;
        //Get distance between buttons
        _worldDistance = (int)Mathf.Abs(_button[1].GetComponent<RectTransform>().anchoredPosition.y - _button[0].GetComponent<RectTransform>().anchoredPosition.y);
    }
        // if(!dragging){
        //     LerpToButton(_minButtonNum * _worldDistance);
        // }
    public void LerpToWorld(int index) {
        index *= _worldDistance;
        float newY = Mathf.Lerp(_panel.anchoredPosition.y, index, Time.deltaTime * 15f);
        Vector2 newPosition = new Vector2 (_panel.anchoredPosition.x, newY);
        _panel.anchoredPosition = newPosition;
    }
    public void NextWorld() {
        _currentWorld++;
        LerpToWorld(_currentWorld);
    }
    public void PrevWorld() {
        _currentWorld--;
        LerpToWorld(_currentWorld);
    }
}
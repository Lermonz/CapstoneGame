using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SelectButtonOnPointerEnter : MonoBehaviour, IPointerEnterHandler
{
    Button _button;
    void Start()
    {
        _button = this.GetComponent<Button>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _button.Select();
    }
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionHandler : MonoBehaviour, ISelectHandler, IDeselectHandler
{
   public bool IsSelected { get; private set; } = false;
   public Action _whenSelected;
   public void OnSelect(BaseEventData eventData) {
      IsSelected = true;
      _whenSelected?.Invoke();
   }
   public void OnDeselect(BaseEventData eventData) {
      IsSelected = false;
   }
}

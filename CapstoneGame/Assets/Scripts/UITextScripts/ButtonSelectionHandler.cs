using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionHandler : MonoBehaviour, ISelectHandler, IDeselectHandler
{
   public bool IsSelected {get; private set;} = false;
   public void OnSelect(BaseEventData eventData) {
      IsSelected = true;
   }
   public void OnDeselect(BaseEventData eventData) {
      IsSelected = false;
   }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Button_Up : UI_Button_Base, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        onEventHandler?.Invoke(eventData);
    }
}

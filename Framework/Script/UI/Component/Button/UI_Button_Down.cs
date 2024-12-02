using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Button_Down : UI_Button_Base, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        onEventHandler?.Invoke(eventData);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Button_Drag : UI_Button_Base, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        onEventHandler?.Invoke(eventData);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Button_Click : UI_Button_Base, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        onEventHandler?.Invoke(eventData);
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class UI_Button : BaseObject
{
    public void BindEvent(Callback_UI action, GlobalEnum.eUIEvent type)
    {
        UI_Button_Base button;
        switch (type)
        {
            case GlobalEnum.eUIEvent.Click:
            {
                button = SelfObject.GetOrAddComponent<UI_Button_Click>();
                break;
            }
            case GlobalEnum.eUIEvent.Drag:
            {
                button = SelfObject.GetOrAddComponent<UI_Button_Drag>();
                break;
            }
            case GlobalEnum.eUIEvent.Down:
            {
                button = SelfObject.GetOrAddComponent<UI_Button_Down>();
                break;
            }
            case GlobalEnum.eUIEvent.Up:
            {
                button = SelfObject.GetOrAddComponent<UI_Button_Up>();
                break;
            }
            default:
                DebugManager.LogError($"Not Include UIEvent Type ({type})");
                return;
        }
        
        button.onEventHandler -= action;
        button.onEventHandler += action;
    }
}

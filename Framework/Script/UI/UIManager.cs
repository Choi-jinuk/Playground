using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<GlobalEnum.eUIType, BaseUI> _dicLoadUI = new Dictionary<GlobalEnum.eUIType, BaseUI>();
    private Dictionary<GlobalEnum.ePriorityType, GameObject> _dicPriorityUI = new Dictionary<GlobalEnum.ePriorityType, GameObject>((int)GlobalEnum.ePriorityType.Count);
    private Stack<BaseUI> _stackUI = new Stack<BaseUI>();
    private Stack<BasePopup> _stackPopupUI = new Stack<BasePopup>();

    private Dictionary<GlobalEnum.eUIAtlasName, SpriteAtlas> _dicAtlas = new Dictionary<GlobalEnum.eUIAtlasName, SpriteAtlas>();

    public void Init()
    {
        
        InputManager.Instance.AddEscapeAction(_EscapeAction);
    }

    public BaseUI MakeUI(GlobalEnum.ePriorityType priorityType, GlobalEnum.eUIType uiType)
    {
        if (_dicPriorityUI.ContainsKey(priorityType) == false)
        {
            DebugManager.LogError($"{priorityType} is Not Loaded");
            return null;
        }

        if (_dicLoadUI.TryGetValue(uiType, out var handler) == false)
        {   //UI가 로드되어 있지 않을 경우 로드
            if (_LoadUI(uiType, ref handler) == false)
                return null;
        }

        _stackUI.Push(handler);
        
        var parent = _dicPriorityUI[priorityType].transform;
        handler.SelfTransform.SetParent(parent, false);
        handler.Init();
        return handler;
    }

    public BasePopup MakePopup(GlobalEnum.ePriorityType priorityType, GlobalEnum.eUIType uiType)
    {
        if (_dicPriorityUI.ContainsKey(priorityType) == false)
        {
            DebugManager.LogError($"{priorityType} is Not Loaded");
            return null;
        }

        if (_dicLoadUI.TryGetValue(uiType, out var handler) == false)
        {   //UI가 로드되어 있지 않을 경우 로드
            if (_LoadUI(uiType, ref handler) == false)
                return null;
        }

        if (handler is BasePopup popup == false)
            return null;
        RootUIDisable(priorityType);
        _stackPopupUI.Push(popup);
        
        var parent = _dicPriorityUI[priorityType].transform;
        popup.SelfTransform.SetParent(parent, false);
        popup.IsRootUI = true;
        popup.Init();
        return popup;
    }

    private bool _LoadUI(GlobalEnum.eUIType uiType, ref BaseUI handler)
    {
        var goUI = AddressableManager.LoadPrefab(uiType.ToString());
        if (goUI == null)
        {
            DebugManager.LogError($"{uiType.ToString()} Not Include Addressable");
            return false; 
        }
        handler = goUI.GetComponent<BaseUI>();
        if (handler == null)
        {
            DebugManager.LogError($"{uiType.ToString()} Not Include BaseUI Component");
            return false;
        }

        _dicLoadUI[uiType] = handler;
        return handler;
    }

    public void RootUIDisable(GlobalEnum.ePriorityType priorityType)
    {
        foreach (var ui in _stackUI)
        {
            if (ui.PriorityType != priorityType)
                continue;
            if (ui.IsRootUI == false)
                continue;
            if(ui.SelfObject)
                ui.SelfObject.SetActive(false);
        }
    }
    
    void _EscapeAction()
    {
        
    }
}

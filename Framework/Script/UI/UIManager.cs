using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<GlobalEnum.eUIType, BaseUI> _dicLoadUI = new Dictionary<GlobalEnum.eUIType, BaseUI>();
    private Dictionary<GlobalEnum.ePriorityType, GameObject> _dicPriorityUI = new Dictionary<GlobalEnum.ePriorityType, GameObject>((int)GlobalEnum.ePriorityType.Count);
    private Stack<BaseUI> _stackUI = new Stack<BaseUI>();
    private Stack<BasePopup> _stackPopupUI = new Stack<BasePopup>();

    private Dictionary<GlobalEnum.eUIAtlasName, SpriteAtlas> _dicAtlas = new Dictionary<GlobalEnum.eUIAtlasName, SpriteAtlas>();

    public void Init()
    {
        InitPriority();
        InitAtlas().Forget();
        
        InputManager.Instance.AddEscapeAction(_EscapeAction);
    }

    private void InitPriority()
    {
        for (int i = 0; i < (int)GlobalEnum.ePriorityType.Count; i++)
        {
            var priority = (GlobalEnum.ePriorityType)i;
            if (_dicPriorityUI.ContainsKey(priority))
                continue;
            
            if(_LoadPriority(priority) == false)
                DebugManager.LogError("Priority is Not Load");
        }
    }

    private async UniTaskVoid InitAtlas()
    {
        for (int i = 0; i < (int)GlobalEnum.eUIAtlasName.Count; i++)
        {
            var atlasType = (GlobalEnum.eUIAtlasName)i;
            if (_dicAtlas.ContainsKey(atlasType))
                continue;
            var atlas = await AddressableManager.Load<SpriteAtlas>(atlasType.ToString("F"), false);
            if (atlas == null)
                return;
            
            _dicAtlas[atlasType] = atlas;
        }
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
    private bool _LoadPriority(GlobalEnum.ePriorityType priorityType)
    {
        var goUI = AddressableManager.LoadPrefab(priorityType.ToString());
        if (goUI == null)
        {
            DebugManager.LogError($"{priorityType.ToString()} Not Include Addressable");
            return false; 
        }

        Object.DontDestroyOnLoad(goUI);
        GameObjectUtil.RemoveCloneTag(goUI);
        _dicPriorityUI[priorityType] = goUI;
        return goUI;
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

    public Sprite GetAtlasIconSprite(GlobalEnum.eUIAtlasName atlasName, string key)
    {
        if (string.IsNullOrEmpty(key))
            return null;
        if (_dicAtlas.ContainsKey(atlasName) == false)
            return null;
        var sprite = _dicAtlas[atlasName].GetSprite(key);
        if (sprite == null)
        {
            DebugManager.LogError($"NullReferenceException: ({key}) Not In ({atlasName.ToString()})");
            return null;
        }
        return sprite;
    }
    
    void _EscapeAction()
    {
        
    }
}

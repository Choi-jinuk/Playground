using System;
using System.Collections.Generic;
using UnityEngine;

public partial class GameDataManager : Singleton<GameDataManager>
{
    Dictionary<GlobalEnum.eTemplateManagerType, BaseManager> _templateDictionary = new ();

    public bool HasTemplate(GlobalEnum.eTemplateManagerType type)
    {
        return _templateDictionary.ContainsKey(type);
    }

    public T GetTemplate<T>(GlobalEnum.eTemplateManagerType type) where T : BaseManager
    {
        if (_templateDictionary.TryGetValue(type, out var data) == false)
        {
            DebugManager.LogError($"Not in Template Data Dictionary ({type.ToString()})");
            return null;
        }
        
        return data as T;
    }
}

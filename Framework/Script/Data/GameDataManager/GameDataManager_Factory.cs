using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameDataManager
{
    BaseManager _CreateTemplateManager(GlobalEnum.eTemplateManagerType type)
    {
        if (_templateDictionary.ContainsKey(type))
        {
            DebugManager.LogError($"Duplicate Manager, ({type}) is Already Create Manager");
            return null;
        }
        
        switch (type)
        {
            case GlobalEnum.eTemplateManagerType.SkillEffect :
                return new SkillEffectManager();
            default:
                return null;
        }
    }
}

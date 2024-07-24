using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameDataManager
{
    BaseManager _CreateOrGetTemplateManager(GlobalEnum.eTemplateManagerType type)
    {
        if (_templateDictionary.ContainsKey(type))
        {
            return _templateDictionary[type];
        }
        
        switch (type)
        {
            case GlobalEnum.eTemplateManagerType.Character :
                return new CharacterTemplateManager();
            case GlobalEnum.eTemplateManagerType.StatSheet :
                return new StatSheetManager();
            case GlobalEnum.eTemplateManagerType.Skill :
                return new SkillTemplateManager();
            case GlobalEnum.eTemplateManagerType.SkillEffect :
                return new SkillEffectManager();
            default:
                return null;
        }
    }
}

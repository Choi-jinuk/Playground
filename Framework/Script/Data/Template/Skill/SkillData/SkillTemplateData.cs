using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class SkillTemplateData
{
    public string Key { get { return _key; } }
    public GlobalEnum.eSkillType Type { get { return _skillType; } }
    
    private string _key;
    private GlobalEnum.eSkillType _skillType;
    
    public static SkillTemplateData Load(JSONNode node)
    {
        if (node == null)
            return null;

        SkillTemplateData data = new SkillTemplateData();
        data._key = JsonLoader.GetString(node, GlobalString.Key);
        
        var typeString = JsonLoader.GetString(node, GlobalString.Type);
        if (EnumUtil.TryParse(typeString, out GlobalEnum.eSkillType type))
            data._skillType = type;
        
        return data;
    }
}

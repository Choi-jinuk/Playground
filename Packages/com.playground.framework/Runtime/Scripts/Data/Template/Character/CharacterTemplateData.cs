using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class CharacterTemplateData
{
    public string Key { get { return _key; } }
    public List<string> SkillList { get { return _skillList; } }

    private string _key;
    private List<string> _skillList = new List<string>();
    
    public static CharacterTemplateData Load(JSONNode node)
    {
        if (node == null)
            return null;

        CharacterTemplateData data = new CharacterTemplateData();
        data._key = JsonLoader.GetString(node, GlobalString.Key);
        
        var skillArrayData = JsonLoader.GetObject<JSONArray>(node, GlobalString.SkillList);
        if (skillArrayData != null)
        {   //시작부터 갖고 있는 스킬 (평타 등) 없는 캐릭터도 있으니 null 대응
            foreach (var skillNode in skillArrayData.Children)
            {
                data._skillList.Add(skillNode);
            }   
        }

        return data;
    }
}

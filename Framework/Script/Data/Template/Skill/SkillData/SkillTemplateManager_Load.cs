using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public partial class SkillTemplateManager : BaseTemplateManager<SkillTemplateManager>
{
    private Dictionary<string, SkillTemplateData> _dataDictionary = new Dictionary<string, SkillTemplateData>();
    protected override bool _LoadData(JSONObject jsonObject)
    {
        foreach (KeyValuePair<string, JSONNode> data in jsonObject)
        {
            JSONArray arrNode = data.Value as JSONArray;
            if (arrNode == null)
            {
                DebugManager.LogError($"{GetType()} Load ({data.Key})Data is Null");
                return false;
            }

            var nodeCount = arrNode.Count;
            for (int i = 0; i < nodeCount; i++)
            {
                JSONNode node = arrNode[i];
                var skillData = SkillTemplateData.Load(node);
                if (skillData == null)
                {
                    DebugManager.LogError($"SkillTemplateData ({data.Key})_({i})Data Load Error");
                    return false;
                }
                
                _dataDictionary.Add(data.Key, skillData);
            }
        }


        return true;
    }
}

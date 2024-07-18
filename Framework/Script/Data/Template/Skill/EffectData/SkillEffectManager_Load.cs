using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public partial class SkillEffectManager : BaseTemplateManager<SkillEffectManager>
{
    private Dictionary<string, SkillEffectData> _dataDictionary = new Dictionary<string, SkillEffectData>();

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
                var effectData = SkillEffectData.Load(node);
                if (effectData == null)
                {
                    DebugManager.LogError($"SkillEffectData ({data.Key})_({i})Data Load Error");
                    return false;
                }
                
                _dataDictionary.Add(effectData.Key, effectData);
            }
        }

        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public partial class CharacterTemplateManager : BaseTemplateManager<CharacterTemplateManager>
{
    private Dictionary<string, CharacterTemplateData> _dataDictionary = new Dictionary<string, CharacterTemplateData>();
    protected override bool _LoadData(string fileName, JSONObject jsonObject)
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
                var characterData = CharacterTemplateData.Load(node);
                if (characterData == null)
                {
                    DebugManager.LogError($"CharacterTemplateData ({data.Key})_({i})Data Load Error");
                    return false;
                }
                
                _dataDictionary.Add(data.Key, characterData);
            }
        }


        return true;
    }
}

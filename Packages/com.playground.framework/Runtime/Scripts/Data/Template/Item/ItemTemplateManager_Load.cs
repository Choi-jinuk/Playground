using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public partial class ItemTemplateManager
{
    private Dictionary<string, ItemTemplateData> _dataDictionary = new Dictionary<string, ItemTemplateData>();

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
                var itemData = ItemTemplateData.Load(node);
                if (itemData == null)
                {
                    DebugManager.LogError($"ItemTemplateData ({data.Key})_({i})Data Load Error");
                    return false;
                }

                _dataDictionary[itemData.Key] = itemData;
            }
        }

        return true;
    }
}

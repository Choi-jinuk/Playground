using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class StringTableManager : BaseTemplateManager<StringTableManager>
{
    private Dictionary<GlobalEnum.eCountry, Dictionary<string, string>> _languagePack = new Dictionary<GlobalEnum.eCountry, Dictionary<string, string>>();
    
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
                var key = JsonLoader.GetString(node, GlobalString.Key);
                if (string.IsNullOrEmpty(key))
                    continue;

                foreach (var country in Enum.GetNames(typeof(GlobalEnum.eCountry)))
                {
                    var stringData = JsonLoader.GetString(node, country);
                    if (string.IsNullOrEmpty(stringData))
                        continue;

                    _AddString(country, key, stringData);
                }
            }

        }

        return true;
    }

    private void _AddString(string country, string key, string data)
    {
        if (EnumUtil.TryParse<GlobalEnum.eCountry>(country, out var countryKey) == false)
            return;
        if (_languagePack.TryGetValue(countryKey, out var languageData) == false)
        {
            languageData = new Dictionary<string, string>();
            _languagePack[countryKey] = languageData;
        }

        languageData[key] = data;
    }

    public string GetString(string key)
    {
        if (string.IsNullOrEmpty(key))
            return key;
        if (_languagePack.TryGetValue(PlatformSystem.Instance.Country, out var languagePack) == false)
            return key;
        if (languagePack.TryGetValue(key, out var data) == false)
            return key;
        
        return data;
    }
}

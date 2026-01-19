using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public partial class StatSheetManager : BaseTemplateManager<StatSheetManager>
{
    private Dictionary<string, StatSheet> _dataDictionary = new Dictionary<string, StatSheet>();
    private Dictionary<string, StatLimit> _limitDictionary = new Dictionary<string, StatLimit>();
    protected override bool _LoadData(string fileName, JSONObject jsonObject)
    {
        if (fileName.Equals(GlobalString.StatSheet))
        {
            return _LoadData_StatSheet(jsonObject);
        }
        
        if (fileName.Equals(GlobalString.StatLimit))
        {
            return _LoadData_StatLimit(jsonObject);
        }

        return false;
    }

    bool _LoadData_StatSheet(JSONObject jsonObject)
    {
        foreach (KeyValuePair<string, JSONNode> data in jsonObject)
        {
            var statData = StatSheet.Load(data.Key, data.Value);
            if (statData == null)
            {
                DebugManager.LogError($"StatSheet LoadData Error {data.Key}");
                return false;
            }
            _dataDictionary[statData.Key] = statData;
        }
        
        return true;
    }

    bool _LoadData_StatLimit(JSONObject jsonObject)
    {
        foreach (KeyValuePair<string, JSONNode> data in jsonObject)
        {
            var limitData = StatLimit.Load(data.Value);
            if (limitData == null)
            {
                DebugManager.LogError($"StatSheet LoadData Error {data.Key}");
                return false;
            }
            _limitDictionary[limitData.Key] = limitData;
        }
        
        return true;
    }
}

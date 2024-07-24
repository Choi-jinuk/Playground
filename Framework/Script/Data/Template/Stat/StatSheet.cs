using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class StatSheet
{
    struct stStatSheetInfo
    {
        public GlobalEnum.eStatType StatType;
        public int Type;

        public double Stat;
        public float LevelIncreaseStat;
    }

    public string Key { get { return _key; } }
    
    private string _key;
    private Dictionary<GlobalEnum.eStatType, stStatSheetInfo> _statSheetInfos = new Dictionary<GlobalEnum.eStatType, stStatSheetInfo>();

    public static StatSheet Load(string key, JSONNode node)
    {
        if (node == null)
            return null;
        
        JSONArray nodeArray = node as JSONArray;
        if (nodeArray == null)
            return null;

        StatSheet data = new StatSheet();
        data._key = key;

        for (int i = 0; i < nodeArray.Count; i++)
        {
            JSONNode nodeData = nodeArray[i];
            if (nodeData == null)
            {
                DebugManager.LogError($"nodeData is Null");
                continue;
            }

            string statName = JsonLoader.GetString(nodeData, GlobalString.Name);
            if (EnumUtil.TryParse(statName, out GlobalEnum.eStatType statType) == false)
            {
                DebugManager.LogError($"Not Exist {statName} in eStatType");
                continue;
            }

            stStatSheetInfo info = new stStatSheetInfo()
            {
                StatType = statType,
                Type = JsonLoader.GetInteger(nodeData, GlobalString.Type),
                Stat = JsonLoader.GetDouble(nodeData, GlobalString.Stat),
                LevelIncreaseStat = JsonLoader.GetFloat(nodeData, GlobalString.LevelIncreaseStat),
            };
            
            data._statSheetInfos[info.StatType] = info;
        }

        return data;
    }

    public double GetStat(GlobalEnum.eStatType statType, stCharacterInfoParam param, int statLevel)
    {
        if (_statSheetInfos.TryGetValue(statType, out var info) == false)
            return 0;

        return _CalcStat(info, param, statLevel);
    }

    double _CalcStat(stStatSheetInfo info, stCharacterInfoParam param, int statLevel)
    {
        int characterLevel = param.Level;

        switch (info.Type)
        {
            case 0:
            {   //메인 캐릭터 스탯 (스탯 별 레벨 사용)
                var result = info.Stat;
                result += info.LevelIncreaseStat * statLevel;
                
                return result;
            }
            case 1:
            {   //서브 캐릭터 스탯 (캐릭터 레벨 사용)
                var result = info.Stat;
                result += info.LevelIncreaseStat * characterLevel;
                
                return result;
            }
            default:
                return 0;
        }
    }
}

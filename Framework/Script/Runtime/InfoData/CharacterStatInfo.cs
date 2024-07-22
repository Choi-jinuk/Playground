using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatLevelInfo
{
    public int Count { get { return _statList.Count; } }
    public List<stStatLevel> StatList { get { return _statList; } set { _statList = value; } }
    private List<stStatLevel> _statList = new List<stStatLevel>();

    stStatLevel GetInfo(GlobalEnum.eStatType type)
    {
        int index = _statList.FindIndex(item => item.Type == type);
        stStatLevel stat;
        if (index < 0)
        {
            stat = new stStatLevel() { Type = type };
            _statList.Add(stat);
        }
        else
        {
            stat = _statList[index];
        }

        return stat;
    }
}

public class CharacterStatInfo
{
    private Dictionary<GlobalEnum.eStatType, double> _stat = new Dictionary<GlobalEnum.eStatType, double>();
    private StatLevelInfo _statLevel;
    private stCharacterInfoParam _param;
}

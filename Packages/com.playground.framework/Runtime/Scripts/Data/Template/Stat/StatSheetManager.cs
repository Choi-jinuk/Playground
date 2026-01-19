using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatSheetManager
{
    public StatSheet GetStatSheet(string key)
    {
        if (string.IsNullOrEmpty(key))
            return null;
        if (_dataDictionary.TryGetValue(key, out var sheet) == false)
        {
            return null;
        }

        return sheet;
    }
    /// <summary> 스탯의 리미트가 있을 경우 Min Max 값 체크 </summary> <returns> 리미트에 걸렸는지 체크 </returns>
    public bool CalcStatLimit(GlobalEnum.eStatType statType, ref double stat)
    {
        if (_limitDictionary.TryGetValue(statType.ToString(), out var limit) == false)
            return false;

        if (stat > limit.Max)
        {
            stat = limit.Max;
            return true;
        }
        
        if (stat < limit.Min)
        {
            stat = limit.Min;
            return true;
        }

        return false;
    }
}

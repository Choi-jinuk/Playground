using System.Collections.Generic;
using SimpleJSON;

public struct stBoardMeta
{
    public enum META
    {
        Target, //타겟으로 설정된 GameCharacter의 uid
    }

    public META Type;
    public ulong UlValue;
} 

public class Blackboard
{
    private Dictionary<stBoardMeta.META, stBoardMeta> _metas = new Dictionary<stBoardMeta.META, stBoardMeta>();

    bool GetMeta(stBoardMeta.META type, out stBoardMeta meta)
    {
        return _metas.TryGetValue(type, out meta);
    }

    public ulong GetValueULong(stBoardMeta.META type)
    {
        return GetMeta(type, out stBoardMeta meta) ? meta.UlValue : 0;
    }
    
    public bool IsData(stBoardMeta.META type)
    {
        return _metas.ContainsKey(type);
    }

    public void Clear()
    {
        _metas.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolDebug
{
    public string DebugName();
    /// <summary> Use/ Total </summary>
    public string DebugCount();
}

public static class PoolDebugManager
{
    public static List<IPoolDebug> _PoolDebugs = new List<IPoolDebug>();

    public static void AddPool(IPoolDebug debug)
    {
        _PoolDebugs.Add(debug);
    }

    public static void RemovePool(IPoolDebug debug)
    {
        _PoolDebugs.Remove(debug);
    }

    private static string _cacheLog = string.Empty;
    public static void LogPool()
    {
        _cacheLog = string.Empty;
        _PoolDebugs.ForEach(debug =>
        {
            _cacheLog = StringUtil.AppendLine(_cacheLog, debug.DebugName(), debug.DebugCount());
        });
        
        DebugManager.Log(_cacheLog);
    }
}

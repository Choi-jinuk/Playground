using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class StatLimit
{
    public string Key { get { return _key; } }
    public double Max { get { return _max; } }
    public double Min { get { return _min; } }
    
    private string _key;
    private double _max;
    private double _min;

    public static StatLimit Load(JSONNode node)
    {
        if (node == null)
            return null;

        var data = new StatLimit();
        data._key = JsonLoader.GetString(node, GlobalString.Key);
        data._max = JsonLoader.GetDouble(node, GlobalString.Max);
        data._min = JsonLoader.GetDouble(node, GlobalString.Min);

        return data;
    }
}

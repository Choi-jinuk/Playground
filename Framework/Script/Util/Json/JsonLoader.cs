using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public static class JsonLoader
{
    public static int GetInteger(JSONNode node, string key)
    {
        if (node == null)
            return 0;
        if (node[key].Tag == JSONNodeType.None)
            return 0;
        
        return node[key].AsInt;
    }
    public static long GetLong(JSONNode node, string key)
    {
        if (node == null)
            return 0;
        if (node[key].Tag == JSONNodeType.None)
            return 0;

        return node[key].AsLong;
    }
    public static float GetFloat(JSONNode node, string key)
    {
        if (node == null)
            return 0;
        if (node[key].Tag == JSONNodeType.None)
            return 0;

        return node[key].AsFloat;
    }
    public static double GetDouble(JSONNode node, string key)
    {
        if (node == null)
            return 0;
        if (node[key].Tag == JSONNodeType.None)
            return 0;

        return node[key].AsDouble;
    }
    public static bool GetBool(JSONNode node, string key)
    {
        if (node == null)
            return false;
        if (node[key].Tag == JSONNodeType.None)
            return false;

        return node[key].AsBool;
    }
    public static string GetString(JSONNode node, string key)
    {
        if (node == null)
            return string.Empty;
        if (node[key].Tag == JSONNodeType.None)
            return string.Empty;

        string data = node[key];
        return data;
    }
    public static Vector3 GetVector3(JSONNode node, string key)
    {
        if (node == null)
            return default;
        if (node[key].Tag == JSONNodeType.None)
            return default;

        string vector = node[key];
        if (string.IsNullOrEmpty(vector))
            return default;
        var vecArray = vector.Split(":");
        if (vecArray.Length < 3)
            return default;
                
        var result = Array.ConvertAll(vecArray, float.Parse);
        return new Vector3(result[0], result[1], result[2]);
    }
    public static Vector2 GetVector2(JSONNode node, string key)
    {
        if (node == null)
            return default;
        if (node[key].Tag == JSONNodeType.None)
            return default;

        string vector = node[key];
        if (string.IsNullOrEmpty(vector))
            return default;
        var vecArray = vector.Split(":");
        if (vecArray.Length < 2)
            return default;
                
        var result = Array.ConvertAll(vecArray, float.Parse);
        return new Vector2(result[0], result[1]);
    }
    public static T GetObject<T>(JSONNode node, string key) where T : JSONNode
    {
        if (node == null)
            return null;
        if (node[key].Tag == JSONNodeType.None)
            return null;
        
        return node[key] as T;
    }
}
